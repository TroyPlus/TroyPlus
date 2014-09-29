using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Data.DataContext;
using Troy.Model.Years;

namespace Troy.Data.Repository.MasterData
{
    public class YearRepository:BaseRepository,IYearRepository
    {
        MasterDataContext masterDataContext = new MasterDataContext();

        public IList<FinancialYear> GetAllFinancialYears()
        {            
            return masterDataContext.Years.Select(y => y).ToList();
        }
    }
}
