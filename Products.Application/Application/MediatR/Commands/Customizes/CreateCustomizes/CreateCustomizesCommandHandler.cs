using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Customizes.CreateCustomizes
{
    public class CreateCustomizesCommandHandler : AbstractRequestHandler<CreateCustomizesCommand>
    {
        private readonly ICustomizeRepository _customizeRepository;

        public CreateCustomizesCommandHandler(ICustomizeRepository customizeRepository)
        {
            _customizeRepository = customizeRepository;
        }

        internal override HandleResponse HandleIt(CreateCustomizesCommand request, CancellationToken cancellationToken)
        {
            var result = _customizeRepository.AddAllAsync(request.Customizes).Result;

            if (!result.Any())
                return new HandleResponse()
                {
                    Error = "No customize inserted!"
                };

            return new HandleResponse()
            {
                Content = result
            };
        }
    }
}
