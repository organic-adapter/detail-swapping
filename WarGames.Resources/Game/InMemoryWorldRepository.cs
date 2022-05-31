using WarGames.Contracts.Game;

namespace WarGames.Resources.Game
{
	public class InMemoryWorldRepository : InMemoryRepository<World, Guid>
	{
		public InMemoryWorldRepository(World preLoad)
		{
			Save(preLoad);
		}
		public InMemoryWorldRepository()
		{
		}
		public override Guid Save(World entity)
		{
			//TODO: normally we should put a mapper here. We are going to K.I.S.S. for now.
			var saveMe = entity;
			if (entity.Id == Guid.Empty)
				saveMe = new World() { Id = Guid.NewGuid(), Countries = entity.Countries };

			if (!repo.TryAdd(saveMe.Id, saveMe))
				repo.TryUpdate(saveMe.Id, saveMe, repo[saveMe.Id]);

			return saveMe.Id;
		}
	}
}