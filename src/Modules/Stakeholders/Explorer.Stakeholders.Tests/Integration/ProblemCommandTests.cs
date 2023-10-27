﻿using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Stakeholders.Tests;

namespace Explorer.Stakeholders.Tests.Integration;

[Collection("Sequential")]
public class ProblemCommandTests : BaseStakeholdersIntegrationTest
{
    public ProblemCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new ProblemCreateDto
        {
            Category = "Kategorija1",
            Priority = "Bitno",
            Description = "Smislicu",
            DateTime = DateTime.UtcNow,
            TourId = -3,
            TouristId=-1,
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ProblemResponseDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TourId.ShouldBe(newEntity.TourId);
        result.TouristId.ShouldBe(newEntity.TouristId);
        // Assert - Database
        var storedEntity = dbContext.Problem.FirstOrDefault(i => i.TourId == newEntity.TourId && i.TouristId==newEntity.TouristId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ProblemCreateDto
        { 
            Category = "", 
            Priority = "High", 
            Description = "", 
            DateTime = DateTime.UtcNow,
            TourId = 0, 
            TouristId = 0 
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
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new ProblemUpdateDto
        {
            Id = -1,
            Category = "Kategorija4",
            Priority = "Bitno",
            Description = "Nije bilo nekih vecih problema.",
            DateTime = DateTime.UtcNow,
            TourId = 1,
            TouristId = 3,
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ProblemResponseDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Priority.ShouldBe(updatedEntity.Priority);
        result.TouristId.ShouldBe(updatedEntity.TouristId);
        result.DateTime.ShouldBe(updatedEntity.DateTime);
        result.TourId.ShouldBe(updatedEntity.TourId);
        result.Description.ShouldBe(updatedEntity.Description);
        // Assert - Database
        var storedEntity = dbContext.Problem.FirstOrDefault(i => i.Description== "Nije bilo nekih vecih problema.");
        storedEntity.ShouldNotBeNull();
        storedEntity.Priority.ShouldBe(updatedEntity.Priority);
        storedEntity.DateTime.ShouldBe(updatedEntity.DateTime);
        storedEntity.TouristId.ShouldBe(updatedEntity.TouristId);
        storedEntity.TourId.ShouldBe(updatedEntity.TourId);
        var oldEntity = dbContext.Problem.FirstOrDefault(i => i.Description== "Nije ukljuceno u turu sve sto je bilo navedeno.");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ProblemUpdateDto
        {
            Id = -1000,
            Category = "Kategorija1",
            Priority = "Veoma bitno",
            Description = "Nije ukljuceno u turu sve sto je bilo navedeno.",
            DateTime = DateTime.UtcNow,
            TourId = -3,
            TouristId = -5,
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
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Problem.FirstOrDefault(i => i.Id == -3);
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

    private static ProblemController CreateController(IServiceScope scope)
    {
        return new ProblemController(scope.ServiceProvider.GetRequiredService<IProblemService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}