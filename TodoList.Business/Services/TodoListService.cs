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

        public async Task<TodoListDto> CreateAsync(CreateTodoListDto todoList, string? imageUrl)
        {
            var createdTodoList = _mapper.Map<Core.Entities.TodoList>(todoList);
            createdTodoList.ImageUrl = imageUrl;

            _unitOfWork.TodoListRepository.Add(createdTodoList);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TodoListDto>(createdTodoList);
        }

        public async Task DeleteAsync(Guid id)
        {
            var todoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (todoList != null)
            {
                _unitOfWork.TodoListRepository.Delete(todoList);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<TodoListDto>> GetAllAsync()
        {
            var todoLists = await _unitOfWork.TodoListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoListDto>>(todoLists);
        }

        public async Task<TodoListDto> GetByIdAsync(Guid id)
        {
            var todoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);
            return _mapper.Map<TodoListDto>(todoList);
        }

        public async Task UpdateAsync(Guid id, UpdateTodoListDto todoList)
        {
            var existingTodoList = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (existingTodoList != null)
            {
                var updatedTodoList = _mapper.Map(todoList, existingTodoList);
                _unitOfWork.TodoListRepository.Update(updatedTodoList);

                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
