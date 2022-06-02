namespace Map.Engine.NUnit.CoordTests
{
	[TestFixture]
	public class AngularCoordTests
	{
		[Test]
		public void AngularCoord_Can_Construct_Accurately()
		{
			var seconds = 13f;
			var minutes = 7f;
			var degrees = 3f;
			var testMe = new AngularCoord(degrees + minutes/60f + seconds / 3600f);
			Assert.That(testMe.Degrees, Is.EqualTo(degrees));
			Assert.That(testMe.Minutes, Is.EqualTo(minutes));
			Assert.That(testMe.Seconds, Is.EqualTo(seconds));
		}
	}
}