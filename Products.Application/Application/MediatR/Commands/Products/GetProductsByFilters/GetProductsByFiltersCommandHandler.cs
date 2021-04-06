using AutoMapper;
using Newtonsoft.Json;
using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities.DTO;
using Products.Infraestructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.GetProductsByFilters
{
    public class GetProductsByFiltersCommandHandler : AbstractRequestHandler<GetProductsByFiltersCommand>
    {
        private readonly IProductAggregationRepository _productsRepository;
        private readonly IServerRepository _server;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetProductsByFiltersCommandHandler(IProductAggregationRepository productsRepository,
            IServerRepository server,
            IMapper mapper,
            ILogger logger)
        {
            _productsRepository = productsRepository;
            _server = server;
            _mapper = mapper;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(GetProductsByFiltersCommand request, CancellationToken cancellationToken)
        {
            var result = _productsRepository.GetProductsDtoByFilters(request.Filters).Result;

            var game = result.First()?.Category?.Game?.Id;
            if (game == null)
            {
                return new HandleResponse() 
                {
                    Error = "Found products without category!"
                };
            }

            var servers = _server.GetAll(game ?? 1).Result;
            var products = result.GroupBy(a => a.Id);
            var objDto = new List<ProductDto>();

            foreach (var product in products)
            {
                var dto = product.FirstOrDefault();

                dto.Variants = Domain.Entities.Variants
                    .RemoveDuplicates(product.Select(v => v.Variants?.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name))));
                dto.Variants = GetVariants(dto.Variants, servers);
                dto.Tags = product.Select(t => t.Tags?.FirstOrDefault())?.Distinct().ToArray();
                dto.Images = new Domain.Entities.Images[]
                {
                    new Domain.Entities.Images
                    {
                        Alt = "white",
                        Src = dto.Image.Src
                    }
                };
                dto.Customizes = Domain.Entities.Customize
                    .RemoveDuplicates(product.Select(c => c.Customizes?.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name))));

                objDto.Add(dto);
            }

            return new HandleResponse()
            {
                Content = objDto
            };
        }

        private Domain.Entities.Variants[] GetVariants(Domain.Entities.Variants[] vars, 
            IEnumerable<Domain.Entities.Server> servers)
        {
            var variants = new List<Domain.Entities.Variants>();
            foreach (var server in servers)
            {
                if (!vars.Any(a => a.Server.Id.Equals(server.Id)))
                    variants.Add(_mapper.Map<Domain.Entities.Variants>(server));
            }
            variants.AddRange(vars.ToList());

            return variants.ToArray();
        }
    }
}
