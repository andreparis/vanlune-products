using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.S3;
using Products.Infraestructure.Logging;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.CreateOrUpdateProducts
{
    public class CreateOrUpdateProductsCommandHandler : AbstractRequestHandler<CreateOrUpdateProductsCommand>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger _logger;

        public CreateOrUpdateProductsCommandHandler(IProductsRepository productsRepository,
            ILogger logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(CreateOrUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            _logger.Info("Initi CreateOrUpdateProductsCommand");

            var path = string.Concat(request.Product.Category.Name.Trim().ToLowerInvariant(), "/", request.Product.Title).Trim().ToLowerInvariant();

            _productsRepository.AddProduct(path, request.Product);

            return new HandleResponse();
        }
    }
}
