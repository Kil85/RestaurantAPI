﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RestaurantAPI.Models
{
    public class UpdateClass
    {
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
        public bool? HasDelivery { get; set; }
    }
}
