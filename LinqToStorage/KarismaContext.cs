using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace LinqToStorage
{
  class KarismaContext : DbContext
  {
    public KarismaContext(string connection)
      : base(connection)
    {
      Templates = new KarismaTemplates(this);
    }

    public DbSet<PatientRecord> Patients { get; set; }
    //public DbSet<Request> Requests { get; set; }
    //public DbSet<Report> Reports { get; set; }

    public DbSet<WorkListReport> WorkListReport { get; set; }

    public KarismaTemplates Templates { get; private set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      //modelBuilder.Conventions.Remove<KeyDiscoveryConvention>();
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      modelBuilder.Conventions.Add(new StorageKeyDiscoveryConvention());
      //modelBuilder.Conventions.Add(new NavigationPropertyNameForeignKeyDiscoveryConvention());
      //modelBuilder.Conventions.Add(new StorageNavigationPropertyNameForeignKeyDiscoveryConvention());

      modelBuilder.HasDefaultSchema("Current");

      modelBuilder.Entity<WorkListReport>().ToTable("Karisma.WorkList.Report", "Virtual");

      modelBuilder.Entity<ContactInstance>().ToTable("[Karisma.Contact.Instance]");
      modelBuilder.Entity<ContactAddress>().ToTable("[Karisma.Contact.Address]");
      modelBuilder.Entity<PatientRecord>().ToTable("[Karisma.Patient.Record]");
      /*modelBuilder.Entity<PatientRecord>()
        .HasOptional<PatientName>(P => P.PreferredName).WithOptionalDependent()
        .Map(m => m.MapKey("PreferredNameKey"));*/
      modelBuilder.Entity<PatientName>().ToTable("[Karisma.Patient.Name]");
      modelBuilder.Entity<PatientIdentifier>().ToTable("[Karisma.Patient.Identifier]");
      /*modelBuilder.Entity<PatientIdentifier>()
        .HasRequired(PI => PI.Type).WithRequiredDependent()
        .Map(m => m.MapKey("PatientIdentifierTypeKey"));*/
      modelBuilder.Entity<PatientIdentifierType>().ToTable("[Karisma.Patient.IdentifierType]");
    }
  }

  class KarismaTemplates
  {
    public KarismaTemplates(KarismaContext context)
    {
      this.context = context;
    }

    public DbQuery<PatientRecord> PatientProperties
    {
      get
      {
        return context.Patients
          .Include("PreferredName")
          .Include("Identifier");
      }
    }

    private KarismaContext context;
  }

  class KarismaSampleInitialiser : DropCreateDatabaseAlways<KarismaContext>
  {
    protected override void Seed(KarismaContext context)
    {
      context.Patients.Add(new PatientRecord()
      {
        PreferredName = new PatientName() { FirstName = "Gavin", LastName = "Kestral" },
        Identifier = new[] {
          new PatientIdentifier() { PatientIdentifierType = new PatientIdentifierType() { Code = "MC" }, Value = "1234 567890 1-2" }
        }
      });
      context.SaveChanges();
    }
  }

  class StorageKeyDiscoveryConvention : KeyDiscoveryConvention
  {
    protected override System.Collections.Generic.IEnumerable<System.Data.Entity.Core.Metadata.Edm.EdmProperty> MatchKeyProperty(
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType,
      System.Collections.Generic.IEnumerable<System.Data.Entity.Core.Metadata.Edm.EdmProperty> primitiveProperties)
    {
      return new[] { System.Data.Entity.Core.Metadata.Edm.EdmProperty.CreatePrimitive("Key", System.Data.Entity.Core.Metadata.Edm.PrimitiveType.GetEdmPrimitiveType(System.Data.Entity.Core.Metadata.Edm.PrimitiveTypeKind.Int32)) };
    }
  }

  class StorageNavigationPropertyNameForeignKeyDiscoveryConvention : NavigationPropertyNameForeignKeyDiscoveryConvention
  {
    protected override bool MatchDependentKeyProperty(
      System.Data.Entity.Core.Metadata.Edm.AssociationType associationType,
      System.Data.Entity.Core.Metadata.Edm.AssociationEndMember dependentAssociationEnd,
      System.Data.Entity.Core.Metadata.Edm.EdmProperty dependentProperty,
      System.Data.Entity.Core.Metadata.Edm.EntityType principalEntityType,
      System.Data.Entity.Core.Metadata.Edm.EdmProperty principalKeyProperty)
    {
      return base.MatchDependentKeyProperty(associationType, dependentAssociationEnd, dependentProperty, principalEntityType, principalKeyProperty);
    }
    protected override bool SupportsMultipleAssociations
    {
      get
      {
        return base.SupportsMultipleAssociations;
      }
    }
    public override void Apply(System.Data.Entity.Core.Metadata.Edm.AssociationType item, DbModel model)
    {
      base.Apply(item, model);
    }
  }
}