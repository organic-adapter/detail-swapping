using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	public interface IGameDefaults
	{
		public void CalculateAiTargets(Func<IEnumerable<Settlement>> targets, Action<Settlement, TargetPriority> addAction);

		public ArsenalAssignment ArsenalAssignment { get; }

		public IEnumerable<string> ArsenalTags { get; }

		public CountryAssignment CountryAssignment { get; }

		public IEnumerable<string> CountryTags { get; }

		public bool MetRequirements();

		public void Trigger();
	}
}