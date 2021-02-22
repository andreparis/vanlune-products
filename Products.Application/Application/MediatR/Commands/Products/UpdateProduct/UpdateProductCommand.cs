using MediatR;
using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using System.Collections.Generic;

namespace Products.Application.Application.MediatR.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Response>
    {
        public Product Product { get; set; }
        public IEnumerable<Domain.Entities.Variants> NewVariants { get; set; }
        public IEnumerable<Domain.Entities.Variants> RemovedVariants { get; set; }
        public string[] NewTags { get; set; }
        public string[] RemovedTags { get; set; }

    }
}
