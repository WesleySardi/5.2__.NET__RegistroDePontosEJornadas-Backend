using AutoMapper;
using Moq;
using ProjetoBMA.Domain.Entities;
using ProjetoBMA.DTOs.Commands;
using ProjetoBMA.DTOs.Results;
using ProjetoBMA.DTOs.ViewModels;
using ProjetoBMA.Repositories.Interfaces;
using ProjetoBMA.Services;
using Xunit;

namespace ProjetoBMA.Tests.Services
{
    public class TimeEntryServiceTests
    {
        private readonly Mock<ITimeEntryRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly TimeEntryService _service;

        public TimeEntryServiceTests()
        {
            _repoMock = new Mock<ITimeEntryRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateTimeEntryCommand, TimeEntry>();
                cfg.CreateMap<UpdateTimeEntryCommand, TimeEntry>();
                cfg.CreateMap<TimeEntry, TimeEntryResult>();
                cfg.CreateMap<TimeEntry, TimeEntryViewModel>();
            });

            _mapper = config.CreateMapper();
            _service = new TimeEntryService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ReturnsResult()
        {
            var command = new CreateTimeEntryCommand
            {
                EmployeeId = "123",
                EmployeeName = "Teste",
                Timestamp = DateTime.UtcNow,
                Type = "Entrada"
            };

            var result = await _service.CreateAsync(command);

            Assert.NotNull(result);
            Assert.Equal(command.EmployeeId, result.EmployeeId);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<TimeEntry>(), default), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
                     .ReturnsAsync((TimeEntry?)null);

            var result = await _service.DeleteAsync(Guid.NewGuid());

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenFound()
        {
            var entity = new TimeEntry { Id = Guid.NewGuid(), EmployeeId = "123", EmployeeName = "Teste" };
            _repoMock.Setup(r => r.GetByIdAsync(entity.Id, default))
                     .ReturnsAsync(entity);

            var result = await _service.DeleteAsync(entity.Id);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity, default), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }
    }
}
