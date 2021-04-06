using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Customizes.UpdateCustomizes
{
    public class UpdateCustomizesCommand : IRequest<Response>
    {
        public IEnumerable<Customize> Customizes { get; set; }
    }
}
