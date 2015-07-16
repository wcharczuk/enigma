using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class Rotor
	{
		public Rotor(String id)
		{
			this.Id = id;
			_initWires(WireMatrix.RandomWires());
			this.RotateAt = 'A';
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(String id, IEnumerable<Char> wires)
		{
			this.Id = id;
			_initWires(wires);
			this.RotateAt = 'A';
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(String id, IEnumerable<Char> wires, Char rotateAt)
		{
			this.Id = id;
			_initWires(wires);
			this.RotateAt = rotateAt;
			this.InitialPosition = 'A';
			this.OffsetPosition = 'A';
		}

		public Rotor(String id, IEnumerable<Char> wires, Char rotateAt, Char rotateAtSecondary)
		{
			this.Id = id;
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

		public String Id { get; private set; }

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
			var offsetIndex = WireMatrix.ProjectCharacter(this.OffsetPosition);
			var delta = positionIndex - offsetIndex;
			if (delta < 0)
			{
				delta = 26 + delta;
			}

			for(int currentIndex = 0; currentIndex < delta; currentIndex++)
			{
				this.WiresLeft.Rotate();
				this.WiresRight = this.WiresLeft.Invert();

				offsetIndex = offsetIndex + 1;
				if (offsetIndex >= 26)
				{
					offsetIndex = 0;
				}

				this.OffsetPosition = WireMatrix.ProjectIndex(offsetIndex);
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

		public override string ToString()
		{
			return String.Format("Rotor Id: {0} Initial: {1} Offset: {2}", this.Id, this.InitialPosition, this.OffsetPosition);
		}
	}
}
