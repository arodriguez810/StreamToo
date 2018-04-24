using Admin.CustomCode;
using Admin.Models;
using Admin.Models.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Controllers
{

    [CustomAuthorize]
    public class BaseMenuController : BaseController
    {
        [IsView]
        public ActionResult Index()
        {
            return PartialView();
        }

        [IsView]
        public ActionResult Menu()
        {
            Helper.executeNonQUery("update BaseDynamicColumnList set show=0 where name='id' and listID in(select L.id from BaseDynamicColumnList B INNER JOIN BaseDynamicList L ON L.id=B.listID where L.name in ( select TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE='uniqueidentifier' and COLUMN_NAME='id' ))", db);

            try
            {
                Helper.InsertControllerAndActions();
            }
            catch (System.Exception)
            {
            }
            
            PermissionsModel model = new PermissionsModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Menu(FormCollection form)
        {
            List<MenuJson> model = JsonConvert.DeserializeObject<List<MenuJson>>(form["json"].ToString());
            int mainOrder = 0;
            foreach (var item in model)
            {
                BaseMenu menu = db.BaseMenus.Find(item.id);
                if (menu != null)
                {
                    menu.noOrder = mainOrder;
                    menu.menuID = null;
                    mainOrder++;
                    db.SaveChanges();
                    if (item.children != null)
                    {
                        UpdateMenuRecursive(item);
                    }
                }
            }
            return Json(1);
        }

        public ActionResult AllForm()
        {
            return PartialView(new BaseMenu());
        }

        public ActionResult Edit(int id = 0)
        {
            BaseMenu menu = db.BaseMenus.Find(id);
            return PartialView(menu);
        }

        public ActionResult Admin()
        {
            PermissionsModelAdmin model = new PermissionsModelAdmin();
            return PartialView(model);
        }

        public ActionResult PermissionList(int id = 0)
        {
            PermissionsModelAdmin model = new PermissionsModelAdmin();
            return PartialView(model);
        }


        /*For Controller****************************************************************************/
        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseMenu());
            else
            {
                BaseMenu model = db.BaseMenus.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseMenu model, FormCollection form)
        {
            if (model.id != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                db.BaseMenus.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }

        public JsonResult delete(int id = 0)
        {
            BaseMenu model = db.BaseMenus.Find(id);
            model.BaseProfileMenus.Clear();
            model.BaseUserMenus.Clear();

            List<BaseMenu> hijosdelpadre = new List<BaseMenu>();
            List<BaseMenu> hijosdehijos = new List<BaseMenu>();

            int cant = model.BaseMenu1.Count();
            foreach (var item in model.BaseMenu1)
            {
                if (item.BaseMenu1.Count() > 0)
                {
                    foreach (var i in item.BaseMenu1)
                    {
                        hijosdehijos.Add(i);
                    }
                }

                hijosdelpadre.Add(item);
            }

            if (hijosdehijos.Count() > 0)
            {
                foreach (var i in hijosdehijos)
                {
                    db.BaseMenus.Remove(i);
                    db.SaveChanges();
                }
            }

            if (hijosdelpadre.Count() > 0)
            {
                foreach (var i in hijosdelpadre)
                {
                    db.BaseMenus.Remove(i);
                    db.SaveChanges();
                }
            }



            db.BaseMenus.Remove(model);
            db.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        /*For Controller****************************************************************************/

        public void UpdateMenuRecursive(MenuJson parent)
        {
            int internalOrder = 0;
            foreach (var item in parent.children)
            {
                BaseMenu menu = db.BaseMenus.Find(item.id);
                if (menu != null)
                {
                    menu.noOrder = internalOrder;
                    menu.menuID = parent.id;
                    internalOrder++;
                    db.SaveChanges();
                    if (item.children != null)
                    {
                        UpdateMenuRecursive(item);
                    }
                }
            }
        }
    }
}
