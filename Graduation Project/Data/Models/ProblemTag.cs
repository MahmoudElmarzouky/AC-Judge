﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class ProblemTag
    {
        public int ProblemId { get; set; }
        
        public int TagId { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
