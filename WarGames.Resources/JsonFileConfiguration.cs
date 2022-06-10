using WarGames.Contracts;

namespace WarGames.Resources
{
	public class JsonFileConfiguration<TInt, TId>
		where TInt : IUnique<TId>
		where TId : notnull
	{
		public JsonFileConfiguration(string rootPath)
		{
			RootPath = rootPath;
		}

		public JsonFileConfiguration()
		{
			RootPath = string.Empty;
		}

		public string RootPath { get; set; }

		public ConversionRequired? ConversionRequired { get; set; }

		public bool IsConversionRequired => ConversionRequired != null; 
	}
}