using CoEco.Front.Auth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Data
{
    internal static class Queries
    {
        public static IQueryable<bool> GetResults(this IQueryable<LoginResultLog> source, string username, int count) =>
            source
                .Where(r => !r.Disabled && r.Username == username)
                .OrderByDescending(r => r.CreatedOn)
                .Take(5)
                .Select(a => a.Succes);
    }
}
