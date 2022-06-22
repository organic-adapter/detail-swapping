using WarGames.Contracts;

namespace WarGames.Resources
{
	public class InMemoryReadResource<T, TId> : IReadResource<T, TId>
		where T : IUnique<TId>
		where TId : notnull
	{
		private readonly Dictionary<TId, T> repo;

		public InMemoryReadResource()
		{
			repo = new Dictionary<TId, T>();
		}

		public T Get(TId id)
		{
			return repo[id];
		}

		public IEnumerable<T> GetAll()
		{
			return repo.Values;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await Task.Run(() => GetAll());
		}

		public async Task<T> GetAsync(TId id)
		{
			return await Task.Run(() => Get(id));
		}
	}
}