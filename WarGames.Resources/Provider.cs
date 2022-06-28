namespace WarGames.Resources
{
	public class Bucket<T>
	{
		public Bucket()
		{
			Dump = () => Activator.CreateInstance<T>();
		}

		public Func<T> Dump { get; set; }
	}
}