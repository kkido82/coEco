using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Eventing
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>() where T : class;
        IList<IAsyncConsumer<T>> GetAyncSubscriptions<T>() where T : class;
    }
}
