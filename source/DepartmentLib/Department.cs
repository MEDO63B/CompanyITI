using UtilsLib;
using Microsoft.Data.SqlClient;
namespace DepartmentLib;

public class Department: IDataReaderMapper<Department>
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

    public Department FromReader(SqlDataReader reader)
    {
        return new Department()
        {
            ID = Convert.ToInt32(reader[QueriesUtils.DEPARTMENT_FIELDS["ID"]]),
            Name = reader[QueriesUtils.DEPARTMENT_FIELDS["Name"]].ToString()
        };
    }


    public int GetHashCode(Department obj)
    {
        return obj.ID.GetHashCode();
    }

    // comparer is used for testing purposes
    public static bool Compare(Department? x, Department? y)
    {
        if (x == null || y == null) return false;
        return (x.ID.Equals(y.ID) && x.Name.Equals(y.Name));
    }
}

