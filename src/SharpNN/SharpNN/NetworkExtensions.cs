using System;
using System.Linq;
using System.Collections.Generic;

namespace SharpNN
{
	public static class NetworkExtensions
	{
		public static double[] ExtractWeights(this MLPNetwork network)
		{
			var weights = new List<double>();
			foreach (var n in network.Layers)
			{
				foreach (var n2 in n)
				{
					foreach (var l in n2.InputLinks)
					{
						weights.Add(l.Weight);
					}
				}
			}

			return weights.ToArray();
		}
	}
}
