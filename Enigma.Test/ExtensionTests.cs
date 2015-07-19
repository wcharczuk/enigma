using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enigma.Test
{
	public class ExtensionTests
	{
		[Fact]
		public void Extension_Combinations()
		{
			var elems = new List<int>() { 1, 2, 3, 4, 5 };
			var combinations = elems.Combinations(2);
			Assert.Equal(10, combinations.Count());
		}
	}
}
