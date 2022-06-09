using AutoMapper;
using Map.Engine;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Resources.Game
{
	public class WorldMapperProfiles : Profile
	{
		private readonly IRepository<Country, string> countryRepository;

		public WorldMapperProfiles(IRepository<Country, string> countryRepository)
		{
			this.countryRepository = countryRepository;
			CreateMap<SimpleMapEntry, Country>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.TerrainType, opt => opt.MapFrom(src => TerrainType.Land));

			CreateMap<SimpleMapEntry, Settlement>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.city))
				.ForMember(dest => dest.TargetValues, opt => opt.MapFrom(src => new List<TargetValue>() { new CivilianPopulation(int.Parse(src.population)) }))
				.ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location(countryRepository.Get(src.country), new Coord(double.Parse(src.lat), double.Parse(src.lng)))));
		}
	}
}