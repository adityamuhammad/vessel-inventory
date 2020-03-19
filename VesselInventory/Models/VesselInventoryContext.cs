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

        public virtual DbSet<doc_sequence> doc_sequence { get; set; }
        public virtual DbSet<item> items { get; set; }
        public virtual DbSet<item_dimension> item_dimension { get; set; }
        public virtual DbSet<item_group> item_group { get; set; }
        public virtual DbSet<lookup_value> lookup_value { get; set; }
        public virtual DbSet<rf> rfs { get; set; }
        public virtual DbSet<rf_item> rf_item { get; set; }
        public virtual DbSet<ship> ships { get; set; }
        public virtual DbSet<ship_initial> ship_initial { get; set; }
        public virtual DbSet<uom> uoms { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<vessel_good_issued> vessel_good_issued { get; set; }
        public virtual DbSet<vessel_good_issued_item> vessel_good_issued_item { get; set; }
        public virtual DbSet<vessel_good_journal> vessel_good_journal { get; set; }
        public virtual DbSet<vessel_good_receive> vessel_good_receive { get; set; }
        public virtual DbSet<vessel_good_receive_item> vessel_good_receive_item { get; set; }
        public virtual DbSet<vessel_good_receive_item_reject> vessel_good_receive_item_reject { get; set; }
        public virtual DbSet<vessel_good_return> vessel_good_return { get; set; }
        public virtual DbSet<vessel_good_return_item> vessel_good_return_item { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rf>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<rf_item>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<rf_item>()
                .Property(e => e.remarks)
                .IsUnicode(false);

            modelBuilder.Entity<vessel_good_issued>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<vessel_good_issued_item>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<vessel_good_journal>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<vessel_good_receive>()
                .Property(e => e.sync_status)
                .IsFixedLength();

            modelBuilder.Entity<vessel_good_receive_item>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<vessel_good_receive_item_reject>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);

            modelBuilder.Entity<vessel_good_return_item>()
                .Property(e => e.qty)
                .HasPrecision(10, 2);
        }
    }
}
