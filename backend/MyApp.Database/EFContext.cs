namespace MyApp.Database
{
    using MyApp.Logic.Entities;
    using Reusable.CRUD.Entities;
    using Reusable.CRUD.JsonEntities;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class EFContext : DbContext
    {
        public EFContext()
            : base(ConfigurationManager.AppSettings["dbConnectionString"])
        {
        }

        #region App
        ///start:generated:dbsets<<<
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        ///end:generated:dbsets<<<
        #endregion

        #region Reusable
        public virtual DbSet<AdvancedSort> AdvancedSorts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<FilterData> FilterDatas { get; set; }
        public virtual DbSet<Revision> Revisions { get; set; }
        public virtual DbSet<SortData> SortDatas { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<Contact>();

            ///start:slot:model<<<///end:slot:model<<<
        }
    }
}