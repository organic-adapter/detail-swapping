namespace WarGames.Contracts.Game
{
	public static class Time
	{
		public const float HOURS_PER_MINUTE = 1 / MINUTES_PER_HOUR;
		public const float HOURS_PER_SECOND = 1 / SECONDS_PER_HOUR;
		public const float MINUTES_PER_HOUR = 60f;
		public const float MINUTES_PER_SECOND = 1 / SECONDS_PER_MINUTE;
		public const float SECONDS_PER_HOUR = SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
		public const float SECONDS_PER_MINUTE = 60f;
	}
}