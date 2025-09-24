using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjetoBMA.Controllers;
using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;
using ProjetoBMA.Services.Interfaces;
using Xunit;

namespace ProjetoBMA.Tests.Controllers
{
    public class TimeEntriesControllerTests
    {
        private readonly Mock<ITimeEntryService> _serviceMock;
        private readonly Mock<ILogger<TimeEntriesController>> _loggerMock;
        private readonly TimeEntriesController _controller;

        public TimeEntriesControllerTests()
        {
            _serviceMock = new Mock<ITimeEntryService>();
            _loggerMock = new Mock<ILogger<TimeEntriesController>>();
            _controller = new TimeEntriesController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var expected = new TimeEntryViewModel { Id = id, EmployeeId = "123", EmployeeName = "Teste" };
            _serviceMock.Setup(s => s.GetByIdAsync(id, default)).ReturnsAsync(expected);

            var result = await _controller.GetById(id, default);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<TimeEntryViewModel>(ok.Value);
            Assert.Equal(expected.Id, model.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id, default)).ReturnsAsync((TimeEntryViewModel?)null);

            var result = await _controller.GetById(id, default);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var command = new CreateTimeEntryCommand { EmployeeId = "123", EmployeeName = "Teste", Timestamp = DateTime.UtcNow, Type = Domain.Enums.TimeEntryType.Entrada };
            var created = new TimeEntryResult { Id = Guid.NewGuid(), EmployeeId = "123", EmployeeName = "Teste" };

            _serviceMock.Setup(s => s.CreateAsync(command, default)).ReturnsAsync(created);

            var result = await _controller.Create(command, default);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<TimeEntryResult>(createdAt.Value);
            Assert.Equal(created.Id, model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id, default)).ReturnsAsync(true);

            var result = await _controller.Delete(id, default);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id, default)).ReturnsAsync(false);

            var result = await _controller.Delete(id, default);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
