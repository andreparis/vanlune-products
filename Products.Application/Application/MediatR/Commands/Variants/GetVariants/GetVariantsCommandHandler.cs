using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Variants.GetVariants
{
    public class GetVariantsCommandHandler : AbstractRequestHandler<GetVariantsCommand>
    {
        private readonly IVariantsRepository _variantsRepository;

        public GetVariantsCommandHandler(IVariantsRepository variantsRepository)
        {
            _variantsRepository = variantsRepository;
        }

        internal override HandleResponse HandleIt(GetVariantsCommand request, CancellationToken cancellationToken)
        {
            var result = _variantsRepository.GetAll().Result;

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
