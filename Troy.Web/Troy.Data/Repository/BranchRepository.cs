using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Countries;
using Troy.Model.Cities;
using Troy.Model.States;
using Troy.Utilities.CrossCutting;
using System.Data.Entity;
using Troy.Model.Branches;
using System.Xml;
using Troy.Model.SAP_OUT;


namespace Troy.Data.Repository
{
    public class BranchRepository : BaseRepository, IBranchRepository
    {
        private BranchContext branchContext = new BranchContext();

        private CountryContext countryContext = new CountryContext();

        private CityContext cityContext = new CityContext();

        private StateContext stateContext = new StateContext();


        private SAPOUTContext sapcontext = new SAPOUTContext();



        public List<Branch> GetAllBranch()
        {
            List<Branch> qList = new List<Branch>();

            qList = (from p in branchContext.Branch
                   select p).ToList();

            return qList;
        }
        public List<ViewBranches> GetAllUserBranch()
        {
            List<ViewBranches> qList = new List<ViewBranches>();

            qList = (from item in branchContext.Branch
                     join c in branchContext.country
                        on item.Country_ID equals c.ID
                     join s in branchContext.state
                        on item.State_ID equals s.ID
                     join ct in branchContext.city
                        on item.City_ID equals ct.ID

                     select new ViewBranches()
                    {

                        Branch_Id = item.Branch_Id,
                        Branch_Code = item.Branch_Code,
                        Branch_Name = item.Branch_Name,
                        Address1 = item.Address1,
                        Address2 = item.Address2,
                        Address3 = item.Address3,
                        Country_ID = item.Country_ID,
                        State_ID = item.State_ID,
                        City_ID = item.City_ID,
                        //country = item.country,
                        //city = item.city,
                        //state = item.state,
                        Order_Num = item.Order_Num,
                        Pin_Code = item.Pin_Code,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id,
                        Country_Name = c.Country_Name,
                        State_Name = s.State_Name,
                        City_Name = ct.City_Name
                    }).ToList();

            return qList;
        }

        //public List<ViewBranches> GetAllBranches()
        //{
        //    List<ViewBranches> qList = new List<ViewBranches>();

        //    //qList = (from p in branchContext.Branch
        //    //         select p).ToList();

        //    branchContext.Database.Initialize(force: false);

        //    var cmd = branchContext.Database.Connection.CreateCommand();
        //    cmd.CommandText = "[dbo].[USP_GetAllBranches]";
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        branchContext.Database.Connection.Open();
        //        // Run the sproc  
        //        var reader = cmd.ExecuteReader();

        //        var result = ((IObjectContextAdapter)branchContext)
        //            .ObjectContext
        //            .Translate<ViewBranches>(reader, "Branch", MergeOption.AppendOnly);


        //        foreach (var item in result)
        //        {
        //            ViewBranches model = new ViewBranches()


        //            {

        //                Branch_Id = item.Branch_Id,
        //                Branch_Code = item.Branch_Code,
        //                Branch_Name = item.Branch_Name,
        //                Address1 = item.Address1,
        //                Address2 = item.Address2,
        //                Address3 = item.Address3,
        //                Country_ID = item.Country_ID,
        //                State_ID = item.State_ID,
        //                City_ID = item.City_ID,
        //                Order_Num = item.Order_Num,
        //                Pin_Code = item.Pin_Code,
        //                IsActive = item.IsActive,
        //                Country_Name=item.Country_Name,
        //                State_Name=item.State_Name,
        //                City_Name=item.City_Name
        //            };

        //            qList.Add(model);
        //        }
        //    }
        //    finally
        //    {
        //        branchContext.Database.Connection.Close();
        //    }

        //    return qList;
        //}

        public List<ViewBranches> GetFilterBranch(string searchColumn, string searchString, Guid userId)
        {
            List<ViewBranches> qList = new List<ViewBranches>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            branchContext.Database.Initialize(force: false);

            var cmd = branchContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetBranch]";
            cmd.CommandType = CommandType.StoredProcedure;

            //var searchParam = new SqlParameter();
            //searchParam.ParameterName = "@SearchColumn";
            //searchParam.SqlDbType = SqlDbType.NVarChar;
            //searchParam.SqlValue = searchColumn;
            ////searchParam.ParameterDirection = ParameterDirection.Output;

            //var stringParam = new SqlParameter();
            //stringParam.ParameterName = "@SearchString";
            //stringParam.SqlDbType = SqlDbType.NVarChar;
            //stringParam.SqlValue = searchString;
            ////stringParam.ParameterDirection = ParameterDirection.Output;

            //cmd.Parameters.Add(searchParam);
            //cmd.Parameters.Add(stringParam);

            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                branchContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)branchContext)
                    .ObjectContext
                    .Translate<ViewBranches>(reader, "Branch", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    ViewBranches model = new ViewBranches()


                    {

                        Branch_Id = item.Branch_Id,
                        Branch_Code = item.Branch_Code,
                        Branch_Name = item.Branch_Name,
                        Address1 = item.Address1,
                        Address2 = item.Address2,
                        Address3 = item.Address3,
                        Country_ID = item.Country_ID,
                        State_ID = item.State_ID,
                        City_ID = item.City_ID,
                        //country = item.country,
                        //city = item.city,
                        //state = item.state,
                        Order_Num = item.Order_Num,
                        Pin_Code = item.Pin_Code,
                        IsActive = item.IsActive,
                        Created_Branc_Id = item.Created_Branc_Id,
                        Created_Dte = item.Created_Dte,
                        Created_User_Id = item.Created_User_Id,
                        Modified_Branch_Id = item.Modified_Branch_Id,
                        Modified_Dte = item.Modified_Dte,
                        Modified_User_Id = item.Modified_User_Id,
                        Country_Name = item.Country_Name,
                        State_Name = item.State_Name,
                        City_Name = item.City_Name
                    };

                    qList.Add(model);
                }
            }
            finally
            {
                branchContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<Branch> GetAllBranchByFilter()
        {
            List<Branch> qList = new List<Branch>();

            return qList;
        }

        public Branch FindOneBranchById(int qId)
        {
            return (from p in branchContext.Branch
                    where p.Branch_Id == qId
                    select p).FirstOrDefault();
        }

        public Branch CheckDuplicateName(string brname)
        {
            return (from p in branchContext.Branch
                    where p.Branch_Code.Equals(brname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public Branch CheckDuplicateBranchName(string bname)
        {
            return (from p in branchContext.Branch
                    where p.Branch_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }


        public Branch CheckDuplicateBranch(string bname, string CheckingType)
        {
            Branch qList = new Branch();
            if (CheckingType == "Code")
            {
                qList = (from p in branchContext.Branch
                         where p.Branch_Code.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                         select p).FirstOrDefault();
            }
            else if (CheckingType == "Name")
            {
                qList = (from p in branchContext.Branch
                         where p.Branch_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                         select p).FirstOrDefault();
            }
            return qList;
        }
        public Country CheckCountry(string bname)
        {
            
            
            return (from p in countryContext.country
                         where p.Country_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                         select p).FirstOrDefault();       
       }

        public State CheckState(string bname)
        {


            return (from p in stateContext.state
                    where p.State_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }
        public City CheckCity(string bname)
        {


            return (from p in cityContext.city
                    where p.City_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
      }

            
        //    else if (CheckingType == "state")
        //    {
        //        qList = (from p in countryContext.state
        //                 where p.State_Name.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
        //                 select p).FirstOrDefault();
        //    }
        //    else if (CheckingType == "state")
        //    {
        //        qList = (from p in branchContext.Branch
        //                 where p.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
        //                 select p).FirstOrDefault();
        //    }
        //    return qList;
        //}

        public int FindIdForCountryName(string name)
        {
            int Country_id = (from p in countryContext.country
                      where p.Country_Name == name
                      select p.ID).FirstOrDefault();
            return Country_id;

        }

        public int FindIdForStateName(string name)
        {
            int State_id = (from p in stateContext.state
                      where p.State_Name == name
                      select p.ID).FirstOrDefault();
            return State_id;

        }

        public int FindIdForCityName(string name)
        {
            int City_id = (from p in cityContext.city
                      where p.City_Name == name
                      select p.ID).FirstOrDefault();
            return City_id;

        }

        public string FindCodeForCountryId(int country_id)
        {

            string sap_country_code = (from p in branchContext.country
                    where p.ID == country_id
                    select p.SAP_Country_Code).FirstOrDefault();

            return sap_country_code;

        }


        public string FindCodeForStateId(int state_id)
        {

            string sap_state_code = (from p in branchContext.state
                                       where p.ID == state_id
                                       select p.SAP_State_Code).FirstOrDefault();

            return sap_state_code;

            //int Country_id = (from p in countryContext.country
            //                  where p.ID == name
            //                  select p.SAP_Country_Code).FirstOrDefault();
        }


        public string FindNameForCityId(int city_id)
        {

            string cityname = (from p in branchContext.city
                                       where p.ID == city_id
                                       select p.City_Name).FirstOrDefault();

            return cityname;

        }

        public IEnumerable<Branch> _ExporttoExcel()
        {
            //var data = branchDb._ExporttoExcel(branch);
            //List<ViewBranches> qList = new List<ViewBranches>();


            return (from e in branchContext.Branch
                    select e);
        }


        public bool InsertFileUploadDetails(List<Branch> branch)
        {
            try
            {
                branchContext.Branch.AddRange(branch);
                branchContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                ExceptionHandler.LogException(ex);
                return false;
            }
        }


        public bool GenerateXML(Object obj)
        {
            try
            {
                string data = ModeltoSAPXmlConvertor.ConvertModelToXMLString(obj);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);

                SAPOUT mSAP = new SAPOUT();
                mSAP.Object_typ = "Branch";
                mSAP.Branch_Cde = "1";
                mSAP.Troy_Created_Dte = Convert.ToDateTime(DateTime.Now.ToString());
                mSAP.Troy_XML = doc.InnerXml;

                SAPOUTRepository prgrprepo = new SAPOUTRepository();
                if (prgrprepo.AddNew(mSAP))
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

        public List<CountryList> GetAddresscountryList()
        {
            var item = (from a in branchContext.country
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
            var item = (from a in branchContext.state
                        select new StateList
                        {
                            ID = a.ID,
                            State_Name = a.State_Name
                        }).ToList();

            return item;
        }

        public List<CityList> GetAddresscityList()
        {
            var item = (from a in branchContext.city
                        select new CityList
                        {
                            ID = a.ID,
                            City_Name = a.City_Name
                        }).ToList();

            return item;
        }



        public bool AddNewBranch(Branch branch)
        {
            try
            {
                branchContext.Branch.Add(branch);

                branchContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool EditBranch(Branch branch)
        {
            try
            {
                branchContext.Entry(branch).State = EntityState.Modified;
                branchContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }



    }
}