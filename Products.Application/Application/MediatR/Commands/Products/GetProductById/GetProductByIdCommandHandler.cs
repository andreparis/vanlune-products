using AutoMapper;
using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.GetProductById
{
    public class GetProductByIdCommandHandler : AbstractRequestHandler<GetProductByIdCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;
        private readonly IServerRepository _server;
        private readonly IMapper _mapper;

        public GetProductByIdCommandHandler(IProductAggregationRepository productsRepository,
            IServerRepository server,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _server = server;
            _mapper = mapper;
        }

        internal override HandleResponse HandleIt(GetProductByIdCommand request, CancellationToken cancellationToken)
        {
            var result = _productsRepository
                .GetProductsDtoByFilters(
                new Dictionary<string, string>()
                {
                    {"id", request.Id.ToString() }
                }).Result;

            if (!result.Any())
            {
                return new HandleResponse()
                {
                    Error = $"No product was found for id {request.Id}!"
                };
            }

            var servers = _server.GetAll(result.FirstOrDefault().Category.Game.Id).Result;

            var products = result.GroupBy(a => a.Id);
            var objDto = new List<ProductDto>();

            foreach (var product in products)
            {
                var dto = product.FirstOrDefault();

                dto.Variants = Domain
                    .Entities
                    .Variants
                    .RemoveDuplicates(product.Select(v => v.Variants.FirstOrDefault()));
                var variants = new List<Domain.Entities.Variants>();
                foreach (var server in servers)
                {
                    if (!dto.Variants.Any(a => a.Server.Id.Equals(server.Id)))
                        variants.Add(_mapper.Map<Domain.Entities.Variants>(server));
                }
                variants.AddRange(dto.Variants.ToList());
                dto.Variants = variants.ToArray();
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
