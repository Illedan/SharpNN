using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GenerateNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            var weightsName = "weights.txt";
            var configName = "network.txt";

            var wrapper = new NetworkWrapper(weightsName, configName);

            Console.WriteLine("HI! Let's find the biggest number! Network is located at: " + weightsName + " - " + configName);
            while (true)
            {
                Console.WriteLine("Print 2 numbers, A and B on each line. 10000 > A >= 0 & 10000 > B >= -10000");
                var a = ReadInt();
                var b = ReadInt();

                var result = wrapper.Calculate(a, b);
                Console.WriteLine("Number: " + result + " is biggest");
            }


        }

        private static int ReadInt() => int.Parse(Console.ReadLine());
        
        //File.WriteAllText("TrainData.txt", string.Join("\n",  CreateArray(10000).Select(s => string.Join(",", s))));

        //File.WriteAllText("TestData.txt", string.Join("\n",  CreateArray(1000).Select(s => string.Join(",", s))));

        //private static Random rnd = new Random(42);

        //private static List<int[]> CreateArray(int count)
        //{
        //    var numbers = new List<int[]>();
        //    for(var i = 0; i < count; i++)
        //    {
        //        var newNumbs = new int[3];
        //        newNumbs[0] = rnd.Next(0, 10000);
        //        newNumbs[1] = rnd.Next(-10000, 10000);
        //        newNumbs[2] = newNumbs[0] > newNumbs[1] ? 0 : 1;
        //        numbers.Add(newNumbs);
        //    }
        //    return numbers;
        //}
    }
}
