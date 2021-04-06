using MediatR;
using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.CreateProducts
{
    public class CreateOrUpdateProductsCommand : IRequest<Response>
    {
        public ProductDto Product { get; set; }
    }
}
