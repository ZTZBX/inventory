/*
    INVENTORY ZTZBX SQL SCRIPT
*/

CREATE TABLE itemsmetadata (
    `name` varchar(50) NOT NULL,
    `image` varchar(100),
    descriptiontitle varchar(100) NOT NULL,
    `description` varchar(255) NOT NULL,
    `type` varchar(255) NOT NULL, -- this is to indicate if is a consumible, money or clothing
    unit varchar(10) NOT NULL,
    PRIMARY KEY(`name`)
)ENGINE=InnoDB;


CREATE TABLE inventory (
    username varchar(50) NOT NULL,
    slotposition int NOT NULL,
    `name` varchar(50) NOT NULL,
    quantity int NOT NULL DEFAULT 0, -- if the item is 0 is interpret like the item dont exits in the inventory
    FOREIGN KEY (`name`) REFERENCES itemsmetadata(`name`),
    FOREIGN KEY (username) REFERENCES players(username),
    UNIQUE KEY `unique_player_item` (`username`,`name`),
    UNIQUE KEY `unique_player_item_position` (`slotposition`, `username`)
)ENGINE=InnoDB;


CREATE TABLE inventoryconfig (
    username varchar(50) NOT NULL,
    maxslots int NOT NULL DEFAULT 6,
    FOREIGN KEY (username) REFERENCES players(username)
)ENGINE=InnoDB;