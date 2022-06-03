namespace Map.Engine
{
	public struct Coord
	{
		public const double MAX_LAT_DEG = 90f;
		public const double MAX_LONG_DEG = 180f;

		public Coord(double lat, double lon)
		{
			Latitude = lat;
			Longitude = lon;
			LatCardinal = CalculateCardinalLat(lat);
			LongCardinal = CalculateCardinalLong(lon);
		}

		public double Latitude { get; }
		public CardinalLat LatCardinal { get; }
		public double Longitude { get; }
		public CardinalLong LongCardinal { get; init; }

		/// <summary>
		/// Derived from https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
		/// </summary>
		/// <param name="coord1"></param>
		/// <param name="coord2"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		public static double DistanceTo(Coord coord1, Coord coord2, char unit = 'K')
		{
			var lat1 = coord1.Latitude * (int)coord1.LatCardinal;
			var lat2 = coord2.Latitude * (int)coord2.LatCardinal;
			var lon1 = coord1.Longitude * (int)coord2.LongCardinal;
			var lon2 = coord2.Longitude * (int)coord2.LongCardinal;

			double rlat1 = Math.PI * lat1 / 180;
			double rlat2 = Math.PI * lat2 / 180;
			double theta = lon1 - lon2;
			double rtheta = Math.PI * theta / 180;
			double dist =
				Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
				Math.Cos(rlat2) * Math.Cos(rtheta);
			dist = Math.Acos(dist);
			dist = dist * 180 / Math.PI;
			dist = dist * 60 * 1.1515;

			switch (unit)
			{
				case 'K': //Kilometers -> default
					return dist * 1.609344;
				case 'N': //Nautical Miles 
					return dist * 0.8684;
				case 'M': //Miles
					return dist;
			}

			return dist;
		}

		public double DistanceKm(Coord end)
		{
			return DistanceTo(this, end);
		}

		private static CardinalLat CalculateCardinalLat(double lat)
		{
			return lat >= 0 ? CardinalLat.N : CardinalLat.S;
		}

		private static CardinalLong CalculateCardinalLong(double lon)
		{
			return lon >= 0 ? CardinalLong.W : CardinalLong.E;
		}
	}
}