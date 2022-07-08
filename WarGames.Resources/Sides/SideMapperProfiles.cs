using AutoMapper;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public class SideMapperProfiles : Profile
	{
		public SideMapperProfiles()
		{
			CreateMap<Side, ISideUnique>()
				.ForMember(dest => dest.SideId, opt => opt.MapFrom(src => src.Id));

			CreateMap<KeyValuePair<Player, Side>, PlayerSide>()
				.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Key.Id))
				.ForMember(dest => dest.SideId, opt => opt.MapFrom(src => src.Value.Id));
		}
	}
}