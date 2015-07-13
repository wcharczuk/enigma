using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
	public class PlugBoardException : EnigmaException
	{
		public PlugBoardException() : base() { }
		public PlugBoardException(String message) : base(message) { }
	}

	public class PlugBoard
	{
		public PlugBoard()
		{
			Plugs = new Dictionary<char, char>();
		}

		public Dictionary<Char, Char> Plugs { get; private set; }

		/// <summary>
		/// Process the input character.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Char Process(Char input)
		{
			char output;
			if (Plugs.TryGetValue(input, out output))
			{
				return output;
			}
			return input;
		}

		/// <summary>
		/// Adds a plug mapping
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void AddPlug(Char from, Char to)
		{
			var workingFrom = from;
			if (char.IsLower(workingFrom))
			{
				workingFrom = Char.ToUpper(workingFrom);
			}

			var workingTo = to;
			if (char.IsLower(workingTo))
			{
				workingTo = Char.ToUpper(workingTo);
			}
			
			if (Plugs.ContainsKey(from))
			{
				var existingTo = this.Plugs[from];
				throw new PlugBoardException("Already mapped {0} <=> {1}".Format(from, existingTo));
			}
			if (Plugs.ContainsKey(to))
			{
				var existingFrom = this.Plugs[to];
				throw new PlugBoardException("Already mapped {0} <=> {1}".Format(existingFrom, to));
			}

			Plugs.Add(workingFrom, workingTo);
			Plugs.Add(workingTo, workingFrom);
		}

		/// <summary>
		/// Removes a plug mapping
		/// </summary>
		/// <param name="from"></param>
		public void RemovePlug(Char from)
		{
			if (Plugs.ContainsKey(from))
			{
				var to = Plugs[from];
				Plugs.Remove(from);
				Plugs.Remove(to);
			}
		}

		public override string ToString()
		{
			return String.Join(", ", this.Plugs.Select(_ => "{0}-{1}".Format(_.Key, _.Value)));
		}
	}
}
