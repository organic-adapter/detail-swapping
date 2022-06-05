using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.Arsenal
{
	public class Target
	{
		public Target(Settlement key, TargetPriority priority)
		{
			Key = key;
			Assignments = new List<IMissileDeliverySystem>();
			Priority = priority;
		}

		public List<IMissileDeliverySystem> Assignments { get; }
		public Settlement Key { get; }
		public TargetPriority Priority { get; }
		public void Assign(IMissileDeliverySystem missileDeliverySystem)
		{
			Assignments.Add(missileDeliverySystem);
		}
	}
}
