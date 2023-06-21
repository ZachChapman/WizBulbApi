using Microsoft.UI.Xaml;

namespace WizBulbApi.WinUI;

public class SettingsEntry
{
	public int HomeId { get; set; }
	public string NetworkInterfaceId { get; set; } = string.Empty;
	public ApplicationTheme Theme { get; set; }
	public List<SavedScene> SavedScenes { get; set; } = new();
}
