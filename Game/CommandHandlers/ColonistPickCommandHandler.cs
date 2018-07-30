using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class ColonistPickCommandHandler : IRequestHandler<ColonistPickCommand, GameState>
    {
        public Task<GameState> Handle(ColonistPickCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
