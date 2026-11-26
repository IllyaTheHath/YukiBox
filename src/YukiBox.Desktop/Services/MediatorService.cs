using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services;

public class MediatorService : IMediatorService
{
    private readonly Dictionary<string, Dictionary<object, Action<object>>> _participants;

    public MediatorService()
    {
        _participants = [];
    }

    public void Register(object sender, string @event, Action<object> callback)
    {
        if (!_participants.ContainsKey(@event))
        {
            Dictionary<object, Action<object>> dic = new()
            {
                { sender, callback }
            };
            _participants.Add(@event, dic);
        }
        else
        {
            var dic = _participants[@event];
            if (!dic.ContainsKey(sender))
            {
                dic.Add(sender, callback);
            }
        }
    }

    public void UnRegister(object sender, string @event)
    {
        if (_participants.ContainsKey(@event))
        {
            var dic = _participants[@event];
            if (dic.ContainsKey(sender))
            {
                dic.Remove(sender);
            }
        }
    }

    public void BroadcastMessage(string @event, object arg)
    {
        if (_participants.ContainsKey(@event))
        {
            var dic = _participants[@event];
            foreach (var ps in dic)
            {
                ps.Value?.Invoke(arg);
            }
        }
    }
}
