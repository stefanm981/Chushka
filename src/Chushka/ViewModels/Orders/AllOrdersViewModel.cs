using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chushka.Web.ViewModels.Orders
{
    public class AllOrdersViewModel
    {
        public AllOrdersViewModel()
        {
            this.Orders = new List<OrderDto>();
        }

        public ICollection<OrderDto> Orders { get; set; }
    }
}