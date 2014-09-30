using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.BusinessPartner;
using Troy.Utilities.CrossCutting;
using Troy.Model.Cities;
using Troy.Model.Countries;
using Troy.Model.States;
using Troy.Model.Groups;
using Troy.Model.PriceLists;
using Troy.Model.Branches;
using Troy.Model.Ledgers;
using Troy.Model.Employees;
using Troy.Model.SAP_OUT;
using System.Xml;
using System.Xml.Serialization;

namespace Troy.Data.Repository
{
    public class BusinessPartnerRepository : BaseRepository, IBusinessPartnerRepository
    {
        private BusinessPartnerContext businesspartnercontext = new BusinessPartnerContext();

        public List<BusinessPartner> GetAllBusinessPartner()
        {
            List<BusinessPartner> qList = new List<BusinessPartner>();

            qList = (from p in businesspartnercontext.BusinessPartner
                     select p).ToList();

            return qList;
        }

        public List<BusinessPartner> GetFilterBusinessPartner(string searchColumn, string searchString, Guid userId)
        {
            List<BusinessPartner> qList = new List<BusinessPartner>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            businesspartnercontext.Database.Initialize(force: false);

            var cmd = businesspartnercontext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetBusinessPartner]";
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                businesspartnercontext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)businesspartnercontext)
                    .ObjectContext
                    .Translate<BusinessPartner>(reader, "BusinessPartner", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    BusinessPartner model = new BusinessPartner()
                    {

                        BP_Id = item.BP_Id,
                        BP_Name = item.BP_Name,
                        Group_Type = item.Group_Type,
                        Group_id = item.Group_id,
                        Ship_Address1 = item.Ship_Address1,
                        Ship_address2 = item.Ship_address2,
                        Ship_address3 = item.Ship_address3,
                        Ship_City = item.Ship_City,
                        Ship_State = item.Ship_State,
                        Ship_Country = item.Ship_Country,
                        Ship_pincode = item.Ship_pincode,
                        Bill_Address1 = item.Bill_Address1,
                        Bill_address2 = item.Bill_address2,
                        Bill_address3 = item.Bill_address3,
                        Bill_City = item.Bill_City,
                        Bill_State = item.Bill_State,
                        Bill_Country = item.Bill_Country,
                        Bill_pincode = item.Bill_pincode,
                        IsActive = item.IsActive,
                        Pricelist = item.Pricelist,
                        Emp_Id = item.Emp_Id,
                        Branch_id = item.Branch_id,
                        Phone1 = item.Phone1,
                        Phone2 = item.Phone2,
                        Mobile = item.Mobile,
                        Email_Address = item.Email_Address,
                        Website = item.Website,
                        Contact_person = item.Contact_person,
                        Remarks = item.Remarks,
                        Ship_method = item.Ship_method,
                        Control_account_id = item.Control_account_id,
                        Opening_Balance = item.Opening_Balance,
                        Due_date = item.Due_date,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                businesspartnercontext.Database.Connection.Close();
            }

            return qList;
        }

        public BusinessPartner FindOneBusinessPartnerById(int qId)
        {
            return (from p in businesspartnercontext.BusinessPartner
                    where p.BP_Id == qId
                    select p).FirstOrDefault();
        }

        public BusinessPartner CheckDuplicateName(string mBusinessPartner_Name)
        {
            return (from p in businesspartnercontext.BusinessPartner
                    where p.BP_Name.Equals(mBusinessPartner_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public bool InsertFileUploadDetails(List<BusinessPartner> businesspartner)
        {
            // throw new NotImplementedException();
            try
            {
                businesspartnercontext.BusinessPartner.AddRange(businesspartner);
                businesspartnercontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool AddNewBusinessPartner(BusinessPartner businesspartner)
        {
            try
            {
                businesspartnercontext.BusinessPartner.Add(businesspartner);

                businesspartnercontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool EditExistingBusinessPartner(BusinessPartner businesspartner)
        {
            try
            {
                businesspartnercontext.Entry(businesspartner).State = EntityState.Modified;
                businesspartnercontext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool AddBulkBusinessPartner(Object obj)
        {
            //manufactureContext.Manufacture.Add(obj);  

            return true;
        }

        public List<GroupList> GetGroupList()
        {
            var item = (from a in businesspartnercontext.Group
                        select new GroupList
                        {
                            Group_Id = a.Group_Id,
                            Group_Name = a.Group_Name
                        }).ToList();

            return item;
        }

        public List<PricelistLists> GetPriceList()
        {
            var item = (from a in businesspartnercontext.PriceList
                        select new PricelistLists
                        {
                            Id = a.Id,
                            Price_List_Desc = a.Price_List_Desc
                        }).ToList();

            return item;
        }

        public List<BranchList> GetBranchList()
        {
            var item = (from a in businesspartnercontext.Branch
                        select new BranchList
                        {
                            BranchId = a.Branch_Id,
                            BranchName = a.Branch_Name
                        }).ToList();

            return item;
        }

        public List<LedgerList> GetLedgerList()
        {
            var item = (from a in businesspartnercontext.Ledger
                        select new LedgerList
                        {
                            Ledger_Id = a.Ledger_Id,
                            Ledger_Name = a.Ledger_Name
                        }).ToList();

            return item;
        }

        public List<EmployeeList> GetEmployeeList()
        {
            var item = (from a in businesspartnercontext.Employee
                        select new EmployeeList
                        {
                            Emp_Id = a.Emp_Id,
                            First_Name = a.First_Name
                        }).ToList();

            return item;
        }

        public List<CountryList> GetAddresscountryList()
        {
            var item = (from a in businesspartnercontext.Country
                        select new CountryList
                        {
                            ID = a.ID,
                            Country_Name = a.Country_Name
                            //BranchId = a.Branch_Id
                        }).ToList();

            return item;
        }

        public List<StateList> GetAddressstateList()
        {
            var item = (from a in businesspartnercontext.State
                        select new StateList
                        {
                            ID = a.ID,
                            State_Name = a.State_Name
                        }).ToList();

            return item;
        }

        public List<CityList> GetAddresscityList()
        {
            var item = (from a in businesspartnercontext.City
                        select new CityList
                        {
                            ID = a.ID,
                            City_Name = a.City_Name
                        }).ToList();

            return item;
        }

        public bool GenerateXML(Object obj1)
        {
            try
            {
                string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(obj1);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);


                SAPOUT mSAP = new SAPOUT();
                mSAP.Object_typ = "BUSINESS PARTNER";
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

        public Country CheckCountry(string bname)
        {
            return (from p in businesspartnercontext.Country
                    where p.Country_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public State CheckState(string bname)
        {
            return (from p in businesspartnercontext.State
                    where p.State_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public City CheckCity(string bname)
        {
            return (from p in businesspartnercontext.City
                    where p.City_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public Group CheckGroup(string bname)
        {
            return (from p in businesspartnercontext.Group
                    where p.Group_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public PriceList CheckPriceList(string bname)
        {
            return (from p in businesspartnercontext.PriceList
                    where p.Price_List_Desc.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public Employee CheckEmployee(string bname)
        {
            return (from p in businesspartnercontext.Employee
                    where p.First_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public Branch CheckBranch(string bname)
        {
            return (from p in businesspartnercontext.Branch
                    where p.Branch_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public Ledger CheckControlAccountID(string bname)
        {
            return (from p in businesspartnercontext.Ledger
                    where p.Ledger_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public string FindSAPCodeForCountryId(int country_id)
        {
            string sap_country_code = (from p in businesspartnercontext.Country
                                       where p.ID == country_id
                                       select p.SAP_Country_Code).FirstOrDefault();

            return sap_country_code;
        }

        public string FindSAPCodeForCityId(int city_id)
        {
            string sap_city_code = (from p in businesspartnercontext.City
                                    where p.ID == city_id
                                    select p.City_Name).FirstOrDefault();

            return sap_city_code;
        }

        public string FindSAPCodeForStateId(int state_id)
        {
            string sap_state_code = (from p in businesspartnercontext.State
                                     where p.ID == state_id
                                     select p.SAP_State_Code).FirstOrDefault();

            return sap_state_code;
        }

        public string FindGroupNameForGroupId(int group_id)
        {
            string group_name = (from p in businesspartnercontext.Group
                                 where p.Group_Id == group_id
                                 select p.Group_Name).FirstOrDefault();

            return group_name;
        }

        public string FindEmpNameForEmpId(int emp_id)
        {
            string emp_name = (from p in businesspartnercontext.Employee
                               where p.Emp_Id == emp_id
                               select p.Father_Name).FirstOrDefault();

            return emp_name;
        }

        public string FindPriceListDescForPricelist(int pricelist_id)
        {
            string pricelist_desc = (from p in businesspartnercontext.PriceList
                                     where p.Id==pricelist_id
                                     select p.Price_List_Desc).FirstOrDefault();

            return pricelist_desc;
        }

        public string FindBranchNameForBranchId(int branch_id)
        {
            string branch_name = (from p in businesspartnercontext.Branch
                                  where p.Branch_Id == branch_id
                                     select p.Branch_Name).FirstOrDefault();

            return branch_name;
        }
    }
}
