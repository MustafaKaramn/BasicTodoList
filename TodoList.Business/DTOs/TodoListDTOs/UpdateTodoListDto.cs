using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Business.DTOs.TodoListDTOs
{
    public class UpdateTodoListDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
