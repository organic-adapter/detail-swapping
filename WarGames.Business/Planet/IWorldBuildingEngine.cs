using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts.V2;

namespace WarGames.Business.Planet
{
	public interface IWorldBuildingEngine
	{
		public Task BuildAsync(GameSession gameSession);
	}
}
