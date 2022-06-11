using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface IDamageCalculator
	{
		public Task<World> CalculateAfterMathAsync(World world);
	}
}