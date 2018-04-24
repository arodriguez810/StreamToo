using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BaseProfilesController : BaseController
    {
        //
        // GET: /Profiles/
        [IsView]
        public ActionResult Index()
        {
            //Para obtener columnas apagadas por Default
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;

            //Nombre de la tabla o vista
            ViewBag.tableName = "VWBaseProfile";

            return PartialView(new BaseProfile());
        }

        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseProfile());
            else
            {
                BaseProfile model = db.BaseProfiles.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseProfile model)
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
                model.creationDate = DateTime.Now;
                db.BaseProfiles.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseProfile model = db.BaseProfiles.Find(id);
            if (model != null)
            {
                model.BaseActions.Clear();
                model.BaseProfileMenus.Clear();
                model.BaseWidgets.Clear();
                model.BaseWidgets1.Clear();
                model.BaseGraphs.Clear();
                model.BaseUsers.Clear();                
                db.BaseProfiles.Remove(model);
                db.SaveChanges();
            } return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWBaseProfile]";
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR name like '%{0}%' OR description like '%{0}%' OR displayName like '%{0}%' ";
            table.VarName = "csBaseProfile";
            table.ReferenceText = "Profile";
            table.ReplacedText.Add("displayName", ("Default Page"));
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.link, Href = URLHelper.getActionUrlWithBaseHome("BasePermissions", "Index2"), Title = "Configuration", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseProfiles", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }

        public FileResult getCSV()
        {
            HttpRequestBase req = this.Request; //Parametros GET
            string consulta = crearConsulta(req); //String que almacena toda la consulta

            var wb = new XLWorkbook();
            DataTable data = Helper.getData(consulta, db);
            wb.Worksheets.Add(data, "Perfiles");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Perfiles de Usuario {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }

    }
}
