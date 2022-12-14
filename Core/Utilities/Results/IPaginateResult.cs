using System.Collections;
using System.Collections.Generic;

namespace Core.Utilities.Results
{
    public interface IPaginateResult<T> : IResult
    {
        int Index { get; }

        int Size { get; }

        int Count { get; }

        int Pages { get; }

        List<T> Items { get; }

        bool HasPrevious { get; }

        bool HasNext { get; }
    }
}
