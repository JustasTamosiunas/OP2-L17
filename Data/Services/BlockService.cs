using System;
using System.Collections.Generic;
using System.Linq;

using OP2_L1_17.Data.Models;

namespace OP2_L1_17.Data.Services
{
    using System.Diagnostics;
    using System.IO;

    public static class BlockService
    {
        public static BlockContainer GenerateBlocks(int height, int width)
        {
            var container = new BlockContainer(height, width);
            var rng = new Random();
            var values = Enum.GetValues(typeof(BlockColor));

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var block = new Block(i, j);
                    block.TopSideColor = (BlockColor) values.GetValue(rng.Next(values.Length));
                    block.BottomSideColor = (BlockColor)values.GetValue(rng.Next(values.Length));
                    container.AddBlock(i, j, block);
                }
            }

            FindBiggestArea(container);

            return container;
        }

        public static void FindBiggestArea(BlockContainer container)
        {
            List<Area> areas = new List<Area>();
            for (int i = 0; i < container.Height; i++)
            {
                for (int j = 0; j < container.Width; j++)
                {
                    var topArea = new List<Block>();
                    var bottomArea = new List<Block>();
                    var block = container.GetBlock(i, j);
                    if (!block.IsTopVisited)
                    {
                        FindArea(block, block.TopSideColor, topArea, container);
                    }

                    if (!block.IsBotVisited)
                    {
                        FindArea(block, block.BottomSideColor, bottomArea, container, true);
                    }

                    if (topArea.Any())
                    {
                        areas.Add(new Area{BottomArea = new List<Block>(), TopArea = topArea});
                    }

                    if (bottomArea.Any())
                    {
                        areas.Add(new Area { TopArea = new List<Block>(), BottomArea = bottomArea });
                    }
                }
            }

            areas.MergeAreas();

            var biggestArea = areas.Aggregate(
                (curMin, x) => (curMin.AreaSize == 0 || x.AreaSize > curMin.AreaSize ? x : curMin));
            biggestArea.TopArea.ForEach(x => x.IsTopBiggest = true);
            biggestArea.BottomArea.ForEach(x => x.IsBottomBiggest = true);
        }

        public static void FindArea(Block block, BlockColor targetColor, List<Block> matchingBlocks, BlockContainer container, bool bottom = false)
        {
            if (bottom == false)
            {
                if (!block.IsTopSameColor(targetColor) || block.IsTopVisited || matchingBlocks.Contains(block))
                {
                    return;
                }

                block.IsTopVisited = true;
            }
            else
            {
                if (!block.IsBottomSameColor(targetColor) || block.IsBotVisited || matchingBlocks.Contains(block))
                {
                    return;
                }

                block.IsBotVisited = true;
            }

            matchingBlocks.Add(block);

            if (block.X > 0)
            {
                FindArea(container.GetBlock(block.X - 1, block.Y), targetColor, matchingBlocks, container, bottom);
            }

            if (block.Y > 0)
            {
                FindArea(container.GetBlock(block.X, block.Y - 1), targetColor, matchingBlocks, container, bottom);
            }

            if (block.X < container.Height - 1)
            {
                FindArea(container.GetBlock(block.X + 1, block.Y), targetColor, matchingBlocks, container, bottom);
            }

            if (block.Y < container.Width - 1)
            {
                FindArea(container.GetBlock(block.X, block.Y + 1), targetColor, matchingBlocks, container, bottom);
            }
        }

        private static void MergeAreas(this List<Area> areas)
        {
            foreach (var area in areas)
            {
                var mergesDone = true;
                while (mergesDone)
                {
                    mergesDone = false;
                    var sameColorBlocksTop =
                        area.TopArea.Where(x => x.IsBlockSameColor() && !area.BottomArea.Contains(x));

                    var areasToMergeBot = areas.Where(x => sameColorBlocksTop.Any(c => x.BottomArea.Contains(c)));

                    foreach (var areaToMerge in areasToMergeBot)
                    {
                        area.BottomArea.AddRange(areaToMerge.BottomArea);
                        areaToMerge.BottomArea.Clear();
                        mergesDone = true;
                    }

                    var sameColorBlocksBot =
                        area.BottomArea.Where(x => x.IsBlockSameColor() && !area.TopArea.Contains(x));

                    var areasToMergeTop = areas.Where(x => sameColorBlocksBot.Any(c => x.TopArea.Contains(c)));

                    foreach (var areaToMerge in areasToMergeTop)
                    {
                        area.TopArea.AddRange(areaToMerge.TopArea);
                        areaToMerge.BottomArea.Clear();
                        mergesDone = true;
                    }
                }
            }

            areas.RemoveAll(x => x.AreaSize == 0);
        }

        public static void GenerateReport(BlockContainer container)
        {
            var lines = new List<string>();
            lines.Add("Virsus");
            for (int i = 0; i < container.Height; i++)
            {
                var tempLine = new string(string.Empty);
                for (int j = 0; j < container.Width; j++)
                {
                    var block = container.GetBlock(i, j);
                    if (block.IsTopBiggest)
                    {
                        tempLine += "X ";
                        continue;
                    }
                    switch (block.TopSideColor)
                    {
                        case BlockColor.Green:
                            tempLine += "G ";
                            break;
                        case BlockColor.Red:
                            tempLine += "R ";
                            break;
                        case BlockColor.Yellow:
                            tempLine += "Y ";
                            break;
                    }
                }
                lines.Add(tempLine);
            }
            lines.Add(string.Empty);
            lines.Add("Apacia");
            for (int i = 0; i < container.Height; i++)
            {
                var tempLine = new string(string.Empty);
                for (int j = 0; j < container.Width; j++)
                {
                    var block = container.GetBlock(i, j);
                    if (block.IsBottomBiggest)
                    {
                        tempLine += "X ";
                        continue;
                    }
                    switch (block.BottomSideColor)
                    {
                        case BlockColor.Green:
                            tempLine += "G ";
                            break;
                        case BlockColor.Red:
                            tempLine += "R ";
                            break;
                        case BlockColor.Yellow:
                            tempLine += "Y ";
                            break;
                    }
                }
                lines.Add(tempLine);
            }
            lines.Add(string.Empty);
            lines.Add($"Didziausia plota sudaro virsuje {container.TopAreaSize()} langeliu ir apacioje {container.BottomAreaSize()} langeliu.");
            var joiningBlock = container.GetJoiningBlock();
            lines.Add($"Vienas is jungianciu langeliu yra {joiningBlock.X} st., ir {joiningBlock.Y} eil.");
            File.WriteAllLines(Environment.CurrentDirectory + "\\rez.txt", lines);
        }
    }
}
