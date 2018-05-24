using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoteTool.ViewModels
{
    internal enum AddError
    {
        [Description("")]
        None,
        [Description("Emote name cannot be this name.")]
        InvalidName,
        [Description("Please choose an image.")]
        InvalidImage
    }
}
