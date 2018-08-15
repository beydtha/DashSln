using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoradApp.ViewModels
{
    public class ProductsViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPriceProduct { get; set; }
        public int UnitsInStock { get; set; }
        public string ProductImage { get; set; }
        public string ProductType { get; set; }
        public int QteSelected { get; set; }
    }
}