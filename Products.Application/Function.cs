using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Products.Application.Extensions;
using MediatR;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.APIGatewayEvents;
using System.Collections.Generic;
using System.Net;
using Products.Application.Application.MediatR.Commands.GetProducts;
using Products.Application.Application.MediatR.Commands.DeleteProducts;
using Products.Application.Application.MediatR.Commands.Category.GetCategory;
using Products.Application.Application.MediatR.Commands.Tags.CreateOrUpdateTags;
using Products.Application.Application.MediatR.Commands.Tags.GetTags;
using Products.Application.Application.MediatR.Commands.Category.GetAllCategories;
using Products.Application.Application.MediatR.Commands.Variants.CreateOrUpdateVariants;
using Products.Application.Application.MediatR.Commands.Variants.GetVariants;
using Products.Application.Application.MediatR.Commands.CreateProducts;
using Products.Application.Application.MediatR.Commands.Category.CreateCategory;
using Products.Application.Application.MediatR.Commands.GetProductsByTags;
using Products.Application.Application.MediatR.Commands.GetProductById;
using Products.Application.Application.MediatR.Commands.GetProductsByFilters;
using Products.Application.Application.MediatR.Commands.Customizes.DeleteCustomizes;
using Products.Application.Application.MediatR.Commands.Customizes.UpdateCustomizes;
using Products.Application.Application.MediatR.Commands.Customizes.CreateCustomizes;
using Products.Application.Application.MediatR.Commands.Customizes.GetCustomizesByFilters;
using Products.Application.Application.MediatR.Commands.Variants.GetServersVariants;
using Products.Application.Application.MediatR.Commands.UploadImages;
using Products.Application.Application.MediatR.Commands.Category.GetCategoryByGame;
using Products.Application.Application.MediatR.Commands.Game.GetAllGames;
using Products.Application.Application.MediatR.Commands.Category.UpdateCategory;
using Products.Application.Application.MediatR.Commands.Category.DeleteCategory;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Products.Application
{
    public class Function
    {
        protected IServiceProvider _serviceProvider = null;
        protected ServiceCollection _serviceCollection = new ServiceCollection();
        protected IMediator _mediator;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            ConfigureServices();
            _mediator = _serviceProvider.GetService<IMediator>();
        }

        #region APIs

        #region Products
        public APIGatewayProxyResponse CreateProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateOrUpdateProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse UpdateProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateOrUpdateProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse UploadImages(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<UploadImagesCommand>(request.Body);
        }

        public APIGatewayProxyResponse DeleteProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<DeleteProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetProductsByFilters(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {

            if (request.QueryStringParameters.Keys.Count == 0)
                return Response("You must inform a game's id!");

            lambdaContext.Logger.LogLine($"GetProductById query");

            var command = new GetProductsByFiltersCommand()
            {
                Filters = request.QueryStringParameters
            };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetProductById(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["product"]}");

            var product = request.QueryStringParameters["product"];

            if (string.IsNullOrEmpty(product))
                return Response("You must inform a product's id!");

            lambdaContext.Logger.LogLine($"GetProductById query");

            var command = new GetProductByIdCommand()
            {
                Id = Convert.ToInt32(product)
            };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetAllProductsByCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["category"]}");

            var name = request.QueryStringParameters["category"];
            var gameId = request.QueryStringParameters["game"];

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(gameId))
                return Response("You must inform a category's name!");

            lambdaContext.Logger.LogLine($"GetAllProducts query");

            var command = new GetProductsCommand()
            {
                CategoryName = name,
                GameId = Convert.ToInt32(gameId)
            };

            return MediatrSend(command);
        }
                
        public APIGatewayProxyResponse GetAllProductsByTag(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["tag"]}");

            var name = request.QueryStringParameters["tag"];
            var gameId = request.QueryStringParameters["game"];

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(gameId))
                return Response("You must inform a tag's name!");

            lambdaContext.Logger.LogLine($"GetAllProductsByTag query");

            var command = new GetProductsByTagsCommand()
            {
                TagName = name,
                GameId = Convert.ToInt32(gameId)
            };

            return MediatrSend(command);
        }
        #endregion

        #region Categories
        public APIGatewayProxyResponse CreateCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateCategoryCommand>(request.Body);
        }

        public APIGatewayProxyResponse UpdateCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<UpdateCategoryCommand>(request.Body);
        }

        public APIGatewayProxyResponse DeleteCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            var id = request.QueryStringParameters["id"];
            if (string.IsNullOrEmpty(id))
                return Response("You must inform a category's Id!");

            var command = new DeleteCategoryCommand() { Id = Convert.ToInt32(id) };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["id"]}");

            var id = request.QueryStringParameters["id"];

            if (string.IsNullOrEmpty(id))
                return Response("You must inform a category's Id!");

            lambdaContext.Logger.LogLine($"GetAllCategories query");

            var command = new GetCategoryCommand()
            {
                Id = Convert.ToInt32(id)
            };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetCategoryByGame(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["game"]}");

            var id = request.QueryStringParameters["game"];

            if (string.IsNullOrEmpty(id))
                return Response("You must inform a category's Id!");

            lambdaContext.Logger.LogLine($"GetAllCategories query");

            var command = new GetCategoryByGameCommand()
            {
                Game = Convert.ToInt32(id)
            };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetAllCategories(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"GetAllCategories query");

            var command = new GetAllCategoriesCommand();

            return MediatrSend(command);
        }
        #endregion

        #region Tags
        public APIGatewayProxyResponse CreateTag(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateOrUpdateTagsCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetTags(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"GetTags query");

            var command = new GetTagsCommand();

            return MediatrSend(command);
        }
        #endregion

        #region Variansts
        public APIGatewayProxyResponse CreateVariants(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateOrUpdateVariantsCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetVariants(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"GetVariants query");

            var command = new GetVariantsCommand();

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse GetAllServersVariants(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["game"]}");

            var game = request.QueryStringParameters["game"];

            if (string.IsNullOrEmpty(game))
                return Response("You must inform a game's id!");

            var command = new GetServersVariantsCommand() 
            {
                Game = Convert.ToInt32(game)
            };

            return MediatrSend(command);
        }
        #endregion

        #region Customizes
        public APIGatewayProxyResponse CreateCustomize(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateCustomizesCommand>(request.Body);
        }

        public APIGatewayProxyResponse UpdateCustomizes(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<UpdateCustomizesCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetCustomizesByFilters(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["game"]}");

            var game = request.QueryStringParameters["game"];

            if (string.IsNullOrEmpty(game))
                return Response("You must inform a game's id!");

            lambdaContext.Logger.LogLine($"GetCustomizesByFilters query");

            var command = new GetCustomizesByFiltersCommand()
            {
                Game = Convert.ToInt32(game),
                Filters = request.QueryStringParameters
            };

            return MediatrSend(command);
        }

        public APIGatewayProxyResponse DeleteCustomizes(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.QueryStringParameters["id"]}");

            var id = request.QueryStringParameters["id"];
            if (string.IsNullOrEmpty(id))
                return Response("You must inform a category's Id!");

            var command = new DeleteCustomizesCommand() { Ids = new int[] { Convert.ToInt32(id) } };

            return MediatrSend(command);
        }
        #endregion

        #region Game

        #endregion
        public APIGatewayProxyResponse GetAllGames(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"GetGames query");

            var command = new GetAllGamesCommand();

            return MediatrSend(command);
        }
        #endregion

        #region Private Methods
        private void SqsResquest<T>(SQSEvent sqsEvent, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Beginning to process {sqsEvent.Records.Count} records...");

            foreach (var record in sqsEvent.Records)
            {
                var message = JsonConvert.DeserializeObject<T>(record.Body);

                _mediator.Send(message);
            }

            lambdaContext.Logger.LogLine("Processing complete.");

            lambdaContext.Logger.LogLine($"Processed {sqsEvent.Records.Count} records.");
        }
        private APIGatewayProxyResponse Request<T>(string body)
        {
            Console.WriteLine("body is "+ body);

            var request = JsonConvert.DeserializeObject<T>(body);
            return MediatrSend<T>(request);
        }

        private APIGatewayProxyResponse MediatrSend<T>(T request)
        {
            var result = _mediator.Send(request).Result;
            return Response(JsonConvert.SerializeObject(result));
        }

        private APIGatewayProxyResponse Response(string message)
        {
            var header = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Credentials", "true" }
            };

            return new APIGatewayProxyResponse
            {
                Headers = header,
                Body = message,
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        
        private void ConfigureServices()
        {
            _serviceCollection.AddDependencies();
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            _mediator = _serviceProvider.GetService<IMediator>();
        }
        #endregion
    }
}
