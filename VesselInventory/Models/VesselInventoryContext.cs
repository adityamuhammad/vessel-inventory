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

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Uom> UOM { get; set; }
        public virtual DbSet<Ship> Ship { get; set; }
        public virtual DbSet<ShipInitial> ShipInitial { get; set; }
        public virtual DbSet<RequestForm> RequestForm { get; set; }
        public virtual DbSet<RequestFormItem> RequestFormItem { get; set; }
        public virtual DbSet<User> User { get; set; }
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
        }
    }
}
