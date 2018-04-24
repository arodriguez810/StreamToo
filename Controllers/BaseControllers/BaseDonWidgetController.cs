using Admin.BaseClass;
using Admin.BaseClass.UI;
using Admin.CustomCode;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class BaseDonWidgetController : BaseController
    {
        [IsView]
        public ActionResult Index()
        {
            return PartialView(new BaseWidget());
        }
        [IsView]
        public ActionResult Form(int id = 0)
        {
            if (id == 0)
                return PartialView(new BaseWidget());
            else
            {
                BaseWidget model = db.BaseWidgets.Find(id);
                return PartialView(model);
            }
        }
        [HttpPost]
        public JsonResult Save(BaseWidget model, FormCollection form)
        {
            if (model.id != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    if (form["GraphsID"] != null)
                    {
                        Helper.executeNonQUery("delete from dbo.BaseWidgetGraph where WidgetID = " + model.id, db);

                        foreach (var item in form["GraphsID"].Split(','))
                        {
                            BaseGraph g = db.BaseGraphs.Find(int.Parse(item));
                            if (g != null)
                            {
                                model.BaseGraphs.Add(g);
                            }
                        }
                    }
                    if (form["ProfilesID"] != null)
                    {
                        Helper.executeNonQUery("delete from dbo.BaseWidgetUserProfile where WidgetID = " + model.id, db);

                        foreach (var item in form["ProfilesID"].Split(','))
                        {
                            BaseProfile g = db.BaseProfiles.Find(int.Parse(item));
                            if (g != null)
                            {
                                model.BaseProfiles.Add(g);
                            }
                        }
                    }

                    if (form["description"] != null)
                    {
                        model.description = form["description"];
                    }
                    else
                    {
                        model.description = "";
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                try
                {
                    if (form["GraphsID"] != null)
                    {
                        foreach (var item in form["GraphsID"].Split(','))
                        {
                            BaseGraph g = db.BaseGraphs.Find(int.Parse(item));
                            if (g != null)
                            {
                                model.BaseGraphs.Add(g);
                            }
                        }
                    }

                    if (form["description"] != null)
                    {
                        model.description = form["description"];
                    }
                    else
                    {
                        model.description = "";
                    }


                    db.BaseWidgets.Add(model);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
            return Json(model.id);
        }

        public JsonResult Delete(int id = 0)
        {
            BaseWidget model = db.BaseWidgets.Find(id);
            if (model != null)
            {
                model.BaseGraphs.Clear();

                db.BaseWidgets.Remove(model);
                db.SaveChanges();
            } return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public JsonResult data(FormCollection form, string filter = "")
        {
            JsonCsTableHelper table = new JsonCsTableHelper(db, form, Request.QueryString);
            table.TableName = "[VWWidgetGraph]";
            table.PrimaryKey = "id";
            table.SearchCondition = " id like '%{0}%' OR Nombre Widget like '%{0}%' OR description like '%{0}%' OR Nombre Gráfico like '%{0}%' ";
            table.VarName = "csBaseWidget";
            table.ReferenceText = "BaseWidget";
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.button, Href = URLHelper.getActionUrl("BaseDonWidget", "Form"), Title = "Edit", Css = "btn btn-warning btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_pencil });
            table.Links.Add(new CsTableLink() { ButtonType = CsTableLinkType.deleteButton, Href = URLHelper.getActionUrl("BaseDonWidget", "Delete"), Title = "Delete", Css = "btn btn-danger btn-circle refreshTable", Icon = BaseUIIconText.fa.fa_fa_trash_o });
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

        public ActionResult _GraphView(int widgetID = 0, string fechad = "", string fechah = "")
        {
            if (widgetID > 0)
            {
                StringBuilder javascriptGraficos = new StringBuilder();
                StringBuilder JSONjavascriptGraficos = new StringBuilder();
                List<string> ltypeGraphs = new List<string>();

                List<datosdivs> liTabs = new List<datosdivs>();
                List<datosdivs> divsTabs = new List<datosdivs>();
                List<datosdivs> divsGraficos = new List<datosdivs>();

                var widget = db.BaseWidgets.Find(widgetID);
                string active = "active";
                StringBuilder textFilter = new StringBuilder();

                if (fechad != "" && fechah != "")
                {
                    textFilter.AppendLine(" <div> ");
                    textFilter.AppendFormat("<p>From: {0} hasta: {1}</p>", fechad, fechah);
                    textFilter.AppendLine("</div>");
                }
                else if (fechah != "" && fechad == "")
                {
                    textFilter.AppendLine(" <div> ");
                    textFilter.AppendFormat("<p>To: {0} </p>", fechah);
                    textFilter.AppendLine("</div>");
                }
                else if (fechad != "" && fechah == "")
                {
                    textFilter.AppendLine(" <div> ");
                    textFilter.AppendFormat("<p>From: {0} To: {1}</p>", fechad, DateTime.Now.ToString("yyyy-MM-dd"));
                    textFilter.AppendLine("</div>");
                }
                else if (fechad == "" && fechah == "" && widget.hasRange)
                {
                    textFilter.AppendLine(" <div> ");
                    textFilter.AppendFormat("<p>No Filters</p>", fechad, fechah);
                    textFilter.AppendLine("</div>");
                }

                var graficos = widget.BaseGraphs.ToList();
                if (graficos.Count(x => x.queryGraphID != null) > 0)
                {
                    javascriptGraficos.AppendLine(" var startMorris = function () { \n");
                }

                foreach (var g in graficos)
                {
                    ltypeGraphs = new List<string>();

                    if (g.queryGraphID != null)
                    {
                        ViewBag.filter = textFilter;
                    }

                    //form fields
                    string nameGraph = g.name;
                    string titleGraph = g.title;
                    string queryGraph = "";
                    var query = db.BaseQueryGraphs.Find(g.queryGraphID);
                    if (query != null) { queryGraph = query.query; }

                    foreach (var item in g.BaseTypesGraphs.ToList())
                    {
                        var typ = db.BaseTypesGraphs.Find(item.id);
                        if (typ != null)
                        { ltypeGraphs.Add(typ.name); }
                    }

                    //para el grafico
                    string namedivsTab = "";
                    string namedivsgraficos = "";

                    //li de los tabs
                    string textli = g.title;
                    foreach (var item in ltypeGraphs)
                    {
                        namedivsTab = nameGraph.Replace(" ", "") + "div" + item + widgetID;
                        //liTabs.Add(new datosdivs() { name = namedivsTab, status = active, text = textli });
                        liTabs.Add(new datosdivs() { name = namedivsTab, status = active, text = TypeGraphsTranslate(item) });
                        //divs a donde se moveran los graficos                        
                        divsTabs.Add(new datosdivs() { name = namedivsTab, status = active });
                        active = "";
                    }

                    //divs de los graficos
                    foreach (var item in ltypeGraphs)
                    {
                        namedivsgraficos = "morris-" + item + "_" + nameGraph.Replace(" ", "_") + widgetID;
                        divsGraficos.Add(new datosdivs() { name = namedivsgraficos });
                    }

                    //json
                    string con = "";
                    string consulta = "";
                    if (g.queryGraphID != null)
                    {
                        con = queryGraph;

                        if (fechad != "" && fechah != "")
                        {
                            con = queryGraph.Replace("2015-01-01", fechad).Replace("3000-02-01", fechah);
                        }

                        consulta = MorrisHelper.getJson(con, db);

                        //javascriptGraficos.AppendLine(" var startMorris = function () { \n");
                        //javascriptGraficos.AppendFormat(" var json = {0}; \n", consulta);
                        //javascriptGraficos.AppendLine("");
                        //javascriptGraficos.AppendLine("");
                        foreach (var item in ltypeGraphs)
                        {
                            string namejson = "JSON_" + item + "" + g.id;
                            JSONjavascriptGraficos.AppendFormat(" var {1} = {0}; \n", consulta, namejson);
                            JSONjavascriptGraficos.AppendLine("");

                            namedivsgraficos = "#morris-" + item + "_" + nameGraph.Replace(" ", "_") + widgetID;
                            javascriptGraficos.AppendLine(" $('" + namedivsgraficos + "').buildMorris({ \n");
                            javascriptGraficos.AppendFormat(" type: \"{0}\", \n", item.ToLower());
                            //javascriptGraficos.AppendLine(" json: json \n");
                            javascriptGraficos.AppendFormat(" json: {0} \n", namejson);
                            javascriptGraficos.AppendLine("}); \n");

                            //moviendo divs de los graficos a divs de tabs
                            namedivsTab = "#" + nameGraph.Replace(" ", "") + "div" + item + widgetID;
                            javascriptGraficos.AppendFormat(" $(\"{0}\").appendTo(\"{1}\");\n", namedivsgraficos, namedivsTab);
                            javascriptGraficos.AppendLine("");
                        }
                        //javascriptGraficos.AppendLine("}");
                        //javascriptGraficos.AppendLine("");
                        //javascriptGraficos.AppendLine("");
                        //javascriptGraficos.AppendLine("$.fn.buildMorris.loadMorris(startMorris);");                    

                        javascriptGraficos.AppendLine("}");
                        javascriptGraficos.AppendLine("");
                        javascriptGraficos.AppendLine("");
                        javascriptGraficos.AppendLine("$.fn.buildMorris.loadMorris(startMorris);");
                    }
                }
                ViewBag.tabs = liTabs;
                ViewBag.divsTabs = divsTabs;
                ViewBag.divsGraficos = divsGraficos;
                ViewBag.javascriptGraficos = javascriptGraficos.ToString();
                ViewBag.JSONjavascriptGraficos = JSONjavascriptGraficos.ToString();
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
