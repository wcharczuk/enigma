﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class EnigmaException : System.Exception
	{
		public EnigmaException() : base() { }
		public EnigmaException(String message) : base(message) { }
	}

    public class Enigma
    {
		public static Rotor Rotor_I()
		{
			return new Rotor("I", "EKMFLGDQVZNTOWYHXUSPAIBRCJ", rotateAt: 'Q');
		}

		public static Rotor Rotor_II()
		{
			return new Rotor("II", "AJDKSIRUXBLHWTMCQGZNPYFVOE", rotateAt: 'E');
		}

		public static Rotor Rotor_III()
		{
			return new Rotor("III", "BDFHJLCPRTXVZNYEIWGAKMUSQO", rotateAt: 'V');
		}

		public static Rotor Rotor_IV()
		{
			return new Rotor("IV", "ESOVPZJAYQUIRHXLNFTGKDCMWB", rotateAt: 'J');
		}

		public static Rotor Rotor_V()
		{
			return new Rotor("V", "VZBRGITYUPSDNHLXAWMJQOFECK", rotateAt: 'Z');
		}

		public static Rotor Rotor_VI()
		{
			return new Rotor("VI", "JPGVOUMFYQBENHZRDKASXLICTW", rotateAt: 'Z', rotateAtSecondary: 'M');
		}

		public static Rotor Rotor_VII()
		{
			return new Rotor("VII", "NZJHGRCXMYSWBOUFAIVLPEKQDT", rotateAt: 'Z', rotateAtSecondary: 'M');
		}

		public static Rotor Rotor_VIII()
		{
			return new Rotor("VIII", "FKQHTLXOCBJSPDZRAMEWNIUYGV", rotateAt: 'Z', rotateAtSecondary: 'M');
		}

		public static Reflector Reflector_A()
		{
			return new Reflector("A", "EJMZALYXVBWFCRQUONTSPIKHGD");
		}

		public static Reflector Reflector_B()
		{
			return new Reflector("B", "YRUHQSLDPXNGOKMIEBFZCWVJAT");
		}

		public static Reflector Reflector_C()
		{
			return new Reflector("C", "FVPJIAOYEDRZXWGCTKUQSBNMHL");
		}

		public Enigma()
		{
			this.PlugBoard = new PlugBoard();
		}

		public void Initialize()
		{
			if (Reflector == null)
			{
				throw new EnigmaException("Reflector not installed!");
			}

			if (Rotor_1 == null)
			{
				throw new EnigmaException("Rotor 1 not installed!");
			}

			if (Rotor_2 == null)
			{
				throw new EnigmaException("Rotor 2 not installed!");
			}

			if (Rotor_3 == null)
			{
				throw new EnigmaException("Rotor 3 not installed!");
			}

			this.Rotor_1.RotateToPosition(this.Rotor_1.InitialPosition);
			this.Rotor_2.RotateToPosition(this.Rotor_2.InitialPosition);
			this.Rotor_3.RotateToPosition(this.Rotor_3.InitialPosition);
			if (this.Rotor_4 != null)
			{
				this.Rotor_4.RotateToPosition(this.Rotor_4.InitialPosition);
			}
		}

		/// <summary>
		/// Reflector
		/// </summary>
		public Reflector Reflector { get; set; }

		/// <summary>
		/// Rotor 1
		/// </summary>
		public Rotor Rotor_1 { get; set; }

		/// <summary>
		/// Rotor 2
		/// </summary>
		public Rotor Rotor_2 { get; set; }

		/// <summary>
		/// Rotor 3
		/// </summary>
		public Rotor Rotor_3 { get; set; }

		/// <summary>
		/// Rotor 4 (Optional)
		/// </summary>
		public Rotor Rotor_4 { get; set; }

		/// <summary>
		/// Plug Board
		/// </summary>
		public PlugBoard PlugBoard { get; set; }

		/// <summary>
		/// Process a key press.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Char Input(Char input)
		{
			var working = input;

			if (!Char.IsLetter(input))
			{
				throw new EnigmaException("Invalid character: {0}".Format(input));
			}

			if (Char.IsLower(input))
			{
				working = Char.ToUpper(input);
			}

			working = PlugBoard.Process(working);
			
			var should_rotate_next = Rotor_1.Rotate();
			working = Rotor_1.ProcessLeft(working);

			if (should_rotate_next)
			{
				should_rotate_next = Rotor_2.Rotate();
			}
			working = Rotor_2.ProcessLeft(working);

			if (should_rotate_next)
			{
				should_rotate_next = Rotor_3.Rotate();
			}
			working = Rotor_3.ProcessLeft(working);

			if (Rotor_4 != null)
			{
				if (should_rotate_next)
				{
					Rotor_4.Rotate();
				}
				working = Rotor_4.ProcessLeft(working);
			}

			working = Reflector.Process(working);

			if (Rotor_4 != null)
			{
				working = Rotor_4.ProcessRight(working);
			}

			working = Rotor_3.ProcessRight(working);
			working = Rotor_2.ProcessRight(working);
			working = Rotor_1.ProcessRight(working);

			working = PlugBoard.Process(working);

			return working;
		}

		public override string ToString()
		{
			if (Rotor_4 != null)
			{
				return String.Format("{0} {1} {2} {3} {4} Plugs: {5}", Reflector.ToString(), Rotor_1.ToString(), Rotor_2.ToString(), Rotor_3.ToString(), Rotor_4.ToString(), PlugBoard.ToString());
			}
			return String.Format("{0} {1} {2} {3} Plugs: {4}", Reflector.ToString(), Rotor_1.ToString(), Rotor_2.ToString(), Rotor_3.ToString(), PlugBoard.ToString());
		}
	}
}
