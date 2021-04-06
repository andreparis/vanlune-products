using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Variants.GetServersVariants
{
    public class GetServersVariantsCommand : IRequest<Response>
    {
        public int Game { get; set; }
    }
}
