using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Utilities.Hubs
{
    public abstract class BaseSignalRHub<T> : Microsoft.AspNetCore.SignalR.Hub
    {
        protected readonly T Service;
        protected BaseSignalRHub(T service)
        {
            Service = service;
        }
    }
}


