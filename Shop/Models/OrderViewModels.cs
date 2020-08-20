using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ConfirmViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не введена дата доставки")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата доставки")]
        public DateTime ShipmentDate { get; set; }
    }
}