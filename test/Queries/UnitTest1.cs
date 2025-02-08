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

    [Fact]
    public void SelectAllTest()
    {

        SQLDataProvider dataProvider = new SQLDataProvider();
        EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(dataProvider);

        List<Employee> employeesActual = employeeDataProvider.GetEmployees();

        Assert.Equal(employeesExpected, employeesActual, new EmployeeComparer());
    }

    [Fact]
    public void UpdateTest()
    {
        Assert.True(true);
    }
}



public class DepartmentDataProviderTests
{

}
