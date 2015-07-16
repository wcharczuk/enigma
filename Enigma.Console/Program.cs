using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Console
{
	class Program
	{
		static Enigma CreateSampleEnigma()
		{
			var enigma = new Enigma();
			enigma.Reflector = Enigma.Reflector_A();
			enigma.Rotor_1 = Enigma.Rotor_III();
			enigma.Rotor_2 = Enigma.Rotor_I();
			enigma.Rotor_3 = Enigma.Rotor_V();

			enigma.Rotor_1.InitialPosition = 'F';
			enigma.Rotor_2.InitialPosition = 'T';
			enigma.Rotor_3.InitialPosition = 'W';

			//enigma.PlugBoard.AddPlug('f', 'q');
			//enigma.PlugBoard.AddPlug('t', 's');
			//enigma.PlugBoard.AddPlug('a', 'z');
			//enigma.PlugBoard.AddPlug('g', 'j');
			//enigma.PlugBoard.AddPlug('m', 'n');
			//enigma.PlugBoard.AddPlug('b', 'o');

			enigma.Initialize();
			return enigma;
		}

		static void Main(string[] args)
		{
			Break();
		}

		static void Test()
		{
			var enigma_a = CreateSampleEnigma();
			var enigma_b = CreateSampleEnigma();

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
			var enigma = CreateSampleEnigma();

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
			var plainText = "TISBUTATEST";

			var sampleEnigma = CreateSampleEnigma();

			var cipherTextList = new List<Char>();
			foreach(var c in plainText)
			{
				cipherTextList.Add(sampleEnigma.Input(c));
			}

			var cipherText = new String(cipherTextList.ToArray());

			bool solved = false;
			Enigma solution = null;
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			int processed = 0;
			var presets = _spanRotorPresets().ToArray();

			var consoleUpdater = System.Threading.Tasks.Task.Run(() =>
			{
				while (!solved)
				{
					System.Console.SetCursorPosition(0, 0);
					System.Console.WriteLine("Presets Tried: {0}".Format(processed));
					System.Threading.Thread.Sleep(500);
				}

				if (solution != null)
				{
					System.Console.WriteLine(String.Format("Found Solution In: {0} ms", stopWatch.ElapsedMilliseconds));
					System.Console.WriteLine(String.Format("Solution: {0}", solution.ToString()));
				}
			});

			System.Threading.Tasks.Parallel.ForEach(presets, new ParallelOptions() { MaxDegreeOfParallelism = 16 }, (preset) =>
			{
				foreach(var rotorPosition in _rotorStartingPositions())
				{
					if (solved) { return; }

					preset.Rotor_1.InitialPosition = rotorPosition.Item1;
					preset.Rotor_2.InitialPosition = rotorPosition.Item2;
					preset.Rotor_3.InitialPosition = rotorPosition.Item3;
					preset.Initialize();

					if (_check(preset, cipherText, plainText))
					{
						stopWatch.Stop();
						solved = true;
						solution = preset;
					}
					System.Threading.Interlocked.Increment(ref processed);
				}
			});

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
			return enigma;
		}

		static bool _check(Enigma machine, String cipherText, String expectedPlaintext)
		{
            int index = 0;
			var plainText = "";
			foreach(var letter in cipherText)
			{
				var output = machine.Input(letter);
				if (!output.Equals(expectedPlaintext[index]))
				{
					return false;
				}
				plainText = plainText + output;
				index = index + 1;
			}
			return true;
		}

		static bool _isCorrect(Enigma machine)
		{
			if (machine.Rotor_1.Id == "III" && machine.Rotor_2.Id == "I" && machine.Rotor_3.Id == "V")
			{
				if (machine.Rotor_1.InitialPosition == 'F' && machine.Rotor_2.InitialPosition == 'T' && machine.Rotor_3.InitialPosition == 'W')
				{
					return true;
				}
			}
			return false;
		}
	}
}
