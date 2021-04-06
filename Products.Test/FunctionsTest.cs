using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Products;
using Products.Application;
using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
                .With(x => x.Body, "{\"product\":{\"variants\":[{\"name\":\"Area-52\",\"factor\":1,\"Server\":null,\"id\":2},{\"name\":\"Tichondrius\",\"factor\":1,\"Server\":null,\"id\":3},{\"name\":\"Illidan\",\"factor\":1,\"Server\":null,\"id\":4},{\"name\":\"Mal'Ganis\",\"factor\":1,\"Server\":null,\"id\":5},{\"name\":\"Zul'Jin\",\"factor\":1,\"Server\":null,\"id\":6},{\"name\":\"Thrall\",\"factor\":1,\"Server\":null,\"id\":7},{\"name\":\"Kil'jaeden\",\"factor\":1,\"Server\":null,\"id\":8},{\"name\":\"Bleeding-Hollow\",\"factor\":1,\"Server\":null,\"id\":9},{\"name\":\"Wyrmrest Accord\",\"factor\":1,\"Server\":null,\"id\":10},{\"name\":\"Barthilas\",\"factor\":1,\"Server\":null,\"id\":11},{\"name\":\"Stormrage\",\"factor\":1,\"Server\":null,\"id\":12},{\"name\":\"Nemesis\",\"factor\":1.1,\"Server\":{\"Name\":null,\"id\":13},\"id\":4},{\"name\":\"Azralon\",\"factor\":1.2,\"Server\":{\"Name\":null,\"id\":1},\"id\":5}],\"images\":[{\"image_id\":0,\"alt\":\"white\",\"src\":\"https:\\/\\/vanlune-site-images.s3.amazonaws.com\\/products\\/gold.png\",\"variant_id\":null,\"id\":0}],\"tags\":[\"new\",\"topSeller\",\"special\"],\"title\":\"Gold 1M\",\"description\":\"Buy 1M WoW's Gold!!\",\"category\":{\"name\":\"Gold\",\"description\":null,\"id\":1},\"sale\":false,\"price\":599.99,\"quantity\":1,\"discount\":0,\"Image\":{\"image_id\":0,\"alt\":null,\"src\":\"https:\\/\\/vanlune-site-images.s3.amazonaws.com\\/products\\/gold.png\",\"variant_id\":null,\"id\":0},\"Game\":{\"Name\":\"World of Warcraft\",\"id\":1},\"id\":4,\"customizes\":[{\"name\":\"Keys\",\"value\":[{\"name\":\"Specific Key\",\"factor\":1.33}],\"game\":{\"name\":\"World of Warcraft\",\"id\":1},\"id\":1}]}}")
                .Create();

            _function.CreateProducts(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void GetAllPorductsByFilters()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.QueryStringParameters,
                new Dictionary<string, string>()
                { {"categoryName","Gold"} })
                .Create();

            var result = _function.GetProductsByFilters(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void GetAllPorductsByCategory()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x=>x.QueryStringParameters, 
                new Dictionary<string, string>() 
                { {"category","gold"}, {"game","1"} })
                .Create();

            var result = _function.GetAllProductsByCategory(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void GetAllPorductsByTag()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.QueryStringParameters,
                new Dictionary<string, string>()
                { {"tag","new"}, {"game","1"} })
                .Create();

            var result = _function.GetAllProductsByTag(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void GetAllTags()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .Create();

            var result = _function.GetTags(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void InsertCategoryTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<ProductDto>()
                .Create();
            var json = JsonConvert.SerializeObject(product);
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\r\n    \"Category\": {\r\n        \"name\":\"Mythic\",\r\n        \"description\": \"Mythic+ for WoW\"\r\n, \"game\":{\"id\":1}    }\r\n}")
                .Create();

            _function.CreateCategory(apiContext, lambdaContext.Object);
        }

        [Fact]
        public void UpdateCategoryTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<ProductDto>()
                .Create();
            var json = JsonConvert.SerializeObject(product);
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\"category\":{\"id\":1,\"name\":\"Gold\",\"description\":\"1M of WoW's Gold\",\"game\":{\"id\":1}}}")
                .Create();

            _function.UpdateCategory(apiContext, lambdaContext.Object);
        }

        [Fact]
        public void InsertTagsTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<Product>()
                .Create();
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\r\n  \"tags\": [\r\n    {\r\n      \"name\": \"topSeller\"\r\n    }\r\n  ]\r\n}")
                .Create();

            _function.CreateTag(apiContext, lambdaContext.Object);
        }


        [Fact]
        public void InsertVariantsTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\"variants\": [\r\n    {\r\n      \"name\": \"Nemesis\",\r\n      \"factor\": 1.1,\r\n      \"id\": 1\r\n    },\r\n    {\r\n      \"name\": \"Azarlon\",\r\n      \"factor\": 1.2,\r\n      \"id\": 2\r\n    },\r\n    {\r\n      \"name\": \"Hyjal\",\r\n      \"factor\": 1.05,\r\n      \"id\": 3\r\n    }\r\n  ]}")
                .Create();

            _function.CreateVariants(apiContext, lambdaContext.Object);
        }


        [Fact]
        public void CreteCustomizeTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<ProductDto>()
                .Create();
            var json = JsonConvert.SerializeObject(product);
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\r\n    \"customizes\": [{\r\n        \"name\":\"Specific Key\",\r\n        \"value\": [{\"name\":\"Specific Key\", \"factor\":1.33}], \"game\":{\"id\":1}    }]\r\n}")
                .Create();

            _function.CreateCustomize(apiContext, lambdaContext.Object);
        }

        [Fact]
        public void UpdateCustomizeTest()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<ProductDto>()
                .Create();
            var json = JsonConvert.SerializeObject(product);
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, "{\"customize\":{\"id\":1,\"name\":\"Keys\",\"value\":[{\"name\":\"Specific Key\",\"factor\":\"1.34\"}],\"game\":{\"id\":1}}}")
                .Create();

            _function.UpdateCustomizes(apiContext, lambdaContext.Object);
        }

        [Fact]
        public void GetCustomizes()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.QueryStringParameters,
                new Dictionary<string, string>()
                { {"game","1"} })
                .Create();

            var result = _function.GetCustomizesByFilters(apiContext, lambdaContext.Object);

        }

        [Fact]
        public void DeleteCustomizes()
        {
            var lambdaContext = new Mock<ILambdaContext>();
            var product = _fixture
                .Build<ProductDto>()
                .Create();
            var json = JsonConvert.SerializeObject(product);
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.QueryStringParameters, new Dictionary<string, string> { { "id", "5" } })
                .Create();

            var result = _function.DeleteCustomizes(apiContext, lambdaContext.Object);
        }

        [Fact]
        public async Task UploadImage()
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(@"C:\Projects\vanlune\vanlune-products\Products.Test\Files\mythic.jpg");
            var base64ImageRepresentation = Convert.ToBase64String(imageArray);
            
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.Body, JsonConvert.SerializeObject(new { File = base64ImageRepresentation, FileName = "testando.jpg" }))
                .Create();

            var result = _function.UploadImages(apiContext, lambdaContext.Object);

        }


        [Fact]
        public void GetCategoriesByGame()
        {
            var lambdaContext = new Mock<ILambdaContext>(); ;
            var apiContext = _fixture
                .Build<APIGatewayProxyRequest>()
                .With(x => x.QueryStringParameters,
                new Dictionary<string, string>()
                { {"game","1"} })
                .Create();

            var result = _function.GetCategoryByGame(apiContext, lambdaContext.Object);

        }
    }
}