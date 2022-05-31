using WarGames.Contracts;

namespace WarGames.Resources
{
	public interface IRepository<T, TId>
		where T : IUnique<TId>
		where TId : notnull
	{
		public void Delete(TId id);

		public Task DeleteAsync(TId id);

		public T Get(TId id);

		public IEnumerable<T> GetAll();

		public Task<IEnumerable<T>> GetAllAsync();

		public Task<T> GetAsync(TId id);

		public TId Save(T entity);

		public Task<TId> SaveAsync(T entity);
	}
}