using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Category.GetCategoryByGame
{
    public class GetCategoryByGameCommandHandler : AbstractRequestHandler<GetCategoryByGameCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByGameCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        internal override HandleResponse HandleIt(GetCategoryByGameCommand request, CancellationToken cancellationToken)
        {
            var result = _categoryRepository.GetCategoriesByGameId(request.Game).Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
