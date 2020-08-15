using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommonSystem
    {
        public static string AssemblyVersion = "1.0.0.0";
        public static string gConnectStringSQL = "";
        public static decimal defaultBegin = 0;
        public static decimal defaultEnd = 1000;
    }
    public class MessageAlert
    {
        public static string AddDone = "Thêm vào giỏ hàng thành công !";
        public static string RemoveDone = "Xóa khỏi giỏ hàng thành công !";
        public static string AddFail = "Thêm vào giỏ hàng không thành công !";
        public static string RemoveFail = "Xóa khỏi giỏ hàng không thành công !";
        public static string CountItemInCart = " - Items";
    }
    public enum TypeAction
    {
        Add,
        Remove
    }
    public enum ItemStatus
    {
        Normal,
        InCart
    }
    public enum CheckActionStatus
    {
        Succcess = 1,
        Fail  = -99
    }
}
