using CRM.Application.DTOs.Customer;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using CRM.Application.Services;
using CRM.DataAccess.Repositories;
using CRM.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _requestRepositoryMock = new Mock<IRequestRepository>();

            _requestMapper = new RequestMapper();

            _customerMapper = new CustomerMapper();


            _customerService = new CustomerService(_customerRepositoryMock.Object, _requestRepositoryMock.Object, _customerMapper, _requestMapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            var result = await _customerService.GetByIdAsync(999);

            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCustomers_WhenCustomersExist()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Company A"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Company B"
                }
            };

            _customerRepositoryMock
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(customers);

            var result = await _customerService.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoCustomersExist()
        {
            _customerRepositoryMock
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(new List<Customer>());

            var result = await _customerService.GetAllAsync();


            Xunit.Assert.NotNull(result);
            Xunit.Assert.Empty(result);
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

            var result = await _customerService.CreateAsync(createDto);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(1, result.Id);
            Xunit.Assert.Equal(createDto.Name, result.Name);
            Xunit.Assert.Equal(createDto.Email, result.Email);

            _customerRepositoryMock.Verify(
                x => x.CreateAsync(It.IsAny<Customer>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSuccessful()
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

            var result = await _customerService.UpdateAsync(updateDto);

            Xunit.Assert.True(result);

            _customerRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Customer>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFailed()
        {
            var updateDto = new CustomerUpdateDto
            {
                Id = 999,
                Name = "Unknown"
            };

            _customerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(false);

            var result = await _customerService.UpdateAsync(updateDto);

            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSuccessful()
        {
            _customerRepositoryMock
                .Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);

            var result = await _customerService.DeleteAsync(1);

            Xunit.Assert.True(result);

            _customerRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenDeleteFailed()
        {
            _customerRepositoryMock
                .Setup(x => x.DeleteAsync(999))
                .ReturnsAsync(false);

            var result = await _customerService.DeleteAsync(999);

            Xunit.Assert.False(result);
        }
    }

}