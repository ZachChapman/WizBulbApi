using Microsoft.Extensions.DependencyInjection;
using MvvmFramework;

namespace WizBulbApi.WinUI;

public class BulbListItemViewModel : ViewModel
{
	public static BulbListItemViewModel Create(BulbHandle handle)
	{
		var viewModel = App.Host.Services.GetRequiredService<BulbListItemViewModel>();
        viewModel.Handle = handle;

		return viewModel;
	}

	public BulbHandle Handle { get; private set; }
	public string Name => Handle.Name;
}
