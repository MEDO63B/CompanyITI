namespace DepartmentLib;

public class Department
{
    public int ID { get; set; }
    public string? Name { get; set; }

    public Department() { }
    public Department(int id, string name)
    {
        ID = id;
        Name = name;
    }

    public void Display()
    {
        Console.WriteLine($"    ID: {ID}");
        Console.WriteLine($"    Name: {Name}");
    }
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}";
    }
}
