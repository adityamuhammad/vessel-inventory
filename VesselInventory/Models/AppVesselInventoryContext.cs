namespace VesselInventory.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppVesselInventoryContext : DbContext
    {
        public AppVesselInventoryContext()
            : base("name=AppVesselInventoryContext")
        {
        }

        public virtual DbSet<DocSequence> DocSequence { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemDimension> ItemDimension { get; set; }
        public virtual DbSet<ItemGroup> ItemGroup { get; set; }
        public virtual DbSet<LookupValue> LookupValue { get; set; }
        public virtual DbSet<RequestForm> RequestForm { get; set; }
        public virtual DbSet<RequestFormItem> RequestFormItem { get; set; }
        public virtual DbSet<Ship> Ship { get; set; }
        public virtual DbSet<ShipInitial> ShipInitial { get; set; }
        public virtual DbSet<Uom> Uom { get; set; }
        public virtual DbSet<VesselGoodIssued> VesselGoodIssued { get; set; }
        public virtual DbSet<VesselGoodIssuedItem> VesselGoodIssuedItem { get; set; }
        public virtual DbSet<VesselGoodJournal> VesselGoodJournal { get; set; }
        public virtual DbSet<VesselGoodReceive> VesselGoodReceive { get; set; }
        public virtual DbSet<VesselGoodReceiveItem> VesselGoodReceiveItem { get; set; }
        public virtual DbSet<VesselGoodReceiveItemReject> VesselGoodReceiveItemReject { get; set; }
        public virtual DbSet<VesselGoodReturn> VesselGoodReturn { get; set; }
        public virtual DbSet<VesselGoodReturnItem> VesselGoodReturnItem { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestForm>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<RequestForm>()
                .HasMany(e => e.RequestFormItem)
                .WithRequired(e => e.RequestForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestFormItem>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<RequestFormItem>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<VesselGoodIssued>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<VesselGoodIssued>()
                .HasMany(e => e.VesselGoodIssuedItem)
                .WithRequired(e => e.VesselGoodIssued)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VesselGoodIssuedItem>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodJournal>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodJournal>()
                .Property(e => e.SyncStatus)
                .IsFixedLength();

            modelBuilder.Entity<VesselGoodReceive>()
                .Property(e => e.SyncStatus)
                .IsFixedLength();

            modelBuilder.Entity<VesselGoodReceive>()
                .HasMany(e => e.VesselGoodReceiveItemReject)
                .WithRequired(e => e.VesselGoodReceive)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VesselGoodReceive>()
                .HasMany(e => e.VesselGoodReceiveItem)
                .WithRequired(e => e.VesselGoodReceive)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VesselGoodReceiveItem>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodReceiveItemReject>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodReturn>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<VesselGoodReturn>()
                .HasMany(e => e.VesselGoodReturnItem)
                .WithRequired(e => e.VesselGoodReturn)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VesselGoodReturnItem>()
                .Property(e => e.Qty)
                .HasPrecision(10, 2);
        }
    }
}
