using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Customizes.GetCustomizesByFilters
{
    public class GetCustomizesByFiltersCommand : IRequest<Response>
    {
        public int Game { get; set; }
        public IDictionary<string, string> Filters { get; set; }
    }
}
