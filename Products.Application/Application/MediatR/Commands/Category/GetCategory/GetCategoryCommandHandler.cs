using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.GetCategory
{
    public class GetCategoryCommandHandler : AbstractRequestHandler<GetCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(GetCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = _categoryRepository.GetCategory(request.Id).Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
