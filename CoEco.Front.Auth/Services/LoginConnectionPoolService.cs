using System;
using System.Linq;
using CoEco.Front.Auth.Domain;
using CoEco.Front.Auth.Data;
using CoEco.Core.Ordering.Dto.Responses;
using static CoEco.Front.Auth.Domain.AuthError;

namespace CoEco.Front.Auth.Services
{
    public class LoginConnectionPoolService : ILoginConnectionPoolService
    {
        const int DEFAULT_NUM_OF_CONNECTIONS = 5;
        const string MaxConnectionsPerUserKey = "MaxConnectionsPerUser";
        private readonly int maxConnectionsPerUser;
        private readonly AuthDbContext db;

        public LoginConnectionPoolService(AuthDbContext db)
        {
            this.db = db;
            maxConnectionsPerUser = DEFAULT_NUM_OF_CONNECTIONS;
        }

        public Result<bool> CheckPoolAvailibility(string username)
        {
            var numConnections = GetNumConnections(username);

            bool poolIsFull = numConnections >= maxConnectionsPerUser;
            if (poolIsFull)
            {
                return ConnectionPoolIsFull;
            }

            return true;
        }

        public string SetConnection(string username, DateTime expire)
        {
            var id = Guid.NewGuid().ToString();
            db.Connections.Add(new Connection { ConnectionId = id, Username = username, ExpireDate = expire });
            db.SaveChanges();
            return id;
        }

        public void SetExpire(string connId, int expireInMinutes)
        {
            var connection = db.Connections.FirstOrDefault(c => c.ConnectionId == connId);
            connection.ExpireDate = DateTime.Now.AddMinutes(expireInMinutes);
            db.SaveChanges();
        }

        int GetNumConnections(string username) => db.Connections.Where(c => c.Username == username && c.ExpireDate > DateTime.Now).Count();
    }
}
