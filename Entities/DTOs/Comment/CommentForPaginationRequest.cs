﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Comment
{
    public class CommentForPaginationRequest : BasePaginationRequest
    {
        public int WriterId{ get; set; }
    }
}
