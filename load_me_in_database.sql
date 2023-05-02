/*
    INVENTORY ZTZBX SQL SCRIPT
    */

    CREATE TABLE inventory (
    username varchar(50) NOT NULL,
    name varchar(50) NOT NULL,
    quantity int NOT NULL DEFAULT 0, -- if the item is 0 is interpret like the item dont exits in the inventory
    unit varchar(10) NOT NULL,
    `image` varchar(100),
    descriptiontitle varchar(100) NOT NULL,
    `description` varchar(255) NOT NULL,
    FOREIGN KEY (username) REFERENCES players(username),
    UNIQUE KEY `unique_player_item` (`username`,`name`)
)ENGINE=InnoDB;