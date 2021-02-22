using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Infraestructure.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Application.Application.MediatR.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : AbstractRequestHandler<UpdateProductCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;
        private readonly ILogger _logger;

        public UpdateProductCommandHandler(IProductAggregationRepository productsRepository,
            ILogger logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.Info("Initi UpdateProductCommand");

            var task = new List<Task>
            {
                _productsRepository.SetTagsToProduct(request.NewTags, request.Product),
                _productsRepository.RemoveTagsByProduct(request.RemovedTags, request.Product),
                _productsRepository.SetVariantsToProduct(request.NewVariants, request.Product),
                _productsRepository.RemoveVariantsByProducts(request.RemovedVariants, request.Product),
                _productsRepository.UpdateProduct(request.Product)
            };

            Task.WaitAll(task.ToArray());

            return new HandleResponse();
        }
    }
}
