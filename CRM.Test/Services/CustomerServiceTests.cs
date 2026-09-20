using CRM.Application.DTOs.Customer;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CRM.Test.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IRequestRepository> _requestRepositoryMock;
        private readonly CustomerMapper _customerMapper;
        private readonly RequestMapper _requestMapper;
        private readonly CustomerService _sut;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>(MockBehavior.Strict);
            _requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            _customerMapper = new CustomerMapper();
            _requestMapper = new RequestMapper();

            _sut = new CustomerService(_customerRepositoryMock.Object, _requestRepositoryMock.Object, _customerMapper, _requestMapper);
        }

        private static Customer CreateCustomer(
            int id = 1,
            string name = "Company A",
            string contactPerson = "Ana",
            string telephone = "+1 555 000 0000",
            string email = "info@company.com",
            string comment = "Comment")
        {
            return new Customer
            {
                Id = id,
                Name = name,
                ContactPerson = contactPerson,
                Telephone = telephone,
                Email = email,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Request CreateRequest(int id, int customerId)
        {
            return new Request
            {
                Id = id,
                Title = $"Request {id}",
                Description = "Description",
                Status = Status.New,
                Priority = Priority.Medium,
                CustomerId = customerId,
                ManagerId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedList_WhenCustomersExist()
        {
            var customers = new List<Customer> { CreateCustomer(1, "Company A"), CreateCustomer(2, "Company B") };
            _customerRepositoryMock.Setup(x => x.GetAllAsync(null)).ReturnsAsync(customers);

            var result = await _sut.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);
            _customerRepositoryMock.Verify(x => x.GetAllAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoCustomersExist()
        {
            _customerRepositoryMock.Setup(x => x.GetAllAsync(null)).ReturnsAsync(new List<Customer>());

            var result = await _sut.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllActiveAsync_ReturnsMappedSelectDtos()
        {
            var customers = new List<Customer> { CreateCustomer(1), CreateCustomer(2) };
            _customerRepositoryMock.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(customers);

            var result = await _sut.GetAllActiveAsync();

            Xunit.Assert.Equal(2, result.Count);
            _customerRepositoryMock.Verify(x => x.GetAllActiveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCardsAsync_ReturnsListDtos_WithRequestCounts()
        {
            var customers = new List<Customer> { CreateCustomer(1), CreateCustomer(2) };
            _customerRepositoryMock.Setup(x => x.GetAllAsync(null)).ReturnsAsync(customers);
            _requestRepositoryMock.Setup(x => x.CountByCustomerAndStatusAsync(1, null)).ReturnsAsync(5);
            _requestRepositoryMock.Setup(x => x.CountByCustomerAndStatusAsync(1, Status.Active)).ReturnsAsync(2);
            _requestRepositoryMock.Setup(x => x.CountByCustomerAndStatusAsync(2, null)).ReturnsAsync(3);
            _requestRepositoryMock.Setup(x => x.CountByCustomerAndStatusAsync(2, Status.Active)).ReturnsAsync(1);

            var result = await _sut.GetAllCardsAsync();

            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Equal(5, result[0].NumberOfRequests);
            Xunit.Assert.Equal(2, result[0].NumberOfActiveRequests);
            Xunit.Assert.Equal(3, result[1].NumberOfRequests);
            Xunit.Assert.Equal(1, result[1].NumberOfActiveRequests);
            _customerRepositoryMock.Verify(x => x.GetAllAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetAllCardsAsync_PassesSearchTerm_ToRepository()
        {
            var search = "Company";
            _customerRepositoryMock.Setup(x => x.GetAllAsync(search)).ReturnsAsync(new List<Customer>());

            var result = await _sut.GetAllCardsAsync(search);

            Xunit.Assert.Empty(result);
            _customerRepositoryMock.Verify(x => x.GetAllAsync(search), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDetailsDto_WithRequests_WhenCustomerExists()
        {
            var customer = CreateCustomer(1);
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(customer);

            var requests = new List<Request> { CreateRequest(100, 1) };
            _requestRepositoryMock.Setup(x => x.GetByCustomerAsync(1)).ReturnsAsync(requests);

            var result = await _sut.GetByIdAsync(1);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(1, result.Id);
            Xunit.Assert.Single(result.Requests);
            _customerRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
            _requestRepositoryMock.Verify(x => x.GetByCustomerAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
        {
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(999));

            _customerRepositoryMock.Verify(x => x.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CountAsync_ReturnsValueFromRepository()
        {
            _customerRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(7);

            var result = await _sut.CountAsync();

            Xunit.Assert.Equal(7, result);
            _customerRepositoryMock.Verify(x => x.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task ArchiveAsync_CallsRepository()
        {
            _customerRepositoryMock.Setup(x => x.ArchiveAsync(1)).Returns(Task.CompletedTask);

            await _sut.ArchiveAsync(1);

            _customerRepositoryMock.Verify(x => x.ArchiveAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCustomer_WhenDataIsValid()
        {
            var createDto = new CustomerCreateDto
            {
                Name = "Sweet Entertainment",
                ContactPerson = "Ana",
                Telephone = "+1 (555) 839-2041",
                Email = "support@sweet.entertainment",
                Comment = "Big entertainment company in France"
            };

            var createdCustomer = new Customer
            {
                Id = 1,
                Name = createDto.Name,
                ContactPerson = createDto.ContactPerson,
                Telephone = createDto.Telephone,
                Email = createDto.Email,
                Comment = createDto.Comment,
                CreatedAt = DateTime.Now
            };

            _customerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(createdCustomer);

            var result = await _sut.CreateAsync(createDto);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(1, result.Id);
            Xunit.Assert.Equal(createDto.Name, result.Name);
            Xunit.Assert.Equal(createDto.Email, result.Email);

            _customerRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenUpdateSuccessful()
        {
            var updateDto = new CustomerUpdateDto
            {
                Id = 1,
                Name = "Updated Company",
                ContactPerson = "John",
                Telephone = "+373 123456",
                Email = "updated@mail.com",
                Comment = "Updated comment"
            };

            _customerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(true);

            var result = await _sut.UpdateAsync(updateDto);

            Xunit.Assert.True(result);
            _customerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsNotFoundException_WhenUpdateFailed()
        {
            var updateDto = new CustomerUpdateDto { Id = 999, Name = "Unknown" };

            _customerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(false);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateAsync(updateDto));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeleteSuccessful()
        {
            _customerRepositoryMock.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _sut.DeleteAsync(1);

            Xunit.Assert.True(result);
            _customerRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenDeleteFailed()
        {
            _customerRepositoryMock.Setup(x => x.DeleteAsync(999)).ReturnsAsync(false);

            await Xunit.Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(999));
        }
    }
}