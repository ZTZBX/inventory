using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;

namespace inventory.Server
{
    public class InventoryData : BaseScript
    {
        public InventoryData()
        {
        }

        public void DropItems(string token, string itemname, int quantity)
        {
            // getting user
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);

            // get existing items 
            string itemQuery = $"SELECT quantity FROM `inventory` WHERE username = '{username[0][0]}' and name = '{itemname}'";
            dynamic itemQuantity = Exports["fivem-mysql"].raw(itemQuery);

            if (Int32.Parse(itemQuantity[0][0]) == quantity)
            {
                string deleteItem = $"DELETE FROM `inventory` WHERE username = '{username[0][0]}' and name = '{itemname}'";
                Exports["fivem-mysql"].raw(deleteItem);
                return;
            }

            int currentQuantity = Int32.Parse(itemQuantity[0][0]) - quantity;

            string changeQuantity = $"UPDATE `inventory` SET `quantity` = '{currentQuantity.ToString()}' WHERE username = '{username[0][0]}' and name = '{itemname}'";
            Exports["fivem-mysql"].raw(changeQuantity);
        }

        public int GetCurrentBackPackLevel(string token)
        {
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);

            string query = $"select backpacklevel from inventoryconfig where username='{username[0][0]}'";
            dynamic result = Exports["fivem-mysql"].raw(query);
            
            return Int32.Parse(result[0][0]);
        }

        public int GetInventorySize(string token)
        {
            string query = $"select maxslots from inventoryconfig as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            if (result[0][0] == "")
            {
                string usernameQuery = $"SELECT username from players where token='{token}'";
                dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
                query = $"INSERT INTO `inventoryconfig` (`username`) VALUES ('{username[0][0]}')";
                Exports["fivem-mysql"].raw(query);
                query = $"select maxslots from inventoryconfig as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}'";
                result = Exports["fivem-mysql"].raw(query);
            }

            return Int32.Parse(result[0][0]);
        }

        public bool CheckItemIn(string token, string itemname)
        {

            string query = $"select name from inventory as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}' and pinv.name='{itemname}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            if (result[0][0] == "") { return false; }

            return true;
        }


        public int CheckSlotPositionCanPlace(string token, string username)
        {

            int size = GetInventorySize(token);

            string query = $"select slotposition from `inventory` where `username`='{username}'";
            dynamic slotsUsed = Exports["fivem-mysql"].raw(query);

            List<int> positions = new List<int>();

            foreach (var s in slotsUsed)
            {
                positions.Add(Int32.Parse(s[0]));
            }

            for (int i = 1; i <= size; i++)
            {
                if (!positions.Contains(i))
                {
                    return i;
                }
            }

            return -1;

        }

        public void AddItemToInventory(string token, string itemname, int quantity)
        {
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            int slot = CheckSlotPositionCanPlace(token, username[0][0]);
            if (slot == -1) { return; }
            string query = $"INSERT INTO `inventory` (`username`, `name`, `quantity`, `slotposition`) VALUES ('{username[0][0]}', '{itemname}', '{quantity.ToString()}', '{slot.ToString()}')";

            Exports["fivem-mysql"].raw(query);
        }

        public void AddItemQuantityInInventory(string token, string itemname, int quantity)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string getQuantity = $"SELECT quantity from `inventory` where `username` = '{username[0][0]}' and `name` = '{itemname}'";
            dynamic dbQuantity = Exports["fivem-mysql"].raw(getQuantity);
            int quantityResult = Int32.Parse(dbQuantity[0][0]) + quantity;
            string query = $"UPDATE `inventory` SET `quantity` = '{quantityResult.ToString()}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";
            Exports["fivem-mysql"].raw(query);
        }

        public void RemoveItemQuantityInInventory(string token, string itemname, int quantity)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string query;
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string getQuantity = $"SELECT quantity from `inventory` where `username` = '{username[0][0]}' and `name` = '{itemname}'";
            dynamic dbQuantity = Exports["fivem-mysql"].raw(getQuantity);
            int quantityResult = Int32.Parse(dbQuantity[0][0]) - quantity;
            if (quantityResult == 0)
            {
                query = $"DELETE FROM `inventory` WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";
                Exports["fivem-mysql"].raw(query);
                return;
            }

            query = $"UPDATE `inventory` SET `quantity` = '{quantityResult.ToString()}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";
            Exports["fivem-mysql"].raw(query);
        }

        public void DeleteItemFromInventory(string token, string itemname)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"delete from inventory where username = '{username[0][0]}' and name = '{itemname}'";
            Exports["fivem-mysql"].raw(query);
        }

        public void ChangeItemPosition(string token, string itemname, string position)
        {
            if (!CheckItemIn(token, itemname)) { return; }

            string queryUpdateMyPosition;
            string queryUpdateMyPositionToNULL;
            string queryUpdateOtherItemPositionWithMyPosition;
            string queryUpdateOtherItemPositionWithMyNULL;
            bool itemInMovingPosition = false;

            // Getting username
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);

            string itemSwapPosition = $"select name from inventory where username = '{username[0][0]}' and slotposition = '{position}'";
            dynamic currentSwapItemPosition = Exports["fivem-mysql"].raw(itemSwapPosition);

            // check if somebody in the position
            if (currentSwapItemPosition.Count > 0)
            {
                itemInMovingPosition = true;

                // check if i'am already in this position
                if (currentSwapItemPosition[0][0] == itemname) { return; }
            }

            // Swaping positions with the item
            string myPositionQuery = $"select slotposition from inventory where username = '{username[0][0]}' and name = '{itemname}'";
            dynamic myPositionExec = Exports["fivem-mysql"].raw(myPositionQuery);



            string myCurrentPosition = myPositionExec[0][0];

            queryUpdateMyPosition = $"UPDATE `inventory` SET `slotposition` = '{position}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";

            if (!itemInMovingPosition)
            {
                Exports["fivem-mysql"].raw(queryUpdateMyPosition);
                return;

            }

            // Firt null all positions to make constrain dont raise
            // In reality is not a null beacouse the field dont accept null values i set i very big value to do the swapp
            queryUpdateOtherItemPositionWithMyNULL = $"UPDATE `inventory` SET `slotposition` = '9998' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{currentSwapItemPosition[0][0]}'";
            queryUpdateMyPositionToNULL = $"UPDATE `inventory` SET `slotposition` = '9999' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";

            Exports["fivem-mysql"].raw(queryUpdateOtherItemPositionWithMyNULL);
            Exports["fivem-mysql"].raw(queryUpdateMyPositionToNULL);


            // Now we can swap the position between the items
            queryUpdateOtherItemPositionWithMyPosition = $"UPDATE `inventory` SET `slotposition` = '{myCurrentPosition}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{currentSwapItemPosition[0][0]}'";

            Exports["fivem-mysql"].raw(queryUpdateMyPosition);
            Exports["fivem-mysql"].raw(queryUpdateOtherItemPositionWithMyPosition);
        }

        public List<Dictionary<string, string>> GetItemsMetadata(string token)
        {

            List<Dictionary<string, string>> listResult = new List<Dictionary<string, string>>();

            string query = $"select name, quantity, slotposition from inventory as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            string itemData = $"select name, image, descriptiontitle, `description`, `type`, unit, `weight` from itemsmetadata";
            dynamic metaDataProd = Exports["fivem-mysql"].raw(itemData);

            if (result[0][0] != "")
            {
                foreach (dynamic line in result)
                {

                    foreach (dynamic mT in metaDataProd)
                    {
                        if (mT[0] == line[0])
                        {
                            Dictionary<string, string> r = new Dictionary<string, string>();
                            r.Add("name", line[0]);
                            r.Add("quantity", line[1]);
                            r.Add("unit", mT[5]);
                            r.Add("image", mT[1]);
                            r.Add("descriptiontitle", mT[2]);
                            r.Add("description", mT[3]);
                            r.Add("slotposition", line[2]);
                            r.Add("type", mT[4]);
                            r.Add("weight", mT[6]);
                            listResult.Add(r);
                            break;
                        }
                    }
                }
            }

            return listResult;
        }


    }
}