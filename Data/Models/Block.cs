using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OP2_L1_17.Data.Models
{
    public class Block
    {
        public BlockColor TopSideColor { get; set; }
        public BlockColor BottomSideColor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsTopVisited { get; set; }
        public bool IsBotVisited { get; set; }
        public bool IsTopBiggest { get; set; }
        public bool IsBottomBiggest { get; set; }

        public Block(int x, int y)
        {
            IsTopVisited = false;
            IsTopBiggest = false;
            IsBottomBiggest = false;
            X = x;
            Y = y;
        }

        public bool IsTopSameColor(BlockColor color)
        {
            return TopSideColor == color;
        }

        public bool IsBottomSameColor(BlockColor color)
        {
            return BottomSideColor == color;
        }

        public bool IsBlockSameColor()
        {
            return TopSideColor.Equals(BottomSideColor);
        }
        public string GetTopText()
        {
            return IsTopBiggest ? "X" : "";
        }

        public string GetBotText()
        {
            return IsBottomBiggest ? "X" : "";
        }
    }
}
