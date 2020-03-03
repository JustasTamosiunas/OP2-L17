using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OP2_L1_17.Data.Models
{
    public class Area
    {
        public List<Block> TopArea { get; set; }

        public List<Block> BottomArea { get; set; }

        public int AreaSize => TopArea.Count + BottomArea.Count;
    }
}
