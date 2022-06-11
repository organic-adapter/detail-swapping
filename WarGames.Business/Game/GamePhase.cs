using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGames.Business.Game
{
	public enum GamePhase
	{
		PickPlayers = 0,
		PickTargets = 1,
		FinalizeAssignments = 2,
		EndOfWorld = 3,
	}
}
