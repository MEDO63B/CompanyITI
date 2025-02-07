using System.Collections;

namespace UtilsLib;

public static class SQLUtils
{
    private static string SERVER_NAME = "mute63";
    private static string DATABASE_NAME = "SW_Company";
    public static string CONNECTION_STRING = $"Server={SERVER_NAME};Database={DATABASE_NAME};Trusted_Connection=True;";
}

public static class QueriesUtils
{

    // Department Utils
    public static SortedList<string, string> DEPARTMENT_FIELDS = new SortedList<string, string>()
    {
        { "ID", "DepartmentID" },
        { "Name", "DepartmentName" }
    };

    public static string department = "Departments";
    public static string SELECT_ALL_Departments = $"SELECT * FROM {department}";
    public static string SELECT_Department_BY_ID = $"SELECT * FROM {department} WHERE {DEPARTMENT_FIELDS["ID"]} = @ID";
    public static string SELECT_Department_BY_NAME = $"SELECT * FROM {department} WHERE {DEPARTMENT_FIELDS["Name"]} = @Name";

    public static string SELECT_Department_SORT_ASC_NAME = $"SELECT * FROM {department} ORDER BY {DEPARTMENT_FIELDS["Name"]} ASC";
    public static string SELECT_Department_SORT_DESC_NAME = $"SELECT * FROM {department} ORDER BY {DEPARTMENT_FIELDS["Name"]} DESC";

    public static string SELECT_Department_SORT_ASC_ID = $"SELECT * FROM {department} ORDER BY {DEPARTMENT_FIELDS["ID"]} ASC";
    public static string SELECT_Department_SORT_DESC_ID = $"SELECT * FROM {department} ORDER BY {DEPARTMENT_FIELDS["ID"]} DESC";

    public static string INSERT_Department = $"INSERT INTO {department} ({DEPARTMENT_FIELDS["Name"]}) VALUES (@Name)";
    public static string UPDATE_Department = $"UPDATE {department} SET {DEPARTMENT_FIELDS["Name"]} = @Name WHERE {DEPARTMENT_FIELDS["ID"]} = @ID";
    public static string DELETE_Department = $"DELETE FROM {department} WHERE {DEPARTMENT_FIELDS["ID"]} = @ID";

    //================================================================

    // Employee Utils
    public static SortedList<string, string> EMPLOYEE_FIELDS = new SortedList<string, string>()
    {
        { "ID", "EmployeeID" },
        { "Name", "EmployeeName" },
        { "DepartmentID", "DepartmentID" }
    };

    public static string employee = "Employees";
    public static string SELECT_ALL_Employees = $"SELECT {employee}.*, {department}.{DEPARTMENT_FIELDS["Name"]} FROM {employee} JOIN {department} ON {employee}.{EMPLOYEE_FIELDS["DepartmentID"]} = {department}.{DEPARTMENT_FIELDS["ID"]}";
    public static string SELECT_ALL_Employees_SORT_ASC_NAME = $"SELECT {employee}.*, {department}.{DEPARTMENT_FIELDS["Name"]} FROM {employee} JOIN {department} ON {employee}.{EMPLOYEE_FIELDS["DepartmentID"]} = {department}.{DEPARTMENT_FIELDS["ID"]} ORDER BY {EMPLOYEE_FIELDS["Name"]} ASC";
    public static string SELECT_ALL_Employees_SORT_DESC_NAME = $"SELECT {employee}.*, {department}.{DEPARTMENT_FIELDS["Name"]} FROM {employee} JOIN {department} ON {employee}.{EMPLOYEE_FIELDS["DepartmentID"]} = {department}.{DEPARTMENT_FIELDS["ID"]} ORDER BY {EMPLOYEE_FIELDS["Name"]} DESC";
    public static string SELECT_ALL_Employees_SORT_ASC_ID = $"SELECT {employee}.*, {department}.{DEPARTMENT_FIELDS["Name"]} FROM {employee} JOIN {department} ON {employee}.{EMPLOYEE_FIELDS["DepartmentID"]} = {department}.{DEPARTMENT_FIELDS["ID"]} ORDER BY {EMPLOYEE_FIELDS["ID"]} ASC";
    public static string SELECT_ALL_Employees_SORT_DESC_ID = $"SELECT {employee}.*, {department}.{DEPARTMENT_FIELDS["Name"]} FROM {employee} JOIN {department} ON {employee}.{EMPLOYEE_FIELDS["DepartmentID"]} = {department}.{DEPARTMENT_FIELDS["ID"]} ORDER BY {EMPLOYEE_FIELDS["ID"]} DESC";

    public static string SELECT_Employee_BY_ID = $"SELECT * FROM {employee} WHERE {EMPLOYEE_FIELDS["ID"]} = @ID";
    public static string SELECT_Employee_BY_NAME = $"SELECT * FROM {employee} WHERE {EMPLOYEE_FIELDS["Name"]} = @Name";
    public static string INSERT_Employee = $"INSERT INTO {employee} ({EMPLOYEE_FIELDS["Name"]}, {EMPLOYEE_FIELDS["DepartmentID"]}) VALUES (@Name, @DepartmentID)";
    public static string UPDATE_Employee = $"UPDATE {employee} SET {EMPLOYEE_FIELDS["Name"]} = @Name, {EMPLOYEE_FIELDS["DepartmentID"]} = @DepartmentID WHERE {EMPLOYEE_FIELDS["ID"]} = @ID";
    public static string DELETE_Employee = $"DELETE FROM {employee} WHERE {EMPLOYEE_FIELDS["ID"]} = @ID";

}
