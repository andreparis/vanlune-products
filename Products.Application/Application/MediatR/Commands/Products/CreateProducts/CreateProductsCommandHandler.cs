using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.DataAccess.S3;
using Products.Infraestructure.Logging;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.CreateProducts
{
    public class CreateProductsCommandHandler : AbstractRequestHandler<UpdateProductsCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;
        private readonly ILogger _logger;

        public CreateProductsCommandHandler(IProductAggregationRepository productsRepository,
            ILogger logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(UpdateProductsCommand request, CancellationToken cancellationToken)
        {
            _logger.Info("Initi CreateOrUpdateProductsCommand");

            var result = _productsRepository.InsertNewProduct(request.Product);

            return new HandleResponse();
        }
    }
}
