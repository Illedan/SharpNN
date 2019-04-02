
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace SharpNN
{
    public class NetworkSetup
    {
        public List<LayerSetup> Layers { get; } = new List<LayerSetup>();
    }

    public class LayerSetup
    {
        public Func<double, double> ActivationFunction { get; set; }
        //Does not include bias.
        public int NodeCount { get; set; }
        // from this layer to the next. Last layer has no weights.
        public double[] Weights { get; set; }
    }


	public class MLPNetwork
	{
		public static double Linear(double x) => x;
		public static double Binary(double x) => x < 0 ? 0 : 1;
		public static double Sigmoid(double x) => 1 / (1 + Math.Pow(Math.E, -x));
		public static double TanH(double x) => Math.Tanh(x);
		public static double SoftSign(double x) => x / (1 + Math.Abs(x));


		public double[] Calculate(double[] input)
		{
			if (input.Length != Layers[0].Length - 1) throw new ArgumentException("Invalid number of input parameters");

			for (var i = 0; i < input.Length; i++)
			{
				Layers[0][i].OutputValue = Layers[0][i].ActivationFunction(input[i]);
			}

            Layers[0].Last().CalculateOutput();

			for (var l = 1; l < Layers.Length; l++)
			{
				for (var n = 0; n < Layers[l].Length; n++)
				{
					Layers[l][n].CalculateOutput();
				}
			}

			return Layers[Layers.Length - 1].Select(l => l.OutputValue).ToArray();
		}



		public static MLPNetwork CreateMLPNetwork(NetworkSetup setup)
		{
			var network = new MLPNetwork { Layers = new Node[setup.Layers.Count][] };
			for (var i = 0; i < setup.Layers.Count; i++)
			{
                var layer = setup.Layers[i];
                var includeBias = i != setup.Layers.Count-1;
				network.Layers[i] = Enumerable.Range(0, layer.NodeCount + (includeBias?1:0)).Select(l => new Node(layer.ActivationFunction, l == layer.NodeCount)).ToArray();
			}

			var rnd = new Random();
			for (var i = 1; i < network.Layers.Length; i++)
			{
			    var weightCounter = 0;
                var currentLayer = network.Layers[i];
                var previousLayer = network.Layers[i - 1];
                var layerSetup = setup.Layers[i];
				foreach (var n in previousLayer)
				{
					foreach (var nextNode in currentLayer)
					{
                        if(nextNode.IsBias) continue;
						nextNode.InputLinks.Add(new Link(layerSetup.Weights[weightCounter++])
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
		public Node(Func<double, double> activationFunction, bool isBias)
		{
			ActivationFunction = activationFunction;
            IsBias = isBias;
		}

        public bool IsBias { get; set; }
		public Func<double, double> ActivationFunction { get; }
		public double OutputValue;
		public List<Link> InputLinks { get; } = new List<Link>();
		public virtual void CalculateOutput()
		{
            if (IsBias)
            {
                OutputValue = 1.0;
                return;
            }

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