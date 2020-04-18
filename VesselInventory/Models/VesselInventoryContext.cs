namespace VesselInventory.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VesselInventoryContext : DbContext
    {
        public VesselInventoryContext()
            : base("name=VesselInventoryContext")
        {
        }

        public virtual DbSet<Item> items { get; set; }
        public virtual DbSet<Uom> uom { get; set; }
        public virtual DbSet<RequestForm> request_form { get; set; }
        public virtual DbSet<RequestFormItem> request_form_item { get; set; }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<VesselGoodIssued> vessel_good_issued { get; set; }
        public virtual DbSet<VesselGoodIssuedItem> vessel_good_issued_item { get; set; }
        public virtual DbSet<VesselGoodJournal> vessel_good_journal { get; set; }
        public virtual DbSet<VesselGoodReceive> vessel_good_receive { get; set; }
        public virtual DbSet<VesselGoodReceiveItem> vessel_good_receive_item { get; set; }
        public virtual DbSet<VesselGoodReceiveItemReject> vessel_good_receive_item_reject { get; set; }
        public virtual DbSet<VesselGoodReturn> vessel_good_return { get; set; }
        public virtual DbSet<VesselGoodReturnItem> vessel_good_return_item { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestForm>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<RequestFormItem>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<RequestFormItem>()
                .Property(e => e.remarks)
                .IsUnicode(false);

            modelBuilder.Entity<VesselGoodIssued>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<VesselGoodIssuedItem>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodJournal>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodReceive>()
                .Property(e => e.sync_status)
                .IsFixedLength();

            modelBuilder.Entity<VesselGoodReceiveItem>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodReceiveItemReject>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VesselGoodReturnItem>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);
        }
    }
}
