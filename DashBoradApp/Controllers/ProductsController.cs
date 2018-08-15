using DashBoradApp.DataAccess;
using DashBoradApp.DataAccess.Entitites;
using DashBoradApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoradApp.Controllers
{
    public class ProductsController : Controller
    {

        public static List<ProductsViewModel> productList = new List<ProductsViewModel>(); 

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductList(int id) // ProductList 
        {
            Session["CustomerId"] = id;
            return View(); 
        }
        [HttpGet]
        public ActionResult GetProductByCategory(string category)
        {
            using (DashboardContext _context = new DashboardContext())
            {
                List<ProductsViewModel> productList = _context.Products
                    .Where(x => x.ProductType.ToLower().Equals(category.ToLower()))
                    .Select(x => new ProductsViewModel
                    {
                         ProductID = x.ID, 
                         ProductName = x.ProductName, 
                         ProductImage = x.ProductImage,
                         ProductType = x.ProductType, 
                         UnitPriceProduct = x.UnitPrice, 
                         UnitsInStock = x.UnitsInStock
                    }).ToList();

              
                return PartialView("~/Views/Products/GetProductByCategory.cshtml", productList);  
            }
        }
        [HttpPost]
        public ActionResult ShoppingCart(ProductsViewModel product)
        {
            string message = string.Empty; 
            if(product != null)
            {
                productList.Add(product);
                message = "Product has successsfully added !"; 
            }
            else
            {
                message = "something Wrong ... !"; 
            }

            return Json(new { message = message}, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult DisplayShoppingCart()
        {
            List<ProductsViewModel> myShoppingcart = productList;
            return PartialView("~/Views/Products/DisplayShoppingCart.cshtml", myShoppingcart); 
        }

        [HttpPost]
        public ActionResult AddOrder(int[] arrIdProduct, int[] arrQteProduct)
        {
            int countProduct = arrIdProduct.Length;
            int customerId = (int)Session["CustomerId"];
            bool statusTran = false;

            DashboardContext _context = new DashboardContext();
            using (DbContextTransaction dbTran = _context.Database.BeginTransaction())
            {
                try
                {

                    Customer customer = _context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        customer.Orders.Add(new Order { OrderDate = DateTime.Now });
                    }
                    Order orderSelected = customer.Orders.LastOrDefault();
                    if (orderSelected != null)
                    {
                        for (int i = 0; i < countProduct; i++)
                        {
                            Product SelectedProduct = _context.Products.Find(arrIdProduct[i]);
                            orderSelected.OrderDetail.Add(new OrderDetails { Product = SelectedProduct, Quatity = arrQteProduct[i] });
                        }
                    }

                    //Save Changes  
                    int countResult = _context.SaveChanges();

                    //Commit Transaction  
                    dbTran.Commit();

                    if (countProduct > 0)
                    {
                        statusTran = true;
                        productList.Clear();
                    }
                }

                catch (Exception)
                {

                    dbTran.Rollback();
                }
            }
            return Json(new { data = statusTran }, JsonRequestBehavior.AllowGet);
        }
    }
}