using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.CustomCode;
using Admin.BaseClass;
using Admin.BaseClass.UI;
using System.IO;
using ClosedXML.Excel;

namespace Admin.Controllers
{

    public class BaseUserStatusController : BaseController
    {
        public ActionResult Index(FormCollection form)
        {
            List<BaseUserStatu> est = db.BaseUserStatus.ToList();
            ViewBag.estatus = est;


            //Para obtener columnas apagadas por Default
            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;

            //Nombre de la tabla o vista
            ViewBag.tableName = "VWUserStatu";

            return PartialView(new VWUserStatu());
        }

        public ActionResult Form(int id = 0)
        {
            List<BaseUserStatu> est = db.BaseUserStatus.ToList();
            ViewBag.drop = est;

            if (id == 0)
                return PartialView(new VWUserStatu());
            else
            {
                VWUserStatu model = db.VWUserStatus.FirstOrDefault(d => d.ID == id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(VWUserStatu model)
        {
            BaseUser us = db.BaseUsers.Find(model.ID);

            List<BaseUserStatu> status = db.BaseUserStatus.ToList();

            string estado = model.Estado;

            foreach (var item in status)
            {
                if (estado.Equals(item.name))
                {
                    us.userStatusID = item.id;
                    break;
                }
            }
            db.SaveChanges();
            return Json(model.ID);
        }

        public JsonResult data(FormCollection form)
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWUserStatu]";
            table.PrimaryKey = "ID";
            table.ReplaceUnderScore = true;
            table.SearchCondition = " (Nombre like '%{0}%' OR Celular like '%{0}%' OR [Primer Apellido] like '%{0}%' OR [Segundo Apellido] like '%{0}%' OR [Tipo Documento] like '%{0}%' OR Identificacion like '%{0}%' OR Teléfono like '%{0}%' OR [Correo Electrónico] like '%{0}%' OR Estado like '%{0}%' OR [Última IP] like '%{0}%') AND";
            table.VarName = "csVWUserStatu";
            table.ReferenceText = "VWUserStatu";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseUserStatus", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            //table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseUserStatus", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTableUser();
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
            wb.Worksheets.Add(data, "Usuario");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Estado de Usuario {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }

    }
}