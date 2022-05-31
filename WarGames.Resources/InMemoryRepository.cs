using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts;

namespace WarGames.Resources
{
	public abstract class InMemoryRepository<TInt, TId> : IRepository<TInt, TId> 
		where TInt : IUnique<TId>
		where TId : notnull
	{
		protected readonly ConcurrentDictionary<TId, TInt> repo;

		public InMemoryRepository(IEnumerable<TInt> preLoad)
		{
			repo = new ConcurrentDictionary<TId, TInt>();
			foreach (var item in preLoad)
				repo.TryAdd(item.Id, item);
		}

		public InMemoryRepository()
		{
			repo = new ConcurrentDictionary<TId, TInt>();
		}

		public void Delete(TId id)
		{
			repo.TryRemove(id, out _);
		}

		public async Task DeleteAsync(TId id)
		{
			await Task.Run(() => Delete(id));
		}

		public TInt Get(TId id)
		{
			if(repo.TryGetValue(id, out var returnMe))
				return returnMe;
			throw new KeyNotFoundException();
		}

		public IEnumerable<TInt> GetAll()
		{
			return repo.Select(r => r.Value);
		}

		public async Task<IEnumerable<TInt>> GetAllAsync()
		{
			return await Task.Run(() => GetAll());
		}

		public async Task<TInt> GetAsync(TId id)
		{
			return await Task.Run(() => Get(id));
		}

		/// <summary>
		/// This will add new or update existing records.
		/// 
		/// In memory saves requires an actual type and 
		/// empty unique Id handling to properly save.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public abstract TId Save(TInt entity);

		public async Task<TId> SaveAsync(TInt entity)
		{
			return await Task.Run(() => Save(entity));
		}
	}
}
