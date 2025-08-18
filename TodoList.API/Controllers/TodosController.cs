using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Business.Interfaces;
using TodoList.Core.Entities;
using TodoList.Core.Helpers;
using TodoList.Business.DTOs.TodoItemDTOs;

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemService _todoService;
        private readonly IMapper _mapper;

        public TodosController(ITodoItemService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TodoItemDto>>> GetAll([FromQuery] TodoQueryParameters queryParameters)
        {
            var todos = await _todoService.GetAllTodoAsync(queryParameters);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoById(Guid id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TodoItemDto>(todo));
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create(CreateTodoItemDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTodo = await _todoService.CreateTodoAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoItemDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTodo = await _todoService.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            await _todoService.UpdateTodoAsync(id, todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}
