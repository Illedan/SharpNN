
using System;
using System.Linq;
using System.Collections.Generic;
namespace SharpNN
{
	public class MLPNetwork
	{
		public static double Identity(double x) => x;
		public static double Binary(double x) => x < 0 ? 0 : 1;
		public static double Logistic(double x) => 1 / (1 + Math.Pow(Math.E, -x));
		public static double TanH(double x) => Math.Tanh(x);
		public static double SoftSign(double x) => x / (1 + Math.Abs(x));

		public double[] Calculate(double[] input)
		{
			if (input.Length != Layers[0].Length) throw new ArgumentException("Invalid number of input parameters");

			for (var i = 0; i < input.Length; i++)
			{
				Layers[0][i].OutputValue = input[i];
			}

			for (var l = 1; l < Layers.Length; l++)
			{
				for (var n = 0; n < Layers[l].Length; n++)
				{
					Layers[l][n].CalculateOutput();
				}
			}

			return Layers[Layers.Length - 1].Select(l => l.OutputValue).ToArray();
		}

		public static MLPNetwork CreateMLPNetwork(int[] layerSetup, double[] weights = null, Func<double, double> activationFunction = null)
		{
			var network = new MLPNetwork { Layers = new Node[layerSetup.Length][] };
			for (var i = 0; i < layerSetup.Length; i++)
			{
				var actualActivationFunc = i == layerSetup.Length - 1 ? Identity : (activationFunction ?? TanH);
				network.Layers[i] = Enumerable.Range(0, layerSetup[i]).Select(l => new Node(actualActivationFunc)).ToArray();
			}

			var weightCounter = 0;
			var rnd = new Random();
			for (var i = 1; i < network.Layers.Length; i++)
			{
				foreach (var n in network.Layers[i - 1])
				{
					foreach (var nextNode in network.Layers[i])
					{
						nextNode.InputLinks.Add(new Link(weights == null ? rnd.NextDouble() : weights[weightCounter++]) 
						{ 
							SourceNode = n, DestinationNode = nextNode
						});
					}
				}
			}

			return network;
		}

		public Node[][] Layers { get; set; }
	}

	public class Node
	{
		public Node(Func<double, double> activationFunction)
		{
			ActivationFunction = activationFunction;
		}

		public Func<double, double> ActivationFunction { get; }
		public double OutputValue;
		public List<Link> InputLinks { get; } = new List<Link>();
		public virtual void CalculateOutput()
		{
			var value = 0.0;
			foreach (var link in InputLinks)
			{
				value += link.SourceNode.OutputValue * link.Weight;
			}

			OutputValue = ActivationFunction(value);
		}
	}

	public class Link
	{
		public Link(double weight)
		{
			Weight = weight;
		}

		public Node SourceNode { get; set; }
		public Node DestinationNode { get; set; }
		public double Weight { get; set; }
	}
}