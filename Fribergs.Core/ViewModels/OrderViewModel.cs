using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using Fribergs.Core.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fribergs.Core.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Display(Name = "Från")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Till")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        [BindNever]
        public List<SelectListItem> Cars { get; set; } = new List<SelectListItem>();
        public decimal PricePerDay { get; set; }
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; }
        public string CarName { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CarDescription { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string UserId { get; set; } = "";
        
    }
}
