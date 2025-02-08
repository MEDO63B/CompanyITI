using Microsoft.Data.SqlClient;
using Moq;
using UtilsLib;
using EmployeeLib;
using DepartmentLib;
using RepositoryLib;

namespace Queries;

public class SQLTests
{

    [Fact]
    public void ConnectTest()
    {
        SqlConnection connection = new SqlConnection(SQLUtils.CONNECTION_STRING);

        connection.Open();
        Assert.Equal(System.Data.ConnectionState.Open, connection.State);

        connection.Close();
        Assert.Equal(System.Data.ConnectionState.Closed, connection.State);
    }

}


public class EmployeeDataProviderTests
{
    static List<Department> departmentsExpected = new List<Department>(){
            new Department(1, "IT"),
            new Department(2, "Finance"),
            new Department(3, "Administration")
        };
    static List<Employee> employeesExpected = new List<Employee>(){
            new Employee(1, "John Smith", departmentsExpected[0]),
            new Employee(2, "Jane Johnson", departmentsExpected[1]),
            new Employee(3, "Robert Brown", null),
            new Employee(4, "Kathy Davis", departmentsExpected[0]),
            new Employee(5, "Michael Miller", departmentsExpected[1]),
            new Employee(6, "Sarah Taylor", departmentsExpected[2])
        };
    
    private readonly EmployeeDataProvider employeeDataProvider;
    private readonly SQLDataProvider dataProvider;

    public EmployeeDataProviderTests(){

        dataProvider = new SQLDataProvider();
        employeeDataProvider = new EmployeeDataProvider(dataProvider);
    }
    [Fact]
    public void SelectAllTest()
    {

        List<Employee> employeesActual = employeeDataProvider.GetEmployees();

        Assert.Equal(employeesExpected, employeesActual, new EmployeeComparer());
        
    }
    [Fact]
    public void SelectByIdTest()
    {

        Employee emp = employeesExpected[2];
        Employee employeesActual = employeeDataProvider.GetEmployee(emp.ID);

        Assert.Equal(emp, employeesActual, new EmployeeComparer());
    }


    [Fact]
    public void InsertTest()
    {

        Employee newEmp = new Employee(0, "John Doe", departmentsExpected[2]);

        Assert.Equal(1, employeeDataProvider.InsertEmployee(newEmp));
    }

    [Fact]
    public void DeleteTest()
    {

        Employee newEmp = new Employee(7, "John Doe", departmentsExpected[2]);

        Assert.Equal(1, employeeDataProvider.DeleteEmployee(newEmp));
    }

    [Fact]
    
    public  void UpdateTest()
    {
        // to update DB
        Employee newEmp = new Employee(7, "John Doe1111", departmentsExpected[1]);
        // to reset DB
        // Employee newEmp = new Employee(7, "John Doe", departmentsExpected[2]);

        Assert.Equal(1, employeeDataProvider.UpdateEmployee(newEmp));
    }
}



public class DepartmentDataProviderTests
{

}
