using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.API.Services.Interfaces;
using TodoList.Business.DTOs.TodoListDTOs;
using TodoList.Business.Interfaces;

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        private readonly IFileService _fileService;

        public TodoListsController(ITodoListService todoListService, IFileService fileService)
        {
            _todoListService = todoListService;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateTodoListDto createTodoListDto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? imageUrl = null;

            if (file != null && file.Length > 0)
            {
                try
                {
                    imageUrl = await _fileService.SaveFileAsync(file, "TodoListImages");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var createdTodoList = await _todoListService.CreateAsync(createTodoListDto, imageUrl);

            return CreatedAtAction(nameof(GetById), new { id = createdTodoList.Id }, createdTodoList);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAll()
        {
            var todoLists = await _todoListService.GetAllAsync();
            return Ok(todoLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> GetById(Guid id)
        {
            var todoList = await _todoListService.GetByIdAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }
            return Ok(todoList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todoList = await _todoListService.GetByIdAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            await _todoListService.DeleteAsync(id);
            _fileService.DeleteFileAsync(todoList.ImageUrl);
            
            return NoContent();
        }
    }
}
