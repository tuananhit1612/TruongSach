namespace TruongSach_API.DTOs
{
    public class DashboardAdminDTO
    {
        public int TotalCampaigns { get; set; }
        public int ActiveCampaigns { get; set; }
        public decimal TotalDonations { get; set; }
        public int DonationCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
    }
}
