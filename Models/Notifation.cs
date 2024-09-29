namespace NBE_Project.Models
{
    public class Notifation
    {
     
     
            public int Id { get; set; }
            public string? Message { get; set; }
            public DateTime DateCreated { get; set; }
            public bool IsRead { get; set; }
        
    }
}
