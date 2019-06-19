using CoEco.Core.Ordering.Dto.Responses;
using System;

namespace CoEco.Front.Auth.Services
{
    public interface ILoginConnectionPoolService
    {
        string SetConnection(string key, DateTime expire);
        void SetExpire(string connid, int expireInMinutes);
        Result<bool> CheckPoolAvailibility(string username);
    }
}
