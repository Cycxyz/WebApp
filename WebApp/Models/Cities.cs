using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class Cities
    {
        public Cities()
        {
            ConcertAdresses = new HashSet<ConcertAdresses>();
        }

        public int CityId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name="Город")]
        public string Name { get; set; }
        
        [Display(Name="Информация про город")]

        public virtual ICollection<ConcertAdresses> ConcertAdresses { get; set; }
    }
}
