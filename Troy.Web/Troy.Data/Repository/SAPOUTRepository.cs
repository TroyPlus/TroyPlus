using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.SAP_OUT;
using Troy.Utilities.CrossCutting;

namespace Troy.Data.Repository
{
    public class SAPOUTRepository : BaseRepository, ISAPOUTRepository
    {
        private SAPOUTContext sapoutcontext = new SAPOUTContext();

        public bool AddNew(SAPOUT sapout)
        {
            try
            {
                sapoutcontext.SAPOUT.Add(sapout);
                sapoutcontext.SaveChanges();

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
