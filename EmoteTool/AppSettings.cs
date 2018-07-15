using System.Collections.Generic;
using EmoteTool.ViewModels;

namespace EmoteTool
{
    public class AppSettings
    {
        public List<(int id,
                     string name,
                     string imagePath,
                     string actualImagePath,
                     ItemSizeMode sizeMode)> 
            SavedEmotes { get; set; }
    }
}