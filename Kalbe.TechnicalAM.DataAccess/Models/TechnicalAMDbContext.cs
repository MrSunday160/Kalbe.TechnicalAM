using Justin.EntityFramework.Model;
using Kalbe.TechnicalAM.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kalbe.TechnicalAM.DataAccess.Models {
    public class TechnicalAMDbContext : BaseDbContext<TechnicalAMDbContext> {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TechnicalAMDbContext(DbContextOptions<TechnicalAMDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor) {

            _httpContextAccessor = httpContextAccessor;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }

        #region Db Setup

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            SetDefaultValues();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
            SetDefaultValues();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            if(modelBuilder != null) {

                _ = modelBuilder.UseIdentityByDefaultColumns();
                _ = modelBuilder.HasPostgresExtension("citext");
                //modelBuilder.Entity<Base>().HasQueryFilter(x => !x.IsDeleted);
                base.OnModelCreating(modelBuilder);

            }
        }

        public override void SetDefaultValues() {

            var entities = ChangeTracker.Entries().Where(x => x.Entity is Base && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            string currUsername = "Anonymous";
            if(_httpContextAccessor.HttpContext != null) {
                // try get username
                var user = _httpContextAccessor.HttpContext.User;
                var userName = user.FindFirst(x => x.Type == "Name");

                if(userName != null)
                    currUsername = userName.Value;

            }

            SwitchState(entities, currUsername);

        }

        public override void SwitchState(IEnumerable<EntityEntry> entities, string currUsername) {
            foreach(EntityEntry entity in entities) {
                switch(entity.State) {
                    case EntityState.Added:
                        ((Base)entity.Entity).CreatedDate = DateTime.Now;
                        ((Base)entity.Entity).IsDeleted = false;
                        ((Base)entity.Entity).CreatedBy = currUsername;
                        break;
                    case EntityState.Modified:
                        if(!((Base)entity.Entity).IsDeleted) {
                            ((Base)entity.Entity).IsDeleted = false;
                        }

                        ((Base)entity.Entity).UpdatedDate = DateTime.Now;
                        ((Base)entity.Entity).UpdatedBy = currUsername;
                        break;
                    case EntityState.Deleted:

                        if(entity.Entity is CartItems) {
                            entity.State = EntityState.Deleted; // hard delete
                            break;
                        }

                        entity.State = EntityState.Modified;
                        ((Base)entity.Entity).IsDeleted = true;
                        break;
                }
            }
        }

        #endregion

            #region DbSet

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }

        #endregion

    }
}
