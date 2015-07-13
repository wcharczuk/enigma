using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public static class StringExtensions
	{
		public static String Format(this String theStr, params object[] args)
		{
			return String.Format(theStr, args);
		}
	}
}
