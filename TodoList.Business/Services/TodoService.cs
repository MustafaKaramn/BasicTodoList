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

        public async Task<PagedResponse<TodoItemDto>> GetAllTodoAsync(PaginationParameters paginationParameters)
        {
            var totalCount = await _unitOfWork.TodoRepository.CountAsync();

            var skipAmount = (paginationParameters.PageNumber - 1) * paginationParameters.PageSize;

            var todoItems = await _unitOfWork.TodoRepository.GetAllAsync(skip: skipAmount, take: paginationParameters.PageSize);

            var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);

            return new PagedResponse<TodoItemDto>(todoItemDtos, currentPage: paginationParameters.PageNumber, pageSize: paginationParameters.PageSize, totalCount: totalCount);
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var todoItem = await _unitOfWork.TodoRepository.GetByIdAsync(id);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<PagedResponse<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted, PaginationParameters paginationParameters)
        {
            Expression<Func<TodoItem, bool>> filter = t => t.IsCompleted == isCompleted;

            //var todos = await _unitOfWork.TodoRepository.GetAllAsync(filter);
            //return _mapper.Map<IEnumerable<TodoItemDto>>(todos);

            var totalCount = await _unitOfWork.TodoRepository.CountAsync(filter);
            var skipAmount = (paginationParameters.PageNumber - 1) * paginationParameters.PageSize;

            var todoItems = await _unitOfWork.TodoRepository.GetAllAsync(filter, skip: skipAmount, take: paginationParameters.PageSize);

            var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);

            return new PagedResponse<TodoItemDto>(todoItemDtos, currentPage: paginationParameters.PageNumber, pageSize: paginationParameters.PageSize, totalCount: totalCount);
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
