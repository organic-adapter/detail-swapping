using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Bomber : BaseMissileDeliverySystem
	{
		public Bomber(short payloadCount, IMissile payloadType)
		{
			PayloadCount = payloadCount;
			PayloadType = payloadType;
			Assignment = Target.Empty;
		}

	}
}