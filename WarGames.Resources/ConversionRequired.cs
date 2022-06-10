namespace WarGames.Resources
{
	public class ConversionRequired
	{
		public ConversionRequired(Type source, Type destination)
		{
			Source = source;
			Destination = destination;
		}

		public Type Source { get; }
		public Type Destination { get; }
		public static ConversionRequired Empty = new ConversionRequired(typeof(object), typeof(object));
	}

	public class ConversionRequired<TSource, TDest> : ConversionRequired
	{
		public ConversionRequired() : base(typeof(TSource), typeof(TDest))
		{
		}
	}
}