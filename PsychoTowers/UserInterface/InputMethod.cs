using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PsychoTowers
{
    public enum Command
    {
        None,
        PlaceWall,
        PlaceTower,
        RemoveAt
    }

    public delegate void Request(Command command, int requestArgs); 

    public interface InputMethod
    {

        int X { get; }
        int Y { get; }


        int Tower { get; }
        bool InUse { get; set; }
        Thread InputListener { get; }

        event Request Act;



    }
}
