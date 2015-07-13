using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class Reflector
	{
		public Reflector()
		{
			this.Wires = new WireMatrix();
		}

		public Reflector(IEnumerable<Char> wires)
		{
			this.Wires = new WireMatrix(wires);
		}

		public WireMatrix Wires { get; private set; }

		public Char Process(Char input)
		{
			var output = this.Wires.Process(input);
			return output;
		}
	}
}
