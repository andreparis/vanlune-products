using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Response>
    {
        public Products.Domain.Entities.Category Category { get; set; }
    }
}
