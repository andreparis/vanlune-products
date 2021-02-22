using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.GetAllCategories
{
    class GetAllCategoriesCommandHandler : AbstractRequestHandler<GetAllCategoriesCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(GetAllCategoriesCommand request, CancellationToken cancellationToken)
        {
            var result = _categoryRepository.GetAll().Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
