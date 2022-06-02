namespace Map.Engine.NUnit.CoordTests
{
	[TestFixture]
	public class CoordTests
	{
		/// <summary>
		/// Precision varies significantly depending on the calculation you use.
		/// </summary>
		[Test]
		public void Coord_Can_Calculate_Distance_Accurately_N_W()
		{
			const int precision = 5;
			var known = (float)Math.Round(314.077362f, precision);
			var sourceAngLat = new AngularCoord(3, 7, 13);
			var sourceAngLon = new AngularCoord(3, 0, 0);
			var targetAngLat = new AngularCoord(5, 7, 13);
			var targetAngLon = new AngularCoord(1, 0, 0);
			var sourceCoord = new Coord(sourceAngLat, CardinalLat.N, sourceAngLon, CardinalLong.W);
			var targetCoord = new Coord(targetAngLat, CardinalLat.N, targetAngLon, CardinalLong.W);

			var calculatedDistance = (float)Math.Round(sourceCoord.DistanceKm(targetCoord), precision);
			Assert.That(calculatedDistance, Is.EqualTo(known));
		}
		/// <summary>
		/// Precision varies significantly depending on the calculation you use.
		/// </summary>
		[Test]
		public void Coord_Can_Calculate_Distance_Accurately_N_E_N_W()
		{
			const int precision = 5;
			var known = (float)Math.Round(8814.41504f, precision);

			var sourceCoord = new Coord(39.76451f,-104.99519f);
			var targetCoord = new Coord(55.58152f, 36.82514f);

			var calculatedDistance = (float)Math.Round(sourceCoord.DistanceKm(targetCoord), precision);
			Assert.That(calculatedDistance, Is.EqualTo(known));
		}
	}
}