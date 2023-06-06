using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    static public class ItemsDroped
    {
        static public Dictionary<int, List<float>> dropedItems = new Dictionary<int, List<float>>();
    }
}