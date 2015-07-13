using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Console
{
	class Program
	{
		static Enigma Create()
		{
			var enigma = new Enigma();
			enigma.Reflector = Enigma.Reflector_A();
			enigma.Rotor_1 = Enigma.Rotor_III();
			enigma.Rotor_2 = Enigma.Rotor_I();
			enigma.Rotor_3 = Enigma.Rotor_V();

			enigma.Rotor_1.InitialPosition = 'F';
			enigma.Rotor_2.InitialPosition = 'T';
			enigma.Rotor_3.InitialPosition = 'W';

			enigma.PlugBoard.AddPlug('f', 'q');
			enigma.PlugBoard.AddPlug('t', 's');
			enigma.PlugBoard.AddPlug('a', 'z');
			enigma.PlugBoard.AddPlug('g', 'j');
			enigma.PlugBoard.AddPlug('m', 'n');
			enigma.PlugBoard.AddPlug('b', 'o');

			enigma.InitializationCheck();
			return enigma;
		}
		static void Main(string[] args)
		{
			Break();
		}

		static void Test()
		{
			var enigma_a = Create();
			var enigma_b = Create();

			System.Console.WriteLine("Machine A:");
			var output_a = enigma_a.Input('A');
			System.Console.WriteLine();
			System.Console.WriteLine("Machine B:");
			var output_b = enigma_b.Input(output_a);

			System.Console.WriteLine();
			System.Console.WriteLine("A => {0}".Format(output_a));
			System.Console.WriteLine("{0} => {1}".Format(output_a, output_b));

			System.Console.ReadKey();
		}

		static void Run()
		{
			var enigma = Create();

			var row = 0;
			while (true)
			{
				var input = System.Console.ReadKey(intercept: true).KeyChar;
				if (Char.IsLetter(input))
				{
					var left = System.Console.CursorLeft;
					var top = System.Console.CursorTop;
					System.Console.Write("{0}".Format(Char.ToUpper(input)));
					var output = enigma.Input(input);
					System.Console.SetCursorPosition(left, top + 1);
					System.Console.WriteLine("{0} ".Format(output));
					System.Console.SetCursorPosition(left + 1, top);
				}
			}
		}

		static void Break()
		{
			var plain_text = "TISBUTATEST";
			var cipher_text = "JCYCTXLUNBM";

			var presets = _spanRotorPresets().ToArray();
            foreach (var machine in presets)
			{
				System.Console.WriteLine(machine.ToString());
			}

			System.Console.ReadKey();
        }

		static IEnumerable<Tuple<Char, Char, Char>> _rotorStartingPositions()
		{
			for(int a = 0; a < 26; a++)
			{
				for(int b = 0; b < 26; b++)
				{
					for(int c = 0; c < 26; c++)
					{
						yield return Tuple.Create(WireMatrix.ProjectIndex(a), WireMatrix.ProjectIndex(b), WireMatrix.ProjectIndex(c));
					}
				}
			}
		}

		static List<Enigma> _spanRotorPresets()
		{
			var rotors = new List<Func<Rotor>>()
			{
				() => { return Enigma.Rotor_I(); },
				() => { return Enigma.Rotor_II(); },
				() => { return Enigma.Rotor_III(); },
				() => { return Enigma.Rotor_IV(); },
				() => { return Enigma.Rotor_V(); }
			};
			var reflectors = new List<Func<Reflector>>()
			{
				() => { return Enigma.Reflector_A(); },
				() => { return Enigma.Reflector_B(); },
				() => { return Enigma.Reflector_C(); },
			};

			var output = new List<Enigma>();

			foreach (var combo in _rotorPermutations(rotors))
			{
				foreach (var reflectorFn in reflectors)
				{
					output.Add(_createMachine(combo.Item1(), combo.Item2(), combo.Item3(), reflectorFn()));
				}
			}

			return output;
		}

		static List<Tuple<Func<Rotor>, Func<Rotor>, Func<Rotor>>> _rotorPermutations(List<Func<Rotor>> allPossible)
		{
			var output = new List<Tuple<Func<Rotor>, Func<Rotor>, Func<Rotor>>>();
			for(int a = 0; a < allPossible.Count; a++)
			{
				for(int b = 0; b < allPossible.Count; b++)
				{
					if (b != a)
					{
						for (int c = 0; c < allPossible.Count; c++)
						{
							if (c != a && c != b)
							{
								output.Add(Tuple.Create(allPossible[a], allPossible[b], allPossible[c]));
							}
						}
					}
				}
			}
			return output;
		}

		static Enigma _createMachine(Rotor rotor1, Rotor rotor2, Rotor rotor3, Reflector reflector)
		{
			var enigma = new Enigma();
			enigma.Rotor_1 = rotor1;
			enigma.Rotor_2 = rotor2;
			enigma.Rotor_3 = rotor3;
			enigma.Reflector = reflector;
			enigma.InitializationCheck();
			return enigma;
		}

		static bool _check(Enigma machine, String cipherText, String expectedPlaintext)
		{
			int index = 0;
			foreach(var letter in cipherText)
			{
				var output = machine.Input(letter);
				if (!output.Equals(expectedPlaintext[index]))
				{
					return false;
				}
				index = index + 1;
			}
			return true;
		}
	}
}
