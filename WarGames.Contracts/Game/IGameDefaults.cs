﻿using WarGames.Contracts.Arsenal;

namespace WarGames.Contracts.Game
{
	/// <summary>
	/// This one will be hard deleted once we are ready to replace.
	/// </summary>
	[Obsolete(ObsoleteConstants.Version2Incoming)]
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