using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Employees;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Troy.Utilities.CrossCutting;
using System.Xml;
using System.Xml.Serialization;
using Troy.Model.SAP_OUT;
using Troy.Model.Designations;
using Troy.Model.Departments;
using Troy.Model.Branches;
using Troy.Model.LeftReasons;
using Troy.Model.MaritalStatus;
using Troy.Model.Genders;

namespace Troy.Data.Repository
{
    public class EmployeeRepository:BaseRepository,IEmployeeRepository
    {
        private EmployeeContext employeecontext = new EmployeeContext();

        public List<Employee> GetAllEmployee()
        {
            List<Employee> qList = new List<Employee>();
            qList = (from p in employeecontext.Employee
                     select p).ToList();
            return qList;
        }

        public List<ViewEmployee> GetFilterEmployee(string searchColumn, string searchString, Guid userId)
        {
            List<ViewEmployee> qList = new List<ViewEmployee>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            employeecontext.Database.Initialize(force: false);

            var cmd = employeecontext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetEmployee]";
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                employeecontext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)employeecontext)
                    .ObjectContext
                    .Translate<Employee>(reader, "Employee", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    ViewEmployee model = new ViewEmployee()
                    {
                        Emp_Id=item.Emp_Id,
                        Emp_No=item.Emp_No,
                        Initial= item.Initial,
                        First_Name=item.First_Name,
                        Middle_Name=item.Middle_Name,
                        Last_Name=item.Last_Name,
                        Father_Name=item.Father_Name,
                        Designation_Id=item.Designation_Id,
                        Department_Id=item.Department_Id,
                        Manager_empid=item.Manager_empid,
                        Branch_Id=item.Branch_Id,
                        ID_Number=item.ID_Number,
                        Mobile_number=item.Mobile_number,
                        Email=item.Email,
                        Start_Dte=item.Start_Dte,
                        Left_Dte=item.Left_Dte,
                        Left_Reason=item.Left_Reason,
                        DOB=item.DOB,
                        Marital_Status=item.Marital_Status,
                        Gender=item.Gender,
                        Noof_Children=item.Noof_Children,
                        Passport_no=item.Passport_no,
                        Passport_Expiry_Dte=item.Passport_Expiry_Dte,
                        Photo=item.Photo,
                        Salary=item.Salary,
                        ETC=item.ETC,
                        Bank_Cde=item.Bank_Cde,
                        Bank_Acc_No=item.Bank_Acc_No,
                        Bank_Branch_Name=item.Bank_Branch_Name,
                        Remarks=item.Remarks,
                        IsActive=item.IsActive,
                        Created_User_Id = item.Created_User_Id,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Modified_User_Id = item.Modified_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Image_Url=item.Image_Url
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                employeecontext.Database.Connection.Close();
            }

            return qList;
        }

        public Employee FindOneEmployeeById(int qId)
        {
            return (from p in employeecontext.Employee
                    where p.Emp_Id == qId
                    select p).FirstOrDefault();
        }

        public Employee CheckDuplicateName(string mEmployee_No)
        {
            return (from p in employeecontext.Employee
                    where p.Emp_No.Equals(mEmployee_No)
                    select p).FirstOrDefault();
        }

        public bool InsertFileUploadDetails(List<Employee> employee)
        {
            // throw new NotImplementedException();
            try
            {
                employeecontext.Employee.AddRange(employee);
                employeecontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool AddNewEmployee(Employee employee)
        {
            try
            {
                employeecontext.Employee.Add(employee);

                employeecontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool EditExistingEmployee(Employee employee)
        {
            try
            {
                employeecontext.Entry(employee).State = EntityState.Modified;
                employeecontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool GenerateXML(Object obj1)
        {
            try
            {
                string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(obj1);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);


                SAPOUT mSAP = new SAPOUT();
                mSAP.Object_typ = "Employee";
                mSAP.Branch_Cde = "1";
                mSAP.Troy_Created_Dte = Convert.ToDateTime(DateTime.Now.ToString());
                mSAP.Troy_XML = doc.InnerXml;
                SAPOUTRepository saprepo = new SAPOUTRepository();
                if (saprepo.AddNew(mSAP))
                {

                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public List<DesignationList> GetDesignationList()
        {
            var item = (from a in employeecontext.Designation
                        select new  DesignationList
                        {
                            Designation_Id=a.Designation_Id,
                            Designation_Name=a.Designation_Name
                        }).ToList();

            return item;
        }

        public List<DepartmentList> GetDepartmentList()
        {
            var item = (from a in employeecontext.Department
                        select new DepartmentList
                        {
                            Department_Id = a.Department_Id,
                            Department_Name = a.Department_Name
                        }).ToList();
            return item;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in employeecontext.Branch
                        select new BranchList
                        {
                            Branch_Id = a.Branch_Id,
                            Branch_Name = a.Branch_Name
                        }).ToList();
            return item;
        }

        public List<GenderList> GetGenderList()
        {
            var item = (from a in employeecontext.Gender
                        select new GenderList
                        {
                            Id = a.Id,
                            Troyvalues = a.Troyvalues
                        }).ToList();
            return item;
        }

        //public List<MaritalStatus> GetMaritalStatusList()
        //{
        //    var item = (from a in employeecontext.MaritalStatus
        //                select new MaritalStatus
        //                {
        //                    Id = a.Id,
        //                    Troy_values = a.Troy_values
        //                }).ToList();
        //    return item;
        //}

        public List<LeftReasonList> GetLeftReasonList()
        {
            var item = (from a in employeecontext.LeftReason
                        select new LeftReasonList
                        {
                            Id = a.Id,
                            Troyvalues = a.Troyvalues
                        }).ToList();
            return item;
        }
    }
}
