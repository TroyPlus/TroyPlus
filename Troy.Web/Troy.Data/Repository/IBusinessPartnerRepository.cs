using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.BusinessPartner;

namespace Troy.Data.Repository
{
    public interface IBusinessPartnerRepository
    {
        List<BusinessPartner> GetAllBusinessPartner();

        List<BusinessPartner> GetFilterBusinessPartner(string searchColumn, string searchString, Guid UserId);

        BusinessPartner FindOneBusinessPartnerById(int qId);

        BusinessPartner CheckDuplicateName(string mBusinessPartner_Name);
        bool InsertFileUploadDetails(List<BusinessPartner> businesspartner);

        bool AddNewBusinessPartner(BusinessPartner businesspartner);

        bool EditExistingBusinessPartner(BusinessPartner businesspartner);

        bool AddBulkBusinessPartner(Object obj);
    }
}
