
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Inferis.Flame.iOS
{

	// The name AppDelegateIPhone is referenced in the MainWindowIPhone.xib file.
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			MessagesViewController controller = new MessagesViewController();

			//If you have defined a view, add it here:
			window.AddSubview (controller.View);
			
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

