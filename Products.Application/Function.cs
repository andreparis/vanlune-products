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

            return Request<UpdateProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse UpdateProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<UpdateProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse DeleteProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<DeleteProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetAllProductsByCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["category"]}");

            var name = request.QueryStringParameters["category"];

            if (string.IsNullOrEmpty(name))
                return Response("You must inform a category's name!");

            lambdaContext.Logger.LogLine($"GetAllProducts query");

            var command = new GetProductsCommand() 
            {
                CategoryName = name
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
        #endregion

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
                { "Access-Control-Allow-Origin", "*" }
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
