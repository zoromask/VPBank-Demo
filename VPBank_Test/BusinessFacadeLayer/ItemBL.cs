using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using ObjectInfo;
using DataAccessLayer;
namespace BusinessFacadeLayer
{
    public class ItemBL
    {
        public static List<ItemInfo> ItemLoad(int i_itemStatus,decimal i_begin, decimal i_end)
        {
            var listObject = new List<ItemInfo>();
            try
            {
                listObject = CBO<ItemInfo>.FillCollectionFromDataSet(ItemDA.ItemLoad(i_itemStatus,i_begin, i_end));
            }
            catch (Exception ex)
            {
            }
            return listObject;
        }
        public static List<ItemInfo> ItemIdInCart()
        {
            var listObject = new List<ItemInfo>();
            try
            {
                listObject = CBO<ItemInfo>.FillCollectionFromDataSet(ItemDA.ItemIdInCart());
            }
            catch (Exception ex)
            {
            }
            return listObject;
        }
        public static int ItemUpdateInCart(ItemInfo Obj)
        {
            try
            {
                return ItemDA.ItemUpdate(Obj);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public static int ItemInCartCount()
        {
            try
            {
                return ItemDA.ItemInCartCount();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
