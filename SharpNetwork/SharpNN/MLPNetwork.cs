using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace SharpNN
{
    public static class ActivationFunctions
    {
        public static double Linear(double x) => x;
		public static double Binary(double x) => x < 0 ? 0 : 1;
		public static double ReLU(double x) => x < 0 ? 0 : x;
		public static double Sigmoid(double x) => 1 / (1 + Math.Pow(Math.E, -x));
		public static double TanH(double x) => Math.Tanh(x);
		public static double SoftSign(double x) => x / (1 + Math.Abs(x));
    }

    public class NetworkOutcome
    {
        public NetworkOutcome(int target, double value)
        {
            Value = value;
            Target = target;
        }

        public int Target { get; }
        public double Value { get; }
    }

    public class NetworkSetup
    {
        public List<LayerSetup> Layers { get; } = new List<LayerSetup>();
    }

    public class LayerSetup
    {
        public Func<double, double> ActivationFunction { get; set; }
        
        //Does not include bias.
        public int NodeCount { get; set; }

        // Into this layer from the previous. No weights into bias.
        public double[] Weights { get; set; }
    }

	public class MLPNetwork
	{
		public List<NetworkOutcome> Calculate(double[] input)
		{
			if (input.Length != Layers[0].Length - 1) throw new ArgumentException("Invalid number of input parameters");

			for (var i = 0; i < input.Length; i++)
			{
				Layers[0][i].OutputValue = Layers[0][i].ActivationFunction(input[i]);
			}

			for (var l = 1; l < Layers.Length; l++)
			{
				for (var n = 0; n < Layers[l].Length; n++)
				{
					Layers[l][n].CalculateOutput();
				}
			}

			var resultLayer = Layers[Layers.Length - 1].Select(l => l.OutputValue).ToArray();
            var outcome = new List<NetworkOutcome>();
            for(var i = 0; i < resultLayer.Length; i++)
            {
                outcome.Add(new NetworkOutcome(i, resultLayer[i]));
            }

            return outcome;
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
		public double OutputValue = 1.0;
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