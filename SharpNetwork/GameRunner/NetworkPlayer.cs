using System;
using System.IO;
using System.Linq;
using GenerateNumbers;
using GenerateNumbers.SharpNN;
using Newtonsoft.Json;
using SharpNN;
using TicTacToe.Game;

namespace GameRunner
{
    public class NetworkPlayer : IPlayer
    {
        private MLPNetwork m_network;
        public NetworkPlayer()
        {
            var setup = ReadSetup("weights.txt", "network.txt");
            m_network = MLPNetworkFactory.CreateMLPNetwork(setup);
        }
        private int _playerId;
        private int OtherPlayer => _playerId == 1 ? 2 : 1;
        public void Initialize(int playerId)
        {
            _playerId = playerId;
        }

        public int GetMove(TicTacToe.Game.TicTacToe game)
        {
            var state = game.GetBoard();
            var gameState = state.Select(s => s == _playerId?1:(0)).Concat(state.Select(s => s == _playerId?0:(s==OtherPlayer?1:0))).ToArray();
            var result = m_network.Calculate(gameState.Select(s => (double)s).ToArray());

            Console.Error.WriteLine("Weights: " + string.Join(" ", result.Select(r => r.Value)));
            
            return result.OrderByDescending(r => r.Value).First(r => game.IsPossible(r.Target)).Target;
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
                Func<double, double> activation = ActivationFunctions.Linear;
                if(layerConfig.config.activation.ToLower() == "relu")
                {
                    activation = ActivationFunctions.ReLU;
                }

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
}