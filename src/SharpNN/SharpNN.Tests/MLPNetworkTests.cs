using System;
using Xunit;
namespace SharpNN.Tests
{
	public class MLPNetworkTests
	{
		[Fact]
		public void VerifyNetwork()
		{
			var cut = MLPNetwork.CreateMLPNetwork(new int[] { 1, 1 }, new double[] { 1.0 }, null);

			var result = cut.Calculate(new double[] { 0.35 });

			Assert.Equal(0.35, result[0]);
		}
	}
}
