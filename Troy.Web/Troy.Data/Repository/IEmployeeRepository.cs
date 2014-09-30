using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Employees;

namespace Troy.Data.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployee();

        List<Employee> GetFilterEmployee(string searchColumn, string searchString, Guid userId);

        Employee FindOneEmployeeById(int qId);

        Employee CheckDuplicateName(string mEmployee_No);

        bool InsertFileUploadDetails(List<Employee> employee);

        bool AddNewEmployee(Employee employee);

        bool EditExistingEmployee(Employee employee);

        bool GenerateXML(Object obj1);
    }
}
