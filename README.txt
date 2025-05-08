*** Tools for building this project:
1. Microsoft Visual Studio
-> Used for developing the program in C# language.
2. Microsoft SQL Server 2018
-> Acts as the database backend for managing and storing data.
====================================================================================================

*** How to Run the Program
(Login Username: Meng, Password: 123) for Admin
1. Open the project file on Microsoft Visual, look for (Data Connection) and find the file name (MyBook.mdf). Right-click, select the property, and on the right side find the connection string and "Copy" connection. 
2. Next, go to the fine folder named "Patterns/Repository.cs" and open private SqlConnection connection = new SqlConnection(@"(past part that you copied from the "connection string"). and run the programme
====================================================================================================

***Features

1. Login form: only for admin: Username (Meng), Password (123) only for Admin
2. CategoryForm : Show the category table that added the form CategoryModule.
3. CategoryModule: Create a category (Name, Address, Phone) and have buttons (Save and Delete)**only for Admin**
4. ProductForm : Show a product table that added a form to ProductModule.
5. ProductModule : Create product (Name, Type, Category, QTY, Price) and buttons (Save and Delete)**only for Admin**
6. UserForm: Show user table that added form UserModule.
7. UserModule: Create user (Name, Phone, Role, and Password) and buttons (Save and Delete)**only for Admin**
8. Showing the amount of customers, the amount of product that sells and price.
====================================================================================================