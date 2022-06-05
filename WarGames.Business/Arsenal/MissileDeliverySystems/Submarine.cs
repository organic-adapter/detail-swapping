﻿using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Submarine : BaseMissileDeliverySystem
	{
		public Submarine(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeedKps = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public override TerrainType MovementConstraint => TerrainType.Ocean;
	}
}