using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler : AbstractRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return new HandleResponse()
                {
                    Error = "Invalid id!"
                };
            }

            _categoryRepository.DeleteAsync(request.Id).GetAwaiter().GetResult();

            return new HandleResponse();
        }
    }
}
