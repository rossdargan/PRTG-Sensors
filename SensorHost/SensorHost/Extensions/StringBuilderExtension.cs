using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorHost.Extensions
{
    public static class StringBuilderExtension
    {
        public static void AddJsonValue(this StringBuilder json, string key, bool value)
        {
            string strVal = value ? "1" : "0";
            json.Append($", \"{key}\":{strVal}");
        }
        public static void AddJsonValue(this StringBuilder json, string key, string value)
        {
            json.Append($", \"{key}\":\"{value}\"");
        }
        public static void AddJsonValue(this StringBuilder json, string key, int value)
        {
            json.Append($", \"{key}\":{value}");
        }
        public static void AddJsonValue(this StringBuilder json, string key, double value)
        {
            json.Append($", \"{key}\":{value}");
        }

        public static void AddXmlValue(this StringBuilder xml, string key, bool value)
        {
            string strVal = value ? "1" : "0";
            xml.AppendLine($"<{key}>{strVal}</{key}>");
        }
        public static void AddXmlValue(this StringBuilder xml, string key, string value)
        {
            xml.AppendLine($"<{key}>{value}</{key}>");

        }
        public static void AddXmlValue(this StringBuilder xml, string key, int value)
        {
            xml.AppendLine($"<{key}>{value}</{key}>");

        }
        public static void AddXmlValue(this StringBuilder xml, string key, double value)
        {
            xml.AppendLine($"<{key}>{value}</{key}>");

        }
    }
}
