using EmployeeLib;
using DepartmentLib;
using RepositoryLib;
using UtilsLib;

namespace CompanyProgram
{
    /*
        [♠] Plan:
            1. Create MainMenu ( New, Edit, Delete, Display, Sort, Exit) 
            2. SubMenu
                2.1 New Menu
                    2.1.1 New Employee ( Name, Department [optional] )
                    2.1.2 New Department ( Name )
                2.2 Edit Menu [!] s to Skip Edit
                    2.2.1 Edit Employee ( Name [optional], Department [optional] )
                    2.2.2 Edit Department ( Name [optional] )
                2.3 Delete ( Delete Employee, Delete Department)
                2.4 Display ( Display Employee, Display Department, Search Employee, Search Department)
                    2.4.1 Search by Name
                    2.4.2 Search by ID 
                2.5 Sort ( Sort Employee, Sort Department)
                    2.5.1 Sort by ID
                    2.5.2 Sort by Name
            3. Highlight Selected Option
            4. Error Handling [optional]
    */

    internal class Program
    {
        private static int windowWidth = Console.WindowWidth;
        private static int windowHeight = Console.WindowHeight;

        private static List<string> menu = new List<string>() { "\tNew", "\tEdit", "\tDelete", "\tDisplay", "\tSort", "\tExit" };
        private static int selectedPosition = 0;
        private static int highlight = 0;
        private static bool isExit = false;
        private enum Models { Employee = 0, Department = 1 };
        private enum SortModels { EmployeeByIdAsc = 0, EmployeeByIdDesc = 2, DepartmentByIdAsc = 1, DepartmentByIdDesc = 3 };

        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                toggleHighlight(false);

                for (int i = 0; i < menu.Count; i++)
                {
                    SetMenuToPosition(i);
                    if (i == selectedPosition) toggleHighlight(true);
                    Console.Write(menu[i]);
                    toggleHighlight(false);
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        selectedPosition = (selectedPosition + 1) % menu.Count;
                        toggleHighlight(highlight == selectedPosition);
                        break;
                    case ConsoleKey.UpArrow:
                        selectedPosition = (selectedPosition - 1 + menu.Count) % menu.Count;
                        toggleHighlight(highlight == selectedPosition);
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        
                        switch (selectedPosition)
                        {
                            case 0:
                                Console.WriteLine("[!] New Menu");
                                switch (selectModel())
                                {
                                    case 0:
                                        NewEmployeeForm();
                                        Console.ReadLine();
                                        break;
                                    case 1:
                                        NewDepartmentForm();
                                        Console.ReadLine();
                                        break;
                                }
                                break;
                            case 1:
                                Console.WriteLine("[!] Edit Menu");
                                switch (selectModel())
                                {
                                    case 0:
                                        EditEmployeeForm();
                                        Console.ReadLine();
                                        break;
                                    case 1:
                                        EditDepartmentForm();
                                        Console.ReadLine();
                                        break;
                                }
                                break;
                            case 2:
                                Console.WriteLine("[!] Delete Menu");
                                switch (selectModel())
                                {
                                    case 0:
                                        DeleteEmployeeForm();
                                        Console.ReadLine();
                                        break;
                                    case 1:
                                        DeleteDepartmentForm();
                                        Console.ReadLine();
                                        break;
                                }
                                break;
                            case 3:
                                Console.WriteLine("[!] Display Menu");
                                switch (selectModel())
                                {
                                    case 0:
                                        DisplayEmployeeForm();
                                        Console.ReadLine();
                                        break;
                                    case 1:
                                        DisplayDepartmentForm();
                                        Console.ReadLine();
                                        break;
                                }
                                break;
                            case 4:
                                Console.WriteLine("[!] Sort Menu");
                                switch (selectModel())
                                {
                                    case 0:
                                        SortEmployeeForm();
                                        Console.ReadLine();
                                        break;
                                    case 1:
                                        SortDepartmentForm();
                                        Console.ReadLine();
                                        break;
                                }
                                break;
                            case 5:
                                Console.WriteLine("Are you sure you want to Exit... Enter to Confirm");
                                isExit = true;
                                Console.ReadLine();
                                break;
                            default:
                                break;
                        }
                        break;

                    case ConsoleKey.Escape:
                        isExit = true;
                        return;

                    default:
                        break;
                }

            } while (!isExit);
        }


        private static void SetMenuToPosition(int position )
        {
            int left = (windowWidth / menu.Count) * 2 + 15;
            int top = (position + 2) * (windowHeight / ( 3+ menu.Count));

            Console.SetCursorPosition( left, top);
        }

        private static void toggleHighlight(bool highlighted = false)
        {
            if (highlighted)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static int selectModel()
        {
            Console.Clear();
            string? input;
            int result;
            do
            {
                Console.WriteLine("\n[?] Select: \n");
                Console.WriteLine("[0] Employee. ");
                Console.WriteLine("[1] Department. ");
                Console.WriteLine("[2] Back. \n");
                Console.Write("[?] Choose:");
                input = Console.ReadLine();

                if(input == null) Console.WriteLine("\n[!] Invalid Input\n");

                if (!int.TryParse(input, out result))
                {
                    Console.WriteLine("\n[!] Invalid Format\n");
                    input = null;
                }    

                if(result < 0 || result > 2)
                {
                    Console.WriteLine("\n[!] Input Out Of Range\n");
                    input = null;
                }

            } while (input == null);
            
            return result;
        }
    
        // new Forms
        private static void NewEmployeeForm() {
            Employee employee = new Employee();
            bool correct = true;
            do{
                Console.WriteLine("[+] New Employee Form\n");
                
                Console.Write("[?] Name: ");
                employee.Name = Console.ReadLine();
                if(employee.Name == null) correct = false;

                DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                List<Department> departments = departmentDataProvider.GetDepartments();
                
                Console.WriteLine("[o] Department:\n");
                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"[{i+1}] {departments[i].Name}");
                }
                Console.WriteLine($"[{departments.Count}] Skip.");
                Console.Write("[?] Choose: ");
                int result;
                if(int.TryParse(Console.ReadLine(), out  result) && result > 0 && result < departments.Count)
                {
                    employee.Department = departments[result-1];
                    employee.DepartmentID = departments[result - 1].ID;
                }
                if(result == departments.Count)
                {
                    employee.Department = null;
                    employee.DepartmentID = 0;
                    correct = false;
                }
                else
                {
                    correct = false;
                }

            }while (correct);

            EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
            employeeDataProvider.InsertEmployee(employee);
            Console.WriteLine("\n[!] Employee Created\n");

        }
        private static void NewDepartmentForm() {
            Department department = new Department();
            bool correct = true;
            Console.WriteLine("[!] New Department Form");
            do{
                Console.Write("[?] Name: ");
                department.Name = Console.ReadLine();
                if(department.Name != null) correct = false;
            }while (correct);

            DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
            departmentDataProvider.InsertDepartment(department);
            Console.WriteLine("\n[!] Department Created\n");
            
        }

        // delete Forms
        private static void DeleteEmployeeForm() {
            List<Employee> employees = new EmployeeDataProvider(new SQLDataProvider()).GetEmployees();
            Console.WriteLine("[!] Delete Employee Form.\n");
            bool correct = true;
            do{
                for (int i = 0; i < employees.Count; i++)
                {
                    Console.WriteLine($"[{i+1}] {employees[i].Name}");
                }
                Console.WriteLine($"[{employees.Count+1}] Back.\n");
                Console.Write("[?] Choose: ");
                int result;
                if(int.TryParse(Console.ReadLine(), out  result) && result > 0 && result <= employees.Count)
                {
                    EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                    employeeDataProvider.DeleteEmployee(employees[result-1]);
                    correct = false;
                    Console.WriteLine("\n[!] Employee Deleted\n");
                }
                if(result == employees.Count+1)
                {
                    Console.WriteLine("\n[!] Back... Enter to Continue\n");
                    correct = false;
                }
            }while (correct);
        }
        private static void DeleteDepartmentForm() {
            List<Department> departments = new DepartmentDataProvider(new SQLDataProvider()).GetDepartments();
            Console.WriteLine("[!] Delete Department Form.\n");
            bool correct = true;
            do{
                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"[{i+1}] {departments[i].Name}");
                }
                Console.WriteLine($"[{departments.Count+1}] Back.\n");
                Console.Write("[?] Choose: ");
                int result;
                if(int.TryParse(Console.ReadLine(), out  result) && result > 0 && result <= departments.Count)
                {
                    DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                    departmentDataProvider.DeleteDepartment(departments[result-1]);
                    correct = false;
                    Console.WriteLine("\n[!] Department Deleted\n");
                }
                if(result == departments.Count+1)
                {
                    Console.WriteLine("\n[!] Back... Enter to Continue\n");
                    correct = false;
                }
            }while (correct);
        }

        // edit Forms
        private static void EditEmployeeForm() {
            Console.WriteLine("Edit Employee Form");
        }
        private static void EditDepartmentForm() {
            Console.WriteLine("Edit Department Form");
        }

        // display Forms
        private static void DisplayEmployeeForm() {
            Console.WriteLine("Display Employee");
        }
        private static void DisplayDepartmentForm() {
            Console.WriteLine("Display Department Form");
        }

        // sort Forms
        private static void SortEmployeeForm() {
            Console.WriteLine("Sort Employee Form");
        }
        private static void SortDepartmentForm() {
            Console.WriteLine("Sort Department Form");
        }
    
    }
}