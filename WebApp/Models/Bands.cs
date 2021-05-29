using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class Bands
    {
        public Bands()
        {
            BandToStyle = new HashSet<BandToStyle>();
            ConcertManToBand = new HashSet<ConcertManToBand>();
            ConcertToBand = new HashSet<ConcertToBand>();
        }
        public int BandId { get; set; }
        [Required(ErrorMessage ="Поле не должно быть пустым")]
        [Display(Name="Название группы")]
        public string Name { get; set; }

        public virtual ICollection<BandToStyle> BandToStyle { get; set; }
        public virtual ICollection<ConcertManToBand> ConcertManToBand { get; set; }
        public virtual ICollection<ConcertToBand> ConcertToBand { get; set; }
    }
}
