namespace Academy.Application.Common.Helpers
{
    public static class GenerateOrderNumber
    {
        public async static Task<string> GenerateOrderNumberByOrderType(string orderType, int number)
        {
            var orderNumber = orderType.Remove(2).ToUpper() + number.ToString("D4");
            return orderNumber;
        }
    }
}
