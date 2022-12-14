namespace Core.Utilities.Results
{
    public class ErrorPaginationResult<T> : PaginationResult<T>
    {
        public ErrorPaginationResult(string message) : base(0, 0, null!, 0, false, message)
        {
        }

        public ErrorPaginationResult() : base(0, 0, null!, 0, false)
        {
        }
    }
}
