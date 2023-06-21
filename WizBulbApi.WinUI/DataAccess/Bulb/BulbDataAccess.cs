using System.Net;
using System.Text.Json;

namespace WizBulbApi.WinUI;

public class BulbDataAccess : IBulbDataAccess
{
    private static readonly List<BulbHandle> _handles = new();
    private readonly string _path;
    private bool _isLoaded;

    public BulbDataAccess(string path)
    {
        _path = path;
    }

    public async Task<BulbHandle?> GetBulbHandleAsync(MacAddress macAddress)
    {
        await TryInitialiseHandlesAsync();

        return _handles.FirstOrDefault(profile => profile.MacAddress == macAddress);
    }

    public async Task<List<BulbHandle>> GetAllBulbHandlesAsync(int? homeId = default)
    {
        await TryInitialiseHandlesAsync();

        return homeId is null
            ? _handles.ToList()
            : _handles.Where(handle => handle.HomeId == homeId).ToList();
    }

    public async Task<bool> AddBulbHandleAsync(BulbHandle handle)
    {
        await TryInitialiseHandlesAsync();

        if(await ContainsAsync(handle.MacAddress))
        {
            return false;
        }

        _handles.Add(handle);

        await SaveAsync();

        return true;
    }

    public async Task UpdateBulbHandleAsync(BulbHandle handle)
    {
        await TryInitialiseHandlesAsync();

        if(!await ContainsAsync(handle.MacAddress))
        {
            await AddBulbHandleAsync(handle);
            return;
        }

        var index = _handles.FindIndex(profile => profile.MacAddress == profile.MacAddress);
        _handles.RemoveAt(index);
        _handles.Insert(index, handle);

        await SaveAsync();
    }

	public async Task UpdateBulbNameAsync(MacAddress macAddress, string newName)
	{
		await TryInitialiseHandlesAsync();

		if(!await ContainsAsync(macAddress))
		{
			return;
		}

		var handle = _handles.First(profile => profile.MacAddress == profile.MacAddress);
        handle.Name = newName;

		await SaveAsync();
	}

	public async Task RemoveBulbHandleAsync(BulbHandle handle)
    {
        await TryInitialiseHandlesAsync();

        if(!await ContainsAsync(handle.MacAddress))
        {
            return;
        }

        _handles.Remove(handle);

        await SaveAsync();
    }

    public async Task<bool> ContainsAsync(MacAddress macAddress)
    {
        await TryInitialiseHandlesAsync();

        return _handles.Any(handle => handle.MacAddress == macAddress);
    }

    private async Task TryInitialiseHandlesAsync()
    {
        if(!_isLoaded)
        {
            _isLoaded = true;

            // Initialise list from file
            if(File.Exists(_path))
            {
                await using var fileStream = File.OpenRead(_path);

                var handles =
                    await JsonSerializer.DeserializeAsync<IEnumerable<BulbHandle>>(
                        fileStream,
                        GetJsonSerializerOptions());

                _handles.AddRange(handles);
            }
        }
    }

    private async Task SaveAsync()
    {
        await using var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fileStream, _handles, GetJsonSerializerOptions());
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
            Converters =
            {
                new StringJsonConverter<IPAddress>(IPAddress.Parse),
                new StringJsonConverter<MacAddress>(s => new(s)),
            },
        };
    }
}