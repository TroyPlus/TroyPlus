//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Troy.Data.DataContext;
//using Troy.Utilities.CrossCutting;
//using System.Data.Entity;
//using Troy.Model.Branches;
//using Troy.Model.AppMembership;


//namespace Troy.Data.Repository
//{
//    public class UserRepository : BaseRepository, IUserRepository
//    {
//        private ApplicationDbContext UserContext = new ApplicationDbContext();


//        public List<ApplicationUser> GetAllUser()
//        {
//            List<ApplicationUser> qList = new List<ApplicationUser>();

//            qList = (from p in UserContext.Users
//                     select p).ToList();

//            return qList;
//        }

//        //public List<ViewBranches> GetAllBranches()
//        //{
//        //    List<ViewBranches> qList = new List<ViewBranches>();

//        //    //qList = (from p in branchContext.Branch
//        //    //         select p).ToList();

//        //    branchContext.Database.Initialize(force: false);

//        //    var cmd = branchContext.Database.Connection.CreateCommand();
//        //    cmd.CommandText = "[dbo].[USP_GetAllBranches]";
//        //    cmd.CommandType = CommandType.StoredProcedure;

//        //    try
//        //    {
//        //        branchContext.Database.Connection.Open();
//        //        // Run the sproc  
//        //        var reader = cmd.ExecuteReader();

//        //        var result = ((IObjectContextAdapter)branchContext)
//        //            .ObjectContext
//        //            .Translate<ViewBranches>(reader, "Branch", MergeOption.AppendOnly);


//        //        foreach (var item in result)
//        //        {
//        //            ViewBranches model = new ViewBranches()


//        //            {

//        //                Branch_Id = item.Branch_Id,
//        //                Branch_Code = item.Branch_Code,
//        //                Branch_Name = item.Branch_Name,
//        //                Address1 = item.Address1,
//        //                Address2 = item.Address2,
//        //                Address3 = item.Address3,
//        //                Country_ID = item.Country_ID,
//        //                State_ID = item.State_ID,
//        //                City_ID = item.City_ID,
//        //                Order_Num = item.Order_Num,
//        //                Pin_Code = item.Pin_Code,
//        //                IsActive = item.IsActive,
//        //                Country_Name=item.Country_Name,
//        //                State_Name=item.State_Name,
//        //                City_Name=item.City_Name
//        //            };

//        //            qList.Add(model);
//        //        }
//        //    }
//        //    finally
//        //    {
//        //        branchContext.Database.Connection.Close();
//        //    }

//        //    return qList;
//        //}

//        public List<ApplicationUser> GetFilterUser(string searchColumn, string searchString, Guid userId)
//        {
//            List<ApplicationUser> qList = new List<ApplicationUser>();

//            if (searchColumn == null)
//            {
//                searchColumn = "";
//                searchString = "";
//            }

//            UserContext.Database.Initialize(force: false);

//            var cmd = UserContext.Database.Connection.CreateCommand();
//            cmd.CommandText = "[dbo].[USP_GetBranch]";
//            cmd.CommandType = CommandType.StoredProcedure;

//            //var searchParam = new SqlParameter();
//            //searchParam.ParameterName = "@SearchColumn";
//            //searchParam.SqlDbType = SqlDbType.NVarChar;
//            //searchParam.SqlValue = searchColumn;
//            ////searchParam.ParameterDirection = ParameterDirection.Output;

//            //var stringParam = new SqlParameter();
//            //stringParam.ParameterName = "@SearchString";
//            //stringParam.SqlDbType = SqlDbType.NVarChar;
//            //stringParam.SqlValue = searchString;
//            ////stringParam.ParameterDirection = ParameterDirection.Output;

//            //cmd.Parameters.Add(searchParam);
//            //cmd.Parameters.Add(stringParam);

//            cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
//            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

//            try
//            {
//                UserContext.Database.Connection.Open();
//                // Run the sproc  
//                var reader = cmd.ExecuteReader();

//                var result = ((IObjectContextAdapter)UserContext)
//                    .ObjectContext
//                    .Translate<ApplicationUser>(reader, "ApplicationUser", MergeOption.AppendOnly);


//                foreach (var item in result)
//                {
//                    ApplicationUser model = new ApplicationUser()


//                    {
//                        Id = item.Id,
//                        UserName = item.UserName,
//                        Emp_Id = item.Emp_Id,
//                        Branch_Id = item.Branch_Id,
//                        Email = item.Email,
//                        EmailConfirmed = item.EmailConfirmed,
//                        PasswordHash = item.PasswordHash,
//                        PhoneNumber = item.PhoneNumber,
//                        PhoneNumberConfirmed = item.PhoneNumberConfirmed,
//                        TwoFactorEnabled = item.TwoFactorEnabled,
//                        LockoutEndDateUtc = item.LockoutEndDateUtc,
//                        LockoutEnabled = item.LockoutEnabled,
//                        AccessFailedCount = item.AccessFailedCount,
//                        PasswordExpiryDate = item.PasswordExpiryDate,
//                        IsActive = item.IsActive,
//                        Created_Branch_Id = item.Created_Branch_Id,
//                        Created_Date = item.Created_Date,
//                        Created_User_Id = item.Created_User_Id,
//                        Modified_Branch_Id = item.Modified_Branch_Id,
//                        Modified_Date = item.Modified_Date,
//                        Modified_User_Id = item.Modified_User_Id,
//                    };

//                    qList.Add(model);
//                }
//            }
//            finally
//            {
//                UserContext.Database.Connection.Close();
//            }

//            return qList;
//        }

//        public List<ApplicationUser> GetAllUserByFilter()
//        {
//            List<ApplicationUser> qList = new List<ApplicationUser>();

//            return qList;
//        }

//        public ApplicationUser FindOneUserById(int uId)
//        {
//            return (from p in UserContext.Users
//                    where p.Id == uId
//                    select p).FirstOrDefault();
//        }

//        public ApplicationUser CheckDuplicateName(string uname)
//        {
//            return (from p in UserContext.Users
//                    where p.UserName.Equals(uname, StringComparison.CurrentCultureIgnoreCase)
//                    select p).FirstOrDefault();
//        }


//        public IEnumerable<ApplicationUser> _ExporttoExcel()
//        {
//            //var data = branchDb._ExporttoExcel(branch);
//            return (from e in UserContext.Users
//                    select e);

//            //e.Branch_Code,
//            //e.Branch_Name,
//            //e.Address1,
//            //e.Address2,
//            //e.Address3,
//            //e.Country_ID,
//            //e.State_ID,
//            //e.City_ID,
//            //e.Pin_Code,
//            //e.Order_Num,
//            //e.IsActive);
//            //try
//            //  {
//            //      branchContext.Database.Connection.Open();
//            //      // Run the sproc  
//            //      //var reader = cmd.ExecuteReader();

//            //      var result = ((IObjectContextAdapter)branchContext)
//            //      //    .ObjectContext
//            //      //    .Translate<Branch>(reader, "Branch", MergeOption.AppendOnly);


//            //      foreach (var item in result)
//            //      {
//            //          Branch model = new Branch()


//            //          {

//            //              Branch_Id = item.Branch_Id,
//            //              Branch_Code=item.Branch_Code,
//            //              Branch_Name = item.Branch_Name,
//            //              Address1=item.Address1,
//            //              Address2=item.Address2,
//            //              Address3=item.Address3,
//            //              Country_ID = item.Country_ID,
//            //              State_ID = item.State_ID,
//            //              City_ID = item.City_ID,
//            //              //country = item.country,
//            //              //city = item.city,
//            //              //state = item.state,
//            //              Order_Num = item.Order_Num,
//            //              Pin_Code = item.Pin_Code,
//            //              IsActive = item.IsActive,
//            //              Created_Branc_Id = item.Created_Branc_Id,
//            //              Created_Dte = item.Created_Dte,
//            //              Created_User_Id = item.Created_User_Id,
//            //              Modified_Branch_Id = item.Modified_Branch_Id,
//            //              Modified_Dte = item.Modified_Dte,
//            //              Modified_User_Id = item.Modified_User_Id
//            //          };

//            //          qList.Add(model);
//            //      }
//            //}
//            //finally
//            //{
//            //    branchContext.Database.Connection.Close();
//            //}

//            //return qList;
//        }





//        //public bool InsertFileUploadDetails(List<Branch> branch)
//        //{
//        //    throw new NotImplementedException();
//        //}






//        //public List<BranchList> GetAddressList()
//        //{
//        //    var item = (from a in branchContext.Branch
//        //                select new BranchList
//        //                {

//        //                    BranchName = a.Branch_Name,
//        //                    BranchId = a.Branch_Id
//        //                }).ToList();

//        //    return item;
//        //}



//        public bool AddNewBranch(ApplicationUser ApplicationUsers)
//        {
//            try
//            {
//                UserContext.Users.Add(ApplicationUsers);

//                UserContext.SaveChanges();

//                return true;
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                return false;
//            }
//        }

//        public bool EditUser(ApplicationUser ApplicationUsers)
//        {
//            try
//            {
//                UserContext.Entry(ApplicationUsers).State = EntityState.Modified;
//                UserContext.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.LogException(ex);
//                return false;
//            }
//        }



//    }
//}
