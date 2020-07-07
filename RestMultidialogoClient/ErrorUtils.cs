using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RestMultidialogoClient
{
    static class ErrorUtils
    { 
        public static Object ParseDotNotation(string errorMsg, Object sentObj)
        {
            foreach (String attributeName in errorMsg.Split('.'))
            {
                if (sentObj == null) { return null; }

                string propName = attributeName;
                if (IsArrayProperty(attributeName))
                {
                    propName = GetPropertyNameFromArrayReference(attributeName);
                }

                Type type = sentObj.GetType();
                FieldInfo info = type.GetField(propName);
                if (info == null) { return null; }

                if (IsArrayProperty(attributeName))
                {
                    int index = GetIndex(attributeName);
                    IEnumerable values = (IEnumerable)info.GetValue(sentObj);
                    sentObj = GetObjectFromList(index, values);
                    Console.WriteLine("Attributo: " + propName + "[" + index + "]");
                }
                else
                {
                    Console.WriteLine("Attributo: " + propName);
                    sentObj = info.GetValue(sentObj);
                }
            }
            return sentObj;
        }

        private static object GetObjectFromList(int index, IEnumerable list)
        {
            int i = 0;
            foreach (object o in list)
            {
                if (i == index)
                {
                    return o;
                }
                i++;
            }
            return null;
        }

        private static int GetIndex(string expr)
        {
            string pattern = @"[\d+]";
            int ret = -1;
            Match m = Regex.Match(expr, pattern);
            if (m.Success)
            {
                Int32.TryParse(m.Value, out ret);
            }
            return ret;
        }

        private static bool IsArrayProperty(string name)
        {
            return name.Contains("[");
        }

        private static string GetPropertyNameFromArrayReference(string arrayReference)
        {
            return arrayReference.Substring(0, arrayReference.IndexOf("["));
        }
    }
}