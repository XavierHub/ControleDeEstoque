drop table Consumption;
drop table Stock;
drop table Product;

CREATE TABLE Product (
    Id             INT          PRIMARY KEY AUTO_INCREMENT,
    PartNumber     VARCHAR(255)                   NOT NULL,
    Name           VARCHAR(255)                   NOT NULL,
    UnitPrice      DECIMAL(10, 2)                 NOT NULL,
    AveragePrice   DECIMAL(10, 2)                 NOT NULL
);

CREATE TABLE Stock (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

CREATE TABLE Consumption (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    QuantityConsumed INT NOT NULL,
    ConsumptionDate DATE NOT NULL,
    TotalCost DECIMAL(10, 2) NOT NULL,
    TotalAveragePrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);


CREATE TABLE logging (
  id int           AUTO_INCREMENT   NOT NULL ,
  Timestamp         varchar(100) DEFAULT NULL,
  Level varchar(15) DEFAULT NULL,
  Template text,
  Message text,
  Exception text,
  Properties text,
  _ts timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
)
