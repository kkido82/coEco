using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Data;
using CoEco.Services.Services;
using System.Data.Entity;
using System.Threading.Tasks;
using static CoEco.Core.Infrastructure.GeneralErrors;

namespace CoEco.Front.Handlers.UnitItems
{
    public abstract class ManageItemBase
    {
        public int MemberId { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }

    }

    public class ManageItemRequest : ManageItemBase, IRequest<UnitItemUpdated>
    {
    }

    public class UnitItemUpdated: ManageItemBase
    {
    }


    public class ManageItemsHandler : 
        IHandler<ManageItemRequest, UnitItemUpdated>
    {
        private readonly IDataService<ItemToUnit> unitItems;
        private readonly IDataService<Member> members;

        public ManageItemsHandler(IDataService<ItemToUnit> unitItems, IDataService<Member> members)
        {
            this.unitItems = unitItems;
            this.members = members;
        }
        public async Task<Result<UnitItemUpdated>> Handle(ManageItemRequest request)
        {
            var member = members.GetById(request.MemberId);
            if (member == null)
                return NotFound();

            var unitItem = await unitItems.FindBy(a => a.ItemID == request.ItemId && a.UnitID == member.UnitID).FirstOrDefaultAsync();

            var isNew = unitItem == null;
            if (unitItem == null)
                unitItem = new ItemToUnit
                {
                    ItemID = request.ItemId,
                    UnitID = member.UnitID
                };

            unitItem.Description = request.Description;
            unitItem.Quantity = request.Qty;
            if (isNew)
            {
                await unitItems.InsertAsync(unitItem);
            } else
            {
                await unitItems.EditAsync(unitItem);
            }

            return new UnitItemUpdated
            {
                MemberId = request.MemberId,
                Description = request.Description,
                Qty = request.Qty,
                ItemId = request.ItemId
            };
        }
    }
}