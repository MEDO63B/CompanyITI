using Microsoft.Data.SqlClient;
using EmployeeLib;
using DepartmentLib;
using UtilsLib;
namespace RepositoryLib;

public class Query
{
    public string? SqlStatment { get; set; }
    public SortedList<string, string>? Parameters { get; set; }
}


public interface IDataProvider
{
    public SqlConnection GetConnection();

    public void CloseConnection();

    public List<T> SelectQuery<T>(Query query) where T : IDataReaderMapper<T>, new();

    public int InsertQuery(Query query);
    public int UpdateQuery(Query query);
    public int DeleteQuery(Query query);
    

}

public class SingleSQLConnection 
{
    private static SqlConnection? connection;

    private SingleSQLConnection(){}

    public static SqlConnection getConnection()
    {
        if(connection == null)
        {
            connection = new SqlConnection(SQLUtils.CONNECTION_STRING);
        }

        return connection;
    }

    public static void closeConnection()
    {
        if(connection != null)
        {
            connection.Close();
            connection = null;
        }
    }
}

public class SQLDataProvider : IDataProvider // apply singleton
{
    public SqlConnection GetConnection()
    {
        return SingleSQLConnection.getConnection();
    }
    public void CloseConnection()
    {
        SingleSQLConnection.closeConnection();
    }

    public int DeleteQuery(Query query)
    {
        SqlConnection connection = GetConnection();
        if(connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }
        SqlCommand command = new SqlCommand(query.SqlStatment, connection);
        foreach (string key in query.Parameters!.Keys)
        {
            command.Parameters.AddWithValue(key, query.Parameters[key]);
        }

        int rows = command.ExecuteNonQuery();

        connection.Close();
        this.CloseConnection();

        return rows;
    }

    public int InsertQuery(Query query)
    {
        SqlConnection connection = GetConnection();
        if(connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }
        SqlCommand command = new SqlCommand(query.SqlStatment, connection);
        foreach (string key in query.Parameters!.Keys)
        {
            command.Parameters.AddWithValue(key, query.Parameters[key]);
        }
        int rows =command.ExecuteNonQuery();

        connection.Close();
        this.CloseConnection();

        return rows;
    }

    public int UpdateQuery(Query query)
    {
        SqlConnection connection = GetConnection();
        if(connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        SqlCommand command = new SqlCommand(query.SqlStatment, connection);
        foreach (string key in query.Parameters!.Keys)
        {
            command.Parameters.AddWithValue(key, query.Parameters[key]);
        }
        int rows = command.ExecuteNonQuery();

        connection.Close();
        this.CloseConnection();

        return rows;
    }

    public List<T> SelectQuery<T>(Query query) where T : IDataReaderMapper<T>, new()
    {
        using (SqlConnection connection = GetConnection())
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            using (SqlCommand command = new SqlCommand(query.SqlStatment, connection))
            {
                if (query.Parameters != null)
                {
                    foreach (string key in query.Parameters!.Keys)
                    {
                        command.Parameters.AddWithValue(key, query.Parameters[key]);
                    }
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<T> result = new List<T>();
                    while (reader.Read())
                    {
                        T obj = new T().FromReader(reader);
                        result.Add(obj);
                    }
                    connection.Close();
                    this.CloseConnection();
                    return result;
                }
            }
        }
    }

}

public class EmployeeDataProvider
{
    private IDataProvider DataProvider;
    public EmployeeDataProvider(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    public Employee GetEmployee(int id)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Employee_BY_ID,
            Parameters = new SortedList<string, string>(){
                {"@ID", id.ToString()}
            }
        };

        return DataProvider.SelectQuery<Employee>(query).ElementAt(0);
    }

    public List<Employee> GetEmployeeByName(string name)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Employee_BY_NAME,
            Parameters = new SortedList<string, string>(){
                {"@NAME", name}
            }
        };

        return DataProvider.SelectQuery<Employee>(query);
    }


    public List<Employee> GetEmployees()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Employees,
            Parameters = null
        };

        return DataProvider.SelectQuery<Employee>(query);
    }

    public List<Employee> GetEmployeesSortedByNameAsc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Employees_SORT_ASC_NAME,
            Parameters = null
        };

        return DataProvider.SelectQuery<Employee>(query);
    }
    public List<Employee> GetEmployeesSortedByNameDesc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Employees_SORT_DESC_NAME,
            Parameters = null
        };

        return DataProvider.SelectQuery<Employee>(query);
    }

    public List<Employee> GetEmployeesSortedByIdDesc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Employees_SORT_DESC_ID,
            Parameters = null
        };

        return DataProvider.SelectQuery<Employee>(query);
    }

    public List<Employee> GetEmployeesSortedByIdAsc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Employees_SORT_ASC_ID,
            Parameters = null
        };

        return DataProvider.SelectQuery<Employee>(query);
    }

    public int InsertEmployee(Employee employee)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.INSERT_Employee,
            Parameters = new SortedList<string, string>(){
                {"@Name", employee.Name},
                {"@DepartmentID", employee.DepartmentID.ToString()}
            }
        };

        return DataProvider.InsertQuery(query);
    }

    public int UpdateEmployee(Employee employee)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.UPDATE_Employee,
            Parameters = new SortedList<string, string>(){
                {"@ID", employee.ID.ToString()},
                {"@Name", employee.Name},
                {"@DepartmentID", employee.DepartmentID.ToString()}
            }
        };

        return DataProvider.UpdateQuery(query);
    }

    public int DeleteEmployee(Employee employee)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.DELETE_Employee,
            Parameters = new SortedList<string, string>(){
                {"@ID", employee.ID.ToString()}
            }
        };

        return DataProvider.DeleteQuery(query);
    }

}

public class DepartmentDataProvider
{
    private IDataProvider DataProvider;
    public DepartmentDataProvider(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    public List<Department> GetDepartment(int id)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_BY_ID,
            Parameters = new SortedList<string, string>(){
                {"@ID", id.ToString()}
            }
        };

        return DataProvider.SelectQuery<Department>(query);
    }
    public List<Department> GetDepartmentByName(string name)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_BY_NAME,
            Parameters = new SortedList<string, string>(){
                {"@Name", name}
            }
        };

        return DataProvider.SelectQuery<Department>(query);
    }

    public List<Department> GetDepartments()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_ALL_Departments,
            Parameters = null
        };

        return DataProvider.SelectQuery<Department>(query);
    }

    public int InsertDepartment(Department department)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.INSERT_Department,
            Parameters = new SortedList<string, string>(){
                {"@Name", department.Name}
            }
        };

        return DataProvider.InsertQuery(query);
    }

    public int UpdateDepartment(Department department)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.UPDATE_Department,
            Parameters = new SortedList<string, string>(){
                {"@ID", department.ID.ToString()},
                {"@Name", department.Name}
            }
        };

        return DataProvider.UpdateQuery(query);
    }

    public int DeleteDepartment(Department department)
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.DELETE_Department,
            Parameters = new SortedList<string, string>()
            {
                {"@ID", department.ID.ToString()}
            }
        };

        return DataProvider.DeleteQuery(query);
    }

    public List<Department> GetDepartmentsSortedByNameAsc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_SORT_ASC_NAME,
            Parameters = null
        };

        return DataProvider.SelectQuery<Department>(query);
    }

    public List<Department> GetDepartmentsSortedByNameDesc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_SORT_DESC_NAME,
            Parameters = null
        };

        return DataProvider.SelectQuery<Department>(query);
    }


    public List<Department> GetDepartmentsSortedByIdAsc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_SORT_ASC_ID,
            Parameters = null
        };

        return DataProvider.SelectQuery<Department>(query);
    }

    public List<Department> GetDepartmentsSortedByIdDesc()
    {
        Query query = new Query()
        {
            SqlStatment = QueriesUtils.SELECT_Department_SORT_DESC_ID,
            Parameters = null
        };

        return DataProvider.SelectQuery<Department>(query);
    }

}

