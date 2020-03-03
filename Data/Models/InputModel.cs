using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OP2_L1_17.Data.Models
{
    public class InputModel
    {
        [Required]
        [Range(1, 20, ErrorMessage = "Maximum height is 20 blocks")]
        public int Height { get; set; }

        [Range(1, 30, ErrorMessage = "Maximum width is 30 blocks")]
        [Required]
        public int Width { get; set; }
    }
}
