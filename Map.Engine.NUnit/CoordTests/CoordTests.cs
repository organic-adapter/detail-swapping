namespace Map.Engine.NUnit.CoordTests
{
	[TestFixture]
	public class CoordTests
	{
		/// <summary>
		/// Precision varies significantly depending on the calculation you use.
		/// </summary>
		[Test]
		public void Coord_Can_Calculate_Distance_Accurately_RawCoord()
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