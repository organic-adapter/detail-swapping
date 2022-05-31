using System.Collections.Concurrent;
using WarGames.Contracts.Competitors;

namespace WarGames.Resources.Competitors
{
	public class InMemoryCompetitorRepository : InMemoryRepository<ICompetitor, string>
	{

		public InMemoryCompetitorRepository(IEnumerable<ICompetitor> preLoad) : base(preLoad)
		{
		}

		public InMemoryCompetitorRepository()
		{
		}
		public override string Save(ICompetitor entity)
		{
			//TODO: normally we should put a mapper here. We are going to K.I.S.S. for now.
			var saveMe = entity;
			if (string.IsNullOrEmpty(entity.Id))
				saveMe = new Competitor(Guid.NewGuid().ToString(), entity.Name);

			if (!repo.TryAdd(saveMe.Id, saveMe))
				repo.TryUpdate(saveMe.Id, saveMe, repo[saveMe.Id]);

			return saveMe.Id;
		}
	}
}