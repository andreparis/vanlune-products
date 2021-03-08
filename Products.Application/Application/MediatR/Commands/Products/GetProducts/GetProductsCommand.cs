using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.GetProducts
{
    public class GetProductsCommand : IRequest<Response>
    {
        public string CategoryName { get; set; }
        public int GameId { get; set; }
    }
}
