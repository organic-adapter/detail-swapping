using WarGames.Contracts.V2.World;

namespace WarGames.Business.Arsenal
{
	public interface IDamageCalculator
	{
		public Task<IEnumerable<Settlement>> CalculateAfterMathAsync(IEnumerable<Settlement> settlements);
	}
}