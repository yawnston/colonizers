using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Serialization
{
    public static class ActionSerializer
    {
        public static JObject Serialize(IRequest<GameState> action)
        {
            var result = new JObject();



            return result;
        }
    }
}
