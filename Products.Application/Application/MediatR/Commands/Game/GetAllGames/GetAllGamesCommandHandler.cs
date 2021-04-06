using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Products.Application.Application.MediatR.Commands.Game.GetAllGames
{
    public class GetAllGamesCommandHandler : AbstractRequestHandler<GetAllGamesCommand>
    {
        private readonly IGameRepository _gameRepository;

        public GetAllGamesCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        internal override HandleResponse HandleIt(GetAllGamesCommand request, CancellationToken cancellationToken)
        {
            var games = _gameRepository.GetAll().Result;

            return new HandleResponse()
            {
                Content = games
            };
        }
    }
}
