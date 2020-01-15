using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;

namespace NETToolBox.RequireHTTPS.Tests
{
    public class RequireHTTPSMiddlewareTests
    {
        [Fact]
        public async Task HTTPSRequestCallsNextDelegate()
        {
            //Arrange
            var context = new DefaultHttpContext();
            context.Request.IsHttps = true;
          
            //just add a delegate so we can test to make sure it got called
            RequestDelegate next = x =>
            {
                x.Response.ContentType = "application/xml";  //just picked an arbitrary non default contenttype
                return Task.CompletedTask;
            };

            var middleware = new RequireHTTPSMiddleware(next);

            //Act
            await middleware.Invoke(context);

            //Assert

            context.Response.ContentType.Should().Be("application/xml"); //make sure our delegate got called  
        }

        [Fact]
        public async Task HTTPRequestReturnsBadRequest()
        {
            //Arrange
            var context = new DefaultHttpContext();
            context.Request.IsHttps = false;
            
          
            var middleware = new RequireHTTPSMiddleware(next:(innerHttpContext)=>
            {
                return Task.CompletedTask;
            }
            );

            //Act
            await middleware.Invoke(context);

            //Assert

            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
