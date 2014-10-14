using System;
using System.Web;
using System.Web.Mvc;
using Troy.Model.AppMembership;

namespace Troy.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Properties

        public ApplicationUser CurrentUser
        {
            get
            {
                return ApplicationUserManager.GetApplicationUser(User.Identity.Name, HttpContext.GetOwinContext());
            }
        }

        public int CurrentBranchId
        {
            get {  
                int userBranchId=0;
                if (Session["CurrentBranch"] != null)
                {
                    var userBranch = Session["CurrentBranch"].ToString();
                    int.TryParse(userBranch, out userBranchId);
                }
                return userBranchId;
            }
        }

        public string CurrentFinancialYear
        {
            get
            {
                if (Session["FinancialYear"] != null)
                {
                    return Session["FinancialYear"].ToString();
                }
                return DateTime.Now.Year.ToString();
            }
        }

        #endregion
    }
}