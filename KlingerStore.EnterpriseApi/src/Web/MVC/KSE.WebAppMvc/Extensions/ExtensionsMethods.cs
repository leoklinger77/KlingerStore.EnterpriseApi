using System.Linq;

namespace KSE.WebAppMvc.Extensions
{
    public static class ExtensionsMethods
    {
        public static string FirtName(this string name)
        {
            string[] array = name.Split(' ').ToArray();
            return array[0];
        }
        public static string LastName(this string name)
        {
            string[] array = name.Split(' ').ToArray();
            name = "";
            for (int i = 1; i < array.Length; i++)            
                name += array[i] + " ";
            
            return name;
        }
    }
}
