﻿using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration;
[Collection("Sequential")]
public class TourCommandTests : BaseToursIntegrationTest
{
    public TourCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourCreateDto
        {
            AuthorId = 1,
            Name = "Tura Novog Sada",
            Description = "The best!",
            Difficulty = 3,
            Tags = new List<string> { "istorija", "kultura" },
            /*Status = TourStatus.Draft,
            Price = 0,
            IsDeleted = false*/
        };

        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.AuthorId.ShouldBe(newEntity.AuthorId);
        result.Name.ShouldBe(newEntity.Name);
        result.Description.ShouldBe(newEntity.Description);
        result.Difficulty.ShouldBe(newEntity.Difficulty);
        result.Price.ShouldBe(newEntity.Price);
        result.Status.ShouldBe(newEntity.Status);
        result.IsDeleted.ShouldBe(newEntity.IsDeleted);
        result.Tags.ShouldBe(newEntity.Tags);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.Name.ShouldBe(result.Name);
        storedEntity.Description.ShouldBe(result.Description);
        storedEntity.Difficulty.ShouldBe(result.Difficulty);
        storedEntity.Tags.ShouldBe(result.Tags);
        storedEntity.Price.ShouldBe(result.Price);
        storedEntity.Status.ToString().ShouldBe(result.Status.ToString());
        storedEntity.IsDeleted.ShouldBe(result.IsDeleted);
    }



    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourCreateDto
        {
            Description = "Test"
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = new TourUpdateDto
        {
            Id = -1,
            AuthorId = 1,
            Name = "Tečnost",
            Description = "The best!",
            Difficulty = 5,
            Tags = new List<string> { "sport", "priroda" },
            Status = TourStatus.Draft,
            Price = 0,
            IsDeleted = false
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.AuthorId.ShouldBe(1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);
        result.Difficulty.ShouldBe(updatedEntity.Difficulty);
        result.Tags.ShouldBe(updatedEntity.Tags);
        result.Price.ShouldBe(updatedEntity.Price);
        result.Status.ShouldBe(updatedEntity.Status);
        result.IsDeleted.ShouldBeFalse();
        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == "Tečnost");
        storedEntity.ShouldNotBeNull();
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        storedEntity.Difficulty.ShouldBe(updatedEntity.Difficulty);
        storedEntity.Tags.ShouldBe(updatedEntity.Tags);
        storedEntity.Price.ShouldBe(updatedEntity.Price);
        storedEntity.Status.ToString().ShouldBe(updatedEntity.Status.ToString());
        storedEntity.IsDeleted.ShouldBe(updatedEntity.IsDeleted);
        var oldEntity = dbContext.Tours.FirstOrDefault(i => i.Name == "Voda");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourUpdateDto
        {
            Id = -1000,
            Name = "Test",
            Description = "Nes",
            Difficulty = 1,
            Tags = new List<string> { "sport", "priroda" }
        };

        // Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Tours.FirstOrDefault(i => i.Id == -3);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.Delete(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }



    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }


}