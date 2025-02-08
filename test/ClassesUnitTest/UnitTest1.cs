using EmployeeLib;
using DepartmentLib;
namespace ClassesUnitTest;

public class UnitTest1
{
    [Fact]
    public void EmployeeInitialization()
    {
        Employee employee = new Employee(1, "John Doe", 1);
        Assert.Equal(1, employee.ID);
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal(1, employee.DepartmentID);
    }

    [Fact]
    public void EmployeeInitializationWithoutDepartment()
    {
        Employee employee = new Employee(1, "John Doe");
        Assert.Equal(1, employee.ID);
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal(0, employee.DepartmentID);
    }

    [Fact]
    public void EmployeeInitializationWithDepartment()
    {
        Department department = new Department(1, "IT");
        Employee employee = new Employee(1, "John Doe", department);
        Assert.Equal(1, employee.ID);
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal(1, employee.DepartmentID);
        Assert.Equal("IT", department.Name);
    }

    [Fact]
    public void EmployeeInitializationWithNullDepartment()
    {
        Employee employee = new Employee(1, "John Doe", null);
        Assert.Equal(1, employee.ID);
        Assert.Equal("John Doe", employee.Name);
        Assert.Equal(0, employee.DepartmentID);
    }

    [Fact]
    public void DepartmentInitialization()
    {
        Department department = new Department(1, "IT");
        Assert.Equal(1, department.ID);
        Assert.Equal("IT", department.Name);
    }

}