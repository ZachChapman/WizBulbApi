using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WizBulbApi.WinUI;

public class TrackedObservableCollection<TElement> : ObservableCollection<TElement>
{
    private readonly List<TElement> _added = new();
    private readonly List<TElement> _removed = new();
    private bool _isTrackingEnabled;

    public TrackedObservableCollection()
    {
        CollectionChanged += CollectionChangedHandler;
    }

    public void EnableTracking(bool enabled = true) => _isTrackingEnabled = enabled;
    public List<TElement> GetAddedItems() => _added.ToList();
    public List<TElement> GetRemovedItems() => _removed.ToList();

    private void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(!_isTrackingEnabled) return;

        if(e.Action is NotifyCollectionChangedAction.Add)
        {
            foreach(var item in e.NewItems.Cast<TElement>())
            {
                _added.Add(item);
            }
        }

        if(e.Action is NotifyCollectionChangedAction.Remove)
        {
            foreach(var item in e.OldItems.Cast<TElement>())
            {
                if(_added.Contains(item))
                {
                    _added.Remove(item);
                    continue;
                }

                _removed.Add(item);
            }
        }

        if(e.Action is NotifyCollectionChangedAction.Reset)
        {
            foreach(var item in e.OldItems.Cast<TElement>())
            {
                if(_added.Contains(item))
                {
                    _added.Remove(item);
                    continue;
                }

                _removed.Add(item);
            }
        }
    }
}
