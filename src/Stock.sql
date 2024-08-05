CREATE TABLE Stock (
    StockId       INT AUTO_INCREMENT PRIMARY KEY,
    ProductId     INT                             NOT NULL,
    Quantity      INT                             NOT NULL,
    UnitPrice     DECIMAL(10, 2)                  NOT NULL,
    AveragePrice  DECIMAL(10, 2)                  NOT NULL,
    Total         DECIMAL(10, 2)                  NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);