namespace WizBulbApi.WinUI;

public interface IViewModelProvider
{
	T Create<T>();
}
