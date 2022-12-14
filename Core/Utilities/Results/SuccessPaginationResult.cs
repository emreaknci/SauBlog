namespace Core.Utilities.Results
{
    public class SuccessPaginationResult<T> : PaginationResult<T>
    {
        public SuccessPaginationResult(int index, int size, List<T> items, int totalCount, string message) : base(index, size, items, totalCount, true, message)
        {
        }
        public SuccessPaginationResult(int index, int size, List<T> items, int totalCount) : base(index, size, items, totalCount, true)
        {
        }
    }
}
