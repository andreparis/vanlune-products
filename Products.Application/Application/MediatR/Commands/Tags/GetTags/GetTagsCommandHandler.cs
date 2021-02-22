using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Tags.GetTags
{
    public class GetTagsCommandHandler : AbstractRequestHandler<GetTagsCommand>
    {
        private readonly ITagsRepository _tagsRepository;

        public GetTagsCommandHandler(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        internal override HandleResponse HandleIt(GetTagsCommand request, CancellationToken cancellationToken)
        {
            var result = _tagsRepository.GetTags().Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
