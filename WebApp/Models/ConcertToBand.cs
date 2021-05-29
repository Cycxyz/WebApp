using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class ConcertToBand
    {
        public int ConcertToBandId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Концерт")]
        public int ConcertId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Группа")]
        public int BandId { get; set; }
       // [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Группа")]
        public virtual Bands Band { get; set; }
        //[Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Концерт")]
        public virtual Concerts Concert { get; set; }
    }
}
