using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Inferis.Apis.Campfire;

namespace Inferis.Flame.iOS
{
	public class MessagesViewDataSource : UITableViewDataSource
	{
		List<CampfireMessage> messages;
		
		public MessagesViewDataSource()
		{
			messages = new List<CampfireMessage>();
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			return messages.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("Cell");
            if (cell == null) {
                cell = new UITableViewCell(UITableViewCellStyle.Default, "Cell");
            }
			
            cell.TextLabel.Text = messages[indexPath.Row].Body;
            return cell;
		}
	}
}

