namespace WizBulbApi.WinUI;

public interface ISettingsDataAccess
{
	Task<SettingsEntry?> LoadAsync();
	Task<SettingsEntry> LoadOrCreateAsync();
	Task SaveAsync(SettingsEntry settings);
}