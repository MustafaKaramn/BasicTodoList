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
    public class TodoItemService : ITodoItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoItemService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto createTodo, Guid userId)
        {
            var todoItem = _mapper.Map<TodoItem>(createTodo);
            todoItem.CreatedDate = DateTime.Now;
            todoItem.UserId = userId;
            todoItem.IsCompleted = false;

            //Gelen todo'daki TodoListIds'ye göre ilişkili TodoList'lerin eklenmesi.
            if (createTodo.TodoListIds.Any())
            {
                var todoLists = await _unitOfWork.TodoListRepository.GetAllAsync(t => t.UserId == userId && createTodo.TodoListIds.Contains(t.Id));

                todoItem.TodoLists = todoLists.ToList();
            }

            _unitOfWork.TodoItemRepository.Add(todoItem);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var todo = await _unitOfWork.TodoItemRepository.GetByIdAsync(id);

            if (todo != null && todo.UserId == userId)
            {
                _unitOfWork.TodoItemRepository.Delete(todo);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<PagedResponse<TodoItemDto>> GetAllAsync(TodoQueryParameters queryParameters, Guid userId)
        {
            Expression<Func<TodoItem, bool>> filter = null;

            if (queryParameters.Status.HasValue)
            {
                filter = t => t.IsCompleted == queryParameters.Status.Value && t.UserId == userId;
            }

            var totalCount = await _unitOfWork.TodoItemRepository.CountAsync(filter);

            var skipAmount = (queryParameters.PageNumber - 1) * queryParameters.PageSize;

            var todoItems = await _unitOfWork.TodoItemRepository.GetAllAsync(filter, skip: skipAmount, take: queryParameters.PageSize);

            var todoItemDtos = _mapper.Map<List<TodoItemDto>>(todoItems);

            return new PagedResponse<TodoItemDto>(todoItemDtos, currentPage: queryParameters.PageNumber, pageSize: queryParameters.PageSize, totalCount: totalCount);
        }

        public async Task<TodoItemDto> GetByIdAsync(Guid id, Guid userId)
        {
            var todoItem = await _unitOfWork.TodoItemRepository.GetByIdAsync(id);

            if (todoItem == null || todoItem.UserId != userId)
            {
                todoItem = null;
            }

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task UpdateAsync(Guid id, UpdateTodoItemDto updateDto, Guid userId)
        {
            //TodoItem'ı ve ilişkili TodoList'leri getir.
            var todoLists = await _unitOfWork.TodoItemRepository.GetAllAsync(
                filter: i => i.Id == id && i.UserId == userId,
                include: i => i.Include(t => t.TodoLists));

            var existingItem = todoLists.FirstOrDefault();

            if (existingItem != null)
            {
                _mapper.Map(updateDto, existingItem);
                existingItem.UpdatedDate = DateTime.Now;

                existingItem.TodoLists.Clear();

                if (updateDto.TodoListIds.Any())
                {
                    var lists = await _unitOfWork.TodoListRepository.GetAllAsync(t => t.UserId == userId && updateDto.TodoListIds.Contains(t.Id));
                    existingItem.TodoLists = lists.ToList();
                }
            }
            _unitOfWork.TodoItemRepository.Update(existingItem);
            await _unitOfWork.CompleteAsync();
        }
    }
}
