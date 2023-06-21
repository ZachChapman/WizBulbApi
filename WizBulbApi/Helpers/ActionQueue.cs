using System.Diagnostics;
using Toolbox;

namespace WizBulbApi;

public class ActionQueue
{
    private readonly List<TaggedAction> _queue = new();
    private readonly Repeater _repeater;
    private readonly TimeSpan _processInterval = TimeSpan.FromMilliseconds(100);
    private readonly object _queueLock = new();

    private bool _isRunning;
    private bool _isHandlingRequest;

    public ActionQueue()
    {
        _repeater = new Repeater(ProcessNext, _processInterval);
    }

    public bool HasRequests => _queue.Any();
    public bool IsHandlingRequest => _isHandlingRequest;

    public void Enqueue(Action action, string tag = default)
    {
        var taggedAction = new TaggedAction(action, tag);

        if(string.IsNullOrWhiteSpace(tag))
        {
            _queue.Add(taggedAction);
            return;
        }

        lock(_queueLock)
        {
            // If we already have this in the queue, replace the existing one.
            var index = _queue.FindIndex(element => element.Tag is not null && element.Tag == tag);
            if(index >= 0)
            {
                _queue[index] = taggedAction;
            }
            else
            {
                _queue.Add(taggedAction);
            }
        }

        if(!_isRunning)
        {
            _isRunning = true;
            _repeater.Start();
        }
    }

    private void ProcessNext()
    {
        if(!_queue.Any())
        {
            _repeater.Stop();
            _isRunning = false;
            return;
        }

        try
        {
            _isHandlingRequest = true;

            var element = default(TaggedAction);
            lock(_queueLock)
            {
                // Remove from queue
                element = _queue.First();
                _queue.Remove(element);
            }

            element.Action();
        }
        catch(Exception ex)
        {
            // TODO: Report to user
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            _isHandlingRequest = false;
        }
    }



    public class TaggedAction
    {
        public TaggedAction(Action action, string tag)
        {
            Action = action;
            Tag = tag;
        }

        public Action Action { get; }
        public string Tag { get; }
    }
}