using CoEco.BO.Models;
using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CoEco.BO.Services
{
    public class DistanceService
    {
        private readonly CoEcoEntities db;

        public DistanceService(CoEcoEntities db)
        {
            this.db = db;
        }

        public async Task<Result<bool>> Update(UnitDistanceModel[] models)
        {
            await Task.CompletedTask;
            var current = await GetCurrent();
           
            foreach (var model in models)
            {
                var key = GetKey(model.A, model.B);
                DistanceUnit distance;
                if (current.ContainsKey(key))
                {
                    distance = current[key];
                } else
                {
                    distance = new DistanceUnit
                    {
                        FirstUnitID = model.A,
                        SecondUnitID = model.B
                    };
                    db.DistanceUnits.Add(distance);
                }

                distance.Distance = model.Distance;
            }

            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return new Error("bo_fail_to_update_distances", ex.Message);
            }
        }


        async Task<Dictionary<string, DistanceUnit>> GetCurrent()
        {
            var distances = await db.DistanceUnits.ToArrayAsync();

            return distances.ToDictionary(u => GetKey(u.FirstUnitID, u.SecondUnitID), u => u);
        }


        string GetKey(int a, int b)
        {
            var first = a > b ? b : a;
            var second = a > b ? a : b;
            return $"{first}_{second}";
        }
    }
}