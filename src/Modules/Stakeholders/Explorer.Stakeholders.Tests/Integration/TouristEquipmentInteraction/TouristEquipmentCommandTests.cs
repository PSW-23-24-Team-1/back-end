﻿using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos.TouristEquipment;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Stakeholders.Tests.Integration.TouristEquipmentInteraction
{
    [Collection("Sequential")]
    public class TouristEquipmentCommandTests : BaseStakeholdersIntegrationTest
    {
        public TouristEquipmentCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new TouristEquipmentCreateDto
            {
                TouristId = 100,
                EquipmentIds = new List<int> { 1, 3 }
            };

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "100") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };


            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentResponseDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TouristId.ShouldBe(newEntity.TouristId);
            result.EquipmentIds.ShouldBe(newEntity.EquipmentIds);

            // Assert - Database
            var storedEntity = dbContext.TouristEquipments.FirstOrDefault(i => i.TouristId == newEntity.TouristId);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
            storedEntity.TouristId.ShouldBe(result.TouristId);
            storedEntity.EquipmentIds.ShouldBe(result.EquipmentIds);
        }

        [Fact]
        public void Create_fails_duplicate()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new TouristEquipmentCreateDto
            {
                TouristId = 1,
                EquipmentIds = new List<int> { 1, 3 }
            };

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "1") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };


            // Act
            var result = (BadRequestResult)controller.Create(newEntity).Result;

            // Assert - Response
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
            var updatedEntity = new TouristEquipmentUpdateDto
            {
                Id = 1,
                TouristId = 1,
                EquipmentIds = new List<int> { 1, 3 }
            };

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "1") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TouristEquipmentResponseDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.TouristId.ShouldBe(updatedEntity.TouristId);
            result.EquipmentIds.ShouldBe(updatedEntity.EquipmentIds);

            // Assert - Database
            var storedEntity = dbContext.TouristEquipments.FirstOrDefault(i => i.Id == 1);
            storedEntity.ShouldNotBeNull();
            storedEntity.TouristId.ShouldBe(updatedEntity.TouristId);
            storedEntity.EquipmentIds.ShouldBe(updatedEntity.EquipmentIds);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TouristEquipmentUpdateDto
            {
                Id = -1000,
                TouristId = 1000,
                EquipmentIds = new List<int>()
            };

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "1000") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
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

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "10") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };


            // Act
            var result = (OkResult)controller.Delete(-3);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.TouristEquipments.FirstOrDefault(i => i.Id == -3);
            storedCourse.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var contextUser = new ClaimsIdentity(new Claim[] { new Claim("id", "-1") }, "test");
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(contextUser)
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            // Act
            var result = (UnauthorizedResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(401);
        }

        private static TouristEquipmentController CreateController(IServiceScope scope)
        {
            return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>(), scope.ServiceProvider.GetRequiredService<IEquipmentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
