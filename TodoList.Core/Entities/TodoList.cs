using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Core.Entities
{
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
