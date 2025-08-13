using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Business.Abstract;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodosController(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoItemDto>>> GetAll()
        {
            var todos = await _todoRepository.GetAllTodoAsync();
            return Ok(_mapper.Map<List<TodoItemDto>>(todos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoById(Guid id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TodoItemDto>(todo));
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create(CreateTodoDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTodo = await _todoRepository.CreateTodoAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoDto todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTodo = await _todoRepository.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            await _todoRepository.UpdateTodoAsync(id, todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _todoRepository.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}
