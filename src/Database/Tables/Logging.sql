CREATE TABLE logging (
  id int PRIMARY KEY  AUTO_INCREMENT NOT NULL,
  Timestamp varchar(100) DEFAULT NULL,
  Level varchar(15) DEFAULT NULL,
  Template text,
  Message text,
  Exception text,
  Properties text,
  _ts timestamp NULL DEFAULT CURRENT_TIMESTAMP  
)
