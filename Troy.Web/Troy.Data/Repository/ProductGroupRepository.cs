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
using Troy.Model.ProductGroup;
using Troy.Utilities.CrossCutting;


namespace Troy.Data.Repository
{
    public class ProductGroupRepository : BaseRepository, IProductGroupRepository
    {
        private ProductGroupContext ProductGroupContext = new ProductGroupContext();

        public List<ProductGroup> GetAllProductGroup()
        {
            List<ProductGroup> qList = new List<ProductGroup>();

            qList = (from p in ProductGroupContext.ProductGroup
                     select p).ToList();

            return qList;
        }

        public List<ProductGroup> GetFilterProductGroup(string searchColumn, string searchString, Guid userId)
        {
            List<ProductGroup> qList = new List<ProductGroup>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            ProductGroupContext.Database.Initialize(force: false);

            var cmd = ProductGroupContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetProductGroup]";
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
                ProductGroupContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)ProductGroupContext)
                    .ObjectContext
                    .Translate<ProductGroup>(reader, "ProductGroup", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    ProductGroup model = new ProductGroup()
                    {

                        Product_Group_Id = item.Product_Group_Id,
                        Product_Group_Name = item.Product_Group_Name,
                        Level = item.Level,
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
                ProductGroupContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<ProductGroup> GetAllProductGroupByFilter()
        {
            List<ProductGroup> qList = new List<ProductGroup>();

            return qList;
        }

        public ProductGroup FindOneProductGroupById(int qId)
        {
            return (from p in ProductGroupContext.ProductGroup
                    where p.Product_Group_Id == qId
                    select p).FirstOrDefault();
        }

        public ProductGroup CheckDuplicateName(string mPGroup_Name)
        {
            return (from p in ProductGroupContext.ProductGroup
                    where p.Product_Group_Name.Equals(mPGroup_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public bool AddNewProductGroup(ProductGroup ProductGroup)
        {
            try
            {
                ProductGroupContext.ProductGroup.Add(ProductGroup);

                ProductGroupContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool InsertFileUploadDetails(List<ProductGroup> ProductGroup)
        {
            throw new NotImplementedException();
        }



        public bool EditExistingProductGroup(ProductGroup ProductGroup)
        {
            throw new NotImplementedException();
        }
    }
}
