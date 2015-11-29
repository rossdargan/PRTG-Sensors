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
        public override IEnumerable<Result> Results()
        {
            return GetData();
        }

        public List<Result> GetData()
        {
            PowerChuteData dataFactory = PowerChuteData.getInstance();
            var currentStatusData = dataFactory.GetCurrentStatusData();
            

            double energyUsage = currentStatusData.m_percentLoad / 100.0 * currentStatusData.m_config_active_power;          

            List<Result> results = new List<Result>();
         
            results.Add(new Result("Load Percent", currentStatusData.m_percentLoad)
            {               
                Unit = UnitTypes.Percent                
            });
            results.Add(new Result("Energy Usage", energyUsage)
            {
                Unit = UnitTypes.Custom,
                CustomUnit = "Watts"
            });            
            return results;
        }
    }
}
