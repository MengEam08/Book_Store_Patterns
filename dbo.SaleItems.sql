CREATE TABLE [dbo].[SaleItems] (
     Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    ProductName NVARCHAR(255) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Quantity INT NOT NULL,
);

