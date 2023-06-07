using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;


namespace inventory.Server
{
    static public class Items
    {

        public static Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

        public static List<Dictionary<string, string>> inInventory = new List<Dictionary<string, string>>();

    }
}