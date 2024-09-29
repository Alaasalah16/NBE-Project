namespace NBE_Project.ViewModels
{
    public class SettingsViewModel
    {
        public string? Theme { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string? Language { get; set; }
        public bool TwoFactorAuthEnabled { get; set; }
        public string? DatabaseConnectionString { get; set; }
        public string? ApiEndpoint { get; set; }
        public string? ComplianceThreshold { get; set; }
    }

}
