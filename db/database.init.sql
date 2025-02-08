-- [!] ( on delete, on update is added on department reference )
/*
    [♦] TODO: 
        1. Create database
        2. Create tables
        3. Insert data
*/

--  clean DB
-- use master;
-- drop database SW_Company;

create database SW_Company;

use SW_Company;


-- Tables used in the App 
CREATE TABLE Departments
(
    DepartmentID INT PRIMARY KEY IDENTITY,
    DepartmentName NVARCHAR(100) NOT NULL
);

CREATE TABLE Employees
(
    EmployeeID INT PRIMARY KEY IDENTITY,
    EmployeeName NVARCHAR(100) NOT NULL,
    DepartmentID INT default null FOREIGN KEY REFERENCES Departments(DepartmentID)  on delete set null on update cascade
);


insert into Departments (DepartmentName) values 
('IT'),
('Finance'),
('Administration');


insert into Employees (EmployeeName, DepartmentID) values 
('John Smith', 1),
('Jane Johnson', 2),
('Robert Brown', null),
('Kathy Davis', 1),
('Michael Miller', 2),
('Sarah Taylor', 3);

-- test insert 
use master;
drop database SW_Company;
use SW_Company;
select * from Departments;

select * from Employees;


-- Queries used in the App


-- display Employees
select 
    Employees.*, Departments.DepartmentName 
    from Employees left join Departments
    on Employees.DepartmentID = Departments.DepartmentID;

-- display Employees sorted by id asc
select 
    * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    order by EmployeeID;

-- display Employees sorted by id desc
select 
    * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    order by EmployeeID desc;

-- display Employees sorted by name asc
select 
    * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    order by EmployeeName;

-- display Employees sorted by name desc
select 
    * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    order by EmployeeName desc;

-- display Employees searched by name
select * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    where EmployeeName = 'John Smith';

-- select * 
--     from Employees join Departments
--     on Employees.DepartmentID = Departments.DepartmentID
--     where EmployeeName = @empname;

-- display Employees searched by id
select * 
    from Employees join Departments
    on Employees.DepartmentID = Departments.DepartmentID
    where EmployeeID = 1;

-- select * 
--     from Employees join Departments
--     on Employees.DepartmentID = Departments.DepartmentID
--     where EmployeeID = @empid;





-- display Departments

select * from Departments;

-- display Departments sorted by id asc
select * from Departments order by DepartmentID;

-- display Departments sorted by id desc
select * from Departments order by DepartmentID desc;

-- display Departments sorted by name asc
select * from Departments order by DepartmentName;

-- display Departments sorted by name desc
select * from Departments order by DepartmentName desc; 

-- display Departments searched by name
select * from Departments where DepartmentName = 'IT';

-- select * from Departments where DepartmentName = @depname;

-- display Departments searched by id
select * from Departments where DepartmentID = 1;

-- select * from Departments where DepartmentID = @depid;

