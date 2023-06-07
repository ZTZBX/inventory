using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    static public class ItemsDroped
    {
        // the first value is the id of the object backpack and the secound value is the backpack coords x,y,z
        static public Dictionary<int, List<float>> dropedItems = new Dictionary<int, List<float>>();

        // the key is the name of the item and the value is the quantity of the backpack
        static public Dictionary<string, int> CurrentBackPack = new Dictionary<string, int>();

        static public int CurrectBackPackIdObject = -1;
    }
}