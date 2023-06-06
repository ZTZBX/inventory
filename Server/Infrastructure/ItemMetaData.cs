using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;

namespace inventory.Server
{
    public class ItemMetaData : BaseScript
    {
        public ItemMetaData()
        {
        }

        public Dictionary<string, List<string>> GetItemMeta()
        {
            string query = $"SELECT name, image, descriptiontitle, description, unit FROM `itemsmetadata`";
            dynamic result = Exports["fivem-mysql"].raw(query);

            Dictionary<string, List<string>> meta = new Dictionary<string, List<string>>();

            if (result.Count > 0)
            {
                foreach (var i in result)
                {
                    List<string> currentItem = new List<string>();
                    foreach (string y in i)
                    {
                        currentItem.Add(y);
                    }

                    meta.Add(currentItem[0], currentItem);
                }

            }

            return meta;
        }



    }
}