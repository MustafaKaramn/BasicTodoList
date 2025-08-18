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
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IMapper _mapper;

        public TodoItemsController(ITodoItemService todoItemService, IMapper mapper)
        {
            _todoItemService = todoItemService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TodoItemDto>>> GetAll([FromQuery] TodoQueryParameters queryParameters)
        {
            var todos = await _todoItemService.GetAllAsync(queryParameters);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoById(Guid id)
        {
            var todo = await _todoItemService.GetByIdAsync(id);
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

            var createdTodo = await _todoItemService.CreateAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoItemDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTodo = await _todoItemService.GetByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            await _todoItemService.UpdateAsync(id, todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todo = await _todoItemService.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
