using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class ConcertMans
    {
        public ConcertMans()
        {
            ConcertManToBand = new HashSet<ConcertManToBand>();
        }

        public int ConcertManId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        public virtual ICollection<ConcertManToBand> ConcertManToBand { get; set; }
    }
}
