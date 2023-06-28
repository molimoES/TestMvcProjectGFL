using System.Data.Entity;

namespace TestMvcProjectGFL.Models
{
    public class CatalogContext : DbContext
    {
        public DbSet<Catalog> Catalogs { get; set; }

        public CatalogContext()
        {
            Database.CreateIfNotExists();
        }
    }

    public class CatalogDbInitializer : DropCreateDatabaseAlways<CatalogContext>
    {
        protected override void Seed(CatalogContext db)
        {
            db.Catalogs.Add(new Catalog { Id = 1, Name = "Creating Digital Images", ParentId = 0 });
            db.Catalogs.Add(new Catalog { Id = 2, Name = "Resources", ParentId = 1 });
            db.Catalogs.Add(new Catalog { Id = 3, Name = "Evidence", ParentId = 1 });
            db.Catalogs.Add(new Catalog { Id = 4, Name = "Graphic Products", ParentId = 1 });
            db.Catalogs.Add(new Catalog { Id = 5, Name = "Primary Sources", ParentId = 2 });
            db.Catalogs.Add(new Catalog { Id = 6, Name = "Secondary Sources", ParentId = 2 });
            db.Catalogs.Add(new Catalog { Id = 7, Name = "Process", ParentId = 4 });
            db.Catalogs.Add(new Catalog { Id = 8, Name = "Final Products", ParentId = 4 });

            base.Seed(db);
        }
    }
}