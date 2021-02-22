using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Tags.CreateOrUpdateTags
{
    public class CreateOrUpdateTagsCommandHandler : AbstractRequestHandler<CreateOrUpdateTagsCommand>
    {
        private readonly ITagsRepository _tagsRepository;

        public CreateOrUpdateTagsCommandHandler(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        internal override HandleResponse HandleIt(CreateOrUpdateTagsCommand request, CancellationToken cancellationToken)
        {
            foreach(var tag in request.Tags)
            {
                var result = _tagsRepository.InsertAsync(tag).Result;
            }
            return new HandleResponse();
        }
    }
}
