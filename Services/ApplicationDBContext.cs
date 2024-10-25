using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;

namespace NBE_Project.Services
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
   
        public DbSet<ThirdParty> ThirdParties { get; set; }
        public DbSet<Notifation> Notifications { get; set; }

        public DbSet<Awareness> AwarenessMessages { get; set; }
        public DbSet<Audit> Audit{ get; set; }
        public DbSet<NDA> NDAs { get; set; }
        public DbSet<ChangeRequest> ChangeRequests { get; set; }
        public DbSet<RegulationCompliance> RegulationCompliances { get; set; }
        public DbSet<SecurityAssurance> SecurityAssurances { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Clause> Clauses { get; set; }
        public DbSet<Questinare> questinares { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            //modelBuilder.Entity<RegulationCompliance>()
            //    .HasOne(rc => rc.ThirdPartyId)
            //    .WithMany(tp => tp.RegulationCompliances)
            //    .HasForeignKey(rc => rc.ThirdPartyId);

            base.OnModelCreating(modelBuilder);
     
            modelBuilder.Entity<ThirdParty>()
                .Ignore(t => t.ContractFile);

            modelBuilder.Entity<Incident>()
           .HasKey(i => i.Id); // Configuring primary key

            modelBuilder.Entity<NDA>()
          .HasKey(i => i.Id); // Configuring primary key

          //  modelBuilder.Entity<Awareness>()
          //.HasOne(a => a.VendorName)
          //.WithMany()
          //.HasForeignKey(a => a.VendorId);

        }
    }



}