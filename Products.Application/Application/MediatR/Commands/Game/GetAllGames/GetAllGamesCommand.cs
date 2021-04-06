using MediatR;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.Game.GetAllGames
{
    public class GetAllGamesCommand : IRequest<Response>
    {
    }
}
