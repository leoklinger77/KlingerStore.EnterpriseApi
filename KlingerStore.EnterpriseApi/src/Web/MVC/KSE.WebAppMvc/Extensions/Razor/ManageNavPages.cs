using Microsoft.AspNetCore.Mvc.Rendering;

namespace KSE.WebAppMvc.Extensions.Razor
{
    public static class ManageNavPages
    {
        public static string MyOrders => "V1.Controllers.ClientController.MyOrders";        
        public static string ReturnsAndRefunds => "V1.Controllers.ClientController.ReturnsAndRefunds";        
        public static string FavoriteProduct => "V1.Controllers.ClientController.FavoriteProduct";        
        public static string Profile => "V1.Controllers.ClientController.Profile";        
        public static string Address => "V1.Controllers.ClientController.Address";        
        public static string Security => "V1.Controllers.ClientController.Security";

        public static string MyOrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyOrders);
        public static string ReturnsAndRefundsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ReturnsAndRefunds);
        public static string FavoriteProductNavClass(ViewContext viewContext) => PageNavClass(viewContext, FavoriteProduct);
        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);
        public static string AddressNavClass(ViewContext viewContext) => PageNavClass(viewContext, Address);
        public static string SecurityNavClass(ViewContext viewContext) => PageNavClass(viewContext, Security);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Contains(page) ? "active" : null;
        }
    }
}
