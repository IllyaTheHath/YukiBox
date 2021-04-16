using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class MediatorService : IMediatorService
    {
        private readonly Dictionary<String, Dictionary<Object, Action<Object>>> _participants;

        public MediatorService()
        {
            this._participants = new Dictionary<string, Dictionary<object, Action<object>>>();
        }

        public void Register(Object sender, String @event, Action<Object> callback)
        {
            if (!this._participants.ContainsKey(@event))
            {
                Dictionary<Object, Action<Object>> dic = new();
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

        public void UnRegister(Object sender, String @event)
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


        public void BroadcastMessage(String @event, Object arg)
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
