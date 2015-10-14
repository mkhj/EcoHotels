using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class SimpleListItem
    {
        public SimpleListItem(Guid id, string name, bool isSelected)
        {
            Id = id;
            Name = name;
            IsSelected = isSelected;
        }

        public Guid Id { get; set; }

        public bool IsSelected { get; set; }

        public string Name { get; set; }
    }
}