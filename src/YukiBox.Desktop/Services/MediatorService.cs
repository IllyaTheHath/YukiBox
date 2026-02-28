using System;
using System.Collections.Generic;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class MediatorService : IMediatorService
    {
        private readonly Dictionary<string, Dictionary<object, Action<object>>> _participants;

        public MediatorService()
        {
            this._participants = new Dictionary<string, Dictionary<object, Action<object>>>();
        }

        public void Register(object sender, string @event, Action<object> callback)
        {
            if (!this._participants.ContainsKey(@event))
            {
                Dictionary<object, Action<object>> dic = new();
                dic.Add(sender, callback);
                this._participants.Add(@event, dic);
            }
            else
            {
                var dic = this._participants[@event];
                if (!dic.ContainsKey(sender))
                {
                    dic.Add(sender, callback);
                }
            }
        }

        public void UnRegister(object sender, string @event)
        {
            if (this._participants.ContainsKey(@event))
            {
                var dic = this._participants[@event];
                if (dic.ContainsKey(sender))
                {
                    dic.Remove(sender);
                }
            }
        }

        public void BroadcastMessage(string @event, object arg)
        {
            if (this._participants.ContainsKey(@event))
            {
                var dic = this._participants[@event];
                foreach (var ps in dic)
                {
                    ps.Value?.Invoke(arg);
                }
            }
        }
    }
}