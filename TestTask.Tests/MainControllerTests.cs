using NUnit.Framework;
using Moq;
using TestTask.DAL;
using TestTask.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TestTask.Tests
{
    public class MainControllerTests
    {
        private Mock<TestTaskDbContext> _dbContextMock;
        private MainController _mainControllerMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextMock = TestHelper.GetDbContextMock();
            _dbContextMock.Object.Database.EnsureCreated();
            _mainControllerMock = TestHelper.GetMainController(_dbContextMock.Object);
        }

        [OneTimeTearDown]
        public void TearDown() 
        {
            _dbContextMock.Object.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetAllIsPassAsync()
        {
            var result = await _mainControllerMock.GetAllContractsAsync() as ObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task AddContractAndThenGetAllIsPassAsync()
        {
            var contract = new DTO.ContractDto()
            {
                ProductQuantity = 1,
                Product = new DTO.ProductDto()
                {
                    Name = "Test",
                    Size = 1
                },
                ProductionRoom = new DTO.ProductionRoomDto() 
                { 
                    Name = "TestRoom",
                    Space = 1
                }
            };

            var addResult = await _mainControllerMock.AddOrUpdateContractAsync(contract) as ObjectResult;
            Assert.That(addResult, Is.Not.Null);
            Assert.That(addResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            var getResult = await _mainControllerMock.GetAllContractsAsync() as ObjectResult;
            Assert.That(getResult, Is.Not.Null);
            Assert.That(getResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            var contractList = getResult.Value as List<DTO.ContractDto>;
            Assert.That(contractList, Is.Not.Null);
            Assert.That(contractList, Has.Count.EqualTo(1));
        }
    }
}