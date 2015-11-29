namespace SensorHost.Serializer
{
    using System;
    using System.Linq;
    using System.Text;

    using SensorHost.DTOs;
    using SensorHost.Extensions;
    using SensorHost.Shared;
    
    /// <summary>
    /// A Json version of the serializer. Not this has not been fully tested, and shouldn't really be used.
    /// </summary>
    [Obsolete("Do not use - this has not being tested fully, and when we did test it it didn't work properly as PRTG ignored it.")]
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// The JSON content type.
        /// </summary>
        public string ContentType
        {
            get
            {
                return "Application/Json";
            }
        }

        /// <summary>
        /// Serialize the DTO as JSON
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public string Serialize(Reply reply)
        {
            string resultsJson = string.Join(",", reply.Prtg.Result.Select(p => ConvertToJson(p)).ToArray());

            string completeResult = "{\"prtg\":{\"result\":[" + resultsJson + "]";

            if (!string.IsNullOrWhiteSpace(reply.Prtg.Text))
            {
                completeResult = completeResult + $", \"text\":\"{reply.Prtg.Text}\"";
            }
            completeResult = completeResult + "}}";

            return completeResult;
        }

        /// <summary>
        /// Convert the channel to json.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string ConvertToJson(Result result)
        {
            StringBuilder json = new StringBuilder("{");
            json.Append($"\"channel\":\"{result.Channel}\"");
            if (result.Unit != UnitTypes.NotSet)
            {
                json.AddJsonValue("unit", result.Unit.ToString());
                if (result.Unit == UnitTypes.Custom)
                {
                    json.AddJsonValue("customUnit", result.CustomUnit);
                }
            }

            if (result.SpeedSize != SensorSpeedSize.NotSet)
            {
                json.AddJsonValue("speedSize", result.SpeedSize.ToString());
                if (result.SpeedTime != SpeedTime.NotSet)
                {
                    json.AddJsonValue("SpeedTime", result.SpeedTime.ToString());
                }
            }

            if (result.VolumeSize != SensorSpeedSize.NotSet)
            {
                json.AddJsonValue("volumeSize", result.VolumeSize.ToString());
            }

            if (result.Mode != SensorModes.NotSet)
            {
                json.AddJsonValue("mode", result.Mode.ToString());
            }
            if (result.ShowChart.HasValue)
            {
                json.AddJsonValue("showCart", result.ShowChart.Value);
            }
            if (result.ShowTable.HasValue)
            {
                json.AddJsonValue("showTable", result.ShowTable.Value);
            }

            if (result.LimitMode.HasValue)
            {
                json.AddJsonValue("limitMode", result.LimitMode.Value);
                if (result.LimitMode.Value)
                {
                    json.AddJsonValue("limitMaxError", result.LimitMaxError);
                    json.AddJsonValue("limitMaxWarning", result.LimitMaxWarning);
                    json.AddJsonValue("limitErrorMsg", result.LimitErrorMsg);
                    json.AddJsonValue("limitWarningMsg", result.LimitWarningMsg);
                }
            }
            if (result.Warning.HasValue)
            {
                json.AddJsonValue("warning", result.Warning.Value);
            }

            if (result.Float.HasValue)
            {
                json.AddJsonValue("float", result.Float.Value);
            }

            json.AddJsonValue("value", result.Value);

            json.Append("}");
            return json.ToString();
        }
    }
}
