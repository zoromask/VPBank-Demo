using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class ItemInfo
    {
        public decimal STT { get; set; }
        public decimal Item_Id { get; set; }
        public string Item_Code { get; set; }
        public string Item_Name { get; set; }
        public string Description { get; set; }
        public int Deleted { get; set; }
        public int IsInCart { get; set; }
    }
}
