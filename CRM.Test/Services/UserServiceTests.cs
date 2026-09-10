using CRM.Application.DTOs.User;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using CRM.Application.Services;
using CRM.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CRM.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IHasherService> _hasherServiceMock;
        private readonly UserMapper _userMapper;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _hasherServiceMock = new Mock<IHasherService>(MockBehavior.Strict);
            _userMapper = new UserMapper();
            _sut = new UserService(_userRepositoryMock.Object, _hasherServiceMock.Object, _userMapper);
        }

        private static User CreateUser(
            int id = 1,
            string firstName = "John",
            string lastName = "Doe",
            string email = "john.doe@test.com",
            string passwordHash = "hashed")
        {
            return new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash
            };
        }

        [Fact]
        public async Task CountAsync_ReturnsValueFromRepository()
        {
            _userRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(5);

            var result = await _sut.CountAsync();

            Xunit.Assert.Equal(5, result);
            _userRepositoryMock.Verify(r => r.CountAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_HashesPassword_CallsRepository_AndReturnsMappedDto()
        {
            var dto = new UserCreateDto
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@test.com",
                Password = "plainPassword"
            };

            _hasherServiceMock.Setup(h => h.HashPassword(dto.Password)).Returns("hashedPassword");
            _userRepositoryMock
                .Setup(r => r.AddAsync(It.Is<User>(u =>
                    u.FirstName == dto.FirstName &&
                    u.LastName == dto.LastName &&
                    u.Email == dto.Email &&
                    u.PasswordHash == "hashedPassword")))
                .ReturnsAsync(true);

            var result = await _sut.AddAsync(dto);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(dto.FirstName, result.FirstName);
            Xunit.Assert.Equal(dto.Email, result.Email);
            _hasherServiceMock.Verify(h => h.HashPassword(dto.Password), Times.Once);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_MapsDtoToDomain_AndReturnsTrue_WhenUpdateSucceeds()
        {
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = "Updated",
                LastName = "Name",
                Email = "updated@test.com"
            };

            _userRepositoryMock
                .Setup(r => r.UpdateAsync(It.Is<User>(u =>
                    u.Id == dto.Id &&
                    u.FirstName == dto.FirstName &&
                    u.Email == dto.Email)))
                .ReturnsAsync(true);

            var result = await _sut.UpdateAsync(dto);

            Xunit.Assert.True(result);
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenUserNotFound()
        {
            var dto = new UserUpdateDto { Id = 999, FirstName = "Nobody" };

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(false);

            var result = await _sut.UpdateAsync(dto);

            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenRepositorySucceeds()
        {
            _userRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _sut.DeleteAsync(1);

            Xunit.Assert.True(result);
            _userRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _sut.DeleteAsync(999);

            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedDto_WhenUserExists()
        {
            var user = CreateUser(id: 5, firstName: "Found");
            _userRepositoryMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(user);

            var result = await _sut.GetByIdAsync(5);

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(5, result!.Id);
            Xunit.Assert.Equal("Found", result.FirstName);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            var result = await _sut.GetByIdAsync(999);

            Xunit.Assert.Null(result);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedList_WhenRepositoryReturnsUsers()
        {
            var users = new List<User> { CreateUser(1), CreateUser(2) };
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _sut.GetAllAsync();

            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Equal(users[0].Id, result[0].Id);
            _userRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            var result = await _sut.GetAllAsync();

            Xunit.Assert.Empty(result);
        }
    }
}