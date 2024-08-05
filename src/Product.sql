CREATE TABLE Product (
    Id              INT PRIMARY KEY AUTO_INCREMENT,
    PartNumber      VARCHAR(255)                    NOT NULL,
    Name            VARCHAR(255)                    NOT NULL,
    Price           DECIMAL(10, 2)                  NOT NULL
);