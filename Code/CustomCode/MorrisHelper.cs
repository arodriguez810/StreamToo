using Admin.Models;
using Admin.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace Admin.CustomCode
{
    public class MorrisHelper
    {


        public static string textWrap(string sentence, int myLimit)
        {
            string[] words = sentence.Split(new char[] { ' ' });
            IList<string> sentenceParts = new List<string>();
            sentenceParts.Add(string.Empty);

            int partCounter = 0;

            foreach (var word in words)
            {
                if (sentenceParts[partCounter].Length + word.Length > myLimit)
                {
                    partCounter++;
                    sentenceParts.Add(string.Empty);
                }

                sentenceParts[partCounter] += word + " ";
            }
            string b = "";
            foreach (var item in sentenceParts)
            {
                b += item + Environment.NewLine;
            }

            string result = b + Environment.NewLine;
            return result;
        } 

        public static string getJson(string query, Context db, bool xLabelAngle = false)
        {
            DataTable data = Helper.getData(String.Format(query), db);
            if (data.Rows.Count == 0)
            {
                data.Columns.Add("label");
                data.Columns.Add("value");
                data.Rows.Add(new object[] { "Sin datos", 0 });
            }

            JSONMorris morris = new JSONMorris();

            morris.xkey = data.Columns[0].ColumnName;

            for (int i = 1; i < data.Columns.Count; i++)
            {
                var column = data.Columns[i];
                morris.ykeys.Add(column.ColumnName);
                morris.labels.Add(column.ColumnName);                
            }

            foreach (DataRow x in data.Rows)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                for (int i = 0; i < x.ItemArray.Length; i++)
                {
                    decimal test = 0;
                    object row = 0;
                    bool isDecimal = decimal.TryParse(x.ItemArray[i].ToString(), out test);
                    if (isDecimal)
                        row = Convert.ToDecimal(x.ItemArray[i]);
                    else
                        row = x.ItemArray[i].ToString();
                    temp.Add(data.Columns[i].ColumnName, row);
                }
                morris.data.Add(temp);
            }
            morris.colors.Add("#1424b8");
            morris.colors.Add("#0aa623");
            morris.colors.Add("#940f3f");
            morris.colors.Add("#148585");
            morris.colors.Add("#098215");
            morris.colors.Add("#b86c14");
            morris.colors.Add("#b83214");
            return JsonConvert.SerializeObject(morris);
        }
    }
}