using TestTask.Controllers;
using TestTask.DAL.Repositories;
using TestTask.DAL;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TestTask.Tests
{
    internal static class TestHelper
    {
        public static MainController GetMainController(TestTaskDbContext dbContext) => new() 
        {
            ControllerContext = new ControllerContext(
                new ActionContext(
                    GetHttpContextMock(dbContext).Object,
                    new RouteData(),
                    new ControllerActionDescriptor()))
        };

        public static Mock<TestTaskDbContext> GetDbContextMock() =>
            new(
                new DbContextOptionsBuilder<TestTaskDbContext>()
                    .UseInMemoryDatabase("Test")
                    .Options)
            {
                CallBase = true
            };

        private static Mock<HttpContext> GetHttpContextMock(TestTaskDbContext dbContext)
        {
            var httpContextMock = new Mock<HttpContext> { CallBase = true };
            httpContextMock
                .Setup(context => context.RequestServices.GetService(typeof(TestTaskDbContext)))
                .Returns(dbContext);

            httpContextMock
                .Setup(context => context.RequestServices.GetService(typeof(ContractRepository)))
                .Returns(new ContractRepository(dbContext));

            var requestMock = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            requestMock
                .Setup(request => request.Headers)
                .Returns(headers);
            httpContextMock
                .Setup(context => context.Request)
                .Returns(requestMock.Object);

            return httpContextMock;
        }
    }
}
