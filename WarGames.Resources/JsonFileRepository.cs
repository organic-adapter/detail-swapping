using Microsoft.Extensions.Options;
using System.Text.Json;
using WarGames.Contracts;
using WarGames.Resources.Exceptions;

namespace WarGames.Resources
{
	public abstract class JsonFileRepository<TInt, TId> : IRepository<TInt, TId>
		where TInt : IUnique<TId>
		where TId : notnull
	{
		private readonly IOptionsMonitor<JsonFileConfiguration<TInt, TId>> options;

		public JsonFileRepository(IOptionsMonitor<JsonFileConfiguration<TInt, TId>> options)
		{
			this.options = options;
		}

		/// <summary>
		/// Deletes a record from a JSON document.
		///
		/// This grabs the entire document in order to find the specific record and then
		/// persists the entire document.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(TId id)
		{
			var list = GetAll().ToList();
			var removeMe = list.FirstOrDefault(item => item.Id.Equals(id));
			if (removeMe == null)
				return;

			list.Remove(removeMe);
		}

		public async Task DeleteAsync(TId id)
		{
			await Task.Run(() => Delete(id));
		}

		/// <summary>
		/// Grabs the entire document to search for the record.
		/// 
		/// Throws an exception if the record cannot be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"></exception>
		public TInt Get(TId id)
		{
			var list = GetAll();
			var found = list.First(item => item.Id.Equals(id));
			if (found == null)
				throw new KeyNotFoundException(id.ToString());

			return found;
		}

		public IEnumerable<TInt> GetAll()
		{
			var json = File.ReadAllText(options.CurrentValue.RootPath);
			var list = JsonSerializer.Deserialize<TInt[]>(json);
			if (list == null)
				throw new EmptyRepositoryException();

			return list;
		}

		public async Task<IEnumerable<TInt>> GetAllAsync()
		{
			var json = await File.ReadAllTextAsync(options.CurrentValue.RootPath);
			var list = JsonSerializer.Deserialize<TInt[]>(json);
			if (list == null)
				throw new EmptyRepositoryException();

			return list;
		}

		public async Task<TInt> GetAsync(TId id)
		{
			var list = await GetAllAsync();
			var found = list.First(item => item.Id.Equals(id));
			if (found == null)
				throw new KeyNotFoundException(id.ToString());

			return found;
		}

		public virtual TId Save(TInt entity)
		{
			var list = GetAll();
			SaveFile(list.Concat(new [] { entity }));
		
			return entity.Id;
		}

		public virtual async Task<TId> SaveAsync(TInt entity)
		{
			var list = await GetAllAsync();
			await SaveFileAsync(list.Concat(new [] { entity }));
			
			return entity.Id;
		}

		private void SaveFile(IEnumerable<TInt> saveUs)
		{
			var json = JsonSerializer.Serialize(saveUs);
			File.WriteAllText(options.CurrentValue.RootPath, json);
		}

		private async Task SaveFileAsync(IEnumerable<TInt> saveUs)
		{
			var json = JsonSerializer.Serialize(saveUs);
			await File.WriteAllTextAsync(options.CurrentValue.RootPath, json);
		}
	}
}