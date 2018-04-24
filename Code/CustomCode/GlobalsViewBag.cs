using System.Collections.Generic;
using System.Linq;

namespace Admin.CustomCode
{
    public static class GlobalsViewBag
    {
        public static Dictionary<string, object> globalViewBags = new Dictionary<string, object>();

        public static void Add(string key, object value)
        {
            if (globalViewBags.Keys.Contains(key))
            {
                globalViewBags[key] = value;
            }
            else
            {
                globalViewBags.Add(key, value);
            }
        }

        public static void Clear()
        {
            globalViewBags.Clear();
        }

        public static object Get(string key)
        {
            if (globalViewBags.Keys.Contains(key))
            {
                return globalViewBags[key];
            }
            else
            {
                return "";
            }
        }
    }
}