using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
