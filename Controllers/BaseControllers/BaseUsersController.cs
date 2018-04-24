using Admin.BaseClass;
using Admin.BaseClass.App;
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
using WebMatrix.WebData;

namespace Admin.Controllers
{
    [CustomAuthorize]
    public class BaseUsersController : BaseController
    {
        [IsView]
        public ActionResult Index(FormCollection form)
        {
            //Para obtener columnas apagadas por Default

            List<BaseDynamicColumnList> bDynamicColumn = db.BaseDynamicColumnLists.ToList();
            List<BaseDynamicList> bDynamicList = db.BaseDynamicLists.ToList();
            ViewBag.column = bDynamicColumn;
            ViewBag.dynamicList = bDynamicList;
            ViewBag.from = "";
            //Nombre de la tabla o vista
            ViewBag.tableName = "VWUser";
            return PartialView(new VWUser());

        }

        public ActionResult Form(int id = 0)
        {
            if (id == 0)
            {
                BaseUser model = new BaseUser();
                model.createUser = WebSecurity.CurrentUserId;
                model.userStatusID = 0;
                return PartialView(model);
            }
            else
            {
                BaseUser model = db.BaseUsers.Find(id);
                model.password = Permission.defaultShowPassword;
                return PartialView(model);
            }
        }

        [IsView]
        [AllowAnonymous]
        new public ActionResult Profile()
        {
            BaseUser model = db.BaseUsers.Find(WebSecurity.CurrentUserId);
            model.password = Permission.defaultShowPassword;
            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Save(BaseUser model, FormCollection form, bool multiUser = false)
        {
            BoolString validation = model.BeforeSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            if (model.ID != 0)
            {
                validation = model.BeforeEdit(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
                if (ModelState.IsValid)
                {
                    bool logout = false;
                    if (model.username != Helper.getData("SELECT username from [BaseUser] where ID=" + model.ID, db).Rows[0][0].ToString())
                    {
                        logout = true;
                    }
                    if (model.password == Permission.defaultShowPassword)
                    {
                        model.password = Helper.getData("SELECT password from [BaseUser] where ID=" + model.ID, db).Rows[0][0].ToString();
                    }
                    else
                    {
                        model.password = Permission.CalculateMD5Hash(model.password);
                    }

                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files["imageUrl"];
                        if (file != null && file.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file.FileName);
                            string filename = model.ID + ".png";
                            string filePath = Path.Combine(HttpContext.Server.MapPath("~/Uploads/UserImages/"),
                                              Path.GetFileName(filename));
                            file.SaveAs(filePath);
                            model.imageUrl = filename;
                        }
                    }
                    if (model.imageUrl == null)
                    {
                        model.imageUrl = string.IsNullOrEmpty(form["imgActual"]) ? "" : form["imgActual"];
                    }
                    db.Entry(model).State = EntityState.Modified;
                    if (db.SaveChanges() != 0)
                    {
                        if (logout)
                        {
                            WebSecurity.Logout();
                        }
                    }
                }

                validation = model.AfterEdit(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["imageUrl"];
                    if (file != null && file.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        string filename = model.ID + ".png";
                        string filePath = Path.Combine(HttpContext.Server.MapPath("~/Uploads/UserImages/"), Path.GetFileName(filename));
                        file.SaveAs(filePath);
                        model.imageUrl = filename;
                    }
                }
                if (form["employeeType"] != null)
                    model.employeeType_Type = int.Parse(form["employeeType"].Split(',')[0]);

                if (form["office"] != null)
                    model.office_office = int.Parse(form["office"].Split(',')[0]);

                validation = model.BeforeCreate(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });

                model.password = Permission.CalculateMD5Hash(model.password);
                db.BaseUsers.Add(model);
                db.SaveChanges();

                validation = model.AfterCreate(db);
                if (validation.BoolValue)
                    return Json(new { Message = validation.StringValue });
            }
            validation = model.AfterSave(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });

            if (multiUser)
            {
                Helper.executeNonQUery("DuplicateUser", db);
            }
            db.SaveChanges();

            //if (form["employeeType"] != null)
            //{
            //    foreach (var item in form["employeeType"].Split(','))
            //    {
            //        SMEmployeeType tpg = db.SMEmployeeTypes.Find(int.Parse(item));
            //        if (tpg != null)
            //        {
            //            employeeModel.SMEmployeeTypes.Add(tpg);
            //        }
            //    }
            //}
            db.SaveChanges();
            return Json(model.ID);

        }

        public JsonResult Delete(int id = 0)
        {
            BaseUser model = db.BaseUsers.Find(id);
            foreach (var item in model.BaseUserMenus.ToList())
                db.BaseUserMenus.Remove(item);
            foreach (var item in model.BaseUserActions.ToList())
                db.BaseUserActions.Remove(item);
            foreach (var item in model.BaseWidgets.ToList())
                db.BaseWidgets.Remove(item);
            model.BaseProfiles.Clear();
            db.BaseUsers.Remove(model);
            db.SaveChanges();
            BoolString validation = model.AfterDelete(db);
            if (validation.BoolValue)
                return Json(new { Message = validation.StringValue });
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult redirect(int userid = 0)
        {
            string parameters = "?id=" + userid;
            string url = URLHelper.getUrl("BasePermissionsUser", "Index") + parameters;
            return Redirect(url);
        }

        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWUser]";
            table.PrimaryKey = "id";
            table.SearchCondition = " ID like '%{0}%' OR fullName like '%{0}%' OR Type like '%{0}%' OR Location like '%{0}%' OR Roles like '%{0}%'";
            table.VarName = "csSMEmployeesReport";
            table.ReferenceText = "User";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseUsers", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseUsers", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
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
            wb.Worksheets.Add(data, "Usuarios");
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] byteInfo = memoryStream.ToArray();
                return File(byteInfo, System.Net.Mime.MediaTypeNames.Application.Octet, String.Format("Usuarios BackOffice {0}.xlsx", DateTime.Now.ToShortDateString()));
            }
        }
    }
}