using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;


namespace inventory.Server
{
    static public class ItemDrop
    {

        public static Dictionary<int, Dictionary<string, int>> stackItemsGround = new Dictionary<int, Dictionary<string, int>>();
        public static Dictionary<int, List<float>> backpackLocations = new Dictionary<int, List<float>>();

    }
}