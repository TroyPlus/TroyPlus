using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Years;

namespace Troy.Data.Repository.MasterData
{
    public interface IYearRepository
    {
        IList<FinancialYear> GetAllFinancialYears();

    }
}
