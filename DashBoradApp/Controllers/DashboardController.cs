using DashBoradApp.DataAccess;
using DashBoradApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoradApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            using (DashboardContext _context = new DashboardContext())
            {
                ViewBag.CountCustomers = _context.Customers.Count();
                ViewBag.CountOrders = _context.Orders.Count();
                ViewBag.CountProducts = _context.Products.Count();
            }
            return View();
        }
        public ActionResult GetDetails(string type)
        {
            List<ProductOrCustomerViewModel> result = GetProductOrCustomer(type);
            return PartialView("~/Views/Dashboard/GetDetails.cshtml", result); 

        }
        public ActionResult TopCustomers()
        {
            List<TopCustomerViewModel> topFiveCustomers = new List<TopCustomerViewModel>();
            using (DashboardContext _context = new DashboardContext())
            {
                var orderByCustomer = (from cus in _context.Orders
                                       group cus by cus.Customer.ID into grpCustomer
                                       orderby grpCustomer.Count() descending
                                       select new
                                       {
                                           CustomerID = grpCustomer.Key,
                                           Count = grpCustomer.Count()
                                       }).Take(5);
                topFiveCustomers = (from cus in _context.Customers
                                    join ord in orderByCustomer on cus.ID equals ord.CustomerID
                                    select new TopCustomerViewModel
                                    {
                                        CustomerName = cus.CustomerName,
                                        CustomerImage = cus.CustomerImage,
                                        CustomerCountry = cus.CustomerCountry,
                                        CountOrder = ord.Count
                                    }).ToList();

            }

            return PartialView("~/Views/Dashboard/TopCustomers.cshtml", topFiveCustomers); 
        }

        public ActionResult CustomersByCountry()
        {
            DashboardContext _context = new DashboardContext();

            var customerByCountry = (from cus in _context.Customers
                                     group cus by cus.CustomerCountry into grpCustomer
                                     orderby grpCustomer.Count() descending
                                     select new {
                                         Country = grpCustomer.Key,
                                         CountCustomer = grpCustomer.Count()
                                     }).ToList();

            return Json(new { result = customerByCountry }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderByCustomer()
        {
            DashboardContext _context = new DashboardContext();

            var ordersByCustomer = (from ord in _context.Orders
                                    group ord by ord.Customer.ID into grp
                                    select new
                                    {
                                        name = _context.Customers.Where(c => c.ID == grp.Key).Select( x => x.CustomerName), 
                                        CountÖrders = grp.Count() 
                                    }).ToList();

            return Json(new { result = ordersByCustomer}, JsonRequestBehavior.AllowGet);

        }

        public ActionResult OrdersByCountry (){

            DashboardContext _context = new DashboardContext();
            var ordersByCountry = (from o in _context.Orders
                                   group 0 by o.Customer.CustomerCountry into grp
                                   orderby grp.Count() descending
                                   select new
                                   {
                                       Country  = grp.Key,
                                       CountOrders = grp.Count()
                                   }).ToList();

            return Json(new { result = ordersByCountry }, JsonRequestBehavior.AllowGet);
        }

        public List<ProductOrCustomerViewModel> GetProductOrCustomer(string type)
        {
            List<ProductOrCustomerViewModel> result = null;
            using (DashboardContext _contest = new DashboardContext())
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if(type == "customers")
                    {
                        result = _contest.Customers.Select(c => new ProductOrCustomerViewModel
                        {
                            Name = c.CustomerName,
                            Image = c.CustomerImage,
                            TypeOrCountry = c.CustomerCountry,
                            Type = "Customers"
                        }).ToList();
                    }
                    else if(type == "products")
                    {
                        result = _contest.Products.Select(p => new ProductOrCustomerViewModel
                        {
                            Name = p.ProductName,
                            Image = p.ProductImage,
                            TypeOrCountry = p.ProductType,
                            Type = p.ProductType
                        }).ToList();
                    }
                }
                return result;
            }

        }
    }
}