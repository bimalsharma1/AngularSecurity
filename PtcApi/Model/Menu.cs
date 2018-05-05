using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace PtcApi.Model
{
  [Table("Menu", Schema = "dbo")]
  public partial class Menu
  {
    public int MenuId { get; set; }

    [Required()]
    [StringLength(80)]
    public string MenuName { get; set; }

    public DateTime? IntroductionDate { get; set; }

    [Column(TypeName = "money")]
    public decimal? Price { get; set; }

    [StringLength(1000)]
    public string DiscountMessage { get; set; }

    [StringLength(1000)]
    public string SurchargeMessage { get; set; }

  
    public System.Drawing MenuPicture { get; set; }
  }
}
