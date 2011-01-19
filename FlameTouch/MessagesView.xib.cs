
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Inferis.Flame.iOS
{
	public partial class MessagesView : UITableViewController
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code

		public MessagesView (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public MessagesView (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public MessagesView () : base("MessagesView", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
	
		
		#endregion
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TableView.DataSource = new MessagesViewDataSource();		}
	}
}

