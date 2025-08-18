using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs.TodoItemDTOs;
using TodoList.Business.Interfaces;
using TodoList.Core.Entities;
using TodoList.Core.Helpers;
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

        public async Task<TodoItemDto> CreateTodoAsync(CreateTodoItemDto createTodo)
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

        public async Task<PagedResponse<TodoItemDto>> GetAllTodoAsync(TodoQueryParameters queryParameters)
        {
            Expression<Func<TodoItem, bool>> filter = null;

            if (queryParameters.Status.HasValue)
            {
                filter = t => t.IsCompleted == queryParameters.Status.Value;
            }

            var totalCount = await _unitOfWork.TodoRepository.CountAsync(filter);

            var skipAmount = (queryParameters.PageNumber - 1) * queryParameters.PageSize;

            var todoItems = await _unitOfWork.TodoRepository.GetAllAsync(filter, skip: skipAmount, take: queryParameters.PageSize);

            var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);

            return new PagedResponse<TodoItemDto>(todoItemDtos, currentPage: queryParameters.PageNumber, pageSize: queryParameters.PageSize, totalCount: totalCount);
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var todoItem = await _unitOfWork.TodoRepository.GetByIdAsync(id);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoItemDto todoItem)
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
