using DepartmentLib;
namespace EmployeeLib;

public class Employee
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

    public void Display()
    {
        Console.WriteLine($"    ID: {ID}");
        Console.WriteLine($"    Name: {Name}");
        Console.WriteLine($"    Department: {Department?.Name}");
    }
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, DepartmentID: {DepartmentID}";
    }
}
