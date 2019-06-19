using CoEco.Core.Infrastructure;

namespace CoEco.Core.Ordering.Domain
{
    public class Errors
    {
        public static Error FailToCreateOrder(string description) => new Error("fail_to_create_order", description);
        public static Error FailToUpdate(string description) => new Error("fail_to_update_order", description);

    }

}
