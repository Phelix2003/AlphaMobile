using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AlphaMobile.MarkupExtensions
{
    // Load image from Iamge folder.
    [ContentProperty("ResourceId")]
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ResourceId))
                return null;
            ResourceId = "AlphaMobile.Images." + ResourceId;
            return ImageSource.FromResource(ResourceId);
        }
    }
}
