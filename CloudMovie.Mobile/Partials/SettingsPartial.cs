using System;
using CloudMovie.Mobile;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public partial class CoreSettings
    {
 
        public static SomeValueConverter UpperText
        {
            get
            {
                return CoreDependencyService.GetConverter<SomeValueConverter>();
            }
        }

    }
}
