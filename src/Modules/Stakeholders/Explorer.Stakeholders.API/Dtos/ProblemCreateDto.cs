﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProblemCreateDto
    {
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public long TouristId { get; set; }
        public int TourId { get; set; }
    }
}