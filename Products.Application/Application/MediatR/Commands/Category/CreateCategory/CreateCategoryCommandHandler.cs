using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandHandler : AbstractRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.Category.Name;
            request.Category.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLowerInvariant());

            var result = _categoryRepository.InsertAsync(request.Category).Result;

            return new HandleResponse() 
            {
                Content = result
            };
        }
    }
}
