namespace WarGames.Contracts.V2.Sides
{
	[Serializable]
	public class PlayerSide : IPlayerUnique, ISideUnique
	{
		public PlayerSide(string playerId, string sideId)
		{
			PlayerId = playerId;
			SideId = sideId;
		}

		public PlayerSide()
		{
			PlayerId = string.Empty;
			SideId = string.Empty;
		}

		public string PlayerId { get; set; }
		public string SideId { get; set; }
	}
}