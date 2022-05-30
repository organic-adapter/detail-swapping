using System.Collections.Concurrent;
using WarGames.Contracts.Competitors;

namespace WarGames.Resources.Competitors
{
	public class InMemoryCompetitorRepository : IRepository<ICompetitor, string>
	{
		private readonly ConcurrentDictionary<string, ICompetitor> repo;

		public InMemoryCompetitorRepository(IEnumerable<ICompetitor> preLoad)
		{
			repo = new ConcurrentDictionary<string, ICompetitor>();
			foreach(var item in preLoad)
				repo.TryAdd(item.Name, item);
		}

		public InMemoryCompetitorRepository()
		{
			repo = new ConcurrentDictionary<string, ICompetitor>();
		}

		public void Delete(string id)
		{
			repo.TryRemove(id, out _);
		}

		public async Task DeleteAsync(string id)
		{
			await Task.Run(() => Delete(id));
		}

		public ICompetitor Get(string id)
		{
			repo.TryGetValue(id, out var returnMe);
			return returnMe ?? Competitor.Empty;
		}

		public IEnumerable<ICompetitor> GetAll()
		{
			return repo.Select(r => r.Value);
		}

		public async Task<IEnumerable<ICompetitor>> GetAllAsync()
		{
			return await Task.Run(() => GetAll());
		}

		public async Task<ICompetitor> GetAsync(string id)
		{
			return await Task.Run(() => Get(id));
		}

		public string Save(ICompetitor entity)
		{
			//TODO: normally we should put a mapper here. We are going to K.I.S.S. for now.
			var saveMe = entity;
			if (string.IsNullOrEmpty(entity.Id))
				saveMe = new Competitor(Guid.NewGuid().ToString(), entity.Name);

			if (!repo.TryAdd(saveMe.Id, saveMe))
				repo.TryUpdate(saveMe.Id, saveMe, repo[saveMe.Id]);

			return saveMe.Id;
		}

		public async Task<string> SaveAsync(ICompetitor entity)
		{
			return await Task.Run(() => Save(entity));
		}
	}
}