using NBE_Project.Models;

namespace NBE_Project.Services
{
    
    public interface INotificationService
        {
            void NotifyKeyResponseTeam(Incident incident);
            void NotifyAboutExpiringNDA(NDA nda);
        void NotifyNewThirdParty(ThirdParty thirdParty);
        void NotifyNDAValidationResult(NDA nda, bool isValid);
        void NotifyComplianceQuestionnaireSubmission(Question questionnaire);
        void NotifyComplianceStatusUpdate(ThirdParty thirdParty, bool isCompliant);
        void NotifyDashboardUpdate();
        void NotifyVendorDataUpload();
        void NotifyNewIncidentReport(Incident incident);
        void NotifyRegulationClauseUpdate(RegulationCompliance clause);
        void NotifyProfileCompletionStatus(ThirdParty profile);
    }

        public class NotificationService : INotificationService
        {
            public void NotifyKeyResponseTeam(Incident incident)
            {
                // Logic to send notification to the key response team
                // This could be an email, SMS, or other form of alert
            }
        public void NotifyAboutExpiringNDA(NDA nda)
        {
            // Logic to notify about the expiring NDA
            // This could involve sending an email or other form of alert
        }

        public void NotifyNewThirdParty(ThirdParty thirdParty)
        {
            var message = $"A new third party has been defined and categorized: {thirdParty.VendorName}. Please review the details.";
            SendEmail("compliance@bank.com", "New Third Party Added", message);
        }

        public void NotifyNDAValidationResult(NDA nda, bool isValid)
        {
            var status = isValid ? "valid" : "invalid";
            //var message = $"NDA document validation complete. The document for {nda.VendorName} is {status}.";
          //  SendEmail("compliance@bank.com", "NDA Validation Result", message);
        }

        public void NotifyComplianceQuestionnaireSubmission(Question questionnaire)
        {
            var message = $"A new compliance questionnaire has been submitted. Please review and provide feedback.";
            SendEmail("compliance@bank.com", "New Compliance Questionnaire", message);
        }

        public void NotifyComplianceStatusUpdate(ThirdParty thirdParty, bool isCompliant)
        {
            var status = isCompliant ? "met" : "failed";
            var message = $"Compliance status update: {thirdParty.VendorName} has {status} the compliance requirements.";
            SendEmail("compliance@bank.com", "Compliance Status Update", message);
        }

        public void NotifyDashboardUpdate()
        {
            var message = "Dashboard has been updated with new data. Check the latest insights and metrics.";
            SendEmail("dashboard@bank.com", "Dashboard Update", message);
        }

        public void NotifyVendorDataUpload()
        {
            var message = "Vendor data has been successfully uploaded or updated.";
            SendEmail("dataupload@bank.com", "Vendor Data Management", message);
        }

        public void NotifyNewIncidentReport(Incident incident)
        {
            var message = $"A new incident report has been filed with severity level {incident.Severity}.";
            SendEmail("incidentmanagement@bank.com", "New Incident Report", message);
        }

        public void NotifyRegulationClauseUpdate(RegulationCompliance clause)
        {
            var message = $"A new clause has been added/updated for regulation {clause.RegulationName}.";
            SendEmail("regulations@bank.com", "Regulation Clause Update", message);
        }

        public void NotifyProfileCompletionStatus(ThirdParty profile)
        {
            var status = profile.IsProfileCompleted ? "Complete" : "Incomplete";
            var message = $"Vendor profile completion status updated. {profile.VendorName}'s profile is now {status}.";
            SendEmail("profiles@bank.com", "Profile Completion Status", message);
        }

        private void SendEmail(string to, string subject, string body)
        {
            // Implement email sending logic here
            // e.g., using SmtpClient or an email service like SendGrid
        }
    }
}


