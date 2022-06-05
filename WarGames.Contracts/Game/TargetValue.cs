using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGames.Contracts.Game
{
	public class TargetValue
	{
		public TargetValue(string name, float value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }
		public float Value { get; set; }
	}
}
