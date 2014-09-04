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


namespace Troy.Data.Repository
{
    public class BranchRepository : BaseRepository, IBranchRepository
    {
        private BranchContext branchContext = new BranchContext();


        public List<Branch> GetAllBranch()
        {
            List<Branch> qList = new List<Branch>();

            qList = (from p in branchContext.Branch
                     select p).ToList();

            return qList;
        }

        public List<Branch> GetFilterBranch(string searchColumn, string searchString, Guid userId)
        {
            List<Branch> qList = new List<Branch>();

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
                    .Translate<Branch>(reader, "Branch", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    Branch model = new Branch()

                    {

                        Branch_Id = item.Branch_Id,
                        Branch_Code=item.Branch_Code,
                        Branch_Name = item.Branch_Name,
                        Address1=item.Address1,
                        Address2=item.Address2,
                        Address3=item.Address3,
                        Country_ID = item.Country_ID,
                        State_ID = item.State_ID,
                        City_ID = item.City_ID,
                        Order_Num = item.Order_Num,
                        Pin_Code = item.Pin_Code,
                        IsActive = item.IsActive,
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





        //public bool InsertFileUploadDetails(List<Branch> branch)
        //{
        //    throw new NotImplementedException();
        //}






        //public List<BranchList> GetAddressList()
        //{
        //    var item = (from a in branchContext.Branch
        //                select new BranchList
        //                {
        //                    BranchName = a.Branch_Name,
        //                    BranchId = a.Branch_Id
        //                }).ToList();

        //    return item;
        //}


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
                            ID=a.ID,
                            City_Name=a.City_Name
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
            catch(Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
    }
}
