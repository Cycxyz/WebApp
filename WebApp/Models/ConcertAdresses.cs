using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class ConcertAdresses
    {
        public ConcertAdresses()
        {
            Concerts = new HashSet<Concerts>();
        }

        public int ConcertAdressId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Адрес")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Город")]
        public int CityId { get; set; }
        [Display(Name = "Детали")]
        public string Details { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Город")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Concerts> Concerts { get; set; }
    }
}
