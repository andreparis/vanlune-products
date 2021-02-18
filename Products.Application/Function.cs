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
using Products.Application.Application.MediatR.Commands.CreateOrUpdateProducts;
using Products.Application.Application.MediatR.Commands.DeleteProducts;

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
        public APIGatewayProxyResponse CreateOrUpdateProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<CreateOrUpdateProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse DeleteProducts(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Body {request.Body}");

            return Request<DeleteProductsCommand>(request.Body);
        }

        public APIGatewayProxyResponse GetAllProductsByCategory(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            Console.WriteLine($"Requested {request.QueryStringParameters["name"]}");

            lambdaContext.Logger.LogLine($"GetAllProducts query");

            var command = new GetProductsCommand() 
            { 
                Category = new Domain.Entities.Category() 
                { 
                    Name = request.QueryStringParameters ["name"]
                } 
            };

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
