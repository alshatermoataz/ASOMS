namespace ASOMS.Cms.Services.AnalyticsInterface
{
    public interface IAnalyticsService
    {
        Task<object> GetDashboardSummaryAsync(string period);
        Task<List<object>> GetCustomerSegmentsAsync();
        Task<List<object>> GetTopProductsAsync(string period);
        Task<List<object>> GetCategorySalesAsync(string period);
        Task<List<object>> GetRevenueTrendAsync(string period);
        Task<object> GetOrdersOverviewAsync(string period);
    }
}
