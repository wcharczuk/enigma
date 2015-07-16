using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enigma.Test
{
	public class EnigmaTest
	{
		Enigma _create(Char rotor1, Char rotor2, Char rotor3, Char? rotor4 = null)
		{
			var machine = new Enigma();
			machine.Reflector = Enigma.Reflector_B();
			machine.Rotor_1 = Enigma.Rotor_III();
			machine.Rotor_2 = Enigma.Rotor_I();
			machine.Rotor_3 = Enigma.Rotor_IV();

			machine.Rotor_1.InitialPosition = rotor1;
			machine.Rotor_2.InitialPosition = rotor2;
			machine.Rotor_3.InitialPosition = rotor3;
			if (rotor4 != null)
			{
				machine.Rotor_4.InitialPosition = rotor4.Value;
			}
			machine.Initialize();
			return machine;
		}

		[Fact]
		public void Enigma_Configure()
		{
			var machineA = _create('F', 'T', 'W');
			Assert.Equal('F', machineA.Rotor_1.InitialPosition);
			Assert.Equal('F', machineA.Rotor_1.OffsetPosition);

			Assert.Equal('T', machineA.Rotor_2.InitialPosition);
			Assert.Equal('T', machineA.Rotor_2.OffsetPosition);

			Assert.Equal('W', machineA.Rotor_3.InitialPosition);
			Assert.Equal('W', machineA.Rotor_3.OffsetPosition);
		}

		[Fact]
		public void Enigma_InverseEqual()
		{
			var machineA = _create('F', 'T', 'W');
			var machineB = _create('F', 'T', 'W');

			var plainText = "TISBUTATEST";
			var cipherA = plainText.Select(_ => machineA.Input(_)).ToList();
			var plaintextB = cipherA.Select(_ => machineB.Input(_)).ToList();

			Assert.True(plainText.SequenceEqual(plaintextB));
		}

		[Fact]
		public void Enigma_RepeatsEqual()
		{
			var machineA = _create('F', 'T', 'W');
			var machineB = _create('F', 'T', 'W');

			var plainText = "TISBUTATEST";
			var cipherA = plainText.Select(_ => machineA.Input(_)).ToList();
			var cipherB = plainText.Select(_ => machineB.Input(_)).ToList();

			Assert.True(cipherA.SequenceEqual(cipherB));
		}

		[Fact]
		public void Enigma_StartingPositions()
		{
			var machineA = _create('F', 'T', 'W');
			var machineB = _create('A', 'A', 'A');

			var plainText = "TISBUTATEST";
			var cipherA = plainText.Select(_ => machineA.Input(_)).ToList();
			var cipherB = plainText.Select(_ => machineB.Input(_)).ToList();

			Assert.False(cipherA.SequenceEqual(cipherB));
		}
	}
}
