using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class ViewCommentModel
    {
        public int commentId { get; set; }
        [MaxLength(50)]
        public string content { get; set; }
        public string commentOwner { get; set; }
        public int vote { get; set; }

        public bool isOwner { get; set; }
        public DateTime creationTime { get; set; }
       
    }
}
