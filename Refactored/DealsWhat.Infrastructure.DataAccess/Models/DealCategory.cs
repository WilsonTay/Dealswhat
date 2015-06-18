﻿using System;
using System.Collections.Generic;

namespace DealsWhat.Models
{
    public class DealCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public List<Deal> Deals { get; set; } 
    }
}