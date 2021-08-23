using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace KSE.WebAppMvc.Extensions.Razor
{
    public static class RazorHelpers
    {
        public static string StockMessage(this RazorPage page, int quantity)
        {
            return quantity > 0 ? $"Apenas {quantity} em estoque!" : "Produto esgotado!";
        }
        public static string CurrencyFormat(this RazorPage page, decimal value)
        {
            return value > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", value) : "Gratuito";
        }
        public static string HasEmailForGravatar(this RazorPage page, string email)
        {
            var data = MD5.Create().ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("X2"));
            }
            return sBuilder.ToString();
        }
        public static string UnityPerProdut(this RazorPage page, int quantity)
        {
            return quantity > 1 ? $"{quantity} unidades" : $"{quantity} unidade";
        }

        public static string SelectOptionPerQuantity(this RazorPage page, int quantity, int valueSelect = 0)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= quantity; i++)
            {
                var select = "";
                if (i == valueSelect) select = "selected";
                sb.Append($"<option {select} value={i}>{i}</option>");
            }
            return sb.ToString();
        }

        public static string UniyPerProductTotalValue(this RazorPage page, int quantity, decimal value)
        {
            return $"R$ {quantity * value}";
        }        
    }
}
