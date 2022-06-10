using WarGames.Contracts;

namespace WarGames.Resources
{
	public interface IReadResource<T, TId>
		where T : IUnique<TId>
		where TId : notnull
	{
		public T Get(TId id);

		public IEnumerable<T> GetAll();

		public Task<IEnumerable<T>> GetAllAsync();

		public Task<T> GetAsync(TId id);
	}
}