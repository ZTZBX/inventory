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

        public void AddItemToInventory(string token, string itemname, int quantity, string unit, string imageName, string descriptiontitle, string description)
        {
            if (CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"INSERT INTO `inventory` (`username`, `name`, `quantity`, `unit`, `image`, `descriptiontitle`, `description`) VALUES ('{username[0][0]}', '{itemname}', '{quantity.ToString()}', '{unit}', '{imageName}', '{descriptiontitle}', '{description}')";
            Exports["fivem-mysql"].raw(query);
        }

        public void ChangeQuantityItem(string token, string itemname, int quantity)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from players where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"UPDATE `inventory` SET `quantity` = '{quantity.ToString()}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";
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

            string query = $"select name, quantity, unit, image, descriptiontitle, description, slotposition from inventory as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            if (result[0][0] != "")
            {
                foreach (dynamic line in result)
                {
                    Dictionary<string, string> r = new Dictionary<string, string>();
                    r.Add("name", line[0]);
                    r.Add("quantity", line[1]);
                    r.Add("unit", line[2]);
                    r.Add("image", line[3]);
                    r.Add("descriptiontitle", line[4]);
                    r.Add("description", line[5]);
                    r.Add("slotposition", line[6]);
                    listResult.Add(r);
                }
            }

            return listResult;
        }


    }
}