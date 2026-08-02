using AutoMapper;
using FluentAssertions;
using Moq;
using TransactionService.Application.DTOs.Users;
using TransactionService.Application.Managers.Implementations;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Repositories.Interfaces;

namespace TransactionService.UnitTests.Managers
{
    public class UserManagerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserManager _userManager;

        private const string UserId = "358142be-984b-4304-8c4f-c1364f1477cd";

        public UserManagerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userManager = new UserManager(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateUser_WhenRequestIsValid()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Name = "Yasemin"
            };

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            _mapperMock
                .Setup(x => x.Map<User>(It.IsAny<CreateUserRequest>()))
                .Returns((CreateUserRequest request) =>
                    new User
                    {
                        Name = request.Name,
                    });

            _mapperMock
                .Setup(x => x.Map<UserResponse>(It.IsAny<User>()))
                .Returns((User user) =>
                    new UserResponse
                    {
                        Id = user.Id,
                        Name = user.Name
                    });

            // Act
            var result = await _userManager.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.Id.Should().HaveLength(36);

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Yasemin"
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "İnci"
                }
            };

            _userRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(users);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<UserResponse>>(It.IsAny<IEnumerable<User>>()))
                .Returns((IEnumerable<User> users) =>
                    users.Select(x => new UserResponse
                    {
                        Id = x.Id,
                        Name = x.Name
                    }));

            // Act
            var result = await _userManager.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);

            _userRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = UserId,
                Name = "Yasemin"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync(user);

            _mapperMock
               .Setup(x => x.Map<UserResponse>(It.IsAny<User>()))
               .Returns((User user) =>
                   new UserResponse
                   {
                       Id = user.Id,
                       Name = user.Name
                   });

            // Act
            var result = await _userManager.GetByIdAsync(UserId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(UserId);
            result.Name.Should().Be("Yasemin");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> action = () => _userManager.GetByIdAsync(UserId);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>();

            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(UserId),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = UserId,
                Name = "Yasemin"
            };

            var request = new UpdateUserRequest
            {
                Name = "Yasemin Albayrak"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            _mapperMock
                .Setup(x => x.Map<User>(It.IsAny<UpdateUserRequest>()))
                .Returns((UpdateUserRequest request) =>
                    new User
                    {
                        Name = request.Name,
                    });

            _mapperMock
                .Setup(x => x.Map<UserResponse>(It.IsAny<User>()))
                .Returns((User user) =>
                    new UserResponse
                    {
                        Id = user.Id,
                        Name = request.Name
                    });

            // Act
            var result = await _userManager.UpdateAsync(UserId, request);

            // Assert
            result.Id.Should().Be(UserId);
            result.Name.Should().Be("Yasemin Albayrak");

            _userRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Name = "Yasemin Albayrak"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> action = () => _userManager.UpdateAsync(UserId, request);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = UserId,
                Name = "Yasemin"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.DeleteAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _userManager.DeleteAsync(UserId);

            // Assert
            _userRepositoryMock.Verify(
                x => x.DeleteAsync(user),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> action = () => _userManager.DeleteAsync(UserId);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }
    }
}
