using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpNN;

namespace GenerateNumbers.SharpNN
{
    public class MLPNetworkFactory
    {
        
		public static MLPNetwork CreateMLPNetwork(NetworkSetup setup)
		{
			var network = new MLPNetwork { Layers = new Node[setup.Layers.Count][] };
			for (var i = 0; i < setup.Layers.Count; i++)
			{
                var layer = setup.Layers[i];
                var includeBias = i != setup.Layers.Count-1;
				network.Layers[i] = Enumerable.Range(0, layer.NodeCount + (includeBias?1:0)).Select(l => new Node(layer.ActivationFunction, l == layer.NodeCount)).ToArray();
			}

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
    }
}
