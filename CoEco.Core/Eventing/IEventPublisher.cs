using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Eventing
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage) where T : class;
        void PublishAsync<T>(T eventMessage) where T : class;
    }
}
