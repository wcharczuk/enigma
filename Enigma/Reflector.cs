using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class Reflector
	{
		public Reflector(String id)
		{
			this.Id = id;
			this.Wires = new WireMatrix();
		}

		public Reflector(String id, IEnumerable<Char> wires)
		{
			this.Id = id;
			this.Wires = new WireMatrix(wires);
		}

		public String Id { get; private set; }
		public WireMatrix Wires { get; private set; }

		public Char Process(Char input)
		{
			var output = this.Wires.Process(input);
			return output;
		}

		public override string ToString()
		{
			return String.Format("Reflector {0}", this.Id);
		}
	}
}
