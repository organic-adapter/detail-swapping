
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class TransportErectorLauncher : BaseMissileDeliverySystem
	{
		public TransportErectorLauncher(short payloadCount, IMissile payloadType)
		{
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}
	}
}