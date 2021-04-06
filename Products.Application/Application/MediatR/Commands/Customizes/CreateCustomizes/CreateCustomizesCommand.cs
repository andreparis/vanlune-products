using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Customizes.CreateCustomizes
{
    public class CreateCustomizesCommand : IRequest<Response>
    {
        public IEnumerable<Customize> Customizes { get; set; }
    }
}
