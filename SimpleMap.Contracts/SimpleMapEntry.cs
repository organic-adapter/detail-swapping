namespace SimpleMap.Contracts
{
	public class SimpleMapEntry
	{
		public string admin_name { get; set; } = string.Empty;
		public string capital { get; set; } = string.Empty;
		public string city { get; set; } = string.Empty;
		public string city_ascii { get; set; } = string.Empty;
		public string country { get; set; } = string.Empty;
		public int id { get; set; }
		public string iso2 { get; set; } = string.Empty;
		public string iso3 { get; set; } = string.Empty;
		public double lat { get; set; }
		public double lng { get; set; }
		public int population { get; set; }
	}
}