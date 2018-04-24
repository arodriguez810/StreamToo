using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.CustomCode;
using Admin.BaseClass;
using Admin.BaseClass.UI;

namespace Admin.Controllers
{

    [CustomAuthorize]
    public class BaseColumnsController : BaseController
    {
        [IsView]
        public ActionResult Index(FormCollection form)
        {
            return PartialView(new VWBaseDynamicList());
        }

        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseDynamicList());
            else
            {
                BaseDynamicList model = db.BaseDynamicLists.Find(id);
                List<BaseDynamicColumnList> col = db.BaseDynamicColumnLists.Where(val => val.listID == id).OrderBy(v => v.orderNum).ToList();
                ViewBag.bdynamic = col;
                return PartialView(model);
            }
        }

        [IsView]
        public ActionResult addNew()
        {
            List<BaseDynamicList> model = db.BaseDynamicLists.Where(v => v.enableControl == false).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Save(BaseDynamicList model)
        {
            BaseDynamicList bdl = db.BaseDynamicLists.Find(model.id);
            bdl.allowSaveConfig = model.allowSaveConfig;
            bdl.allowExport = model.allowExport;

            BaseMenu bm = db.BaseMenus.FirstOrDefault(v => v.title.ToLower() == bdl.nameToDisplay.ToLower());
            if (bm != null)
            {
                bm.title = model.nameToDisplay;
                bm.text = model.nameToDisplay;
                db.Entry(bm).State = EntityState.Modified;
            }
            bdl.nameToDisplay = model.nameToDisplay;
            db.SaveChanges();

            return Json(model.id);
        }

        public JsonResult saveColumn()
        {
            HttpRequestBase req = this.Request;
            string[] data = req.QueryString["column"].Split('-');

            string nombreColum = data[0];
            int listid = Int32.Parse(data[1]);

            BaseDynamicColumnList bdList = db.BaseDynamicColumnLists.FirstOrDefault(v => v.name == nombreColum && v.listID == listid);

            if (bdList.show)
            {
                bdList.show = false;
            }
            else
            {
                bdList.show = true;
            }

            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult saveOrder()
        {
            HttpRequestBase req = this.Request;
            string[] data = req.QueryString["column"].Split('-');

            string nombreColum = data[0];
            int listid = Int32.Parse(data[1]);

            BaseDynamicColumnList bdList = db.BaseDynamicColumnLists.FirstOrDefault(v => v.name == nombreColum && v.listID == listid);

            if (bdList.show)
            {
                bdList.show = false;
            }
            else
            {
                bdList.show = true;
            }

            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult turnOnColum()
        {
            HttpRequestBase req = this.Request;
            string[] data = req.QueryString["column"].Split('-');

            int id = Int32.Parse(data[0]);
            //bool listid = bool.Parse(data[1]);

            BaseDynamicList bdyList = db.BaseDynamicLists.Find(id);
            bdyList.enableControl = true;

            //BaseDynamicColumnList bdList = db.BaseDynamicColumnLists.FirstOrDefault(v => v.name == nombreColum && v.listID == listid
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseDynamicList model = db.BaseDynamicLists.Find(id);
            if (model != null)
            {
                model.enableControl = false;
                model.allowExport = false;
                model.allowSaveConfig = true;

                db.SaveChanges();

                List<BaseDynamicColumnList> col = db.BaseDynamicColumnLists.Where(val => val.listID == id).ToList();
                foreach (var item in col)
                {
                    item.show = true;
                }

                db.SaveChanges();

            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form)
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWBaseDynamicList]";
            table.AdditionalWhere = "enableControl = 1 AND ID != 0";
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR name like '%{0}%' OR";
            table.VarName = "csBaseDynamicList";
            table.ReferenceText = "Control";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseColumns", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { FullText = "¿Desea Delete este control?", ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseColumns", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable();
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }

    }
}