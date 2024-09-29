namespace NBE_Project.Models
{
    public class TransactionLog
    {
        public int Id { get; set; } // Corresponds to the primary key, auto-incrementing identity column

        public string TableName { get; set; } // Corresponds to the table name involved in the transaction

        public string Username { get; set; } = "";// The user who performed the action

        public string Role { get; set; } = "";// The role of the user who performed the action

        public string CommandType { get; set; } // The type of command, e.g., INSERT, UPDATE, DELETE

        public string ActionType { get; set; } = "";     // Custom type of action, like Add Regulation, Send Awareness

        public string ChangeSummary { get; set; } = "";// A summary of what changed

        public DateTime ChangeDate { get; set; } = DateTime.Now; // Default value is the current date and time
    }
}
