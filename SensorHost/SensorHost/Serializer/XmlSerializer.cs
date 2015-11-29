namespace SensorHost.Serializer
{
    using System;
    using System.Linq;
    using System.Text;

    using SensorHost.DTOs;
    using SensorHost.Extensions;
    using SensorHost.Shared;

    /// <summary>
    /// The XML based implementation of the serializer
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        /// <summary>
        /// Returns the xml content type
        /// </summary>
        public string ContentType
        {
            get
            {
                return "text/xml";
            }
        }

        /// <summary>
        /// Serialize the reply as xml
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public string Serialize(Reply reply)
        {
            //return Response.AsJson(new Reply(sensor.Description, resultsList));
            string resultsJson = string.Join(Environment.NewLine, reply.Prtg.Result.Select(this.ConvertToXml).ToArray());

            StringBuilder result = new StringBuilder();
            result.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            result.AppendLine("<prtg>");
            result.AppendLine(resultsJson);

            if (!string.IsNullOrWhiteSpace(reply.Prtg.Text))
            {
                result.AppendLine($"<text>{reply.Prtg.Text}</text>");
            }
            result.AppendLine("</prtg>");


            return result.ToString();
        }

        /// <summary>
        /// Converts a channel to xml
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string ConvertToXml(Result result)
        {
            StringBuilder xml = new StringBuilder("<result>");
            xml.AppendLine($"<channel>{result.Channel}</channel>");
            if (result.Unit != UnitTypes.NotSet)
            {
                xml.AddXmlValue("unit", result.Unit.ToString());
                if (result.Unit == UnitTypes.Custom)
                {
                    xml.AddXmlValue("customUnit", result.CustomUnit);
                }
            }

            if (result.SpeedSize != SensorSpeedSize.NotSet)
            {
                xml.AddXmlValue("speedSize", result.SpeedSize.ToString());
                if (result.SpeedTime != SpeedTime.NotSet)
                {
                    xml.AddXmlValue("SpeedTime", result.SpeedTime.ToString());
                }
            }

            if (result.VolumeSize != SensorSpeedSize.NotSet)
            {
                xml.AddXmlValue("volumeSize", result.VolumeSize.ToString());
            }

            if (result.Mode != SensorModes.NotSet)
            {
                xml.AddXmlValue("mode", result.Mode.ToString());
            }
            if (result.ShowChart.HasValue)
            {
                xml.AddXmlValue("showCart", result.ShowChart.Value);
            }
            if (result.ShowTable.HasValue)
            {
                xml.AddXmlValue("showTable", result.ShowTable.Value);
            }

            if (result.LimitMode.HasValue)
            {
                xml.AddXmlValue("limitMode", result.LimitMode.Value);
                if (result.LimitMode.Value)
                {
                    xml.AddXmlValue("limitMaxError", result.LimitMaxError);
                    xml.AddXmlValue("limitMaxWarning", result.LimitMaxWarning);
                    xml.AddXmlValue("limitErrorMsg", result.LimitErrorMsg);
                    xml.AddXmlValue("limitWarningMsg", result.LimitWarningMsg);
                }
                
            }
            if (result.Warning.HasValue)
            {
                xml.AddXmlValue("warning", result.Warning.Value);
            }

            if (result.Float.HasValue)
            {
                xml.AddXmlValue("float", result.Float.Value);
            }

            xml.AddXmlValue("value", result.Value);

            xml.Append("</result>");
            return xml.ToString();
        }
    }
}
