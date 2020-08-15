using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class SessionInfo
    {
        public decimal TotalRecord { get; set; }
        public decimal TotalItemInCart { get; set; }
        public List<decimal> ListItemAdd { get; set; }
        public List<ItemInfo> ListItemInCartUser { get; set; }
    }
}
