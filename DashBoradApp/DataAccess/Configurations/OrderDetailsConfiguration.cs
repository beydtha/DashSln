using DashBoradApp.DataAccess.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoradApp.DataAccess.Configurations
{
    public class OrderDetailsConfiguration : EntityConfiguration<OrderDetails>
    {
        public OrderDetailsConfiguration()
        {
            Property(o => o.Quatity).IsRequired();
        }
    }
}