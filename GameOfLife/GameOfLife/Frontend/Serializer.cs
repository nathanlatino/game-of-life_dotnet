using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife.Frontend
{
    class Serializer
    {
        public static string Serialize(SinglePlayerWin win)
        {
            var sb = new StringBuilder();
            sb.Append($"{win.width}");
            sb.AppendLine();
            sb.Append($"{win.height}");
            sb.AppendLine();
            sb.Append($"{win.speed}");
            sb.AppendLine();
            sb.Append($"{win.cellColor.Color}");
            sb.AppendLine();
            sb.Append($"{SerializeTab(win.gridRect)}");
            return sb.ToString();
        }

        private static string SerializeTab(Rectangle[][] grid)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < grid.Length; i++)
            {
                for(int j = 0; j < grid[i].Length; j++)
                {
                    if(grid[i][j].Fill != Brushes.White)
                    {
                        sb.Append($"{i}:{j}|");
                    }
                }
            }
            return sb.ToString();
        }

        public static void Deserialize(SinglePlayerWin parent, string[] lines)
        {
            try
            {
                parent.width = Convert.ToInt32(lines[0]);
                parent.height = Convert.ToInt32(lines[1]);
                parent.speed = Convert.ToInt32(lines[2]);
                parent.cellColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(lines[3]));
                parent.iudHeigth.Value = parent.height;
                parent.iudWidth.Value = parent.width;
                parent.iudSpeed.Value = parent.speed;
                parent.clpCell.SelectedColor = parent.cellColor.Color;
            }
            catch { }
            try
            {
                DeserializeTab(parent, lines[4]);
            }
            catch { }
            

  
            
        }

        private static void DeserializeTab(SinglePlayerWin parent, string line)
        {
            foreach (string indexes in line.Split('|'))
            {
                var index = indexes.Split(':');
                try
                {
                    int i = Convert.ToInt32(index[0]);
                    int j = Convert.ToInt32(index[1]);
                    parent.gridRect[i][j].Fill = parent.cellColor;
                }
                catch { }


                
            }
        }
    }
}
