using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Contracts.V2.Games
{
	public interface IGameDefaults
	{
		public ArsenalAssignment ArsenalAssignment { get; }

		public IEnumerable<string> ArsenalTags { get; }

		public CountryAssignment CountryAssignment { get; }

		public IEnumerable<string> CountryTags { get; }

		public void CalculateAiTargets(Func<IEnumerable<Settlement>> targets, Action<Settlement, TargetPriority> addAction);

		public bool MetRequirements();

		public void Trigger();
	}
}