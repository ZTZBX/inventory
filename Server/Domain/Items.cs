using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;


namespace inventory.Server
{
    static public class Items
    {
        // This is the metadata of the items first value is the name and after a list o values
        public static Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

        // this reprezent all players inventories first value is the player token and the List of dicts represent the items he have in the inventory
        public static Dictionary<string, List<Dictionary<string, string>>> inInventory = new Dictionary<string, List<Dictionary<string, string>>>();

    }
}