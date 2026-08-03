using System.Net.Http.Json;
using System.Net;
using TransactionService.Application.DTOs.Users;
using TransactionService.IntegrationTests.Fixtures;
using FluentAssertions;

namespace TransactionService.IntegrationTests.Controllers
{
    public class UsersControllerTests : IntegrationTestBase
    {
        public UsersControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
            
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Name = "Yasemin"
            };
            await Factory.ResetDatabaseAsync();

            // Act
            var response = await Client.PostAsJsonAsync("/api/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await response.Content.ReadFromJsonAsync<UserResponse>();

            user.Should().NotBeNull();
            user!.Name.Should().Be("Yasemin");
            user.Id.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser()
        {
            await Factory.ResetDatabaseAsync();

            var create = await Client.PostAsJsonAsync("/api/users",
                new CreateUserRequest
                {
                    Name = "Yasemin"
                });

            var created = await create.Content.ReadFromJsonAsync<UserResponse>();

            var response = await Client.GetAsync($"/api/users/{created!.Id}");

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserResponse>();

            user.Should().NotBeNull();
            user!.Id.Should().Be(created.Id);
            user.Name.Should().Be("Yasemin");
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUser()
        {
            await Factory.ResetDatabaseAsync();

            var create = await Client.PostAsJsonAsync("/api/users",
                new CreateUserRequest
                {
                    Name = "Yasemin"
                });

            var created = await create.Content.ReadFromJsonAsync<UserResponse>();

            var request = new UpdateUserRequest
            {
                Name = "Yasemin Albayrak"
            };

            var response = await Client.PutAsJsonAsync($"/api/users/{created!.Id}", request);

            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<UserResponse>();

            updated!.Name.Should().Be("Yasemin Albayrak");
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUser()
        {
            await Factory.ResetDatabaseAsync();

            var create = await Client.PostAsJsonAsync("/api/users",
                new CreateUserRequest
                {
                    Name = "Yasemin"
                });

            var created = await create.Content.ReadFromJsonAsync<UserResponse>();

            var delete = await Client.DeleteAsync($"/api/users/{created!.Id}");

            delete.EnsureSuccessStatusCode();

            var get = await Client.GetAsync($"/api/users/{created.Id}");

            get.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
