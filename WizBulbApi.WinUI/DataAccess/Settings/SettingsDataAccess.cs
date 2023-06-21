using System.Text.Json;

namespace WizBulbApi.WinUI;

public class SettingsDataAccess : ISettingsDataAccess
{
    private readonly string _path;

    public SettingsDataAccess(string path)
    {
        _path = path;
    }

    public async Task<SettingsEntry?> LoadAsync()
    {
        if(!File.Exists(_path))
        {
            return default;
        }

        await using var fileStream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<SettingsEntry>(fileStream);
    }

    public async Task<SettingsEntry> LoadOrCreateAsync()
    {
        if(!File.Exists(_path))
        {
            // Create a new file.
            await SaveAsync(new());
        }

        await using var fileStream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<SettingsEntry>(fileStream);
    }

    public async Task SaveAsync(SettingsEntry settings)
    {
        await using var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fileStream, settings);
    }
}
