using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Business.Interfaces;
using TodoList.Core.Entities;
using TodoList.Core.Helpers;
using TodoList.Business.DTOs.TodoItemDTOs;
using Microsoft.AspNetCore.Authorization;
using TodoList.API.Extensions;

namespace TodoList.API.Controllers
{
    [Authorize]
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
            var userId = User.GetUserId();

            var todos = await _todoItemService.GetAllAsync(queryParameters, userId);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoById(Guid id)
        {
            var userId = User.GetUserId();

            var todo = await _todoItemService.GetByIdAsync(id, userId);
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

            var userId = User.GetUserId();

            var createdTodo = await _todoItemService.CreateAsync(todoItem, userId);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoItemDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserId();

            var existingTodo = await _todoItemService.GetByIdAsync(id, userId);
            if (existingTodo == null)
            {
                return NotFound();
            }

            await _todoItemService.UpdateAsync(id, todoItem, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();

            var todo = await _todoItemService.GetByIdAsync(id, userId);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoItemService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
