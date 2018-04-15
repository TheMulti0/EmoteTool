using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoteTool
{
    internal class MainWindowViewModel
    {
        //Window Size
        public double WindowHeight { get; set; } = 500;
        public double WindowWidth { get; set; } = 300;
        
        public Dictionary<string, int> Numbers { get; set; }

        public MainWindowViewModel()
        {
            Numbers = new Dictionary<string, int>
            {
                {"One", 1},
                {"Two", 2},
            };
        }
    }
}
