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
        private readonly Mock<ICustomerRepository> _repositoryMock;

        private readonly CustomerMapper _mapper;

        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();

            _mapper = new CustomerMapper();

            _service = new CustomerService(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            var customer = new Customer()
            {
                Id = 1,
                Name = "Sweet Entertainment",
                ContactPerson = "Ana",
                Telephone = "+1 (555) 839-2041",
                Email = "support@sweet.entertainment",
                Comment = "Big entertainment company in France",
                CreatedAt = DateTime.Now
            };

            var dto = new CustomerDetailsDto()
            {
                Id = 1,
                Name = "Sweet Entertainment",
                ContactPerson = "Ana",
                Telephone = "+1 (555) 839-2041",
                Email = "support@sweet.entertainment",
                Comment = "Big entertainment company in France"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(customer);

            var result = await _service.GetByIdAsync(1);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(1, result.Id);
            Xunit.Assert.Equal("Sweet Entertainment", result.Name);
            Xunit.Assert.Equal("Ana", result.ContactPerson);
            Xunit.Assert.Equal("+1 (555) 839-2041", result.Telephone);
            Xunit.Assert.Equal("support@sweet.entertainment", result.Email);
            Xunit.Assert.Equal("Big entertainment company in France", result.Comment);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            _repositoryMock.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            var result = await _service.GetByIdAsync(999);

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

            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(customers);

            var result = await _service.GetAllAsync();

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoCustomersExist()
        {
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Customer>());

            var result = await _service.GetAllAsync();


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

            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(createdCustomer);

            var result = await _service.CreateAsync(createDto);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(1, result.Id);
            Xunit.Assert.Equal(createDto.Name, result.Name);
            Xunit.Assert.Equal(createDto.Email, result.Email);

            _repositoryMock.Verify(
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

            _repositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(true);

            var result = await _service.UpdateAsync(updateDto);

            Xunit.Assert.True(result);

            _repositoryMock.Verify(
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

            _repositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(false);

            var result = await _service.UpdateAsync(updateDto);

            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSuccessful()
        {
            _repositoryMock
                .Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            Xunit.Assert.True(result);

            _repositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenDeleteFailed()
        {
            _repositoryMock
                .Setup(x => x.DeleteAsync(999))
                .ReturnsAsync(false);

            var result = await _service.DeleteAsync(999);

            Xunit.Assert.False(result);
        }
    }

}
