using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using TopLearn.DataLayer.Entities.Cource;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public TopLearnContext(DbContextOptions<TopLearnContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("TopLearnConnection"));
        }



        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion


        #region Wallet

        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion



        #region Permission

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion


        #region Course

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseGroup> CourseGroup { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public DbSet<CourseVote> CourseVotes { get; set; }

        #endregion


        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Descount> Descounts { get; set; }

        #endregion




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<CourseGroup>()
                .HasQueryFilter(g => !g.IsDelete);



            modelBuilder.Entity<Course>()
             .HasOne<CourseGroup>(f => f.CourseGroup)
             .WithMany(g => g.Courses)
             .HasForeignKey(f => f.GroupId)
             .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Course>()
               .HasOne<CourseGroup>(f => f.Group)
               .WithMany(g => g.SubGroup)
               .HasForeignKey(f => f.SubGroup)
               .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }


    }
}
