﻿using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration
{
    [Collection("Sequential")]
    public class ShoppingCartCommandTest:BaseToursIntegrationTest
    {
        public ShoppingCartCommandTest(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new ShoppingCartCreateDto
            {
                TouristId= 3,
                IsPurchased=false,
            };

            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ShoppingCartResponseDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TouristId.ShouldBe(newEntity.TouristId);
            result.IsPurchased.ShouldBe(newEntity.IsPurchased);

            // Assert - Database
            var storedEntity = dbContext.ShoppingCarts.FirstOrDefault(i => i.TouristId == newEntity.TouristId);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
            storedEntity.TouristId.ShouldBe(result.TouristId);
            storedEntity.IsPurchased.ShouldBe(result.IsPurchased);
        }

        [Fact]
        public void AddOrderItem_fails_invalid_data()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new OrderItemCreateDto
            {
                TourId= -7,
                Price = 10,
                TourName = "Tura",
                ShoppingCartId = -1,
            };

            var result = ((OkResult)controller.AddOrderItem(newEntity));

            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var shoppingCart = ((ObjectResult)controller.GetByTouristId(-1).Result)?.Value as ShoppingCartResponseDto;

            shoppingCart.ShouldNotBeNull();
            shoppingCart.TotalPrice.ShouldNotBe(1000);
        }


        [Fact]
        public void AddOrderItem()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new OrderItemCreateDto
            {
                TourId= -2,
                Price = 10,
                TourName = "Tura",
                ShoppingCartId = -2,
            };

            var result = ((OkResult)controller.AddOrderItem(newEntity));

            result.StatusCode.ShouldBe(200);

            // Assert - Response
         


            // Assert - Database
            var shoppingCart = ((ObjectResult)controller.GetByTouristId(-2).Result)?.Value as ShoppingCartResponseDto;

            shoppingCart.ShouldNotBeNull();
            shoppingCart.TotalPrice.ShouldBe(10);

        }


        [Fact]
        public void RemoveOrderItem()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new OrderItemCreateDto
            {
                TourId = -5,
                Price = 10,
                TourName = "Tura",
                ShoppingCartId = -2,
            };

            var result = ((OkResult)controller.AddOrderItem(newEntity));

            result.StatusCode.ShouldBe(200);

            var item= ((ObjectResult)controller.GetItemByTourId(newEntity.TourId,-2).Result)?.Value as OrderItemResponseDto;



            var resultRemoving= ((OkResult)controller.RemoveOrderItem((int)item.Id,(int)item.ShoppingCartId));

            result.StatusCode.ShouldBe(200);


            // Assert - Database
            var shoppingCart = ((ObjectResult)controller.GetByTouristId(-2).Result)?.Value as ShoppingCartResponseDto;

            shoppingCart.ShouldNotBeNull();
            shoppingCart.TotalPrice.ShouldBe(0);
        }
        private static ShoppingCartController CreateController(IServiceScope scope)
        {
            return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }

}