using AutoMapper;
using FluentAssertions;
using Moq;
using TodoList.Business.DTOs.TodoItemDTOs;
using TodoList.Business.Services;
using TodoList.Core.Entities;
using TodoList.DataAccess.Interfaces;

namespace TodoList.Business.Tests
{
    public class TodoItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TodoItemService _service;

        public TodoItemServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _service = new TodoItemService(_mockMapper.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTodoDto_WhenTodoExists()
        {
            var testId = Guid.NewGuid();
            var testTodoItem = new TodoItem { Id = testId, Title = "Test Todo", IsCompleted = false, CreatedDate = DateTime.Now };
            var testTodoDto = new TodoItemDto { Id = testId, Title = "Test Todo", IsCompleted = false, CreatedDate = DateTime.Now };

            _mockUnitOfWork.Setup(uow => uow.TodoItemRepository.GetByIdAsync(testId)).ReturnsAsync(testTodoItem);
            _mockMapper.Setup(m => m.Map<TodoItemDto>(testTodoItem)).Returns(testTodoDto);

            //Act
            var result = await _service.GetTodoByIdAsync(testId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TodoItemDto>();
            result.Id.Should().Be(testId);
            result.Title.Should().Be("Test Todo");
        }

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
        {
            var testId = Guid.NewGuid();

            _mockUnitOfWork.Setup(uow => uow.TodoItemRepository.GetByIdAsync(testId)).ReturnsAsync((TodoItem)null);

            //Act
            var result = await _service.GetTodoByIdAsync(testId);

            //Assert
            result.Should().BeNull();
        }
    }
}
