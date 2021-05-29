using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class Concerts
    {
        public Concerts()
        {
            ConcertToBand = new HashSet<ConcertToBand>();
            Tickets = new HashSet<Tickets>();
        }
        public int ConcertId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Адресс")]
        public int ConcertAdressId { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Дата и время проведения")]
        public DateTime Date { get; set; }
        [Display(Name = "Адресс")]
        public virtual ConcertAdresses ConcertAdress { get; set; }
        public virtual ICollection<ConcertToBand> ConcertToBand { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
