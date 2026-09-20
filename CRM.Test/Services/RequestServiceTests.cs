using CRM.Application.DTOs.Request;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CRM.Test.Services
{
    public class RequestServiceTests
    {
        private readonly Mock<IRequestRepository> _repositoryMock;
        private readonly RequestMapper _mapper;
        private readonly RequestService _sut;

        public RequestServiceTests()
        {
            _repositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            _mapper = new RequestMapper();
            _sut = new RequestService(_repositoryMock.Object, _mapper);
        }

        private static Request CreateRequest(
            int id = 1,
            string title = "Test title",
            string description = "Test description",
            Status status = Status.New,
            Priority priority = Priority.Medium,
            int customerId = 10,
            int managerId = 20)
        {
            return new Request
            {
                Id = id,
                Title = title,
                Description = description,
                Status = status,
                Priority = priority,
                CustomerId = customerId,
                ManagerId = managerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FinishedAt = null
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedList_WhenRepositoryReturnsRequests()
        {
            var requests = new List<Request> { CreateRequest(1), CreateRequest(2) };
            _repositoryMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(requests);

            var result = await _sut.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Equal(requests[0].Id, result[0].Id);
            Xunit.Assert.Equal(requests[0].Title, result[0].Title);
            _repositoryMock.Verify(r => r.GetAllAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            _repositoryMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(new List<Request>());

            var result = await _sut.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_PassesStatusFilter_ToRepository()
        {
            var requests = new List<Request> { CreateRequest(1, status: Status.Active) };
            _repositoryMock.Setup(r => r.GetAllAsync(Status.Active)).ReturnsAsync(requests);

            var result = await _sut.GetAllAsync(Status.Active);

            Xunit.Assert.Single(result);
            _repositoryMock.Verify(r => r.GetAllAsync(Status.Active), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenRequestExists()
        {
            var request = CreateRequest(id: 5, title: "Found");
            _repositoryMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(request);

            var result = await _sut.GetByIdAsync(5);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(5, result!.Id);
            Xunit.Assert.Equal("Found", result.Title);
            _repositoryMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsNotFoundException_WhenRequestDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Request?)null);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(999));

            _repositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetByUserAsync_ReturnsMappedList_ForGivenUser()
        {
            var userId = 42;
            var requests = new List<Request> { CreateRequest(1, managerId: userId) };
            _repositoryMock.Setup(r => r.GetByUserAsync(userId, null)).ReturnsAsync(requests);

            var result = await _sut.GetByUserAsync(userId);

            Xunit.Assert.Single(result);
            _repositoryMock.Verify(r => r.GetByUserAsync(userId, null), Times.Once);
        }

        [Fact]
        public async Task GetByUserAsync_PassesStatusFilter_ToRepository()
        {
            var userId = 42;
            var requests = new List<Request> { CreateRequest(1, managerId: userId, status: Status.Finished) };
            _repositoryMock.Setup(r => r.GetByUserAsync(userId, Status.Finished)).ReturnsAsync(requests);

            var result = await _sut.GetByUserAsync(userId, Status.Finished);

            Xunit.Assert.Single(result);
            _repositoryMock.Verify(r => r.GetByUserAsync(userId, Status.Finished), Times.Once);
        }

        [Fact]
        public async Task GetRecentByUserAsync_ReturnsMappedRecentDtos()
        {
            var userId = 42;
            var requests = new List<Request> { CreateRequest(1, managerId: userId), CreateRequest(2, managerId: userId) };
            _repositoryMock.Setup(r => r.GetRecentByUserAsync(userId, 10)).ReturnsAsync(requests);

            var result = await _sut.GetRecentByUserAsync(userId);

            Xunit.Assert.Equal(2, result.Count);
            _repositoryMock.Verify(r => r.GetRecentByUserAsync(userId, 10), Times.Once);
        }

        [Fact]
        public async Task GetRecentByUserAsync_ReturnsEmptyList_WhenNoRequests()
        {
            var userId = 42;
            _repositoryMock.Setup(r => r.GetRecentByUserAsync(userId , 10)).ReturnsAsync(new List<Request>());

            var result = await _sut.GetRecentByUserAsync(userId);

            Xunit.Assert.Empty(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(Status.New)]
        [InlineData(Status.Active)]
        [InlineData(Status.Finished)]
        public async Task CountByUserAndStatusAsync_PassesParametersThrough_AndReturnsCount(Status? status)
        {
            var userId = 7;
            _repositoryMock
                .Setup(r => r.CountByUserAndStatusAsync(userId, status))
                .ReturnsAsync(3);

            var result = await _sut.CountByUserAndStatusAsync(userId, status);

            Xunit.Assert.Equal(3, result);
            _repositoryMock.Verify(r => r.CountByUserAndStatusAsync(userId, status), Times.Once);
        }

        [Fact]
        public async Task AddAsync_MapsDtoToDomain_CallsRepository_AndReturnsMappedDetailsDto()
        {
            var createDto = new RequestCreateDto
            {
                Title = "New request",
                Description = "New description",
                Priority = Priority.High,
                CustomerId = 10,
                ManagerId = 20
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.Is<Request>(req =>
                    req.Title == createDto.Title &&
                    req.Description == createDto.Description &&
                    req.CustomerId == createDto.CustomerId)))
                .ReturnsAsync(true);

            var result = await _sut.AddAsync(createDto);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(createDto.Title, result.Title);
            Xunit.Assert.Equal(createDto.Description, result.Description);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Request>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_MapsDtoToDomain_AndReturnsTrue_WhenUpdateSucceeds()
        {
            var updateDto = new RequestUpdateDto
            {
                Id = 1,
                Title = "Updated title",
                Description = "Updated description",
                Priority = Priority.Low,
            };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.Is<Request>(req =>
                    req.Id == updateDto.Id &&
                    req.Title == updateDto.Title)))
                .ReturnsAsync(true);

            var result = await _sut.UpdateAsync(updateDto);

            Xunit.Assert.True(result);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Request>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsNotFoundException_WhenUpdateFails()
        {
            var updateDto = new RequestUpdateDto
            {
                Id = 999,
                Title = "Unknown",
                Description = "Unknown",
                Priority = Priority.Low,
            };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Request>()))
                .ReturnsAsync(false);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateAsync(updateDto));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenRepositorySucceeds()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _sut.DeleteAsync(1);

            Xunit.Assert.True(result);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenRequestNotFound()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(999));
        }

        [Fact]
        public async Task ChangeStatusAsync_DelegatesToRepository_WithAuthorName_AndReturnsResult()
        {
            _repositoryMock
                .Setup(r => r.ChangeStatusAsync(1, Status.Finished, "Ana"))
                .ReturnsAsync(true);

            var result = await _sut.ChangeStatusAsync(1, Status.Finished, "Ana");

            Xunit.Assert.True(result);
            _repositoryMock.Verify(r => r.ChangeStatusAsync(1, Status.Finished, "Ana"), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_ReturnsFalse_WhenRequestNotFound()
        {
            _repositoryMock
                .Setup(r => r.ChangeStatusAsync(999, Status.Finished, "Ana"))
                .ReturnsAsync(false);

            var result = await _sut.ChangeStatusAsync(999, Status.Finished, "Ana");

            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task SetManagerAsync_DelegatesToRepository_AndReturnsResult()
        {
            _repositoryMock
                .Setup(r => r.SetManagerAsync(1, 20))
                .ReturnsAsync(true);

            var result = await _sut.SetManagerAsync(1, 20);

            Xunit.Assert.True(result);
            _repositoryMock.Verify(r => r.SetManagerAsync(1, 20), Times.Once);
        }

        [Fact]
        public async Task SetManagerAsync_ReturnsFalse_WhenRequestOrManagerNotFound()
        {
            _repositoryMock
                .Setup(r => r.SetManagerAsync(999, 999))
                .ReturnsAsync(false);

            var result = await _sut.SetManagerAsync(999, 999);

            Xunit.Assert.False(result);
        }
    }
}