using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.Competitors
{
	/// <summary>
	/// During contract changes this severe everyone tries to "do the least amount of work" possible.
	/// 
	/// How they interpret "least amount of work" is spend an iteration replacing all of the contracts doing direct replacements.
	/// Especially in the old manager contracts (such as game manager).
	/// 
	/// What we could actually do is break it into pieces and do those replacements slowly.
	/// </summary>
	[Obsolete("We are removing the entire concept of competitor in Version 2")]
	public interface ICompetitor : IUnique<string>
	{
		public List<Country> Countries { get; }
		public IEnumerable<Settlement> Settlements { get; }
		public List<IMissileDeliverySystem> MissileDeliverySystems { get; }
		public string Name { get; }

		public void Add(IEnumerable<IMissileDeliverySystem> missileDeliverySystems);

		public void Add(IMissileDeliverySystem missileDeliverySystem);
	}
}