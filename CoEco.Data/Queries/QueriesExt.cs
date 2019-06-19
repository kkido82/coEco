using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoEco.Core.Ordering.Domain;

namespace CoEco.Data.Queries
{
	internal static class QueriesExt
    {
        public static IQueryable<UnitWithDistance> ToUnitWithDistance(this IQueryable<Unit> source, IQueryable<DistanceUnit> distances, int target)
        {
            var arrgared = distances.ArrangeBy(target);

            return from u in source
                   where u.ID != target
                   join d in arrgared on u.ID equals d.From into aa
                   from d2 in aa.DefaultIfEmpty()
                   let distance = d2 == null ? 1000000 : d2.Distance
                   orderby distance, u.Rating descending
                   select new UnitWithDistance { Distance = distance, Unit = u };
        }


		public static async Task<int> GetIncome(this IQueryable<LendingItem> lendingItems, int? lendingUnitId = null, int? requestingUnitId = null)
		{
            var statusIds = new[] { (int)OrderStatusId.Confirmed, (int)OrderStatusId.Active, (int)OrderStatusId.Completed };
			var query =
				from item in lendingItems
				where statusIds.Contains(item.OrderStatusID)
				select item;

			if (lendingUnitId != null)
				query = query.Where(a => a.UnitLendingID == lendingUnitId.Value);

			if (requestingUnitId != null)
				query = query.Where(a => a.UnitRequestsID == requestingUnitId.Value);

			var x = await query.SumAsync(a => (int?)a.Price);
			return x ?? 0;
		}


        static IQueryable<DistanceUnitModel> ArrangeBy(this IQueryable<DistanceUnit> source, int target)
        {
            return from d in source
                   let isFirst = d.FirstUnitID == target
                   let a = isFirst ? d.FirstUnitID : d.SecondUnitID
                   let b = isFirst ? d.SecondUnitID : d.FirstUnitID
                   where d.FirstUnitID == target || d.SecondUnitID == target
                   select new DistanceUnitModel { From = b,Distance = d.Distance};

        }

		internal class DistanceUnitModel
		{
			public int From { get; set; }
			public double Distance { get; set; }
		}

		internal class UnitWithDistance
        {
            public Unit Unit { get; set; }
            public double Distance { get; set; }

            public override string ToString() => $"Id: {Unit.ID}, Distance:{Distance}";
        }
    }
}
