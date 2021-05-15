using BestRoute.Domain.Interfaces;
using BestRoute.Domain.Models;
using BestRoute.Domain.Models.Requests;
using BestRoute.Service;
using BestRoute.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace BestRoute.Test
{
    public class SearchControllerTest : IClassFixture<ConfigFixture>
    {
        private readonly ISearchService _service;
        private readonly SearchController _controller;

        public SearchControllerTest(ConfigFixture fixture)
        {
            _service = new SearchService();
            _controller = new SearchController(_service){ };
        }

        [Theory]
        [InlineData("input-file.csv", "GRU", "SCL")]
        public async Task GetBestRoute_Should_Return_Ok(string dataSource, string from, string to)
        {
            //Arrange
            var fakeRequest = new BestRouteRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to
            };

            //Act
            var result = await _controller.GetBestRoute(fakeRequest);
            var okResult = result as OkObjectResult;
            var resultData = okResult.Value;

            //Assert
            Assert.NotNull(resultData);
            Assert.Equal(200, okResult.StatusCode);
            Assert.StartsWith("Best route found: GRU -> ", resultData.ToString());
        }

        [Theory]
        [InlineData("input-file.csv", "", "SCL")]
        public async Task GetBestRoute_Should_Return_BadRequest_From(string dataSource, string from, string to)
        {
            //Arrange
            var fakeRequest = new BestRouteRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to
            };

            //Act
            var result = await _controller.GetBestRoute(fakeRequest);
            var okResult = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, okResult.StatusCode);
            Assert.Equal("The field From is required.", okResult.Value.ToString());
        }

        [Theory]
        [InlineData("input-file.csv", "BRC", "")]
        public async Task GetBestRoute_Should_Return_BadRequest_To(string dataSource, string from, string to)
        {
            //Arrange
            var fakeRequest = new BestRouteRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to
            };

            //Act
            var result = await _controller.GetBestRoute(fakeRequest);
            var okResult = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, okResult.StatusCode);
            Assert.Equal("The field To is required.", okResult.Value.ToString());
        }

        [Theory]
        [InlineData("", "BRC", "SCL")]
        public async Task GetBestRoute_Should_Return_BadRequest_DataSource(string dataSource, string from, string to)
        {
            //Arrange
            var fakeRequest = new BestRouteRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to
            };

            //Act
            var result = await _controller.GetBestRoute(fakeRequest);
            var okResult = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, okResult.StatusCode);
            Assert.Equal("The field DataSource is required.", okResult.Value.ToString());
        }

        [Theory]
        [InlineData("", "BRC", "BRC")]
        public async Task GetBestRoute_Should_Return_BadRequest_From_Equals_To(string dataSource, string from, string to)
        {
            //Arrange
            var fakeRequest = new BestRouteRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to
            };

            //Act
            var result = await _controller.GetBestRoute(fakeRequest);
            var okResult = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, okResult.StatusCode);
            Assert.Equal("Field To should not be equal to From.", okResult.Value.ToString());
        }

        [Theory]
        [InlineData("input-file.csv", "GRU", "SCL", 10)]
        public async Task CreateRoute_Should_Return_Ok(string dataSource, string from, string to, double? cost)
        {
            //Arrange
            var fakeRequest = new BestRouteCreateRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to,
                Cost = cost
            };

            //Act
            var result = await _controller.CreateRoute(fakeRequest);
            var okResult = result as OkObjectResult;
            var resultData = okResult.Value;

            //Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Route sucessful added!", resultData);
        }

        [Theory]
        [InlineData("input-file.csv", "GRU", "SCL", null)]
        public async Task CreateRoute_Should_Return_BadRequest_Cost(string dataSource, string from, string to, double? cost)
        {
            //Arrange
            var fakeRequest = new BestRouteCreateRequest()
            {
                DataSource = dataSource,
                From = from,
                To = to,
                Cost = cost
            };

            //Act
            var result = await _controller.CreateRoute(fakeRequest);
            var okResult = result as BadRequestObjectResult;

            //Assert
            Assert.Equal("The field Cost is required.", okResult.Value.ToString());
            Assert.Equal(400, okResult.StatusCode);
        }
    }
}
