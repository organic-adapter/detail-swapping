using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts.Game;

namespace WarGames.Business.Planet
{
	/// <summary>
	/// A mapper to the rescue!
	/// We've gone as far as we can with basic casting. 
	/// Now we are stuck with a bug that can only be rectified using an interim mapper.
	/// </summary>
	public class ContractConversionMapperProfiles : Profile
	{
		public ContractConversionMapperProfiles()
		{
			CreateMap<Settlement, Contracts.V2.World.Settlement>();
			CreateMap<Country, Contracts.V2.World.Country>();
		}
	}
}
