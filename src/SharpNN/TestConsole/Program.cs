using System;
using SharpNN;

namespace TestConsole
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var tests = new Func<bool>[] { Test_ReuseNetwork_Sameresult, Test_Weights, Test_Unwrap_Create_SameResult, Test_MoreWeights, Test_MoreWeights_LinearActivation };
			var res = 0;
			foreach (var test in tests)
			{
				var current = test();
				res += current ? 1 : 0;
			}

			Console.WriteLine("TESTS: TOTAL: " + tests.Length + " CORRECT: " + res);
			Console.ReadLine();
		}

		public static bool Test_ReuseNetwork_Sameresult()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 2, 2, 1 }, new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, MLPNetwork.Identity);

			var result = cut.Calculate(new double[] { 0.5, 0.5 });
			var result2 = cut.Calculate(new double[] { 0.5, 0.5 });
			
			return Math.Abs(result[0] - result2[0]) < 0.001;
		}

		public static bool Test_MoreWeights_LinearActivation()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 2, 2, 1 }, new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, MLPNetwork.Identity);

			var result = cut.Calculate(new double[] { 0.5, 0.5 });

			return Math.Abs(result[0] - 2) < 0.001;
		}

		public static bool Test_MoreWeights()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 2, 2, 1 }, new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, null);

			var result = cut.Calculate(new double[] { 0.5, 0.5 });

			var expected = Math.Tanh(1.0) + Math.Tanh(1.0);
			return Math.Abs(result[0] - expected) < 0.001;
		}

		public static bool Test_Unwrap_Create_SameResult()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 1, 1 }, new double[] { 1.0 }, null);

			var result1 = cut.Calculate(new double[] { 0.5 });

			var weights = cut.ExtractWeights();

			var cut2 = MLPNetwork.CreateMLPNetwork(new int[] { 1, 1 }, weights, null);

			var result2 = cut2.Calculate(new double[] { 0.5 });

			return Math.Abs(result1[0] - result2[0]) < 0.001;
		}

		public static bool Test_Weights()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 1, 1 }, new double[] { 1.0 }, null);

			var result = cut.Calculate(new double[] { 0.5 });

			return Math.Abs(result[0] - 0.5) < 0.001;
		}
	}
}
