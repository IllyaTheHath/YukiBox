namespace YukiBox.Desktop.Contracts.Services;

public interface IMediatorService
{
    void Register(object sender, string @event, Action<object> callback);

    void UnRegister(object sender, string @event);

    void BroadcastMessage(string @event, object args);
}
