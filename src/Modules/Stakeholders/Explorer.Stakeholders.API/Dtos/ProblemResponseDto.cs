﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProblemResponseDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string ReportedTime { get; set; }
        public int TouristId { get; set; }
        public int TourId { get; set; }
    }
}
