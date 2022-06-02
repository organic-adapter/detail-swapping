namespace Map.Engine
{
	[Obsolete("I built this thinking I could get a nice database full of angular coordinates. Looks like it'll be easier to get the double lat/long instead. Keep for now.")]
	public struct AngularCoord
	{
		public AngularCoord(double degrees, double minutes, double seconds)
		{
			Degrees = degrees;
			Minutes = minutes;
			Seconds = seconds;
		}

		public AngularCoord(double coord)
		{
			Degrees = Math.Round(coord, 0);
			Minutes = Math.Round((coord - Degrees) * 60f, 0);
			Seconds = Math.Round((coord - Degrees - Minutes / 60f) * 3600f, 0);
		}

		public double Degrees { get; init; }
		public double Minutes { get; init; }
		public double Seconds { get; init; }

		public double ToDouble()
		{
			return (double)(Degrees + Minutes / 60f + Seconds / 3600f);
		}
	}

	public struct Coord
	{
		public const double MAX_LAT_DEG = 90f;
		public const double MAX_LONG_DEG = 180f;

		public Coord(AngularCoord latAng, CardinalLat latCard, AngularCoord lonAng, CardinalLong lonCard)
		{
			Latitude = latAng.ToDouble();
			Longitude = lonAng.ToDouble();
			LatAng = latAng;
			LongAng = lonAng;
			LatCardinal = latCard;
			LongCardinal = lonCard;
		}

		public Coord(double lat, double lon)
		{
			Latitude = lat;
			Longitude = lon;
			LatAng = new AngularCoord(lat);
			LongAng = new AngularCoord(lon);
			LatCardinal = CalculateCardinalLat(lat);
			LongCardinal = CalculateCardinalLong(lon);
		}

		public double Latitude { get; }
		public AngularCoord LatAng { get; }
		public CardinalLat LatCardinal { get; }
		public double Longitude { get; }
		public AngularCoord LongAng { get; init; }
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