﻿using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.ClubJoinRequests
{
    [Collection("Sequential")]
    public class ClubJoinRequestsCommandTests : BaseStakeholdersIntegrationTest
    {
        public ClubJoinRequestsCommandTests(StakeholdersTestFactory factory) : base(factory) { }
        [Fact]
        public void Sends()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var newEntity = new ClubJoinRequestSendDto
            {
                TouristId = -1,
                ClubId = -1
            };

            // Act
            var result = ((ObjectResult)controller.Send(newEntity).Result)?.Value as ClubJoinRequestSendDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TouristId.ShouldNotBe(0);
            result.ClubId.ShouldNotBe(0);

            // Assert - Database
            var storedEntity = dbContext.ClubJoinRequests.FirstOrDefault(r => r.TouristId == newEntity.TouristId && r.ClubId == newEntity.ClubId);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldNotBe(0);
        }

        [Fact]
        public void Sends_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newEntity = new ClubJoinRequestSendDto
            {
                TouristId = 0,
                ClubId = 0
            };

            // Act
            var result = ((ObjectResult)controller.Send(newEntity).Result);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Responds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            long id = -1;
            var response = new ClubJoinRequestResponseDto
            {
                Accepted = true
            };

            // Act
            var result = controller.Respond(id, response);

            // Assert - Response
            Assert.IsType<OkResult>(result);

            // Assert - Database
            var storedEntity = dbContext.ClubJoinRequests.FirstOrDefault(r => r.Id == id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Status.ShouldBe(ClubJoinRequestStatus.Accepted);
        }

        [Fact]
        public void Responds_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            long id = 100;
            var response = new ClubJoinRequestResponseDto
            {
                Accepted = false
            };

            // Act
            var result = ((ObjectResult)controller.Respond(id, response));

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Cancels()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            long id = -2;

            // Act
            var result = controller.Cancel(id);

            // Assert - Response
            Assert.IsType<OkResult>(result);

            // Assert - Database
            var storedEntity = dbContext.ClubJoinRequests.FirstOrDefault(r => r.Id == id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Status.ShouldBe(ClubJoinRequestStatus.Cancelled);
        }

        [Fact]
        public void Cancels_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            long id = 100;

            // Act
            var result = ((ObjectResult)controller.Cancel(id));

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static ClubJoinRequestController CreateController(IServiceScope scope)
        {
            return new ClubJoinRequestController(scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}