#region Namespace
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Troy.Data.Repository;
using Troy.Model.BusinessPartner;
using Troy.Data.DataContext;
using Troy.Web.Models;
using Troy.Web;
using Troy.Utilities.CrossCutting;
using Troy.Model.AppMembership;
#endregion

namespace Troy.Web.Controllers
{
    public class BusinessPartnerController : Controller
    {
        #region Fields
        private readonly IBusinessPartnerRepository businesspartnerDb;
        #endregion

        #region Constructor
        //inject dependency
        public BusinessPartnerController(IBusinessPartnerRepository mrepository)
        {
            this.businesspartnerDb = mrepository;
        }
        #endregion

        #region Controller Actions
        public ActionResult Index(string searchColumn, string searchQuery)
        {
            try
            {
                LogHandler.WriteLog("Business Partner Index page requested by #UserId");
                var qList = businesspartnerDb.GetFilterBusinessPartner(searchColumn, searchQuery, Guid.Empty);   //GetUserId();                

                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartnerList = qList;

                //var manufacturerlist = manufactureDb.GetAllManufacturer().ToList();

                //model.ManufacturerList = manufacturerlist;
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return View("Error");
            }
        }

        //---- check unique key-------          
        public JsonResult CheckForDuplication(BusinessPartner businesspartner, [Bind(Prefix = "BusinessPartner.BP_Name")]string BP_Name)
        {
            var data = businesspartnerDb.CheckDuplicateName(BP_Name);
            if (data != null)
            {
                return Json("Sorry, Business Partner Name already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Partial Views
        public PartialViewResult _CreatePartial()
        {
            return PartialView();
        }

        public PartialViewResult _EditPartial(int id)
        {
            try
            {
                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartner = businesspartnerDb.FindOneBusinessPartnerById(id);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }       

        public PartialViewResult _ViewPartial(int id)
        {
            try
            {
                BusinessPartnerViewModels model = new BusinessPartnerViewModels();
                model.BusinessPartner = businesspartnerDb.FindOneBusinessPartnerById(id);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                ViewBag.AppErrorMessage = ex.Message;
                return PartialView("Error");
            }
        }
        #endregion

        //#region InbuildCode

        //// GET: /BusinessPartner/
        //public ActionResult Index()
        //{
        //    var businesspartner = db.BusinessPartner.Include(b => b.city).Include(b => b.country).Include(b => b.state).Include(b => b.branch).Include(b => b.employee).Include(b => b.group).Include(b => b.ledger).Include(b => b.PList);
        //    return View(businesspartner.ToList());
        //}

        //// GET: /BusinessPartner/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BusinessPartner businesspartner = db.BusinessPartner.Find(id);
        //    if (businesspartner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(businesspartner);
        //}

        //// GET: /BusinessPartner/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Bill_City = new SelectList(db.City, "City_Id", "City_Cde");
        //    ViewBag.Bill_Country = new SelectList(db.Country, "Country_Id", "Country_Cde");
        //    ViewBag.Bill_State = new SelectList(db.State, "State_Id", "State_Cde");
        //    ViewBag.Branch_id = new SelectList(db.Branch, "Branch_Id", "Branch_Cde");
        //    ViewBag.Emp_Id = new SelectList(db.Employee, "Emp_Id", "First_Name");
        //    ViewBag.Group_id = new SelectList(db.Group, "Group_Id", "Group_Name");
        //    ViewBag.Control_account_id = new SelectList(db.Ledger, "Ledger_Id", "Ledger_Name");
        //    ViewBag.bp_Pricelist = new SelectList(db.PriceList, "Id", "Price_List_Desc");
        //    return View();
        //}

        //// POST: /BusinessPartner/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="BP_Id,BP_Name,Group_Type,Group_id,Ship_Address1,Ship_address2,Ship_address3,Ship_City,Ship_State,Ship_Country,Ship_pincode,Bill_Address1,Bill_address2,Bill_address3,Bill_City,Bill_State,Bill_Country,Bill_pincode,IsActive,bp_Pricelist,Emp_Id,Branch_id,Phone1,Phone2,Mobile,Email_Address,Website,Contact_person,Remarks,Ship_method,Control_account_id,Opening_Balance,Due_date,Created_User_Id,Created_Branc_Id,Created_Dte,Modified_User_Id,Modified_Branch_Id,Modified_Dte")] BusinessPartner businesspartner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.BusinessPartner.Add(businesspartner);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Bill_City = new SelectList(db.City, "City_Id", "City_Cde", businesspartner.Bill_City);
        //    ViewBag.Bill_Country = new SelectList(db.Country, "Country_Id", "Country_Cde", businesspartner.Bill_Country);
        //    ViewBag.Bill_State = new SelectList(db.State, "State_Id", "State_Cde", businesspartner.Bill_State);
        //    ViewBag.Branch_id = new SelectList(db.Branch, "Branch_Id", "Branch_Cde", businesspartner.Branch_id);
        //    ViewBag.Emp_Id = new SelectList(db.Employee, "Emp_Id", "First_Name", businesspartner.Emp_Id);
        //    ViewBag.Group_id = new SelectList(db.Group, "Group_Id", "Group_Name", businesspartner.Group_id);
        //    ViewBag.Control_account_id = new SelectList(db.Ledger, "Ledger_Id", "Ledger_Name", businesspartner.Control_account_id);
        //    ViewBag.bp_Pricelist = new SelectList(db.PriceList, "Id", "Price_List_Desc", businesspartner.Pricelist);
        //    return View(businesspartner);
        //}

        //// GET: /BusinessPartner/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BusinessPartner businesspartner = db.BusinessPartner.Find(id);
        //    if (businesspartner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Bill_City = new SelectList(db.City, "City_Id", "City_Cde", businesspartner.Bill_City);
        //    ViewBag.Bill_Country = new SelectList(db.Country, "Country_Id", "Country_Cde", businesspartner.Bill_Country);
        //    ViewBag.Bill_State = new SelectList(db.State, "State_Id", "State_Cde", businesspartner.Bill_State);
        //    ViewBag.Branch_id = new SelectList(db.Branch, "Branch_Id", "Branch_Cde", businesspartner.Branch_id);
        //    ViewBag.Emp_Id = new SelectList(db.Employee, "Emp_Id", "First_Name", businesspartner.Emp_Id);
        //    ViewBag.Group_id = new SelectList(db.Group, "Group_Id", "Group_Name", businesspartner.Group_id);
        //    ViewBag.Control_account_id = new SelectList(db.Ledger, "Ledger_Id", "Ledger_Name", businesspartner.Control_account_id);
        //    ViewBag.bp_Pricelist = new SelectList(db.PriceList, "Id", "Price_List_Desc", businesspartner.Pricelist);
        //    return View(businesspartner);
        //}

        //// POST: /BusinessPartner/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include="BP_Id,BP_Name,Group_Type,Group_id,Ship_Address1,Ship_address2,Ship_address3,Ship_City,Ship_State,Ship_Country,Ship_pincode,Bill_Address1,Bill_address2,Bill_address3,Bill_City,Bill_State,Bill_Country,Bill_pincode,IsActive,bp_Pricelist,Emp_Id,Branch_id,Phone1,Phone2,Mobile,Email_Address,Website,Contact_person,Remarks,Ship_method,Control_account_id,Opening_Balance,Due_date,Created_User_Id,Created_Branc_Id,Created_Dte,Modified_User_Id,Modified_Branch_Id,Modified_Dte")] BusinessPartner businesspartner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(businesspartner).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Bill_City = new SelectList(db.City, "City_Id", "City_Cde", businesspartner.Bill_City);
        //    ViewBag.Bill_Country = new SelectList(db.Country, "Country_Id", "Country_Cde", businesspartner.Bill_Country);
        //    ViewBag.Bill_State = new SelectList(db.State, "State_Id", "State_Cde", businesspartner.Bill_State);
        //    ViewBag.Branch_id = new SelectList(db.Branch, "Branch_Id", "Branch_Cde", businesspartner.Branch_id);
        //    ViewBag.Emp_Id = new SelectList(db.Employee, "Emp_Id", "First_Name", businesspartner.Emp_Id);
        //    ViewBag.Group_id = new SelectList(db.Group, "Group_Id", "Group_Name", businesspartner.Group_id);
        //    ViewBag.Control_account_id = new SelectList(db.Ledger, "Ledger_Id", "Ledger_Name", businesspartner.Control_account_id);
        //    ViewBag.bp_Pricelist = new SelectList(db.PriceList, "Id", "Price_List_Desc", businesspartner.Pricelist);
        //    return View(businesspartner);
        //}

        //// GET: /BusinessPartner/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BusinessPartner businesspartner = db.BusinessPartner.Find(id);
        //    if (businesspartner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(businesspartner);
        //}

        //// POST: /BusinessPartner/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BusinessPartner businesspartner = db.BusinessPartner.Find(id);
        //    db.BusinessPartner.Remove(businesspartner);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        //#endregion
    }
}
