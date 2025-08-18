using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Business.DTOs.TodoListDTOs;
using TodoList.Business.Interfaces;

namespace TodoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListService todoListService;

        public TodoListsController(ITodoListService todoListService)
        {
            this.todoListService = todoListService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAll()
        {
            var todoLists = await todoListService.GetAllAsync();
            return Ok(todoLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> GetById(Guid id)
        {
            var todoList = await todoListService.GetByIdAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }
            return Ok(todoList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todoList = await todoListService.GetByIdAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            await todoListService.DeleteAsync(id);
            return NoContent();
        }
    }
}
