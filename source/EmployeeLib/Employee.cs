using DepartmentLib;
using UtilsLib;
using Microsoft.Data.SqlClient;
namespace EmployeeLib;

public class Employee : IDataReaderMapper<Employee> , IEqualityComparer<Employee>
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public int? DepartmentID { get; set; }

    public Department? Department { get; set; }

    public Employee() { }

    public Employee(int id, string name)
    {
        ID = id;
        Name = name;
        DepartmentID = 0;
        Department = null;
    }

    public Employee(int id, string name, int departmentID) : this(id, name)
    {
        if (departmentID != 0)
        {
            DepartmentID = departmentID;
        }
        else
        {
            DepartmentID = 0;
            Department = null;
        }
    }
    public Employee(int id, string name, Department? department) : this(id, name)
    {
        if (department != null)
        {
            Department = department;
            DepartmentID = department?.ID;
        }
        else
        {
            Department = null;
            DepartmentID = 0;
        }
    }
    
    public Employee FromReader(SqlDataReader reader)
    {

        int DeptID = 0;
        Department department = null;

        if (reader[QueriesUtils.EMPLOYEE_FIELDS["DepartmentID"]] != DBNull.Value)
        {
            DeptID = Convert.ToInt32(reader[QueriesUtils.EMPLOYEE_FIELDS["DepartmentID"]]);
            department = new Department(DeptID, reader[QueriesUtils.DEPARTMENT_FIELDS["Name"]].ToString());
        }

        return new Employee
        {
            ID = Convert.ToInt32(reader[QueriesUtils.EMPLOYEE_FIELDS["ID"]]),
            Name = reader[QueriesUtils.EMPLOYEE_FIELDS["Name"]].ToString(),
            Department = department,
            DepartmentID = DeptID
        };
    }

    public void Display()
    {
        Console.WriteLine($"    ID: {ID}");
        Console.WriteLine($"    Name: {Name}");
        Console.WriteLine($"    Department: {Department?.Name}");
    }
    public override string ToString()
    {
        if(Department != null) return $"ID: {ID}, Name: {Name}, DepartmentID: {DepartmentID}, Department: {Department.Name}";
        return $"ID: {ID}, Name: {Name}, DepartmentID: {DepartmentID}";
    }
    
    
    // comparer is used for testing purposes
    public bool Equals(Employee? x, Employee? y)
    {
        if (x == null || y == null) return false;
        
        if(x.Department == null || y.Department == null) 
        {
            return x.ID == y.ID &&
                    x.Name == y.Name;
        }

        return x.ID == y.ID &&
               x.Name == y.Name &&
               Department.Compare(x.Department, y.Department);
    }

    public int GetHashCode(Employee obj)
    {
        return obj.ID.GetHashCode();
    }
}



// comparer is used for testing purposes
public class EmployeeComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee? x, Employee? y)
    {
        if (x == null || y == null) return false;

        if(x.Department == null || y.Department == null) 
        {
            return x.ID == y.ID &&
                    x.Name == y.Name;
        }

        return x.ID == y.ID &&
               x.Name == y.Name &&
               Department.Compare(x.Department, y.Department);
    }

    public int GetHashCode(Employee obj)
    {
        return obj.ID.GetHashCode();
    }
}
