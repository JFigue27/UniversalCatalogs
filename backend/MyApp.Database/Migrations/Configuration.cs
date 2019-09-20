namespace MyApp.Database.Migrations
{
    using MyApp.Logic.Entities;
    using Reusable.CRUD.Entities;
    using System;
    using System.Collections.Generic;
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
