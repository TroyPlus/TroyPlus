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
                //businesspartnercontext.BusinessPartner.Add(businesspartner);
                //businesspartnercontext.SaveChanges();

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
    }
}
