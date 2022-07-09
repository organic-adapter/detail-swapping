using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class DamageCalculatorTests
	{
		private const float startingFood = 19375f;
		private const float startingIndustry = 876321f;
		private const float startingMilitary = 13579f;
		private const float startingPopulation = 10003f;
		private IDamageCalculator damageCalculator;
		private Settlement settlement;

		#region Set Ups

		[SetUp]
		public void SetUp()
		{
			damageCalculator = new DamageCalculator();
			DefaultSettlement();
		}

		private void DefaultSettlement()
		{
			settlement = new Settlement();
			settlement.TargetValues.Add(new CivilianPopulation(startingPopulation));
			settlement.TargetValues.Add(new MilitaryPower(startingMilitary));
			settlement.TargetValues.Add(new IndustrialOutput(startingIndustry));
			settlement.TargetValues.Add(new FoodOutput(startingFood));
		}

		#endregion Set Ups

		[Test]
		public async Task NoHits_LeaveTargetValuesUnchanged()
		{
			var settlements = new List<Settlement>() { settlement };
			var afterMathWorld = await damageCalculator.CalculateAfterMathAsync(settlements);

			//TODO: This is an example of a bit more abstraction and we make aftermath values a class we can more easily grab various TargetValues
			//The class interface would look like AftermathValues.Get<CivilianPopulation>();
			var afterMathPopulation = settlement.AftermathValues.First(av => av.GetType() == typeof(CivilianPopulation)).Value;
			var afterMathMilitary = settlement.AftermathValues.First(av => av.GetType() == typeof(MilitaryPower)).Value;
			var afterMathIndustry = settlement.AftermathValues.First(av => av.GetType() == typeof(IndustrialOutput)).Value;
			var afterMathFood = settlement.AftermathValues.First(av => av.GetType() == typeof(FoodOutput)).Value;

			Assert.That(afterMathWorld, Is.Not.Null);
			Assert.That(afterMathPopulation, Is.EqualTo(startingPopulation));
			Assert.That(afterMathMilitary, Is.EqualTo(startingMilitary));
			Assert.That(afterMathIndustry, Is.EqualTo(startingIndustry));
			Assert.That(afterMathFood, Is.EqualTo(startingFood));
		}

		[Test]
		public async Task EachHit_ReducesTargetValues()
		{
			settlement.Hits++;
			var settlements = new List<Settlement>() { settlement };

			var afterMathWorld = await damageCalculator.CalculateAfterMathAsync(settlements);

			var afterMathPopulation = settlement.AftermathValues.First(av => av.GetType() == typeof(CivilianPopulation)).Value;
			var afterMathMilitary = settlement.AftermathValues.First(av => av.GetType() == typeof(MilitaryPower)).Value;
			var afterMathIndustry = settlement.AftermathValues.First(av => av.GetType() == typeof(IndustrialOutput)).Value;
			var afterMathFood = settlement.AftermathValues.First(av => av.GetType() == typeof(FoodOutput)).Value;

			Assert.That(afterMathWorld, Is.Not.Null);
			Assert.That(afterMathPopulation, Is.LessThan(startingPopulation));
			Assert.That(afterMathMilitary, Is.LessThan(startingMilitary));
			Assert.That(afterMathIndustry, Is.LessThan(startingIndustry));
			Assert.That(afterMathFood, Is.LessThan(startingFood));
		}		
	}
}