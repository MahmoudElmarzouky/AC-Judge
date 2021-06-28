using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class BlogTag
    {
        [Key, Column(Order = 0)]
        public int BlogId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }
        public Blog blog { get; set; }
        public Tag tag { get; set; }

       
    }
}
