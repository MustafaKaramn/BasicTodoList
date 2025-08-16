using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Core.Helpers
{
    public class TodoQueryParameters : PaginationParameters
    {
        public bool? Status { get; set; }
    }
}
