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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class BaseGraphsController : BaseController
    {
        public ActionResult Index()
        {
            return PartialView(new Admin.Models.BaseGraph());
        }

        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new Admin.Models.BaseGraph());
            else
            {
                BaseGraph model = db.BaseGraphs.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseGraph model, FormCollection form)
        {
            if (model.id != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;

                    if (form["TypeGraphID"] != null)
                    {
                        Helper.executeNonQUery("DELETE FROM dbo.BaseGraphsTypes WHERE GraphID = " + model.id, db);
                        foreach (var item in form["TypeGraphID"].Split(','))
                        {
                            BaseTypesGraph tpg = db.BaseTypesGraphs.Find(int.Parse(item));
                            if (tpg != null)
                            {
                                model.BaseTypesGraphs.Add(tpg);
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                if (form["TypeGraphID"] != null)
                {
                    foreach (var item in form["TypeGraphID"].Split(','))
                    {
                        BaseTypesGraph tpg = db.BaseTypesGraphs.Find(int.Parse(item));
                        if (tpg != null)
                        {
                            model.BaseTypesGraphs.Add(tpg);
                        }
                    }
                }

                db.BaseGraphs.Add(model);
                db.SaveChanges();
            }
            return Json(model.id);
        }
        public JsonResult Delete(int id = 0)
        {
            BaseGraph model = db.BaseGraphs.Find(id);
            if (model != null)
            {
                db.BaseGraphs.Remove(model);
                db.SaveChanges();
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWGraphListView]";
            string[] hides = table.hideColumns;
            List<string> tohide = hides.ToList();
            tohide.Add("queryGraphID");
            table.hideColumns = tohide.ToArray();
            
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR query like '%{0}%' OR name like '%{0}%' OR title like '%{0}%' ";
            table.VarName = "csGraphs";
            table.ReferenceText = "BaseGraphs";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseGraphs", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseGraphs", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
            table.buildDataTable(filter);
            table.buildDefaultJson();
            table.buildLinks();
            return Json(table.Json, JsonRequestBehavior.AllowGet);
        }

        private string TypeGraphsTranslate(string type = "")
        {
            string t = "";
            if (type.Trim() == "Bar".Trim())
            {
                t = "Bar";
            }
            else if (type.Trim() == "Donut".Trim())
            {
                t = "Donut";
            }
            else if (type.Trim() == "Line".Trim())
            {
                t = "Line";
            }
            else if (type.Trim() == "Area".Trim())
            {
                t = "Area";
            }

            return t;
        }

        public ActionResult _GraphView(FormCollection form)
        {
            if (form["queryGraphID"] != null)
            {
                StringBuilder javascriptGraficos = new StringBuilder();
                List<string> ltypeGraphs = new List<string>();

                List<datosdivs> liTabs = new List<datosdivs>();
                List<datosdivs> divsTabs = new List<datosdivs>();
                List<datosdivs> divsGraficos = new List<datosdivs>();

                //form fields
                string nameGraph = form["name"];
                string titleGraph = form["title"];
                string queryGraph = "";
                var query = db.BaseQueryGraphs.Find(int.Parse(form["queryGraphID"]));
                if (query != null) { queryGraph = query.query; }
                foreach (var item in form["TypeGraphID"].Split(','))
                {
                    var typ = db.BaseTypesGraphs.Find(int.Parse(item));
                    if (typ != null)
                    { ltypeGraphs.Add(typ.name); }
                }

                //para el grafico
                string namedivsTab = "";
                string namedivsgraficos = "";
                string active = "active";

                //li de los tabs
                foreach (var item in ltypeGraphs)
                {
                    namedivsTab = nameGraph.Replace(" ", "") + "div" + item;
                    liTabs.Add(new datosdivs() { name = namedivsTab, status = active, text = TypeGraphsTranslate(item) });
                    active = "";
                }

                //divs de los graficos
                foreach (var item in ltypeGraphs)
                {
                    namedivsgraficos = "morris-" + item + "_" + nameGraph.Replace(" ", "_");
                    divsGraficos.Add(new datosdivs() { name = namedivsgraficos });
                }

                //divs a donde se moveran los graficos
                active = "active";
                foreach (var item in ltypeGraphs)
                {
                    namedivsTab = nameGraph.Replace(" ", "") + "div" + item;
                    divsTabs.Add(new datosdivs() { name = namedivsTab, status = active });
                    active = "";
                }
                //json
                string consulta = MorrisHelper.getJson(queryGraph, db);

                javascriptGraficos.AppendLine(" var startMorris = function () { \n");
                javascriptGraficos.AppendFormat(" var json = {0}; \n", consulta);
                javascriptGraficos.AppendLine("");
                javascriptGraficos.AppendLine("");
                foreach (var item in ltypeGraphs)
                {
                    namedivsgraficos = "#morris-" + item + "_" + nameGraph.Replace(" ", "_");
                    javascriptGraficos.AppendLine(" $('" + namedivsgraficos + "').buildMorris({ \n");
                    javascriptGraficos.AppendFormat(" type: \"{0}\", \n", item.ToLower());
                    javascriptGraficos.AppendLine(" json: json \n");
                    javascriptGraficos.AppendLine("}); \n");

                    //moviendo divs de los graficos a divs de tabs
                    namedivsTab = "#" + nameGraph.Replace(" ", "") + "div" + item;
                    javascriptGraficos.AppendFormat(" $(\"{0}\").appendTo(\"{1}\");\n", namedivsgraficos, namedivsTab);
                    javascriptGraficos.AppendLine("");
                }
                javascriptGraficos.AppendLine("}");
                javascriptGraficos.AppendLine("");
                javascriptGraficos.AppendLine("");
                javascriptGraficos.AppendLine("$.fn.buildMorris.loadMorris(startMorris);");

                ViewBag.tabs = liTabs;
                ViewBag.divsTabs = divsTabs;
                ViewBag.divsGraficos = divsGraficos;
                ViewBag.javascriptGraficos = javascriptGraficos.ToString();
            }


            return PartialView();

        }

        public struct datosdivs
        {
            public string name;
            public string status;
            public string text;
        }

    }
}
