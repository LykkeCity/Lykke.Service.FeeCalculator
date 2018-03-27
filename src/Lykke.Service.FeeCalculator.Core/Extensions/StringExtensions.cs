namespace Lykke.Service.FeeCalculator.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateNextId(this string id)
        {
            return string.IsNullOrEmpty(id) 
                ? null 
                : $"{id.Substring(0, id.Length - 1)}{(char) ((int) id[id.Length - 1] + 1)}";
        }
    }
}
