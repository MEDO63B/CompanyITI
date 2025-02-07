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
    }

    public Employee(int id, string name, int? departmentID) : this(id, name)
    {
        if (departmentID != null)
        {
            DepartmentID = departmentID;
        }
    }

    public Employee(int id, string name, Department? department) : this(id, name)
    {
        if (department != null)
        {
            Department = department;
            DepartmentID = department?.ID;
        }
    }

    public void Display()
    {
        Console.WriteLine($"    ID: {ID}");
        Console.WriteLine($"    Name: {Name}");
        Console.WriteLine($"    Department: {Department.Name}");
    }
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, DepartmentID: {DepartmentID}";
    }
}
