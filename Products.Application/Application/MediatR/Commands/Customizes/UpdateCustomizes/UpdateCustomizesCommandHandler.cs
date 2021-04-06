using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Customizes.UpdateCustomizes
{
    public class UpdateCustomizesCommandHandler : AbstractRequestHandler<UpdateCustomizesCommand>
    {
        private readonly ICustomizeRepository _customizeRepository;

        public UpdateCustomizesCommandHandler(ICustomizeRepository customizeRepository)
        {
            _customizeRepository = customizeRepository;
        }

        internal override HandleResponse HandleIt(UpdateCustomizesCommand request, CancellationToken cancellationToken)
        {
            _customizeRepository.UpdateAllAsync(request.Customizes).GetAwaiter().GetResult();
            var result = _customizeRepository
                .GetCustomizesByIds(request.Customizes.Select(x => x.Id).ToArray())
                .Result;

            if (!result.Any())
                return new HandleResponse()
                {
                    Error = "Failed to recover costimzes!"
                };

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
