using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommandHandler : AbstractRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Category.Id <= 0)
            {
                return new HandleResponse()
                {
                    Error = "You must send an Id to be updated!"
                };
            }

            var result = _categoryRepository.UpdateAsync(request.Category).Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
