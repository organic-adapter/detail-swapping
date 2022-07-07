﻿using WarGames.Contracts.V2;

namespace WarGames.Business.Game
{
	public class CurrentGame
	{
		private GameSession gameSession;

		public CurrentGame()
		{
			gameSession = GameSession.NotLoaded;
		}

		public GameSession GameSession
		{
			get { return gameSession; }
			set { gameSession = value; }
		}

		public bool IsNew => gameSession.IsNew;
		public bool NotLoaded => gameSession == GameSession.NotLoaded;
		public class GameSessionNotLoadedException : Exception
		{
		}
	}
}