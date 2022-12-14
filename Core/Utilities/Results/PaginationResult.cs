using System.Collections;

namespace Core.Utilities.Results
{
    public class PaginationResult<T> : Result, IPaginateResult<T>
    {
        public PaginationResult(int index, int size, List<T> items, int totalCount, bool success, string message) : base(success, message)
        {
            Index = index;
            Size = size;
            Items = items;
            Count = totalCount;
        }
        public PaginationResult(int index, int size, List<T> items, int totalCount, bool success) : base(success)
        {
            Index = index;
            Size = size;
            Items = items;
            Count = totalCount;
        }
        public int Index { get; }

        public int Size { get; }

        public int Count { get; }

        public int Pages => (int)Math.Ceiling(Count / (double)Size) >= 0 ? (int)Math.Ceiling(Count / (double)Size) : 0;

        public List<T> Items { get; } = null!;

        public bool HasPrevious => Index != 0;

        public bool HasNext => (Index + 1) * Size < Count;
    }
}
