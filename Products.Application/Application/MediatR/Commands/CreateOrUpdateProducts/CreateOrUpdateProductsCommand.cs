using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.CreateOrUpdateProducts
{
    public class CreateOrUpdateProductsCommand : IRequest<Response>
    {
        public Product Product { get; set; }
    }
}
