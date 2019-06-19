using Autofac;
using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Messages;

namespace CoEco.Core.Ordering
{
    public class OrderingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddHandler<OpenOrderHandler, OpenOrderRequest, OrderOpened>();
            builder.AddHandler<AprroveHanlder, ApproveRequest, Approved>();
            builder.AddHandler<ConfirmHandler, ConfirmOrderRequest, OrderConfirmed>();
            builder.AddHandler<ActivateHandler, ActivateOrderRequest, OrderActivated>();
            builder.AddHandler<CompleteOrderHandler, CompleteOrderRequest, OrderCompleted>();
            builder.AddHandler<CancelNewOrderHandler, CancelNewRequest, CanceledByRequestingUnit>();
            builder.AddHandler<CancelApprovedOrderHandler, CancelApprovedRequest, CanceledByLendingUnit>();
            builder.RegisterType<MessageSender>().AsSelf().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
