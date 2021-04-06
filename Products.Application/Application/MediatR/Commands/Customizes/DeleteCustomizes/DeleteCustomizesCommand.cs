using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Customizes.DeleteCustomizes
{
    public class DeleteCustomizesCommand : IRequest<Response>
    {
        public int[] Ids { get; set; }
    }
}
