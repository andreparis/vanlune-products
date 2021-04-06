using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.GetProductById
{
    public class GetProductByIdCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
