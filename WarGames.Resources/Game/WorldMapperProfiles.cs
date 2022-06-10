using AutoMapper;
using Map.Engine;
using SimpleMap.Contracts;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Resources.Game
{
	public class WorldMapperProfiles : Profile
	{
		public WorldMapperProfiles()
		{
			CreateMap<SimpleMapEntry, Country>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.TerrainType, opt => opt.MapFrom(src => TerrainType.Land));

			CreateMap<SimpleMapEntry, Settlement>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.city))
				.ForMember(dest => dest.TargetValues, opt => opt.MapFrom(src => new List<TargetValue>() { new CivilianPopulation(src.population) }))
				.ForMember(dest => dest.Location, opt => opt.MapFrom<LocationResolver>());
		}
	}

	public class LocationResolver : IValueResolver<SimpleMapEntry, Settlement, ILocation>
	{
		private readonly IReadResource<Country, string> countryResource;

		public LocationResolver(IReadResource<Country, string> countryResource)
		{
			this.countryResource = countryResource;
		}

		public ILocation Resolve(SimpleMapEntry source, Settlement destination, ILocation destMember, ResolutionContext context)
		{
			return new Location(countryResource.Get(source.country), new Coord(source.lat, source.lng));
		}
	}
}