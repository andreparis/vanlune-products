using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.DataAccess.S3;
using Products.Infraestructure.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Application.Application.MediatR.Commands.CreateProducts
{
    public class CreateOrUpdateProductsCommandHandler : AbstractRequestHandler<CreateOrUpdateProductsCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;
        private readonly ILogger _logger;

        public CreateOrUpdateProductsCommandHandler(IProductAggregationRepository productsRepository,
            ILogger logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(CreateOrUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            _logger.Info("Initi CreateOrUpdateProductsCommand");

            var product = _productsRepository
                .GetProductsDtoByFilters(new Dictionary<string, string>()
                {
                    {"id", request.Product.Id.ToString() }
                }).Result;            

            if (product == null ||
                !product.Any())
                _productsRepository.InsertNewProduct(request.Product).GetAwaiter().GetResult();
            else if (request.Product.Id > 0)
            {
                _logger.Info($"products {product.Count()}");

                _productsRepository.UpdateProduct(request.Product).GetAwaiter().GetResult();
                if (request.Product.Tags != null &&
                    request.Product.Tags.Any())
                {
                    _productsRepository.RemoveTagsByProduct(request.Product.Tags, request.Product).GetAwaiter().GetResult();
                    _productsRepository.SetTagsToProduct(request.Product.Tags, request.Product).GetAwaiter().GetResult();
                }
                if (request.Product.Variants != null &&
                    request.Product.Variants.Any())
                {
                    _productsRepository.RemoveVariantsByProducts(request.Product.Variants, request.Product).GetAwaiter().GetResult();
                    _productsRepository
                        .SetVariantsToProduct(request.Product.Variants.Where(x => x.Factor != 1),
                        request.Product).GetAwaiter().GetResult();
                }
                if (request.Product.Customizes != null &&
                    request.Product.Customizes.Any())
                {
                    _productsRepository.RemoveCustomizesByProduct(request.Product.Customizes, request.Product).GetAwaiter().GetResult();
                    _productsRepository.SetCustomizesToProduct(request.Product.Customizes, request.Product).GetAwaiter().GetResult();
                }
            }

            return new HandleResponse();
        }
    }
}
