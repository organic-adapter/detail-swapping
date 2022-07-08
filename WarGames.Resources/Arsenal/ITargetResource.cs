using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Arsenal
{
	/// <summary>
	/// Oh noes! Our experiment failed. Competitors knew about too much information and it leaked
	/// assumptions all over the place.
	/// 
	/// At this point in time we have to Eat the competitor removal to completion in order to fix 
	/// everything. This will make all of the other replacements go smoother.
	/// </summary>
	[Obsolete("At this point all of the targeting assumptions were derived from the Competitor object.")]
	public interface ITargetResource
	{
		public Task<Target> AddTargetAsync(Contracts.V2.World.Settlement settlement, TargetPriority targetPriority);

		public Task<IEnumerable<Target>> GetAllAsync();

		public Task<IEnumerable<Target>> GetAsync(Contracts.V2.Sides.Side side);

		public Task<Target> GetAsync(Contracts.V2.World.Settlement settlement);

		public Task<IEnumerable<Target>> GetAsync(Country country);

		[Obsolete("Not only does this one cheat (It's using the competitor object to find the settlements), Competitor is being removed as a concept.")]
		public Task<IEnumerable<Target>> GetAsync(ICompetitor competitor);
	}
}