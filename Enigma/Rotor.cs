using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class Rotor
	{
		public Rotor()
		{
			_initWires(WireMatrix.RandomWires());
			this.RotateAt = 'A';
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(IEnumerable<Char> wires)
		{
			_initWires(wires);
			this.RotateAt = 'A';
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(IEnumerable<Char> wires, Char rotateAt)
		{
			_initWires(wires);
			this.RotateAt = rotateAt;
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(IEnumerable<Char> wires, Char rotateAt, Char rotateAtSecondary)
		{
			_initWires(wires);
			this.RotateAt = rotateAt;
			this.RotateAtSecondary = rotateAtSecondary;
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		private void _initWires(IEnumerable<Char> wires)
		{
			this.WiresLeft = new WireMatrix(wires);
			this.WiresRight = this.WiresLeft.Invert();
		}

		public WireMatrix WiresLeft { get; private set; }
		public WireMatrix WiresRight { get; private set; }

		public Char InitialPosition { get; set; }

		public Char OffsetPosition { get; set; }

		public Char RotateAt { get; private set; }
		public Char? RotateAtSecondary { get; private set; }

		public void RotateToPosition(Char position)
		{
			if(!Char.IsLetter(position))
			{
				throw new EnigmaException("Invalid rotor position: {0}".Format(position));
			}

			var positionIndex = WireMatrix.ProjectCharacter(position);
			for(int currentIndex = 0; currentIndex < positionIndex; currentIndex++)
			{
				this.WiresLeft.Rotate();
				this.WiresRight = this.WiresLeft.Invert();
			}
		}

		public bool Rotate()
		{
			var shouldAdvance = this.OffsetPosition.Equals(this.RotateAt);
			if (this.RotateAtSecondary != null)
			{
				shouldAdvance = shouldAdvance || this.OffsetPosition.Equals(this.RotateAtSecondary.Value);
			}

			var offsetIndex = WireMatrix.ProjectCharacter(this.OffsetPosition);
			offsetIndex = offsetIndex + 1;
			if (offsetIndex >= 26)
			{
				offsetIndex = 0;
			}
			this.OffsetPosition = WireMatrix.ProjectIndex(offsetIndex);

			this.WiresLeft.Rotate();
			this.WiresRight = this.WiresLeft.Invert();

			return shouldAdvance;
        }

		public Char ProcessLeft(Char input)
		{
			var output = this.WiresLeft.Process(input);
			return output;
		}

		public Char ProcessRight(Char input)
		{
			var output = this.WiresRight.Process(input);			
			return output;
		}
	}
}
