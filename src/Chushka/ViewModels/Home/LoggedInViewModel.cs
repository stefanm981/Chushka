using System.Collections.Generic;

namespace Chushka.Web.ViewModels.Home
{
    public class LoggedInViewModel
    {
        public LoggedInViewModel()
        {
            this.Products = new List<ProductIndexDto>();
        }

        public ICollection<ProductIndexDto> Products { get; set; }
    }
}