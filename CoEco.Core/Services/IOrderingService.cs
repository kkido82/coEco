using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.GatewayResponses.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Repositories
{
    public interface IOrderingService
    {
        Task<Member> GetMemberById(int id);
        Task<Member[]> GetMembers(int unitId, Permission permission);
        Task<CreateOrderResponse> CreateOrder(Order order);
        Task<BaseGatewayResponse> UpdateOrder(Order order);
        Task<Order> GetOrder(int id);
        Task<Item> GetItem(int itemId);
        Task<UnitItem> GetUnitItem(int unitId, int itemId);
        Task<BaseGatewayResponse> Update(UnitItem unitItem);
        Task<BaseGatewayResponse> TransterMoney(int fromUnitId, int toUnitId, int amount);
    }
}
