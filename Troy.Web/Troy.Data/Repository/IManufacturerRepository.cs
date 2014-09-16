using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Manufacturer;

namespace Troy.Data.Repository
{
    public interface IManufacturerRepository
    {
        List<Manufacture> GetAllManufacturer();

        List<Manufacture> GetFilterManufacturer(string searchColumn, string searchString, Guid userId);
        
        Manufacture FindOneManufacturerById(int qId);

        //  List<BranchList> GetAddressList();

        Manufacture CheckDuplicateName(string mManu_Name);

        bool CheckDuplicateNameWithId(int id, string mManu_Name);

        bool InsertFileUploadDetails(List<Manufacture> manufacturer);

        bool AddNewManufacturer(Manufacture manufacturer);

        bool EditExistingManufacturer(Manufacture manufacturer);

        bool AddBulkManufacturer(Object obj);

        bool GenerateXML(Object obj);

    }
}
