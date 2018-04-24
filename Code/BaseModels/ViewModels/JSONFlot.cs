using System.Collections.Generic;

namespace Admin.Models.ViewModels
{
    public class JSONFlot
    {
        public List<FlotDataMultiple> data;

        public JSONFlot()
        {
            data = new List<FlotDataMultiple>();
        }
    }

    public class JsonFlotOptions
    {
        public bool tooltip { get; set; }
        public GridOptions grid { get; set; }
        public Series series { get; set; }
        public XAXISSin xaxis { get; set; }
        public YAXIS yaxis { get; set; }        

        public JsonFlotOptions()
        {           
            grid = new GridOptions();
            series = new Series();
            xaxis = new XAXISSin();
            yaxis = new YAXIS();            
            tooltip = true;
            
        }
    }

    public class JsonFlotOptionsLines
    {            
        public bool tooltip { get; set; }
        public GridOptions grid { get; set; }
        public Series series { get; set; }
        public XAXISLines xaxis { get; set; }
        public YAXISLines yaxis { get; set; }       

        public JsonFlotOptionsLines()
        {   
            grid = new GridOptions();
            series = new Series();
            xaxis = new XAXISLines();
            yaxis = new YAXISLines();            
            tooltip = true;
            
        }
    }

    public class GridOptions
    {
        public bool show;
        public bool hoverable;
        public bool clickable;
        public string tickColor;
        public int borderWidth;
        public string borderColor;


        public GridOptions()
        {
            tickColor = "";
            borderWidth = 0;
            borderColor = "";
            show = true;
            hoverable = true;
            clickable = true;
        }
    }


    #region Clases Chart Bar

    public class Bars
    {
        public bool horizontal;
        public bool show;
        public decimal barWidth;
        public int order;

        public Bars()
        {
            horizontal = false;
        }
    }

    public class SeriesBar
    {
        public Bars bars { get; set; }

        public SeriesBar()
        {
            bars = new Bars();
        }
    }

    public class JsonFlotOptionsBar
    {
        public SeriesBar series { get; set; }
        public XAXIS xaxis { get; set; }
        public GridOptions grid { get; set; }
        public bool tooltip { get; set; }


        public JsonFlotOptionsBar()
        {
            series = new SeriesBar();
            xaxis = new XAXIS();
            grid = new GridOptions();
            tooltip = true;

        }
    }
    #endregion

    #region Clases Chart Pie

    public class JsonFlotOptionsPie
    {

        public bool tooltip { get; set; }
        public SeriesPie series { get; set; }
        public Legend legend { get; set; }

        public JsonFlotOptionsPie()
        {
            series = new SeriesPie();
            legend = new Legend();
            tooltip = true;
        }
    }

    public class SeriesPie
    {
        public Pie pie { get; set; }

        public SeriesPie()
        {
            pie = new Pie();
        }
    }

    public class Pie
    {
        public bool show;
        public decimal innerRadius;
        public int radius;
        public LabelOption label;

        public Pie()
        {
            label = new LabelOption();
        }
    }

    public class LabelOption
    {
        public decimal threshold;
        public bool show;
        public int radius;
        public string formatter;
        public LabelOption() { }
    }

    public class Legend
    {
        public bool show;
        public int noColumns; // number of colums in legend table
        public string labelFormatter; // fn: string -> string
        public string labelBoxBorderColor; // border color for the little label boxes
        public object container; // container (as jQuery object) to put legend in, null means default on top of graph
        public string position; // position of default legend container within plot
        public string margin; // distance from grid edge to default legend container within plot
        public string backgroundColor; // null means auto-detect
        public int backgroundOpacity;// set to 0 to avoid background

        public Legend()
        {
            show = false;
        }

    }

    #endregion

    public class Series
    {
        public Lines lines { get; set; }
        public Points points { get; set; }        
        public int shadowSize { get; set; }

        public ValueLabels valueLabels { get; set; }
        public Series()
        {
            valueLabels = new ValueLabels();            
            lines = new Lines();
            points = new Points();

        }
    }

    public class Lines
    {
        public bool show;
        public int lineWidth;
        public bool fill;
        public bool steps;        
        public Lines() { }
    }

    public class XAXISLines
    {
        public string mode;
        public int tickLength;
        public int ticks;
        public int tickDecimals;
        public XAXISLines()
        {            
        }
    }

    public class YAXISLines
    {
        public string mode;
        public int tickLength;        
        public int ticks;
        public int tickDecimals;
        public decimal min;
        public decimal max;
        public bool show;

        public YAXISLines()
        {
            min = 0;
            tickDecimals = 0;
            show = true;            
            ticks = 3;
        }
    }

    public class XAXIS
    {
        public string mode;
        public int tickLength;
        public List<object[]> ticks;
        public int tickDecimals;        
        public XAXIS()
        {
            ticks = new List<object[]>();
        }
    }

    public class XAXISSin
    {
        public string mode;
        public int tickLength;
        public List<object[]> ticks;
        public int tickDecimals;
        public decimal min;
        public decimal max;
        public XAXISSin()
        {
            ticks = new List<object[]>();
        }
    }

    public class YAXISBarH
    {
        public string mode;
        public int tickLength;
        public List<object[]> ticks;        
        public int tickDecimals;
        public int min;
        public bool show;

        public YAXISBarH()
        {
            min = 0;
            tickDecimals = 0;
            show = true;
            ticks = new List<object[]>();
        }
    }

    public class YAXIS
    {
        public string mode;
        public int tickLength;        
        public int ticks;
        public int tickDecimals;
        public decimal min;
        public decimal max;
        public bool show;

        public YAXIS()
        {
            min = 0;
            tickDecimals = 0;
            show = true;            
            ticks = 3;
        }
    }

    public class Points
    {
        public bool show;
    }
    
    public class ValueLabels
    {
        public bool show;

        public ValueLabels()
        {
            show = true;
        }

    }

    public class TooltipOpts
    {
        public string content;
        public bool defaultTheme;

        public TooltipOpts()
        {
            content = "<b>%x</b> = <span>%y</span>";
            defaultTheme = false;
        }
    }
    public class FlotDataMultiple
    {
        public string label;
        public List<List<decimal>> data;
        public FlotDataMultiple()
        {
            label = "";
            data = new List<List<decimal>>();
        }
    }
}