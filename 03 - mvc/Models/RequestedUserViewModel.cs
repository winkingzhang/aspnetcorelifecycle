using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _03___mvc.Models
{
    public class RequestedUserViewModel
    {
        [Required(ErrorMessage = "Required user Id")]
        [Range(1, 1000, ErrorMessage = "Must be number in [1,1000]")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required user's Name")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Required server shutdown reason")]
        public string Reason { get; set; }
    }
}
