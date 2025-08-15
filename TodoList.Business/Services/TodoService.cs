using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs;
using TodoList.Business.Interfaces;
using TodoList.Core.Entities;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;

namespace TodoList.Business.Services
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createTodo)
        {
            var todoItem = _mapper.Map<TodoItem>(createTodo);
            todoItem.CreatedDate = DateTime.Now;
            todoItem.IsCompleted = false;

            _unitOfWork.TodoRepository.Add(todoItem);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            var todo = await _unitOfWork.TodoRepository.GetByIdAsync(id);

            if (todo != null)
            {
                _unitOfWork.TodoRepository.Delete(todo);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<TodoItemDto>> GetAllTodoAsync()
        {
            var todos = await _unitOfWork.TodoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoItemDto>>(todos);
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var todoItem = await _unitOfWork.TodoRepository.GetByIdAsync(id);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<IEnumerable<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted)
        {
            Expression<Func<TodoItem, bool>> filter = t => t.IsCompleted == isCompleted;

            var todos = await _unitOfWork.TodoRepository.GetAllAsync(filter);
            return _mapper.Map<IEnumerable<TodoItemDto>>(todos);
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoDto todoItem)
        {
            var todo = await _unitOfWork.TodoRepository.GetByIdAsync(id);

            if (todo != null)
            {
                _mapper.Map(todoItem, todo);
                todo.UpdatedDate = DateTime.Now;

                _unitOfWork.TodoRepository.Update(todo);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
