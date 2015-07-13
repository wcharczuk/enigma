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
			Run();
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

			while (true)
			{
				var input = System.Console.ReadKey(intercept: true).KeyChar;
				if (Char.IsLetter(input))
				{
					System.Console.Write("{0}".Format(Char.ToUpper(input)));
					var output = enigma.Input(input);
					System.Console.WriteLine(" => {0} ".Format(output));
				}
				else
				{
					System.Console.WriteLine();
				}
			}
		}
	}
}
