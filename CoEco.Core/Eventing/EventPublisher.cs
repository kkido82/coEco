using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace CoEco.Core.Eventing
{
    class EventPublisher: IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger _logger;
        private readonly ILifetimeScope _lifetimeScope;

        public EventPublisher(ISubscriptionService subscriptionService, ILogger logger, ILifetimeScope lifetimeScope)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
            _lifetimeScope = lifetimeScope;
        }

        public void Publish<T>(T eventMessage) where T : class
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                foreach (var subscription in scope.Resolve<IEnumerable<IConsumer<T>>>().ToList())
                {
                    try
                    {
                        subscription.HandleEvent(eventMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message, ex);
                    }

                }

            }


        }

        public void PublishAsync<T>(T eventMessage) where T : class
        {
            Task.Run(async () =>
            {
                HttpContext.Current = new HttpContext(
                   new HttpRequest("", "http://tempuri.org", ""),
                   new HttpResponse(new StringWriter())
               )
                {
                    User = new GenericPrincipal(
                    new GenericIdentity("umbraco-system"),
                    new string[0]
                )
                };
                using (var scope = _lifetimeScope.BeginLifetimeScope())
                {
                    var subscriptionService = scope.Resolve<ISubscriptionService>();
                    var logger = scope.Resolve<ILogger>();
                    foreach (var subscription in subscriptionService.GetAyncSubscriptions<T>())
                    {
                        try
                        {
                            await subscription.HandleEventAsync(eventMessage);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message, ex);
                        }
                    }
                }

            });
            
        }
    }
}
