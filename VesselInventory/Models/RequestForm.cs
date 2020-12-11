namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RequestForm")]
    public partial class RequestForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestForm()
        {
            RequestFormItem = new HashSet<RequestFormItem>();
        }

        public int RequestFormId { get; set; }

        [Required]
        public string RequestFormNumber { get; set; }

        public string ProjectNumber { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Column(TypeName = "date")]
        public DateTime TargetDeliveryDate { get; set; }

        [Required]
        public string Status { get; set; }

        public int ShipId { get; set; }

        [Required]
        public string ShipName { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }

        [Required]
        public string SyncStatus { get; set; } 

        public bool IsHidden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestFormItem> RequestFormItem { get; set; }
    }
}
