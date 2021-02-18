using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Products;
using Products.Application;
using Products.Domain.Entities;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class Tests
    {
        private Fixture _fixture;
        private Function _function;

        public Tests()
        {
            _fixture = new Fixture();
            _function = new Function();
        }

        [Fact]
        public void InsertProduct()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<Product>()
                .Create();
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\"Product\":{\"Title\":\"GOLD 1M\",\"Description\":\"Buy 1M of GOLD in any server\",\"Category\":{\"Name\":\"Gold\",\"Description\":\"WoW Coin\",\"Id\":1},\"Sale\":false,\"Price\":600.0,\"Quantity\":1,\"Discount\":0.0,\"Tags\":[{\"Name\":\"Nemesis\",\"Factor\":1.0},{\"Name\":\"Azrlon\",\"Factor\":1.15},{\"Name\":\"Pandalandia\",\"Factor\":2.0}],\"Id\":1}}")
                .Create();

            _function.CreateOrUpdateProducts(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void GetAllPorductsByCategory()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x=>x.QueryStringParameters, new Dictionary<string, string>() { {"name","gold"} })
                .Create();

            var result = _function.GetAllProductsByCategory(apiContext, lambdaContext.Object);

        }
    }
}