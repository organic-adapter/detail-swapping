using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Business.Arsenal
{
	public class DamageCalculator : IDamageCalculator
	{
		private readonly Dictionary<Type, Func<int, TargetValue, TargetValue>> calculationMaps;

		public DamageCalculator()
		{
			calculationMaps = new Dictionary<Type, Func<int, TargetValue, TargetValue>>
			{
				{ typeof(CivilianPopulation), CivilianPopulationDamage },
				{ typeof(MilitaryPower), MilitaryPowerDamage },
				{ typeof(FoodOutput), FoodOutputDamage },
				{ typeof(IndustrialOutput), IndustrialOutputDamage }
			};
		}

		public async Task<World> CalculateAfterMathAsync(World world)
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };

			await Parallel.ForEachAsync
				(
					world.Settlements,
					options,
					async (settlement, token) => await CalculateDamage(settlement)
				);
			return world;
		}

		public async Task CalculateAfterMathAsync(IEnumerable<Contracts.V2.World.Settlement> settlements)
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };

			await Parallel.ForEachAsync
				(
					settlements,
					options,
					async (settlement, token) => await CalculateDamage(settlement)
				);
		}

		private async Task CalculateDamage(Contracts.V2.World.Settlement settlement)
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
			await Parallel.ForEachAsync
				(
					settlement.TargetValues,
					options,
					async (targetValue, token) =>
					{
						var damage = await CalculateDamage(settlement.Hits, targetValue);
						settlement.AftermathValues.Add(damage);
					}
				);
		}

		private async Task CalculateDamage(Settlement settlement)
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
			await Parallel.ForEachAsync
				(
					settlement.TargetValues,
					options,
					async (targetValue, token) =>
						{
							var damage = await CalculateDamage(settlement.Hits, targetValue);
							settlement.AftermathValues.Add(damage);
						}
				);
		}

		private async Task<TargetValue> CalculateDamage(int hits, TargetValue targetValue)
		{
			return await Task.Run(() => calculationMaps[targetValue.GetType()](hits, targetValue));
		}

		private TargetValue CivilianPopulationDamage(int hits, TargetValue civilianPopulation)
		{
			const float reductionPerHit = 0.75f;
			float remaining = (float)(Math.Pow(reductionPerHit, hits) * civilianPopulation.Value);
			return new CivilianPopulation(remaining);
		}

		private TargetValue FoodOutputDamage(int hits, TargetValue foodOutput)
		{
			const float reductionPerHit = 0.98f;
			float remaining = (float)(Math.Pow(reductionPerHit, hits) * foodOutput.Value);
			return new FoodOutput(remaining);
		}

		private TargetValue IndustrialOutputDamage(int hits, TargetValue industrialOutput)
		{
			const float reductionPerHit = 0.95f;
			float remaining = (float)(Math.Pow(reductionPerHit, hits) * industrialOutput.Value);
			return new IndustrialOutput(remaining);
		}

		private TargetValue MilitaryPowerDamage(int hits, TargetValue militaryPower)
		{
			const float reductionPerHit = 0.65f;
			float remaining = (float)(Math.Pow(reductionPerHit, hits) * militaryPower.Value);
			return new MilitaryPower(remaining);
		}
	}
}