using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Admin.CustomCode
{
    public abstract class ViewBase<TModel> : System.Web.Mvc.WebViewPage<TModel> where TModel : class
    {
        /// <summary>
        /// Manejador de recursos, para los textos desde los archivos de recursos.
        /// </summary>
        //private static ResourceManager manager = new ResourceManager(typeof(Language));

        /// <summary>
        /// Método de extensión para tomar los mensajes de la aplicación desde el archivo de recursos.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
    }

    public static class InternalHelpers
    {
        private static MvcHtmlString Concat(params MvcHtmlString[] items)
        {
            var sb = new StringBuilder();
            foreach (var item in items.Where(i => i != null))
                sb.Append(item.ToHtmlString());
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}