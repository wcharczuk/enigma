using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enigma.Test
{
    public class RotorTest
    {
		[Fact]
		public void Rotor_Process()
		{
			var rotor = Enigma.Rotor_I();
			var value = rotor.ProcessLeft('A');
			Assert.Equal('E', value);
		}

		[Fact]
		public void Rotor_Rotate()
		{
			var rotor = Enigma.Rotor_I();
			rotor.Rotate();
		}

		[Fact]
		public void Rotor_Rotate_WithValueCheck()
		{
			var rotor = Enigma.Rotor_I();
			rotor.Rotate();
			var value = rotor.ProcessLeft('A');
			var right_value = rotor.ProcessRight(value);
			Assert.Equal('K', value);
			Assert.Equal('A', right_value);

			var ref_check = Enigma.Rotor_I();
			ref_check.Rotate();
			var ref_check_value = ref_check.ProcessLeft('A');
			var ref_check_right_value = ref_check.ProcessRight(ref_check_value);
			Assert.Equal('K', ref_check_value);
			Assert.Equal('A', right_value);
		}

		[Fact]
		public void Rotor_Reflexive()
		{
			var rotor = Enigma.Rotor_I();
			var result_a = rotor.ProcessLeft('A');
			Assert.Equal('E', result_a);
			var right_result_a = rotor.ProcessRight(result_a);
			Assert.Equal('A', right_result_a);
		}

		[Fact]
		public void Rotor_ChainReflexive()
		{
			var rotor_1 = Enigma.Rotor_I();
			var rotor_2 = Enigma.Rotor_II();
			var result_1 = rotor_1.ProcessLeft('A');
			var result_2 = rotor_2.ProcessLeft(result_1);

			var return_result_2 = rotor_2.ProcessRight(result_2);
			var return_result_1 = rotor_1.ProcessRight(return_result_2);

			Assert.Equal('A', return_result_1);
		}

		[Fact]
		public void Rotor_ChainReflected()
		{
			var rotor_1 = Enigma.Rotor_I();
			var rotor_2 = Enigma.Rotor_II();

			var reflector = Enigma.Reflector_B();

			var result_1 = rotor_1.ProcessLeft('A');
			var result_2 = rotor_2.ProcessLeft(result_1);

			var reflected = reflector.Process(result_2);

			var return_result_2 = rotor_2.ProcessRight(reflected);
			var return_result_1 = rotor_1.ProcessRight(return_result_2);

			Assert.NotEqual('A', return_result_1);

			var verify_result_1 = rotor_1.ProcessLeft(return_result_1);
			var verify_result_2 = rotor_2.ProcessLeft(verify_result_1);

			var verify_reflected = reflector.Process(verify_result_2);

			var verify_return_result_2 = rotor_2.ProcessRight(verify_reflected);
			var verify_return_result_1 = rotor_1.ProcessRight(verify_return_result_2);

			Assert.Equal('A', verify_return_result_1);
		}


		[Fact]
		public void Rotor_ChainReflectedRotated()
		{
			var rotor_1_a = Enigma.Rotor_I();
			var rotor_2_a = Enigma.Rotor_II();
			var reflector_a = Enigma.Reflector_B();

			var rotor_1_b = Enigma.Rotor_I();
			var rotor_2_b = Enigma.Rotor_II();
			var reflector_b = Enigma.Reflector_B();
			
			rotor_1_a.Rotate();
			var result_1 = rotor_1_a.ProcessLeft('A');
			var result_2 = rotor_2_a.ProcessLeft(result_1);

			var reflected = reflector_a.Process(result_2);

			var return_result_2 = rotor_2_a.ProcessRight(reflected);
			var return_result_1 = rotor_1_a.ProcessRight(return_result_2);

			Assert.NotEqual('A', return_result_1);

			rotor_1_b.Rotate();
			var verify_result_1 = rotor_1_b.ProcessLeft(return_result_1);
			var verify_result_2 = rotor_2_b.ProcessLeft(verify_result_1);

			var verify_reflected = reflector_b.Process(verify_result_2);

			var verify_return_result_2 = rotor_2_b.ProcessRight(verify_reflected);
			var verify_return_result_1 = rotor_1_b.ProcessRight(verify_return_result_2);

			Assert.Equal('A', verify_return_result_1);
		}
	}
}
