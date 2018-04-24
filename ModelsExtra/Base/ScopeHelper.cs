using Admin.CustomCode;
using System.Collections.Generic;

namespace Admin.ModelsExtra
{
    public class ScopeHelper
    {
        public static string ListToMessage(List<string> messages)
        {
            return string.Join("<br><div style='border-bottom:2px solid white'></div>", messages);
        }

        public static BoolString RegulateMessages(List<string> messages)
        {
            if (messages.Count > 0)
            {
                return new BoolString()
                {
                    StringValue = ScopeHelper.ListToMessage(messages),
                    BoolValue = true
                };
            }
            return new BoolString()
            {
                BoolValue = false
            };
        }
    }
}