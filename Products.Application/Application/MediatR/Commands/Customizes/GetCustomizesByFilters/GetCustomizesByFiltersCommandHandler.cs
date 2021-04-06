using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System.Linq;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Customizes.GetCustomizesByFilters
{
    public class GetCustomizesByFiltersCommandHandler : AbstractRequestHandler<GetCustomizesByFiltersCommand>
    {
        private readonly ICustomizeRepository _customizeRepository;

        public GetCustomizesByFiltersCommandHandler(ICustomizeRepository customizeRepository)
        {
            _customizeRepository = customizeRepository;
        }

        internal override HandleResponse HandleIt(GetCustomizesByFiltersCommand request, CancellationToken cancellationToken)
        {
            var result = _customizeRepository.GetCustomizesByFilters(request.Filters).Result;

            if (!result.Any())
                return new HandleResponse()
                {
                    Error = "No customize found!"
                };

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
