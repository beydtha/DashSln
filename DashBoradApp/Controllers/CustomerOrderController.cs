using DashBoradApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoradApp.Controllers
{
    public class CustomerOrderController : Controller
    {
        // GET: CustomerOrder
        public ActionResult Index()
        {
            DashboardContext _context = new DashboardContext();
            var cusList = _context.Customers.ToList(); 

            return View(cusList);
        }

        [HttpGet]
        public ActionResult CustomerList()
        {
            return View(); 
                
        }
        [HttpGet]
        public ActionResult GetCustomers()
        {
            using (DashboardContext _context = new DashboardContext())
            {
                var customerList = _context.Customers.Select(c => new
                {
                    c.ID,
                    c.CustomerName,
                    c.CustomerPhone,
                    c.CustomerEmail,
                    c.CustomerImage, 
                    c.CustomerCountry
                }).ToList();

                return Json(new  {  data = customerList }, JsonRequestBehavior.AllowGet);
            }
        }
                   
    }
}