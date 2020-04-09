using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.Entities
{
    public class PagingInfo
    {
        public PagingInfo()
        {            
        }

        public PagingInfo(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
