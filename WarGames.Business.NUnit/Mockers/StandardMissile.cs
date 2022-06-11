namespace WarGames.Business.NUnit.Mockers
{
	internal class StandardMissile : TestMissile
	{
		public StandardMissile()
			: base(
					  Defaults.Missile.DAMAGE_RANGE
					  , Defaults.Missile.MIRV_COUNT
					  , Defaults.Missile.RANGE
					  , Defaults.Missile.SPEED
				  )
		{
		}
	}
}