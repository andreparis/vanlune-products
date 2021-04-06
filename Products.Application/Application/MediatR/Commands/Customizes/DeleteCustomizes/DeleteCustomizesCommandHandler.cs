using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Customizes.DeleteCustomizes
{
    public class DeleteCustomizesCommandHandler : AbstractRequestHandler<DeleteCustomizesCommand>
    {
        private readonly ICustomizeRepository _customizeRepository;

        public DeleteCustomizesCommandHandler(ICustomizeRepository customizeRepository)
        {
            _customizeRepository = customizeRepository;
        }

        internal override HandleResponse HandleIt(DeleteCustomizesCommand request, CancellationToken cancellationToken)
        {
            var customizesLinkeds = _customizeRepository.GetCustomizeLinkedToProduct(request.Ids).Result;
            var idsHoldeds = customizesLinkeds.Select(a => a.Id).ToList();
            var idsToDelete = request.Ids.Where(c => !idsHoldeds.Contains(c));

            _customizeRepository.DeleteAllByIdAsync(idsToDelete.ToArray()).GetAwaiter().GetResult();

            return new HandleResponse()
            {
                Content = idsToDelete
            };
        }
    }
}
