using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Silo : BaseMissileDeliverySystem
	{
		public Silo(short payloadCount, IMissile payloadType)
		{
			PayloadCount = payloadCount;
			PayloadType = payloadType;
			Assignment = Target.Empty;
		}
	}
}