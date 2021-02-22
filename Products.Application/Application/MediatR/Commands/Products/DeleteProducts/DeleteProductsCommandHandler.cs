using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.DeleteProducts
{
    public class DeleteProductsCommandHandler : AbstractRequestHandler<DeleteProductsCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;

        public DeleteProductsCommandHandler(IProductAggregationRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        internal override HandleResponse HandleIt(DeleteProductsCommand request, CancellationToken cancellationToken)
        {
            _productsRepository.DeleteProduct(request.Product).Wait();

            return new HandleResponse();
        }
    }
}
