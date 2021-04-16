using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IMediatorService
    {
        void Register(Object sender, String @event,Action<Object> callback);

        void UnRegister(Object sender, String @event);

        void BroadcastMessage(String @event, Object args);
    }
}
