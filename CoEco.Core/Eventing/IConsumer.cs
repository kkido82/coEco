using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Eventing
{
    public interface IConsumer<in T> where T : class
    {
        void HandleEvent(T eventMessage);
    }
    public interface IAsyncConsumer<in T> where T : class
    {
        Task HandleEventAsync(T eventMessage);
    }
}
