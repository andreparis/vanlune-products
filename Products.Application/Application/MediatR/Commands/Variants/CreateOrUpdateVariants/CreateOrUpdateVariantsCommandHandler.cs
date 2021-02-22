using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Variants.CreateOrUpdateVariants
{
    public class CreateOrUpdateVariantsCommandHandler : AbstractRequestHandler<CreateOrUpdateVariantsCommand>
    {
        private readonly IVariantsRepository _variantsRepository;

        public CreateOrUpdateVariantsCommandHandler(IVariantsRepository variantsRepository)
        {
            _variantsRepository = variantsRepository;
        }

        internal override HandleResponse HandleIt(CreateOrUpdateVariantsCommand request, CancellationToken cancellationToken)
        {
            foreach(var variant in request.Variants)
            {
                var result = _variantsRepository.InsertAsync(variant).Result;
            }
            return new HandleResponse();
        }
    }
}
