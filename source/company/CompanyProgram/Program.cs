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
        private enum SortModels { EmployeeByIdAsc = 0, EmployeeByIdDesc = 1, EmployeeByNameAsc = 2, EmployeeByNameDesc = 3, DepartmentByIdAsc = 0, DepartmentByIdDesc = 1, DepartmentByNameAsc = 2, DepartmentByNameDesc = 3};

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
            /*
                1. get all employees
                2. select one to edit 
                3. display edit Options
                4. update select field 
            */
            Console.WriteLine("[!] Edit Employee Form.\n");
            EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
            List<Employee> employees = employeeDataProvider.GetEmployees();

            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {employees[i].Name}");
            }
            Console.WriteLine($"[{employees.Count+1}] Back.\n");
            Console.Write("[?] Choose: ");
            int result;
            if(int.TryParse(Console.ReadLine(), out  result) && result > 0 && result <= employees.Count)
            {
                Console.WriteLine("[!] Edit Employee Form. (leave empty to skip)\n");
                Console.WriteLine($"[=] Current Name: {employees[result-1].Name}\n");
                Console.Write("[?] New Name: ");
                string name = Console.ReadLine();
                if(name != null && name != "") employees[result-1].Name = name;
                
                Console.WriteLine("\n-----------\n");

                Console.WriteLine($"[=] Current Department: {employees[result-1].Department.Name}\n");
                DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                List<Department> departments = departmentDataProvider.GetDepartments();
                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"[{i+1}] {departments[i].Name}");
                }
                Console.WriteLine("[?] New Department: ");
                Console.Write("[?] Choose: ");
                int result2;
                if(int.TryParse(Console.ReadLine(), out  result2) && result2 > 0 && result2 <= departments.Count)
                {
                    employees[result-1].Department = departments[result2-1];
                    employees[result-1].DepartmentID = departments[result2 - 1].ID;
                }
                Employee newEmp = new Employee(employees[result-1].ID, employees[result-1].Name, employees[result-1].Department);
                employeeDataProvider.UpdateEmployee(newEmp);
                Console.WriteLine("\n[!] Employee Updated\n");
                newEmp.Display();
            }
            else if(result == employees.Count+1)
            {
                Console.WriteLine("\n[!] Back... Enter to Continue\n");
            }
        }
        private static void EditDepartmentForm() {
            Console.WriteLine("[!] Edit Department Form.\n");
            DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
            List<Department> departments = departmentDataProvider.GetDepartments();
            for (int i = 0; i < departments.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {departments[i].Name}");
            }
            Console.WriteLine($"[{departments.Count+1}] Back.\n");
            Console.Write("[?] Choose: ");
            int result;
            if(int.TryParse(Console.ReadLine(), out  result) && result > 0 && result <= departments.Count)
            {
                Console.WriteLine("[!] Edit Department Form. (leave empty to skip)\n");
                Console.WriteLine($"[=] Current Name: {departments[result-1].Name}\n");
                Console.Write("[?] New Name: ");
                string name = Console.ReadLine();
                if(name != null && name != "") departments[result-1].Name = name;
                Department newDep = new Department(departments[result-1].ID, departments[result-1].Name);
                departmentDataProvider.UpdateDepartment(newDep);
                Console.WriteLine("\n[!] Department Updated\n");
                newDep.Display();
            }
            else if(result == departments.Count+1)
            {
                Console.WriteLine("\n[!] Back... Enter to Continue\n");
            }
        }

        // display Forms
        private static void DisplayEmployeeForm()
        {
            Console.WriteLine("[!] Display Employee.\n");
            Console.WriteLine("[0] All. \n[1] Search By ID.\n[2] Search By Name.\n[3] Back.\n");
            Console.Write("[?] Choose: ");
            int result;
            if (int.TryParse(Console.ReadLine(), out result) && result >= 0 && result <= 3)
            {
                switch (result)
                {
                    case 0:
                        EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                        foreach (var employee in employeeDataProvider.GetEmployees())
                        {
                            employee.Display();
                        }
                        Console.WriteLine("===\n");
                        break;
                    case 1:
                        EmployeeDataProvider employeeDataProvider2 = new EmployeeDataProvider(new SQLDataProvider());
                        Console.Write("[?] ID: ");
                        int id;
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Employee employee = employeeDataProvider2.GetEmployee(id);

                            employee.Display();

                            Console.WriteLine("===\n");
                        }
                        break;
                    case 2:
                        EmployeeDataProvider employeeDataProvider3 = new EmployeeDataProvider(new SQLDataProvider());
                        Console.Write("[?] Name: ");
                        string? name = Console.ReadLine();
                        List<Employee> employees = employeeDataProvider3.GetEmployeeByName(name);
                        foreach (var employee in employees)
                        {
                            employee.Display();
                            Console.WriteLine("===\n");
                        }

                        break;
                    case 3:
                        Console.WriteLine("\n[!] Back... Enter to Continue\n");
                        break;
                }
            }
        }
        private static void DisplayDepartmentForm() {
            Console.WriteLine("[!] Display Department Form.\n");
            Console.WriteLine("[0] All. \n[1] Search By ID.\n[2] Search By Name.\n[3] Back.\n");
            Console.Write("[?] Choose: ");
            int result;
            if (int.TryParse(Console.ReadLine(), out result) && result >= 0 && result <= 3)
            {
                switch (result)
                {
                    case 0:
                        DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                        foreach (var department in departmentDataProvider.GetDepartments())
                        {
                            department.Display();
                        }
                        Console.WriteLine("===\n");
                        break;
                    case 1:
                        DepartmentDataProvider departmentDataProvider2 = new DepartmentDataProvider(new SQLDataProvider());
                        Console.Write("[?] ID: ");
                        int id;
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Department department = departmentDataProvider2.GetDepartment(id).ElementAt(0);

                            department.Display();

                            Console.WriteLine("===\n");
                        }
                        break;
                    case 2:
                        DepartmentDataProvider departmentDataProvider3 = new DepartmentDataProvider(new SQLDataProvider());
                        Console.Write("[?] Name: ");
                        string? name = Console.ReadLine();
                        List<Department> departments = departmentDataProvider3.GetDepartmentByName(name);
                        foreach (var department in departments)
                        {
                            department.Display();
                            Console.WriteLine("===\n");
                        }

                        break;
                    case 3:
                        Console.WriteLine("\n[!] Back... Enter to Continue\n");
                        break;

                }
            }
        }

        // sort Forms
        private static void SortEmployeeForm() {
            Console.WriteLine("[!] Sort Employee Form.\n");
            SortDirectionForm(Models.Employee);
        }
        private static void SortDepartmentForm() {
            Console.WriteLine("Sort Department Form");
            SortDirectionForm(Models.Department);
        }

        private static int SortDirectionForm(Models model) {
            
            Console.WriteLine("[?] Sort Direction:\n");
            Console.WriteLine("[0] Ascending By ID.");
            Console.WriteLine("[1] Descending By ID.");
            Console.WriteLine("[2] Ascending By Name.");
            Console.WriteLine("[3] Descending By Name.");
            Console.Write("[?] Choose: ");
            int result;
            if(int.TryParse(Console.ReadLine(), out  result) && result >= 0 && result <= 3)
            {
                switch (model)
                {
                    case Models.Employee:
                        switch ((SortModels)result){
                            case SortModels.EmployeeByIdAsc:
                                Console.WriteLine("\n[!] Ascending By ID.");
                                EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                                foreach (var employee in employeeDataProvider.GetEmployeesSortedByIdAsc())
                                {
                                    employee.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.EmployeeByIdDesc:
                                Console.WriteLine("\n[!] Descending By ID.");
                                employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                                foreach (var employee in employeeDataProvider.GetEmployeesSortedByIdDesc())
                                {
                                    employee.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.EmployeeByNameAsc:
                                Console.WriteLine("\n[!] Ascending By Name.");
                                employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                                foreach (var employee in employeeDataProvider.GetEmployeesSortedByNameAsc())
                                {
                                    employee.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.EmployeeByNameDesc:
                                Console.WriteLine("\n[!] Descending By Name.");
                                employeeDataProvider = new EmployeeDataProvider(new SQLDataProvider());
                                foreach (var employee in employeeDataProvider.GetEmployeesSortedByNameDesc())
                                {
                                    employee.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                        }
                        break;
                    case Models.Department:
                        switch ((SortModels)result){
                            case SortModels.DepartmentByIdAsc:
                                Console.WriteLine("\n[!] Ascending By ID.");
                                DepartmentDataProvider departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                                foreach (var department in departmentDataProvider.GetDepartmentsSortedByIdAsc())
                                {
                                    department.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.DepartmentByIdDesc:
                                Console.WriteLine("\n[!] Descending By ID.");
                                departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                                foreach (var department in departmentDataProvider.GetDepartmentsSortedByIdDesc())
                                {
                                    department.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.DepartmentByNameAsc:
                                Console.WriteLine("\n[!] Ascending By Name.");
                                departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                                foreach (var department in departmentDataProvider.GetDepartmentsSortedByNameAsc())
                                {
                                    department.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                            case SortModels.DepartmentByNameDesc:
                                Console.WriteLine("\n[!] Descending By Name.");
                                departmentDataProvider = new DepartmentDataProvider(new SQLDataProvider());
                                foreach (var department in departmentDataProvider.GetDepartmentsSortedByNameDesc())
                                {
                                    department.Display();
                                }
                                Console.WriteLine("===\n");
                                break;
                        }
                        break;
                }
                return result;
            }
            return -1;
            
        }
    }
}