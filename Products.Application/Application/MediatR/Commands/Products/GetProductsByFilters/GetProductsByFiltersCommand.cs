using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.GetProductsByFilters
{
    public class GetProductsByFiltersCommand : IRequest<Response>
    {
        public IDictionary<string, string> Filters { get; set; }
    }
}
