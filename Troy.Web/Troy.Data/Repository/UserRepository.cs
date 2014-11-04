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
using Troy.Utilities.CrossCutting;
using System.Data.Entity;
using Troy.Model.Branches;
using Troy.Model.AppMembership;
using Troy.Model.Employees;


namespace Troy.Data.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private ApplicationDbContext UserContext = new ApplicationDbContext();

        private BranchContext BranchContext = new BranchContext();

        //private UserBranch userbranch = new UserBranch();




        //public List<ViewUsers> GetAllUser()
        //{
        //    List<ViewUsers> qList = new List<ViewUsers>();

        //    qList = (from p in UserContext.Users
        //             select p).ToList();

        //    return qList;
        //}

        public List<ViewUsers> GetAllUser()
        {
            List<ViewUsers> qList = new List<ViewUsers>();

            qList = (from p in UserContext.Users
                     join r in UserContext.Roles 
                                     on p.Roles.FirstOrDefault().RoleId equals r.Id
                                     into u_r
                    from ur in u_r.DefaultIfEmpty()
                     select new ViewUsers()
                     {
                         Id = p.Id,
                         UserName = p.UserName,
                         Email = p.Email,
                         //Branch_Id = ur.Branch_Id,
                         Employee_Id = p.Employee_Id,
                         PasswordExpiryDate = p.PasswordExpiryDate,
                         Role_Id = p.Roles.FirstOrDefault().RoleId,
                         Name=ur.Name,
                         IsActive=p.IsActive                    
                     }).ToList();

            

            return qList;
        }

        public List<EmployeeList> GetAddressEmployeeList()
        {
            var item = (from a in UserContext.employee
                        select new EmployeeList
                        {
                            Emp_Id = a.Emp_Id,
                            First_Name = a.First_Name
                            //BranchId = a.Branch_Id
                        }).ToList();

            return item;
        }



        public List<BranchList> GetAllBranches()
        {

            var item = (from a in UserContext.branch
                        select new BranchList

                       {
                           Branch_Id = a.Branch_Id,
                           Branch_Name = a.Branch_Name,
                           IsSelected = false
                       }).ToList();
            return item;
        }

        public List<BranchList> GetBranchesByUserId(int userId)
        {
            var result = (from ub in UserContext.userbranches
                          where ub.User_Id == userId
                          join b in UserContext.branch
                                     on ub.Branch_Id equals b.Branch_Id
                                     into ub_b
                          from ubs in ub_b.DefaultIfEmpty()                          
                          select new BranchList
                          {
                              Branch_Id = ubs.Branch_Id,
                              Branch_Name = ubs.Branch_Name,
                              IsSelected = false
                          }).ToList();
            return result;
        }
     

         public List<ViewUsers> GetApplicationIdforName()
        {
            List<ViewUsers> qList = new List<ViewUsers>();

            qList = (from p in UserContext.Users
                         //join u in 
                         //      on p.Id equals u.
                         //join s in UserContext.Roles
                         //   on p.Id equals s.Name
                         //    select s.Name).FirstOrDefault();
                       
                         select new ViewUsers()
                         {
                             Id= p.Id,
                             UserName=p.UserName,
                             Email=p.Email,
                             Role_Id=p.Roles.FirstOrDefault().RoleId,
                             //Name =p.Roles.FirstOrDefault().RoleId
                             //Name=p.Roles.FirstOrDefault(Role_Id)
                         }).ToList();

                      

            //return qList;

             var result =(from p in qList
                              join r in UserContext.Roles
                                       on p.Role_Id equals r.Id
                                       //on p.Role_Id equals r.Name
                                    //on q.Role_Id equals r.Name

                                  select new ViewUsers()
                                  {
                                      Id=r.Id,
                                      UserName=p.UserName,
                                      Email=p.Email,
                                      //UserName=r.Users.FirstOrDefault().,
                                      //UserName = q.UserName,
                                      //Email = q.Email,
                                      //Role_Id = q.Role_Id,
                                     Name=r.Name
                                  }).ToList();
             return result;

             return qList;
        }

         public bool SaveUserBranches(List<UserBranches> userBranches,int Id, ref string errorMessage)
         {
             try
             {
                 if (UserContext.userbranches.Count() > 0)
                 {
                     List<UserBranches> tempUserBranches = new List<UserBranches>();
                     // filter input user's branches.

                     tempUserBranches = (from u in UserContext.userbranches
                                         where u.User_Id == Id
                                         select u).ToList();
                    

                     UserContext.userbranches.RemoveRange(tempUserBranches);
                     UserContext.SaveChanges();
                 }

                 UserContext.userbranches.AddRange(userBranches);
                 UserContext.SaveChanges();
                 return true;
             }
             catch (Exception ex)
             {
                 errorMessage = ex.Message;
                 return false;
             }
         }






         public ApplicationUser CheckDuplicateUserName(string bname)
         {
             return (from p in UserContext.Users
                     where p.UserName.Equals(bname, StringComparison.CurrentCultureIgnoreCase)
                     select p).FirstOrDefault();
         }



       //public string  getalluserview()
       // {
           
       // }
        //public string GetApplicationIdforName(int roleid)
        //{
        //    var Name = (from p in UserContext.Roles
        //                where p.Id == roleid
        //                select p).FirstOrDefault();
        //    return Name;
        //}

        public List<ApplicationRole> GetAddressRoleList()
        {
            var item = (from a in UserContext.Roles
                         select a)
                        .ToList();

            return item;
        }
                        
     
       

        //public List<UserBranches> GetAddressBranchList()
        //{
        //    var item = (from a in UserContext.userbranches
        //                select new UserBranches
        //                {
        //                    Branch_Id = a.Branch_Id,
        //                    Branch_Name = a.Branch_Name
        //                    //Emp_Id = a.Emp_Id,
        //                    //First_Name = a.First_Name
        //                    //BranchId = a.Branch_Id
        //                }).ToList();

        //    return item;
        //}

        //public List<UserBranches> GetAddressUserBranchList()
        //{
        //    var item = (from a in UserContext.userbranches
        //                select new UserBranches
        //                {
        //                    User_Id = a.User_Id,
        //                    Branch_Id = a.Branch_Id
        //                }).ToList();
        //    return item;

        //}

        public List<ApplicationUser> GetFilterUser(string searchColumn, string searchString, Guid userId)
        {
            List<ApplicationUser> qList = new List<ApplicationUser>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }
            //UserContext.Users

            //UserContext.Database.Initialize(force: false);

            //var cmd = UserContext.Database.Connection.CreateCommand();
            //cmd.CommandText = "[dbo].[USP_GetBranch]";
            //cmd.CommandType = CommandType.StoredProcedure;

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

            //cmd.Parameters.Add(new SqlParameter("@SearchColumn", searchColumn));
            //cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));

            try
            {
                //UserContext.Database.Connection.Open();
                //// Run the sproc  
                //var reader = cmd.ExecuteReader();

                //var result = ((IObjectContextAdapter)UserContext)
                //    .ObjectContext
                //    .Translate<ApplicationUser>(reader, "ApplicationUser", MergeOption.AppendOnly);


                //foreach (var item in result)
                //{
                //    ApplicationUser model = new ApplicationUser()


                //    {
                //        Id = item.Id,
                //        UserName = item.UserName,
                //        Emp_Id = item.Emp_Id,
                //        Branch_Id = item.Branch_Id,
                //        Email = item.Email,
                //        EmailConfirmed = item.EmailConfirmed,
                //        PasswordHash = item.PasswordHash,
                //        PhoneNumber = item.PhoneNumber,
                //        PhoneNumberConfirmed = item.PhoneNumberConfirmed,
                //        TwoFactorEnabled = item.TwoFactorEnabled,
                //        LockoutEndDateUtc = item.LockoutEndDateUtc,
                //        LockoutEnabled = item.LockoutEnabled,
                //        AccessFailedCount = item.AccessFailedCount,
                //        PasswordExpiryDate = item.PasswordExpiryDate,
                //        IsActive = item.IsActive,
                //        Created_Branch_Id = item.Created_Branch_Id,
                //        Created_Date = item.Created_Date,
                //        Created_User_Id = item.Created_User_Id,
                //        Modified_Branch_Id = item.Modified_Branch_Id,
                //        Modified_Date = item.Modified_Date,
                //        Modified_User_Id = item.Modified_User_Id,
                //    };

                //    qList.Add(model);
                //}
            }
            finally
            {
                //UserContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<ApplicationUser> GetAllUserByFilter()
        {
            List<ApplicationUser> qList = new List<ApplicationUser>();

            return qList;
        }

        public ViewUsers FindOneUserById(int uId)
        {
            //return (from p in UserContext.Users
            //        where p.Id == uId
            //        select p).FirstOrDefault();


            return (from p in UserContext.Users
                    join r in UserContext.Roles
                                    on p.Roles.FirstOrDefault().RoleId equals r.Id
                                    into u_r
                    from ur in u_r.DefaultIfEmpty()
                    join b in UserContext.userbranches
                        on p.Id equals b.User_Id
                        into b_u
                    from urb in b_u.DefaultIfEmpty()
                    where p.Id == uId
                    select new ViewUsers()
                    {
                        Id = p.Id,
                        UserName = p.UserName,
                        Email = p.Email,
                        Branch_Id = urb.Branch_Id != null ? urb.Branch_Id:0,
                        Employee_Id = p.Employee_Id,
                        PasswordExpiryDate = p.PasswordExpiryDate,
                        Role_Id = p.Roles.FirstOrDefault().RoleId,
                        Name = ur.Name,
                        IsActive = p.IsActive,
                        Roles = p.Roles.ToList()
                    }).FirstOrDefault();
        }

        public ICollection<ApplicationUserRole> GetUserApplicationRoles(int userId)
        {
            var result= (from p in UserContext.Users
                    where p.Id == userId
                    select p.Roles).FirstOrDefault();

            return result;
        }

        public ApplicationUser CheckDuplicateName(string uname)
        {
            return (from p in UserContext.Users
                    where p.UserName.Equals(uname, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }


        public IEnumerable<ApplicationUser> _ExporttoExcel()
        {
            //var data = branchDb._ExporttoExcel(branch);
            return (from e in UserContext.Users
                    select e);


        }





        //public bool InsertFileUploadDetails(List<ApplicationUser> ApplicationUsers)
        //{
        //    throw new NotImplementedException();
        //}


        public bool AddNewUser(ApplicationUser ApplicationUsers)
        {
            try
            {

                UserContext.Users.Add(ApplicationUsers);

                //UserContext.userbranch.Add(UserBranches);

                UserContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool EditUser(ApplicationUser ApplicationUsers)
        {
            try
            {
                UserContext.Entry(ApplicationUsers).State = EntityState.Modified;
                UserContext.SaveChanges();
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
