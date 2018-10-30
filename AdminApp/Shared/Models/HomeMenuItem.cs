using System;
using System.Collections.Generic;
using System.Text;

namespace TongTongAdmin.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        DataGrid,
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
