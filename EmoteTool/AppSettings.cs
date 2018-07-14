using System.Collections.Generic;
using EmoteTool.ViewModels;

namespace EmoteTool
{
    public class AppSettings
    {
        public List<(string name,
                     string imagePath,
                     ItemSizeMode sizeMode)> 
            SavedEmotes { get; set; }
    }
}