using MediatR;
using Products.Domain.Entities;
using System.Collections.Generic;

namespace Products.Application.Application.MediatR.Commands.Variants.CreateOrUpdateVariants
{
    public class CreateOrUpdateVariantsCommand : IRequest<Response>
    {
        public IEnumerable<Products.Domain.Entities.Variants> Variants { get; set; }
    }
}
