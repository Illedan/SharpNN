using System;
namespace SharpNN.Tests
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			var tests = new Func<bool>[]{ Test_Weights };
			var res = 0;
			foreach (var test in tests)
			{
				res += test() ? 1 : 0;
			}

			Console.WriteLine("TESTS: TOTAL: " + tests.Length + " CORRECT: " + res);
			Console.ReadLine();
		}


		public static bool Test_Weights()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 1, 1 }, new double[] { 1.0 }, null);

			var result = cut.Calculate(new double[] { 0.5 });

			return Math.Abs(result[0]-0.5) < 0.001;
		}
	}
}
