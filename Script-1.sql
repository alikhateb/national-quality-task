CREATE DATABASE task;

----------------------------------------------------------------------------------------------------------------------------------

-- Create Customer Table
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(100) NOT NULL,      -- Customer name
    Code NVARCHAR(50) NOT NULL,       -- Customer code
    DateOfRegistration DATE NOT NULL -- Date of registration
);

-- Create Product Table
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    Name NVARCHAR(100) NOT NULL,      -- Product name
    Price DECIMAL(18, 2) NOT NULL     -- Product price
);

-- Create CustomerProduct Table (Many-to-Many Relationship)
CREATE TABLE CustomerProducts (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    CustomerId INT NOT NULL,          -- Foreign key to Customer table
    ProductId INT NOT NULL,           -- Foreign key to Product table
    CONSTRAINT FK_CustomerProduct_Customer FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_CustomerProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT UQ_CustomerProduct UNIQUE (CustomerId, ProductId) -- Ensure unique combinations
);

-- Reset the Identity Seed
DBCC CHECKIDENT ('Products', RESEED, 3);
DBCC CHECKIDENT ('Customers', RESEED, 2);
DBCC CHECKIDENT ('CustomerProducts', RESEED, 3);
----------------------------------------------------------------------------------------------------------------------------------

-- Insert sample Customers
INSERT INTO Customers (Name, Code, DateOfRegistration)
VALUES 
    ('John Doe', 'CUST001', '2023-01-15'),
    ('Jane Smith', 'CUST002', '2023-02-20');

-- Insert sample Products
INSERT INTO Products (Name, Price)
VALUES 
    ('tea', 10.00),
    ('sugar', 20.00),
    ('salad', 30.00);

-- Insert sample CustomerProduct relationships
INSERT INTO CustomerProducts (CustomerId, ProductId)
VALUES 
    (1, 1), -- John Doe bought a Laptop
    (1, 2), -- John Doe bought a Smartphone
    (2, 3); -- Jane Smith bought a Tablet
    
----------------------------------------------------------------------------------------------------------------------------------
    
-- Get all customers and their products
SELECT c.Name AS CustomerName, p.Name AS ProductName, p.Price
FROM Customers c
JOIN CustomerProducts cp ON c.Id = cp.CustomerId
JOIN Products p ON cp.ProductId = p.Id;


SELECT * FROM Customers WHERE Id = 1;


 SELECT
     c.Id AS CustomerId,
     c.Name AS CustomerName,
     c.Code AS CustomerCode,
     c.DateOfRegistration,
     p.Id AS ProductId,
     p.Name AS ProductName,
     p.Price
 FROM Customers c
 LEFT JOIN CustomerProducts cp
  ON c.Id = cp.CustomerId
 LEFT JOIN Products p
  ON cp.ProductId = p.Id;






