using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PtcApi.Model
{
    [Table("User", Schema = "Security")]
    public  partial class AppUser
    {
        [Required()]
        [Key()]
        public Guid UserId { get; set; }

        [Required()]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required()]
        [StringLength(255)]
        public string Password { get; set; }

        [Required()]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required()]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required()]
        [StringLength(255)]
        public string Address1 { get; set; }

        [StringLength(255)]
        public string Address2 { get; set; }

        [Required()]
        [StringLength(255)]
        public string Suburb { get; set; }

        [Required()]
        public int SuburbCode { get; set; }

        [Required()]
        [StringLength(255)]
        public string State { get; set; }

        [Required()]
        [StringLength(255)]
        public string Country { get; set; }

        [Required()]
        [StringLength(200)]
        public string Email { get; set; }

        [Required()]
        [StringLength(20)]
        public string Mobile { get; set; }
    }
}