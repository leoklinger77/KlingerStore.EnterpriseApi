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

        public static string SelectInstallments(this RazorPage page, decimal total, int valueSelect = 0)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 12; i++)
            {
                var select = "";
                if (i == valueSelect) select = "selected";

                switch (i)
                {
                    case 1:
                    case 3:
                    case 2:
                        sb.Append($"<option {select} value={i}>{i} X Sem Juros</option>");
                        break;
                    default:
                        sb.Append($"<option {select} value={i}>{i} X Com Juros {InstallmentCalc(total, i).ToString("C")}</option>");
                        break;
                }
                
            }
            return sb.ToString();
        }

        private static decimal InstallmentCalc(decimal value, int quantity)
        {

            double cost = 0;
            decimal @return = 0;

            switch (quantity)
            {
                case 1:
                    cost = 0.0449;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 2:
                    cost = 0.0587;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 3:
                    cost = 0.0656;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 4:
                    cost = 0.0725;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 5:
                    cost = 0.0794;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 6:
                    cost = 0.0863;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 7:
                    cost = 0.0932;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 8:
                    cost = 0.1001;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 9:
                    cost = 0.1070;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 10:
                    cost = 0.1139;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 11:
                    cost = 0.1208;
                    @return = value * decimal.Parse(cost.ToString());
                    break;
                case 12:
                    cost = 0.1277;
                    @return = value * decimal.Parse(cost.ToString());
                    break;                
            }

            return @return + value;
        }

        public static string UniyPerProductTotalValue(this RazorPage page, int quantity, decimal value)
        {
            return $"R$ {quantity * value}";
        }

        public static string ViewOrderStatus(this RazorPage page, int status)
        {
            var statusMensagem = "";
            var statusClasse = "";

            switch (status)
            {
                case 1:
                    statusClasse = "info";
                    statusMensagem = "Em aprovação";
                    break;
                case 2:
                    statusClasse = "primary";
                    statusMensagem = "Aprovado";
                    break;
                case 3:
                    statusClasse = "danger";
                    statusMensagem = "Recusado";
                    break;
                case 4:
                    statusClasse = "success";
                    statusMensagem = "Entregue";
                    break;
                case 5:
                    statusClasse = "warning";
                    statusMensagem = "Cancelado";
                    break;

            }

            return $"<span class='badge badge-{statusClasse}'>{statusMensagem}</span>";
        }
    }
}
