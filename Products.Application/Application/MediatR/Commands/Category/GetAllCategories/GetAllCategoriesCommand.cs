using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Category.GetAllCategories
{
    public class GetAllCategoriesCommand : IRequest<Response>
    {
    }
}
