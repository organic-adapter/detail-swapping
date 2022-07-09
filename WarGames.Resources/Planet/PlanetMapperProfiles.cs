using AutoMapper;
using Map.Engine;
using SimpleMap.Contracts;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Resources.Planet
{
	public class PlanetMapperProfiles : Profile
	{
		public PlanetMapperProfiles()
		{
			CreateMap<SimpleMapEntry, Contracts.V2.World.Country>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.country));

			CreateMap<SimpleMapEntry, Contracts.V2.World.Settlement>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
				.ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.country))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.city))
				.ForMember(dest => dest.TargetValues, opt => opt.MapFrom(src => new List<TargetValue>() { new CivilianPopulation(src.population) }))
				.ForMember(dest => dest.Coord, opt => opt.MapFrom(src => new Coord(src.lat, src.lng)));
		}
	}
}