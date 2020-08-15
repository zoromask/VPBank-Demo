using ObjectInfo;
using System.Web;

namespace VPBank_Test
{
    public class SessionData
    {
        public static decimal TotalRecord = 0;
  
        public static SessionInfo CurrentSession
        {
            get
            {
                if (HttpContext.Current.Session == null)
                {
                    return null;
                }
                if (HttpContext.Current.Session["Account"] == null)
                {
                    return null;
                }
                else
                {
                    return (SessionInfo)HttpContext.Current.Session["Account"];
                }
            }
            set
            {
                HttpContext.Current.Session["Account"] = value;
            }
        }
    }
}