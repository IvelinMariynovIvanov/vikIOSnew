using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace VikRuse
{
    public class ModalViewSource : UITableViewSource
    {
        private List<Customer> mCustomers;

        public ModalViewSource(List<Customer> mCustomers)
        {
            this.mCustomers = mCustomers;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            throw new NotImplementedException();
        }
    }
}