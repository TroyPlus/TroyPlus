#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Manufacturer;
using Troy.Model.SAP_OUT;
using Troy.Utilities.CrossCutting;
#endregion


namespace Troy.Data.Repository
{
    public class ManufactureRepository : BaseRepository, IManufacturerRepository
    {
        private ManufactureContext manufactureContext = new ManufactureContext();
        private SAPOUTContext sapcontext = new SAPOUTContext();

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
            cmd.CommandType = CommandType.StoredProcedure;           

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

                        Manufacturer_Id = item.Manufacturer_Id,
                        Manufacturer_Name = item.Manufacturer_Name,
                        Level = item.Level,
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
                manufactureContext.Database.Connection.Close();
            }

            return qList;
        }


        public Manufacture GetManufacturerById(int qId)
        {
            return (from p in manufactureContext.Manufacture
                    where p.Manufacturer_Id == qId
                    select p).FirstOrDefault();
        }

        public Manufacture CheckDuplicateName(string mManu_Name)
        {
            return (from p in manufactureContext.Manufacture
                    where p.Manufacturer_Name.Equals(mManu_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
        }

        public bool CheckDuplicateNameWithId(int id, string mManu_Name)
        {
            var currentValue = manufactureContext.Manufacture.Find(id);

            if (currentValue.Manufacturer_Name == mManu_Name)
            {               
                return true;
            }
            else
            {
                var response =   (from p in manufactureContext.Manufacture
                    where p.Manufacturer_Name.Equals(mManu_Name, StringComparison.CurrentCultureIgnoreCase)
                    select p).FirstOrDefault();
                if(response != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }           
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
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                return false;
            }
        }

        public bool InsertFileUploadDetails(List<Manufacture> manufacturer)
        {
            try
            {
                manufactureContext.Manufacture.AddRange(manufacturer);
                manufactureContext.SaveChanges();
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
                string data= ModeltoSAPXmlConvertor.ConvertModelToXMLString(obj);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);


                SAPOUT mSAP = new SAPOUT();
                mSAP.Object_typ = "MANUFACTURER";
                mSAP.Branch_Cde = "1";
                mSAP.Troy_Created_Dte = Convert.ToDateTime(DateTime.Now.ToString());
                mSAP.Troy_XML = doc.InnerXml;
                SAPOUTRepository saprepo = new SAPOUTRepository();
                saprepo.AddNew(mSAP);               
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
