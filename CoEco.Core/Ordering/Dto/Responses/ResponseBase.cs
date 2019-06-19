using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering.Domain;

namespace CoEco.Core.Ordering.Dto.Responses
{
    public abstract class ResponseBase
    {
        public bool Success { get; }
        public Error Error { get; }

        protected ResponseBase(bool success = false, Error error = null)
        {
            Success = success;
            Error = error;
        }
    }

    public class Result<T>
    {
        public Result(T value)
        {
            Success = true;
            Value = value;
        }

        public Result(Error error)
        {
            Error = error;
        }

        public bool Success { get; set; }
        public T Value { get; }
        public Error Error { get; }

        public static implicit operator Result<T>(T t)
        {
            var typeName = typeof(T).Name;
            return new Result<T>(t);
        }
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
    }

    public static class ResultExt
    {
        public static Result<T> Cast<T>(this Result<object> result)
        {
            if (result.Success) return new Result<T>((T)result.Value);
            return new Result<T>(result.Error);
        }
    }
}
