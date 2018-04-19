using System.Collections.Generic;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        //Window Size
        public double WindowHeight { get; set; } = 500;
        public double WindowWidth { get; set; } = 800;
        
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
