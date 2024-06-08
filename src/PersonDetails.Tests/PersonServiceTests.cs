using AutoMapper;
using Moq;
using PersonDetails.Api.Data.Entities;
using PersonDetails.Api.Data.Repos;
using PersonDetails.Api.Models;
using PersonDetails.Api.Services;

namespace PersonDetails.Tests;

public class PersonServiceTests
{
    [Fact]
    public async Task GetAllPersonsAsync_ShouldReturnAllPersons()
    {
        var mockRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();

        var dummyPersons = new List<Person>
        {
            new Person
            {
                Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA"
            },
            new Person
            {
                Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA"
            }
        };

        var dummyPersonResponseModels = new List<PersonResponseModel>
        {
            new PersonResponseModel
            {
                FirstName = "John", LastName = "Doe", TelephoneCode = "123", TelephoneNumber = "1234567890",
                Address = "123 Elm Street", Country = "USA"
            },
            new PersonResponseModel
            {
                FirstName = "Jane", LastName = "Doe", TelephoneCode = "123", TelephoneNumber = "0987654321",
                Address = "456 Oak Avenue", Country = "USA"
            }
        };

        mockRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(dummyPersons);

        mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseModel>>(It.IsAny<IEnumerable<Person>>()))
            .Returns(dummyPersonResponseModels);

        var service = new PersonService(new List<IPersonRepository> { mockRepo.Object }, mockMapper.Object);

        var result = await service.GetAllPersonsAsync(string.Empty);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        var resultList = result.ToList();
        Assert.Equal("John", resultList[0].FirstName);
        Assert.Equal("Jane", resultList[1].FirstName);
    }
    [Fact]
    public async Task GetAllPersonsAsync_WithFilter_ShouldReturnFilteredPersons()
    {
        var mockRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();

        var dummyPersons = new List<Person>
            {
                new Person
                {
                    Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA"
                },
                new Person
                {
                    Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA"
                }
            };

        var dummyPersonResponseModels = new List<PersonResponseModel>
            {
                new PersonResponseModel
                {
                    FirstName = "John", LastName = "Doe", TelephoneCode = "123", TelephoneNumber = "1234567890",
                    Address = "123 Elm Street", Country = "USA"
                },
                new PersonResponseModel
                {
                    FirstName = "Jane", LastName = "Doe", TelephoneCode = "123", TelephoneNumber = "0987654321",
                    Address = "456 Oak Avenue", Country = "USA"
                }
            };

        mockRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>()))
           .ReturnsAsync((string filter) => dummyPersons.Where(p => p.Name.Contains(filter)).ToList());

        mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseModel>>(It.IsAny<IEnumerable<Person>>()))
           .Returns((IEnumerable<Person> persons) =>
               persons.Select(p => new PersonResponseModel
               {
                   FirstName = p.Name.Split(' ')[0],
                   LastName = p.Name.Split(' ')[1],
                   TelephoneCode = "123",
                   TelephoneNumber = p.TelephoneNumber,
                   Address = p.Address,
                   Country = p.Country
               }).ToList());

        var service = new PersonService(new List<IPersonRepository> { mockRepo.Object }, mockMapper.Object);

        var result = await service.GetAllPersonsAsync("John");

        Assert.Single(result);
        Assert.Equal("Doe", result.First().LastName);
    }

    [Fact]
    public async Task GetAllPersonsAsync_ShouldReturnAllPersons_FromAllRepositories()
    {
        // Arrange
        var mockSqlRepo = new Mock<IPersonRepository>();
        var mockCsvRepo = new Mock<IPersonRepository>();
        var mockMongoRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();

        var sqlPersons = new List<Person>
    {
        new Person { Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA" }
    };

        var csvPersons = new List<Person>
    {
        new Person { Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA" }
    };

        var mongoPersons = new List<Person>
    {
        new Person { Name = "Alice Smith", TelephoneNumber = "5555555555", Address = "789 Maple Street", Country = "USA" }
    };

        mockSqlRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(sqlPersons);
        mockCsvRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(csvPersons);
        mockMongoRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(mongoPersons);

        var combinedPersons = sqlPersons.Concat(csvPersons).Concat(mongoPersons).ToList();

        mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseModel>>(It.IsAny<IEnumerable<Person>>()))
            .Returns((IEnumerable<Person> persons) => persons.Select(p => new PersonResponseModel
            {
                FirstName = p.Name.Split(' ')[0],
                LastName = p.Name.Split(' ')[1],
                TelephoneCode = "123",
                TelephoneNumber = p.TelephoneNumber,
                Address = p.Address,
                Country = p.Country
            }).ToList());

        var service = new PersonService(new List<IPersonRepository> { mockSqlRepo.Object, mockCsvRepo.Object, mockMongoRepo.Object }, mockMapper.Object);

        // Act
        var result = await service.GetAllPersonsAsync(string.Empty);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Contains(result, p => p.FirstName == "John" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "Jane" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "Alice" && p.LastName == "Smith");
    }
    [Fact]
    public async Task GetAllPersonsAsync_FilterMatchingMultiplePersonsAcrossRepositories_ShouldReturnFilteredPersons()
    {
        // Arrange
        var mockSqlRepo = new Mock<IPersonRepository>();
        var mockCsvRepo = new Mock<IPersonRepository>();
        var mockMongoRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();

        var sqlPersons = new List<Person>
    {
        new Person { Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA" }
    };

        var csvPersons = new List<Person>
    {
        new Person { Name = "John Smith", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA" }
    };

        var mongoPersons = new List<Person>
    {
        new Person { Name = "Alice Smith", TelephoneNumber = "5555555555", Address = "789 Maple Street", Country = "USA" }
    };

        mockSqlRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(sqlPersons);
        mockCsvRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(csvPersons);
        mockMongoRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(mongoPersons);

        mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseModel>>(It.IsAny<IEnumerable<Person>>()))
            .Returns((IEnumerable<Person> persons) => persons.Select(p => new PersonResponseModel
            {
                FirstName = p.Name.Split(' ')[0],
                LastName = p.Name.Split(' ')[1],
                TelephoneCode = "123",
                TelephoneNumber = p.TelephoneNumber,
                Address = p.Address,
                Country = p.Country
            }).ToList());

        var service = new PersonService(new List<IPersonRepository> { mockSqlRepo.Object, mockCsvRepo.Object }, mockMapper.Object);

        // Act
        var result = await service.GetAllPersonsAsync("John");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.FirstName == "John" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "John" && p.LastName == "Smith");
    }
    [Fact]
    public async Task GetAllPersonsAsync_NullFilter_ShouldReturnAllPersons()
    {
        // Arrange
        var mockSqlRepo = new Mock<IPersonRepository>();
        var mockCsvRepo = new Mock<IPersonRepository>();
        var mockMongoRepo = new Mock<IPersonRepository>();
        var mockMapper = new Mock<IMapper>();

        var sqlPersons = new List<Person>
    {
        new Person { Name = "John Doe", TelephoneNumber = "1234567890", Address = "123 Elm Street", Country = "USA" }
    };

        var csvPersons = new List<Person>
    {
        new Person { Name = "Jane Doe", TelephoneNumber = "0987654321", Address = "456 Oak Avenue", Country = "USA" }
    };

        var mongoPersons = new List<Person>
    {
        new Person { Name = "Alice Smith", TelephoneNumber = "5555555555", Address = "789 Maple Street", Country = "USA" }
    };

        mockSqlRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(sqlPersons);
        mockCsvRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(csvPersons);
        mockMongoRepo.Setup(repo => repo.GetPersonsAsync(It.IsAny<string>())).ReturnsAsync(mongoPersons);

        var combinedPersons = sqlPersons.Concat(csvPersons).Concat(mongoPersons).ToList();

        mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseModel>>(It.IsAny<IEnumerable<Person>>()))
            .Returns((IEnumerable<Person> persons) => persons.Select(p => new PersonResponseModel
            {
                FirstName = p.Name.Split(' ')[0],
                LastName = p.Name.Split(' ')[1],
                TelephoneCode = "123",
                TelephoneNumber = p.TelephoneNumber,
                Address = p.Address,
                Country = p.Country
            }).ToList());

        var service = new PersonService(new List<IPersonRepository> { mockSqlRepo.Object, mockCsvRepo.Object, mockMongoRepo.Object }, mockMapper.Object);

        // Act
        var result = await service.GetAllPersonsAsync(null);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Contains(result, p => p.FirstName == "John" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "Jane" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "Alice" && p.LastName == "Smith");
    }

}
