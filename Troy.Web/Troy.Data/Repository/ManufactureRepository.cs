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
using Troy.Model.Manufacturer;
using Troy.Utilities.CrossCutting;


namespace Troy.Data.Repository
{
    public class ManufactureRepository : BaseRepository, IManufacturerRepository
    {
        private ManufactureContext manufactureContext = new ManufactureContext();


        public List<Manufacture> GetAllManufacturer()
        {
            List<Manufacture> qList = new List<Manufacture>();

            qList = (from p in manufactureContext.Manufacture
                     select p).ToList();

            return qList;
        }

        public List<Manufacture> GetFilterManufacturer(string searchColumn, string searchString, Guid userId)
        {
            List<Manufacture> qList = new List<Manufacture>();

            if (searchColumn == null)
            {
                searchColumn = "";
                searchString = "";
            }

            manufactureContext.Database.Initialize(force: false);

            var cmd = manufactureContext.Database.Connection.CreateCommand();
            cmd.CommandText = "[dbo].[USP_GetManufacturer]";

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
                manufactureContext.Database.Connection.Open();
                // Run the sproc  
                var reader = cmd.ExecuteReader();

                var result = ((IObjectContextAdapter)manufactureContext)
                    .ObjectContext
                    .Translate<Manufacture>(reader, "Manufacture", MergeOption.AppendOnly);


                foreach (var item in result)
                {
                    Manufacture model = new Manufacture()
                    {
                       
                        Manufacturer_Id=item.Manufacturer_Id,
                        Manufacturer_Name=item.Manufacturer_Name,
                        Level=item.Level,
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
                manufactureContext.Database.Connection.Close();
            }

            return qList;
        }

        public List<Manufacture> GetAllManufactureByFilter()
        {
            List<Manufacture> qList = new List<Manufacture>();

            return qList;
        }

        public Manufacture FindOneManufacturerById(int qId)
        {
            return (from p in manufactureContext.Manufacture
                    where p.Manufacturer_Id == qId
                    select p).FirstOrDefault();
        }        
    
        public bool AddNewManufacturer(Manufacture manufacturer)
        {
            try
            {
                manufactureContext.Manufacture.Add(manufacturer);

                manufactureContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool EditExistingManufacturer(Manufacture manufacturer)
        {
            try
            {
                manufactureContext.Entry(manufacturer).State = EntityState.Modified;
                manufactureContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }
        
        public bool InsertFileUploadDetails(List<Manufacture> manufacture)
        {
            throw new NotImplementedException();
        }
    }
}
