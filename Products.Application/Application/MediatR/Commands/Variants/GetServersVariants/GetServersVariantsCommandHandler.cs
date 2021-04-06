using AutoMapper;
using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Variants.GetServersVariants
{
    class GetServersVariantsCommandHandler : AbstractRequestHandler<GetServersVariantsCommand>
    {
        private readonly IServerRepository _server;
        private readonly IMapper _mapper;

        public GetServersVariantsCommandHandler(IServerRepository server,
            IMapper mapper)
        {
            _server = server;
            _mapper = mapper;
        }

        internal override HandleResponse HandleIt(GetServersVariantsCommand request, CancellationToken cancellationToken)
        {
            var server = _server.GetAll(request.Game).Result;

            var variants = _mapper.Map<IEnumerable<Domain.Entities.Variants>>(server);

            return new HandleResponse()
            {
                Content = variants
            };
        }
    }
}
