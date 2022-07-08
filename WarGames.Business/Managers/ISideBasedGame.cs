using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Managers
{
	public interface ISideBasedGame
	{
		public IEnumerable<(Player, Side)> GetActivePlayers();
	}
}
