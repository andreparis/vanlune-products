using MediatR;
using Products.Domain.Entities;
using System.Collections.Generic;

namespace Products.Application.Application.MediatR.Commands.Tags.CreateOrUpdateTags
{
    public class CreateOrUpdateTagsCommand : IRequest<Response>
    {
        public IEnumerable<Products.Domain.Entities.Tags> Tags { get; set; }
    }
}
