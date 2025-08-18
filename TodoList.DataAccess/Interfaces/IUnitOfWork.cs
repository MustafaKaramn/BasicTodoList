using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Entities;

namespace TodoList.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TodoItem> TodoItemRepository { get; }
        IRepository<TodoList.Core.Entities.TodoList> TodoListRepository { get; }

        Task<int> CompleteAsync();
    }
}
