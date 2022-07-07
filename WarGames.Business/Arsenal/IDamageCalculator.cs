using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface IDamageCalculator
	{
		public Task<World> CalculateAfterMathAsync(World world);

		public Task CalculateAfterMathAsync(IEnumerable<Contracts.V2.World.Settlement> settlements);
	}
}