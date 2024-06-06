using AutoMapper;
using Moq;
using PersonDetails.Api.Data.Entities;
using PersonDetails.Api.Data.Repos;
using PersonDetails.Api.Services;

namespace PersonDetails.Tests;

public class PersonServiceTests
{
    [Fact]
    public async Task GetAllPersonsAsync_ShouldReturnAllPersons()
    {
        // Arrange
        var mockRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();
        mockRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Person>
            {
                new Person { Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA" },
                new Person { Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA" }
            });

        var service = new PersonService(new List<IPersonRepository> { mockRepo.Object },mockMapper.Object);

        // Act
        var result = await service.GetAllPersonsAsync(string.Empty);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllPersonsAsync_WithFilter_ShouldReturnFilteredPersons()
    {
        // Arrange
        var mockRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();
        mockRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Person>
            {
                new Person { Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA" },
                new Person { Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA" }
            });

        var service = new PersonService(new List<IPersonRepository> { mockRepo.Object }, mockMapper.Object);

        // Act
        var result = await service.GetAllPersonsAsync("John");

        // Assert
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);
    }
}