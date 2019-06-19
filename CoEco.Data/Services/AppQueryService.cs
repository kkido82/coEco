using CoEco.Data.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoEco.Data.Queries;
using System.Collections.Generic;
using CoEco.Core.Ordering.Domain;

namespace CoEco.Data.Services
{
    public class AppQueryService
    {
        private readonly CoEcoEntities db;

        public AppQueryService(CoEcoEntities db)
        {
            this.db = db;
        }
        public async Task<OrderOverview[]> GetRequestingOrders(int memberId)
        {
            var unitId = await db.Members.Where(a => a.ID == memberId).Select(a => a.UnitID).FirstOrDefaultAsync();

            var query = OrderOverviewQuery().Where(a => a.ToUnitId == unitId);


            return await query.ToArrayAsync();
        }

        public async Task<OrderOverview[]> GetLendingOrders(int memberId)
        {
            var unitId = await db.Members.Where(a => a.ID == memberId).Select(a => a.UnitID).FirstOrDefaultAsync();

            var query = OrderOverviewQuery().Where(a => a.FromUnitId == unitId);

            return await query.ToArrayAsync();

        }

        public Task<Member> GetMember(int id) => db.Members.FindAsync(id);

        public async Task<OrderOverview> GetOrderOverview(int id)
        {
            var query = OrderOverviewQuery().Where(o => o.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        private IQueryable<OrderOverview> OrderOverviewQuery()
        {
            return from itl in db.LendingItems
                   where !itl.Disable
                   orderby itl.OrderDate descending
                   select new OrderOverview
                   {
                       Id = itl.ID,
                       ItemId = itl.ItemID,
                       ItemName = itl.Item.Name,
                       OrderDate = itl.OrderDate,
                       StatusId = itl.OrderStatusID,
                       StatusName = itl.OrderStatus.StatusName,
                       FromUnit = itl.UnitLending.UnitName,
                       FromUnitId = itl.UnitLendingID,
                       ToUnitId = itl.UnitRequestsID,
                       ToUnit = itl.UnitRequest.UnitName
                   };


        }

        public async Task<OrderDetails> GetOrderDetails(int memberId, int orderId)
        {
            var member = await db.Members.FindAsync(memberId);
            var lendingItem = await db.LendingItems.FindAsync(orderId);
            var itemToUnit = await db.ItemToUnits
                .Where(itu => itu.UnitID == lendingItem.UnitLendingID && itu.ItemID == lendingItem.ItemID)
                .FirstOrDefaultAsync();


            if ((member.UnitID != lendingItem.UnitRequestsID
                && member.UnitID != lendingItem.UnitLendingID)
                || itemToUnit == null)
                return null;

            var unitIds = new[] { lendingItem.UnitRequestsID, lendingItem.UnitLendingID };
            var distance =
                (from d in db.DistanceUnits
                 where unitIds.Contains(d.FirstUnitID) && unitIds.Contains(d.SecondUnitID)
                 select d.Distance)
                .FirstOrDefault();


            var contactPerson =
                (from m in db.Members
                 where !m.Disable && m.UnitID == lendingItem.UnitLendingID && m.PermissionsProfile.OrderConfirmation
                 select m).FirstOrDefault();

            var actions = GetAllowedActions(member, lendingItem);

            return new OrderDetails
            {
                Id = lendingItem.ID,
                ItemId = lendingItem.ItemID,
                ItemName = lendingItem.Item.Name,
                Cost = lendingItem.Item.Cost,
                OrderDate = lendingItem.OrderDate,
                StatusId = lendingItem.OrderStatusID,
                StatusName = lendingItem.OrderStatus.StatusName,
                Remarks = lendingItem.Remarks,
                Distance = distance,
                FromUnit = lendingItem.UnitLending.UnitName,
                FromUnitId = lendingItem.UnitLendingID,
                ItemDescription = itemToUnit.Description,
                ContactPersonName = contactPerson?.FirstName + " " + contactPerson?.LastName,
                ContactPersonPhone = contactPerson?.PhoneNumber,
                Actions = actions
            };
        }

        public async Task<OrderDetails> GetNewOrder(int memberId, int unitItemId)
        {
            var member = await db.Members.FindAsync(memberId);
            var itemToUnit = await db.ItemToUnits.FindAsync(unitItemId);
            if (member == null || itemToUnit == null)
                return null;

            var unitIds = new[] { member.UnitID, itemToUnit.UnitID };
            var distance = await
                (from d in db.DistanceUnits
                 where unitIds.Contains(d.FirstUnitID) && unitIds.Contains(d.SecondUnitID)
                 select d.Distance)
                .FirstOrDefaultAsync();


            var contactPerson = await
                (from m in db.Members
                 where !m.Disable && m.UnitID == itemToUnit.UnitID && m.PermissionsProfile.OrderConfirmation
                 select m).FirstOrDefaultAsync();

            return new OrderDetails
            {
                Id = 0,
                ItemId = itemToUnit.ItemID,
                ItemName = itemToUnit.Item.Name,
                Cost = itemToUnit.Item.Cost,
                OrderDate = DateTime.Now,
                StatusId = -1,
                StatusName = "",
                Remarks = "",
                Distance = distance,
                FromUnit = itemToUnit.Unit.UnitName,
                FromUnitId = itemToUnit.UnitID,
                ItemDescription = itemToUnit.Description,
                ContactPersonName = contactPerson?.FirstName + " " + contactPerson?.LastName,
                ContactPersonPhone = contactPerson?.PhoneNumber,
                Actions = new int[] { (int)OrderStatusId.New }
            };
        }

        public Task<ItemModel[]> GetItems()
        {
            var query =
                from item in db.Items
                where !item.Disable
                orderby item.Name
                select new ItemModel
                {
                    ItemId = item.ID,
                    ItemName = item.Name,
                    IconName = item.IconStore.Url
                };

            return query.ToArrayAsync();
        }

        public Task<MessageModel[]> GetMessages(int memberId)
        {
            var query =
                from message in db.Messages
                where !message.Disable && message.MemberID == memberId && message.OpeningDate == null
                orderby message.CreatedOn descending
                select new MessageModel
                {
                    Id = message.ID,
                    OrderId = message.OrderID,
                    Date = message.CreatedOn,
                    Title = message.Content
                };

            return query.ToArrayAsync();
        }

        public async Task<UnitItemSearchModel[]> SearchUnitItems(int memberId, int[] itemIds)
        {
            var member = await db.Members.FindAsync(memberId);
            var toUnitId = member.UnitID;

            var query =
                from ud in db.Units.ToUnitWithDistance(db.DistanceUnits, toUnitId)
                join itu in db.ItemToUnits on ud.Unit.ID equals itu.UnitID
                where itemIds.Contains(itu.ItemID)
                select new UnitItemSearchModel
                {
                    ItemId = itu.ItemID,
                    Distance = ud.Distance,
                    Description = itu.Description,
                    ItemUnitId = itu.ID,
                    UnitName = ud.Unit.UnitName,
                    Rating = (int)ud.Unit.Rating
                };


            return await query.ToArrayAsync();
        }

        public async Task<UnitBalanceModel> GetBalance(int memberId)
        {
            var member = await db.Members.FindAsync(memberId);
            var unit = member.Unit;
            var incoming = await db.LendingItems.GetIncome(lendingUnitId: unit.ID);
            var outgoing = await db.LendingItems.GetIncome(requestingUnitId: unit.ID);
            return new UnitBalanceModel
            {
                CurrentBalance = unit.CurrentWheelQuantity,
                OriginalBalance = unit.OriginalWheelsQuantity,
                IncomingBalance = incoming,
                OutgoingBalance = outgoing
            };
        }

        private int[] GetAllowedActions(Member member, LendingItem lendingItem)
        {
            var actions = new List<int>();
            var isRequesting = member.UnitID == lendingItem.UnitRequestsID;
            var profile = member.PermissionsProfile;
            if (isRequesting)
            {
                switch ((OrderStatusId)lendingItem.OrderStatusID)
                {
                    case OrderStatusId.New when profile.OrderConfirmation:
                        actions.Add((int)OrderStatusId.Approved);
                        actions.Add((int)OrderStatusId.CanceledByRequestingUnit);
                        break;


                    case OrderStatusId.Confirmed when profile.OpenAnOrder:
                        actions.Add((int)OrderStatusId.Active);
                        break;

                }
            }
            else
            {
                switch ((OrderStatusId)lendingItem.OrderStatusID)
                {
                    case OrderStatusId.Approved when profile.OrderConfirmation:
                        actions.Add((int)OrderStatusId.Confirmed);
                        actions.Add((int)OrderStatusId.CanceledByLendingUnit);
                        break;

                    case OrderStatusId.Active when profile.UpdateInventory:
                        actions.Add((int)OrderStatusId.Completed);
                        break;
                }
            }


            return actions.ToArray();
        }

        public async Task<AspNetUser[]> GetProblemSolvers()
        {
            var userClaims = await db.AspNetUserClaims.Where(uc => uc.ClaimType == "ActiveRoles").ToArrayAsync();
            var ids = userClaims
                .Where(uc =>{
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(uc.ClaimValue);
                    return data.ContainsKey("problemSolving") && data["problemSolving"] > 0;
                })
                .Select(a => a.UserId)
                .ToArray();

            var users = await db.AspNetUsers.Where(u => ids.Contains(u.Id)).ToArrayAsync();

            return users;
        }


        public async Task<UnitItemModel[]> GetUnitItems(int memberId)
        {
            var member = await db.Members.FindAsync(memberId);
            var query =
                from ui in db.ItemToUnits
                where !ui.Disable && ui.UnitID == member.UnitID
                select new UnitItemModel
                {
                    ItemId = ui.ItemID,
                    Description = ui.Description,
                    ItemName = ui.Item.Name,
                    Qty = ui.Quantity
                };

            return await query.ToArrayAsync();

        }

    }
}
