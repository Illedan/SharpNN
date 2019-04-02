using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using SharpNN;

namespace GenerateNumbers
{
    public class NetworkWrapper
    {
        private MLPNetwork m_network;
        public NetworkWrapper(string filename, string configName)
        {
            var setup = ReadSetup(filename, configName);
            m_network = MLPNetwork.CreateMLPNetwork(setup);
        }

        public int Calculate(int a, int b)
        {
            var result = m_network.Calculate(new double[]{ a, b });
            Console.Error.WriteLine("Weights: " + string.Join(" ", result));
            return result[0] > result[1] ? 0 : 1;
        }

        public static NetworkSetup ReadSetup(string weightname, string configName)
        {
            var config = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(configName));
            var weights = ExtractWeights(File.ReadAllText(weightname));

            var network = new NetworkSetup();

            // Input neuron
            network.Layers.Add(new LayerSetup
            {
                ActivationFunction = x => x,//x => (x + 10000.0) / 20000.0,
                NodeCount = config.config.build_input_shape.First(i => i != null).Value
            });

            // Other neurons
            var previousCount = network.Layers.First().NodeCount;
            var weightCounter = 0;
            for(var i = 0; i < config.config.layers.Count; i++)
            {
                var layerConfig = config.config.layers[i];
                Func<double, double> activation = MLPNetwork.Linear;
                //if(layerConfig.config.activation != "linear")
                //{
                //    activation = MLPNetwork.Sigmoid;
                //}

                var setup = new LayerSetup
                {
                    ActivationFunction = activation,
                    NodeCount = layerConfig.config.units
                };

                setup.Weights = weights.Skip(weightCounter).Take(setup.NodeCount*(previousCount + 1)).ToArray();
                weightCounter += setup.Weights.Length;

                network.Layers.Add(setup);

                previousCount = setup.NodeCount;
            }

            return network;
        }

        private static double[] ExtractWeights(string txt)
        {
            txt = txt.Replace("[", " ").Replace("]", " ").Replace(",", " ").Replace("\n", " ").Replace("\r", " ").Replace("\"", " ").Trim();
            while(txt.Contains("  "))
                txt = txt.Replace("  ", " ").Trim();

            txt = txt.Replace(".", ",");

            return txt.Split().Select(double.Parse).ToArray();
        }
    }

    public class LayerConfig
    {
        public int units { get; set; }
        public string activation { get; set; }
        public bool use_bias { get; set; }
    }

    public class Layer
    {
        public LayerConfig config { get; set; }
    }

    public class Config
    {
        public List<Layer> layers { get; set; }
        public List<int?> build_input_shape { get; set; }
    }

    public class RootObject
    {
        public Config config { get; set; }
    }
}
