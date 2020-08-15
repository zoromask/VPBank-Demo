using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFacadeLayer;
using ObjectInfo;
using Common;
using System.Threading;
using System.Runtime.InteropServices;
using System.Web.DynamicData.ModelProviders;

namespace VPBank_Test.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionData.CurrentSession == null)
            {
                SessionData.CurrentSession = new SessionInfo();
                SessionData.CurrentSession.ListItemAdd = new List<decimal>();
            }
            var lstItems = new List<ItemInfo>();
            try
            {
                lstItems = ItemBL.ItemLoad(Convert.ToInt32(ItemStatus.Normal),CommonSystem.defaultBegin, CommonSystem.defaultEnd);
                SessionData.TotalRecord = CommonSystem.defaultEnd;
                SessionData.CurrentSession.ListItemAdd =  ItemBL.ItemIdInCart().Select(d=>d.Item_Id).ToList();
            }
            catch (Exception ex)
            {
            }
            return View(lstItems);
        }
        [HttpPost]
        public ActionResult AutoLoad(int p_lastIndex,int p_itemStatus)
        {
            decimal p_begin = SessionData.TotalRecord + 1;
            decimal p_end = CommonSystem.defaultEnd * p_lastIndex;
            var _lst = new List<ItemInfo>();
            try
            {
                _lst = ItemBL.ItemLoad(p_itemStatus,p_begin, p_end);
                if (_lst.Count() == 0)
                {
                    p_lastIndex = 0;
                }
                SessionData.TotalRecord = p_end;
            }
            catch (Exception ex)
            {
            }
            return Json(new
            {
                success = -2,
                _lastpage = p_lastIndex,
                _html = RenderViewHelper.RenderRazorViewToString("~/Views/Home/_ItemList.cshtml", _lst, this.ViewData, this.ControllerContext, this.TempData)
            });
        }
        [HttpPost]
        public ActionResult AddItem(decimal itemId)
        {
            try
            {
                var _obj = new ItemInfo();
                    _obj.Item_Id = itemId;

                var MsgAlert = "";
                int p_return = 0;
                if (SessionData.CurrentSession.ListItemAdd.Contains(_obj.Item_Id))
                {
                    SessionData.CurrentSession.ListItemAdd.Remove(_obj.Item_Id);
                    _obj.IsInCart = Convert.ToInt32(ItemStatus.Normal);
                    p_return = ItemBL.ItemUpdateInCart(_obj);
                    if (p_return >= Convert.ToInt32(CheckActionStatus.Succcess)){
                        MsgAlert = MessageAlert.RemoveDone;
                        p_return = Convert.ToInt32(TypeAction.Remove);
                    }
                }
                else
                {
                    SessionData.CurrentSession.ListItemAdd.Add(_obj.Item_Id);
                    _obj.IsInCart = Convert.ToInt32(ItemStatus.InCart);
                    p_return = ItemBL.ItemUpdateInCart(_obj);
                    if(p_return > Convert.ToInt32(CheckActionStatus.Succcess))
                    {
                        MsgAlert = MessageAlert.AddDone;
                        p_return = Convert.ToInt32(TypeAction.Add);
                    }
                    else
                    {
                        MsgAlert = MessageAlert.AddFail;
                    }
                }
                var countItemInCart = ItemBL.ItemInCartCount().ToString() + MessageAlert.CountItemInCart;
                return Json(new { success = p_return, message = MsgAlert, countItem = countItemInCart });
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult  ItemInCart()
        {
            var lstItems = new List<ItemInfo>();
            try
            {
                lstItems = ItemBL.ItemLoad(Convert.ToInt32(ItemStatus.InCart),CommonSystem.defaultBegin, CommonSystem.defaultEnd);
                SessionData.TotalRecord = CommonSystem.defaultEnd;
                SessionData.CurrentSession.TotalItemInCart = ItemBL.ItemInCartCount();
            }
            catch (Exception ex)
            {
            }
            return View(lstItems);
        }
    }
}