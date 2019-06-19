using Ordering = CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.GatewayResponses.Repositories;
using CoEco.Core.Ordering.Repositories;
using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CoEco.Services.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly CoEcoEntities db;

        public OrderingService(
            CoEcoEntities db)
        {
            this.db = db;
        }

        public async Task<CreateOrderResponse> CreateOrder(Ordering.Order order)
        {
            var lendingItem = new LendingItem
            {
                ItemID = order.ItemId,
                OrderStatusID = (int)order.Status,
                UnitRequestsID = order.RequestingUnitId,
                UnitLendingID = order.LendingUnitId,
                Remarks = order.Remarks,
                MemberID = order.RequestingMemberId,
                OrderDate = DateTime.Now
            };
            try
            {
                db.LendingItems.Add(lendingItem);
                await db.SaveChangesAsync();

                return new CreateOrderResponse(lendingItem.ID);

            }
            catch (Exception ex)
            {
                var errors = db.GetValidationErrors();
                return new CreateOrderResponse(ex.Message);
            }
        }

        public async Task<Ordering.Item> GetItem(int itemId)
        {
            return await db.Items
                .Select(i => new Ordering.Item
                {
                    Id = i.ID,
                    Price = i.Cost
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Ordering.Member> GetMemberById(int id)
        {
            var member = await db.Members.FindAsync(id);

            if (member == null)
            {
                return null;
            }

            return Map(member);

        }

        public async Task<Ordering.Member[]> GetMembers(int unitId, Ordering.Permission permission)
        {
            var query =
                from member in db.Members
                where member.UnitID == unitId
                select member;

            switch (permission)
            {
                case Ordering.Permission.CanOpenAnOrder:
                    query = query.Where(a => a.PermissionsProfile.OpenAnOrder);
                    break;
                case Ordering.Permission.CanUpdateInventory:
                    query = query.Where(a => a.PermissionsProfile.UpdateInventory);
                    break;
                case Ordering.Permission.CanConfirmOrder:
                    query = query.Where(a => a.PermissionsProfile.OrderConfirmation);
                    break;
            }

            var members = await query.ToArrayAsync();
            return members.Select(Map).ToArray();
        }

        public Task<Ordering.Order> GetOrder(int id)
        {
            return db.LendingItems
                .Where(a=> a.ID == id)
                .Select(item => new Ordering.Order
                {
                    Id = id,
                    ItemId = item.ItemID,
                    LendingUnitId = item.UnitLendingID,
                    Remarks = item.Remarks,
                    RequestingMemberId = item.MemberID,
                    RequestingUnitId = item.UnitRequestsID,
                    Status = (Ordering.OrderStatusId)item.OrderStatusID,
                    Price = item.Price
                })
                .FirstOrDefaultAsync();
        }

        public Task<Ordering.UnitItem> GetUnitItem(int unitId, int itemId)
        {
            return db.ItemToUnits
                .Select(item => new Ordering.UnitItem
                {
                    ItemId = item.ItemID,
                    UnitId = item.UnitID,
                    Amount = item.Quantity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BaseGatewayResponse> TransterMoney(int fromUnitId, int toUnitId, int amount)
        {
            var fromUnit = await db.Units.FindAsync(fromUnitId);
            var toUnit = await db.Units.FindAsync(toUnitId);

            if (fromUnit == null || toUnit == null)
            {
                return new BaseGatewayResponse(false, "unit not found");
            }

            fromUnit.CurrentWheelQuantity -= amount;
            toUnit.CurrentWheelQuantity += amount;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return new BaseGatewayResponse(false, ex.Message);
            }


            return new BaseGatewayResponse(true);
        }

        public async Task<BaseGatewayResponse> Update(Ordering.UnitItem unitItem)
        {
            var item = await db.ItemToUnits
                .FirstOrDefaultAsync(a => a.ItemID == unitItem.ItemId && a.UnitID == unitItem.UnitId);
            item.Quantity = unitItem.Amount;

            try
            {
                await db.SaveChangesAsync();
                return new BaseGatewayResponse(true);
            }
            catch (Exception ex)
            {

                return new BaseGatewayResponse(false, ex.Message);
            }
        }

        public async Task<BaseGatewayResponse> UpdateOrder(Ordering.Order order)
        {
            var item = await db.LendingItems.FindAsync(order.Id);

            if (item == null)
                return new BaseGatewayResponse(false, "order not found");

            item.ItemID = order.ItemId;
            item.OrderStatusID = (int)order.Status;
            item.UnitRequestsID = order.RequestingUnitId;
            item.UnitLendingID = order.LendingUnitId;
            item.Remarks = order.Remarks;
			item.Price = order.Price;

			try
            {
                await db.SaveChangesAsync();
                return new BaseGatewayResponse(true);
            }
            catch (Exception ex)
            {
                return new BaseGatewayResponse(false, ex.Message);
            }
        }


        Ordering.Member Map(Member member)
        {
            var permissions = new List<Ordering.Permission>();
            if (member.PermissionsProfile.OpenAnOrder)
                permissions.Add(Ordering.Permission.CanOpenAnOrder);

            if (member.PermissionsProfile.OrderConfirmation)
                permissions.Add(Ordering.Permission.CanConfirmOrder);

            if (member.PermissionsProfile.UpdateInventory)
                permissions.Add(Ordering.Permission.CanUpdateInventory);

            return new Ordering.Member(member.ID, member.UnitID, permissions.ToArray());

        }
    }
    
}
