using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Submarine : BaseMissileDeliverySystem
	{
		public Submarine(short payloadCount, IMissile payloadType)
		{
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}
	}
}