CREATE TABLE Consumption (
    Id                  INT PRIMARY KEY AUTO_INCREMENT,
    ProductId           INT                             NOT NULL,
    QuantityConsumed    INT                             NOT NULL,
    ConsumptionDate     DATE                            NOT NULL,
    AveragePrice        DECIMAL(10, 2)                  NOT NULL,
    TotalCost           DECIMAL(10, 2)                  NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
