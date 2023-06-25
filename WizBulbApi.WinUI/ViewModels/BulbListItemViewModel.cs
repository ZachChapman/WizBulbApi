using MvvmFramework;

namespace WizBulbApi.WinUI;

public class BulbListItemViewModel : ViewModel
{
	public BulbHandle Handle { get; private set; }
	public string Name => Handle.Name;

	public async Task Initialise(BulbHandle handle)
	{
        Handle = handle;
    }
}
