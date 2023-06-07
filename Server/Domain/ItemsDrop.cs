using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;


namespace inventory.Server
{
    static public class ItemDrop
    {
        // the key is the backpack object id and the value is a dictionary with a key ho is the item name and the quantity at value.
        public static Dictionary<int, Dictionary<string, int>> stackItemsGround = new Dictionary<int, Dictionary<string, int>>();

        // the key is the backpack object id and the value are the coords x,y,z
        public static Dictionary<int, List<float>> backpackLocations = new Dictionary<int, List<float>>();

    }
}