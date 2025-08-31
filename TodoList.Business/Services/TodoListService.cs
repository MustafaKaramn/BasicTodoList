using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs.TodoListDTOs;
using TodoList.Business.Interfaces;
using TodoList.DataAccess.Interfaces;

namespace TodoList.Business.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TodoListDto> CreateAsync(CreateTodoListDto todoList, string? imageUrl, Guid userId)
        {
            var createdTodoList = _mapper.Map<Core.Entities.TodoList>(todoList);
            createdTodoList.ImageUrl = imageUrl;
            createdTodoList.UserId = userId;

            _unitOfWork.TodoListRepository.Add(createdTodoList);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TodoListDto>(createdTodoList);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var todoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (todoList != null && todoList.UserId == userId)
            {
                _unitOfWork.TodoListRepository.Delete(todoList);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<TodoListDto>> GetAllAsync(Guid userId)
        {
            var todoLists = await _unitOfWork.TodoListRepository.GetAllAsync();

            todoLists = todoLists.Where(t => t.UserId == userId);

            return _mapper.Map<IEnumerable<TodoListDto>>(todoLists);
        }

        public async Task<TodoListDto> GetByIdAsync(Guid id, Guid userId)
        {
            var todoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (todoList == null || todoList.UserId != userId)
            {
                return _mapper.Map<TodoListDto>(null);
            }

            return _mapper.Map<TodoListDto>(todoList);
        }

        public async Task UpdateAsync(Guid id, UpdateTodoListDto todoList, Guid userId)
        {
            var existingTodoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (existingTodoList != null && existingTodoList.UserId == userId)
            {
                var updatedTodoList = _mapper.Map(todoList, existingTodoList);
                _unitOfWork.TodoListRepository.Update(updatedTodoList);

                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
