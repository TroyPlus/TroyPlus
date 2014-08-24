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
using Troy.Model.Branch;
using Troy.Utilities.CrossCutting;


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

            var searchParam = new SqlParameter();
            searchParam.ParameterName = "@SearchColumn";
            searchParam.SqlDbType = SqlDbType.NVarChar;
            searchParam.SqlValue = searchColumn;
            //searchParam.ParameterDirection = ParameterDirection.Output;

            var stringParam = new SqlParameter();
            stringParam.ParameterName = "@SearchString";
            stringParam.SqlDbType = SqlDbType.NVarChar;
            stringParam.SqlValue = searchString;
            //stringParam.ParameterDirection = ParameterDirection.Output;

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
                        Branch_Name = item.Branch_Name,
                        Country_Cde=item.Country_Cde,
                        State_Cde=item.State_Cde,
                        City_Cde=item.City_Cde,
                        Order_Num=item.Order_Num,
                        Pin_Cod=item.Pin_Cod,
                        IsActive=item.IsActive,
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

     

        public bool InsertFileUploadDetails(List<Branch> branch)
        {
            throw new NotImplementedException();
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
    }
}
