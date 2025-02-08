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
    private readonly Mock<IDataProvider> _mockDataProvider;
    private readonly EmployeeDataProvider _employeeDataProvider;

    public EmployeeDataProviderTests()
    {
        _mockDataProvider = new Mock<IDataProvider>();
        _employeeDataProvider = new EmployeeDataProvider(_mockDataProvider.Object);
    }

    [Fact]
    public void GetEmployee_ShouldReturnEmployee()
    {
        // Arrange
        var employeeId = 1;
        var expectedEmployee = new Employee(employeeId, "John Doe", 1);
        _mockDataProvider.Setup(dp => dp.SelectQuery<Employee>(It.IsAny<Query>()))
                         .Returns(new List<Employee> { expectedEmployee });

        // Act
        var result = _employeeDataProvider.GetEmployee(employeeId);

        // Assert
        Assert.Single(result);
        Assert.Equal(expectedEmployee, result.First());
    }

    [Fact]
    public void GetEmployees_ShouldReturnAllEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee(1, "John Doe", 1),
            new Employee(2, "Jane Doe", 2)
        };
        _mockDataProvider.Setup(dp => dp.SelectQuery<Employee>(It.IsAny<Query>()))
                         .Returns(employees);

        // Act
        var result = _employeeDataProvider.GetEmployees();

        // Assert
        Assert.Equal(employees.Count, result.Count);
    }

    // Additional test cases for other methods can be implemented similarly.
}



public class DepartmentDataProviderTests
{
    
}
