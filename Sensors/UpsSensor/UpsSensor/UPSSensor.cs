using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerChuteCS;
using SensorHost.Shared;

namespace UpsSensor
{
    public class UPSSensor : SensorHost.Shared.SensorBase
    {
        private PowerChuteData _dataFactory;
        public UPSSensor()
        {
            Console.WriteLine("Created instance of PowerChute");
            _dataFactory = PowerChuteData.getInstance();

        }

        public override IEnumerable<Result> Results()
        {
            return GetData();
        }

        public List<Result> GetData()
        {
            List<Result> results = new List<Result>();

            try
            {
                var currentStatusData = _dataFactory.GetCurrentStatusData();


                double energyUsage = currentStatusData.m_percentLoad / 100.0 * currentStatusData.m_config_active_power;


                results.Add(new Result("Load Percent", currentStatusData.m_percentLoad) { Unit = UnitTypes.Percent });
                results.Add(new Result("Energy Usage", energyUsage) { Unit = UnitTypes.Custom, CustomUnit = "Watts" });
                return results;
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error getting data from UPS: {err}");
            }
            return results;
        }
    }
}
