namespace WarGames.Contracts.V2.Sides
{
	[Serializable]
	public class PlayerSide
	{
		public PlayerSide()
		{
			PlayerId = string.Empty;
			SideId = string.Empty;
		}

		public string PlayerId { get; set; }
		public string SideId { get; set; }
	}
}