using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class BasePaginationRequest
{
    public string? SearchValue { get; set; }
    public string? SearchValueField { get; set; }
    public string? OrderByField { get; set; } = "Id";
    public string? OrderType { get; set; } = "asc"; //asc-desc
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 5;
}

