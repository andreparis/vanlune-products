using MediatR;
using Products.Domain.Entities;

namespace Products.Application.Application.MediatR.Commands.GetProductsByTags
{
    public class GetProductsByTagsCommand : IRequest<Response>
    {
        public string TagName { get; set; }
        public int GameId { get; set; }
    }
}
