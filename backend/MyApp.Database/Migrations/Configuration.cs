namespace MyApp.Database.Migrations
{
    using Reusable.CRUD.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EFContext context)
        {
            ///start:slot:seed<<<///end:slot:seed<<<
        }
    }
}
