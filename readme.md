# ITI Company

`<img src='https://images.app.goo.gl/1gd8VRDF8vBoXajj7'>`

![Dot net 8 logo](https://images.app.goo.gl/1gd8VRDF8vBoXajj7)


## How To Install Project

1. clone Repo ` git clone https://github.com/MEDO63B/CompanyITI.git`
2. visit `project\source\UtilsLib\Utils.cs` and update connection string in **SQLUtils** Class
3. visit `project\db\database.init.sql` run
   1. `create database SW_Company;`
   2. `use SW_Company;`
   3. ```sql
      CREATE TABLE Departments
      (
          DepartmentID INT PRIMARY KEY IDENTITY,
          DepartmentName NVARCHAR(100) NOT NULL
      );
      ```
   4. ```sql
      CREATE TABLE Employees
      (
          EmployeeID INT PRIMARY KEY IDENTITY,
          EmployeeName NVARCHAR(100) NOT NULL,
          DepartmentID INT default null FOREIGN KEY REFERENCES Departments(DepartmentID) 
          on delete set null on update cascade
      );
      ```
   5. ```sql
      insert into Departments (DepartmentName) values 
      ('IT'),
      ('Finance'),
      ('Administration');
      ```
   6. ```sql
      insert into Employees (EmployeeName, DepartmentID) values 
      ('John Smith', 1),
      ('Jane Johnson', 2),
      ('Robert Brown', null),
      ('Kathy Davis', 1),
      ('Michael Miller', 2),
      ('Sarah Taylor', 3);
      ```


## Run With in test Mode

>  [ ! ] avoid run test with out filter to prevent colision of asynchronus test cases that is not handled 
>
>  [ + ] must run like this: `dotnet test .\source\company\company.sln --filter='EmployeeDataProviderTests.DeleteTest'`
