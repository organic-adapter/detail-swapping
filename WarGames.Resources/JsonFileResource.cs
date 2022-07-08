using AutoMapper;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WarGames.Contracts;
using WarGames.Resources.Exceptions;

namespace WarGames.Resources
{
	public class ReadonlyJsonFileResource<TInt, TId> : IReadResource<TInt, TId>
		where TInt : IUnique<TId>
		where TId : notnull
	{
		private readonly Dictionary<TId, TInt> cached;
		private readonly IMapper mapper;
		private readonly IOptionsMonitor<JsonFileConfiguration<TInt, TId>> options;

		public ReadonlyJsonFileResource(IOptionsMonitor<JsonFileConfiguration<TInt, TId>> options, IMapper mapper)
		{
			this.options = options;
			this.mapper = mapper;
			cached = new Dictionary<TId, TInt>();
			LoadFromFile();
		}

		private ConversionRequired conversionRequired => options.CurrentValue.ConversionRequired ?? ConversionRequired.Empty;
		private bool IsConversionRequired => options.CurrentValue.IsConversionRequired;

		/// <summary>
		/// Grabs the entire document to search for the record.
		///
		/// Throws an exception if the record cannot be found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"></exception>
		public TInt Get(TId id)
		{
			if (!cached.ContainsKey(id))
				throw new KeyNotFoundException(id.ToString());

			return cached[id];
		}

		public IEnumerable<TInt> GetAll()
		{
			if (!cached.Any())
				throw new EmptyRepositoryException();

			return cached.Select(kvp => kvp.Value);
		}

		public async Task<IEnumerable<TInt>> GetAllAsync()
		{
			return await Task.Run(() => GetAll());
		}

		public async Task<TInt> GetAsync(TId id)
		{
			return await Task.Run(() => Get(id));
		}

		private List<TInt> Convert(string json)
		{
			var sourceList = JsonSerializer.Deserialize(json, conversionRequired.Source);
			return (mapper.Map(sourceList, conversionRequired.Source, conversionRequired.Destination) as List<TInt>) ?? new List<TInt>();
		}

		private void LoadFromFile()
		{
			var json = File.ReadAllText(options.CurrentValue.RootPath);

			var list = IsConversionRequired ? Convert(json) : JsonSerializer.Deserialize<List<TInt>>(json);
			var unique = list.Distinct();
			foreach (var item in unique)
				cached.Add(item.Id, item);
		}
	}
}