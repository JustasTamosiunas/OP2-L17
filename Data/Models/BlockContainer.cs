using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OP2_L1_17.Data.Models
{
    public class BlockContainer
    {
        private readonly Block[,] _blocks;
        public int Height { get;  }
        public int Width { get; }

        public BlockContainer(int height, int width) { 
            _blocks = new Block[height, width];
            this.Height = height;
            this.Width = width;
        }

        public void AddBlock(int x, int y, Block block)
        {
            _blocks[x, y] = block;
        }

        public Block GetBlock(int x, int y)
        {
            return _blocks[x, y];
        }

        public int TopAreaSize()
        {
            var count = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (GetBlock(i, j).IsTopBiggest)
                        count++;
                }
            }

            return count;
        }

        public int BottomAreaSize()
        {
            var count = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (GetBlock(i, j).IsBottomBiggest)
                        count++;
                }
            }

            return count;
        }

        public Block GetJoiningBlock()
        {
            for (int i = 0; i<Height; i++)
            {
                for (int j = 0; j<Width; j++)
                {
                    var block = GetBlock(i, j);
                    if (block.IsTopBiggest && block.IsBlockSameColor())
                        return new Block(i+1, j+1);
                }
            }

            return null;
        }
    }
}
