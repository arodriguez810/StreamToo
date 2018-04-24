using System.Collections.Generic;

namespace Admin.Models.ViewModels
{
    public class JsonInlineChart
    {
        public List<object> values;
        public JsonInlineChart() {
            values = new List<object>();
        }
    }

    public class InlineChart {
        /// <summary>
        /// Propiedad para todos los tipos de graficos
        /// </summary>
        public string type;

        public InlineBar bar;
        public InlinePie pie;
        public InlineLine line;

        public InlineChart()
        {
            bar = new InlineBar();
            pie = new InlinePie();
            line = new InlineLine();
        }
    }
    public class InlinePie
    {
        /// <summary>
        /// Propiedad de los graficos tipo Pie
        /// </summary>
        public int offset;
        /// <summary>
        /// Propiedad de los graficos tipo Pie
        /// </summary>
        public int piesize;
        public InlinePie() { }
    }

    public class InlineBar
    {
        /// <summary>
        /// Propiedad de los graficos tipo Bar,line
        /// </summary>
        public int width;
        /// <summary>
        /// Propiedad de los graficos tipo Bar,line
        /// </summary>
        public int height;
        /// <summary>
        /// Propiedad de los graficos tipo Bar
        /// </summary>
        public int barwidth;
        /// <summary>
        /// Propiedad de los graficos tipo Bar
        /// </summary>
        public int barspacing;

        public InlineBar() { }
    }

    public class InlineLine
    {
        /// <summary>
        /// Propiedad de los graficos tipo Bar,line
        /// </summary>
        public int width;
        /// <summary>
        /// Propiedad de los graficos tipo Bar,line
        /// </summary>
        public int height;

        /// <summary>
        /// Propiedad de los graficos tipo line
        /// </summary>
        public string color;
        /// <summary>
        /// Propiedad de los graficos tipo line
        /// </summary>
        public string line_color;
        /// <summary>
        /// Propiedad de los graficos tipo line
        /// </summary>
        public int spotradius;

        public InlineLine() { }
    }
    

}