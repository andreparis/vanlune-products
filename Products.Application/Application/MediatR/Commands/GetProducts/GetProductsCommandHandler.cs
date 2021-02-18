using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.S3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.GetProducts
{
    public class GetProductsCommandHandler : AbstractRequestHandler<GetProductsCommand>
    {
        private readonly IProductsRepository _productsRepository;

        public GetProductsCommandHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        internal override HandleResponse HandleIt(GetProductsCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Path is {request.Category.Name.Trim().ToLowerInvariant()}");

            var result = _productsRepository.GetProductsByCategory(request.Category.Name.Trim().ToLowerInvariant());

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
