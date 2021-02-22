using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Category.GetCategory
{
    public class GetCategoryCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
