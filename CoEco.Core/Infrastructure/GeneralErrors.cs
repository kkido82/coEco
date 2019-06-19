namespace CoEco.Core.Infrastructure
{
    public class GeneralErrors
    {
        public static Error UnauthorizedError(string description = "unauthorized request") => new Error("unauthorized", description);
        public static Error NotFound(string description = "not found") => new Error("not_found", description);
    }
}
