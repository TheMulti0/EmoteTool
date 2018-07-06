using System.ComponentModel;

namespace EmoteTool.ViewModels
{
    internal enum ItemError
    {
        [Description("")]
        None,

        [Description("Please choose an image.")]
        InvalidImage
    }
}