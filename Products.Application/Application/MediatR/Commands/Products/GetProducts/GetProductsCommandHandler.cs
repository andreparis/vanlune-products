using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.DataAccess.S3;
using Products.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.GetProducts
{
    public class GetProductsCommandHandler : AbstractRequestHandler<GetProductsCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;

        public GetProductsCommandHandler(IProductAggregationRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        internal override HandleResponse HandleIt(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var category = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.CategoryName.Trim().ToLowerInvariant());
            var result = _productsRepository.GetProductsDtoByCategory(category).Result;

            var products = result.GroupBy(a => a.Id);
            var objDto = new List<ProductDto>();

            foreach(var product in products)
            {
                var dto = product.FirstOrDefault();

                dto.Variants = Domain.Entities.Variants.RemoveDuplicates(product.Select(v => v.Variants.FirstOrDefault()));
                dto.Tags = product.Select(t => t.Tags?.FirstOrDefault())?.Distinct().ToArray();

                dto.Images = new Domain.Entities.Images[] 
                {
                    new Domain.Entities.Images 
                    {
                        Alt = "white",
                        Src = dto.Image.Src
                    }
                };

                objDto.Add(dto);
            }

            return new HandleResponse()
            {
                Content = objDto
            };
        }
    }
}
