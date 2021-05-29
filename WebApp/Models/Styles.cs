using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class Styles
    {
        public Styles()
        {
            BandToStyle = new HashSet<BandToStyle>();
        }

        public int StyleId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Стиль")]
        public string Type { get; set; }

        public virtual ICollection<BandToStyle> BandToStyle { get; set; }
    }
}
