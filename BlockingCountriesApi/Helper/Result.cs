namespace BlockingCountriesApi.Helper
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Ok(string message = "") => new Result(true, message);

        public static Result Fail(string message) => new Result(false, message);
    }
}
