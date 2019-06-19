using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering.Domain;
using Resources;

namespace CoEco.Front.Helpers
{
    public static class ResourcesExtensions
    {
        public static string GetErrorMessage(this Error error)
        {
            var key = error.Code;
            return ErrorMessages.ResourceManager.GetString(key) ?? key;
        }
    }
}