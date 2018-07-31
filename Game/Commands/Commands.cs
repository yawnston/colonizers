using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    public enum Command
    {
        PickColonist,
        TakeOmnium,
        DrawModules,
        KeepModule,
        DoNothing,
        StealOmnium,
        SwapHands,
        BuildNothing,
        BuildModule
    }
}