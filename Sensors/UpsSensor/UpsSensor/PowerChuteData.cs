using Microsoft.Win32;
using System;
using System.Collections;
using UIControl;

namespace PowerChuteCS
{
    public class PowerChuteData
    {
        private CUIControl UIControlObj = null;

        private static RegistryHandler regHandler;

        private static PowerChuteData powerChuteDataInstance;

        private PowerChuteData()
        {
            this.UIControlObj = new CUIControl();
            this.UIControlObj.Initialize();
        }

        public static PowerChuteData getInstance()
        {
            PowerChuteData result;
            if (PowerChuteData.powerChuteDataInstance == null)
            {
                result = new PowerChuteData();
            }
            else
            {
                result = PowerChuteData.powerChuteDataInstance;
            }
            return result;
        }

        public MGD_CurrStatus_Params GetCurrentStatusData()
        {
            MGD_CurrStatus_Params result;
            try
            {
                MGD_CurrStatus_Params mGD_CurrStatus_Params = new MGD_CurrStatus_Params();
                this.UIControlObj.GetCurrentStatus(mGD_CurrStatus_Params);
                result = mGD_CurrStatus_Params;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public MGD_Voltage_Params GetVoltageParameters()
        {
            MGD_Voltage_Params result;
            try
            {
                MGD_Voltage_Params mGD_Voltage_Params = new MGD_Voltage_Params();
                this.UIControlObj.GetVoltageParams(mGD_Voltage_Params);
                result = mGD_Voltage_Params;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void SetVoltageParameters(MGD_Voltage_Params volParams)
        {
            this.UIControlObj.SetVoltageParams(volParams);
        }

        public MGD_Perf_Params GetPerformanceData(ulong noOfWeeks)
        {
            return new MGD_Perf_Params();
        }

        public double[] GetSEQOutletDelayData()
        {
            double[] array = new double[3];
            double[] array2 = array;
            for (int i = 0; i < 3; i++)
            {
                array2[i] = this.UIControlObj.MGDGetOutletDelay(i + 1);
            }
            return array2;
        }

        public void SetSEQOutletDelay(double[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != -1.0)
                {
                    this.UIControlObj.MGDSetOutletDelay(i + 1, values[i]);
                }
            }
        }

        public void EnableNotificationSounds(int enable)
        {
            this.UIControlObj.MGDEnableNotificationSound(enable);
        }

        public string GetRemainingBatteryTime()
        {
            double num = this.UIControlObj.MGDGetRemainingBatteryTime();
            int num2 = Convert.ToInt32(num / 60.0);
            string result;
            if (num > 119.0)
            {
                result = num2 + " mins";
            }
            else
            {
                result = num2 + " min";
            }
            return result;
        }

        public ArrayList GetEvents()
        {
            ArrayList arrayList = new ArrayList();
            this.UIControlObj.MGDGetEvents(arrayList);
            return arrayList;
        }

        public ArrayList GetUPSSupportedFeatures()
        {
            PowerChuteData.regHandler = RegistryHandler.GetRegistryHandlerInstance();
            ArrayList arrayList = new ArrayList();
            this.UIControlObj.MGDGetSupportedFeatures(arrayList);
            if (PowerChuteData.IsNormalVoltUPS())
            {
                arrayList.Add("NormalVoltUPS");
            }
            if (PowerChuteData.IsAdvancedUPS())
            {
                arrayList.Add("AdvancedUPS");
            }
            if (PowerChuteData.IsSelfTestSupported())
            {
                arrayList.Add("SelfTest");
            }
            if (PowerChuteData.LongRunUPS())
            {
                arrayList.Add("Long Run UPS");
            }
            return arrayList;
        }

        private static bool IsNormalVoltUPS()
        {
            RegistryKey regKeyHandle = PowerChuteData.regHandler.GetRegKeyHandle("Technical Data");
            return PowerChuteData.regHandler.GetRegKeyValue("NormalVoltageUPS", regKeyHandle).ToString().Equals("1");
        }

        private static bool IsAdvancedUPS()
        {
            RegistryKey regKeyHandle = PowerChuteData.regHandler.GetRegKeyHandle("Settings");
            return PowerChuteData.regHandler.GetRegKeyValue("AdvancedUPS", regKeyHandle).ToString().Equals("1");
        }

        private static bool IsSelfTestSupported()
        {
            RegistryKey regKeyHandle = PowerChuteData.regHandler.GetRegKeyHandle("Technical Data");
            return PowerChuteData.regHandler.GetRegKeyValue("TestUsage", regKeyHandle).ToString().Equals("1");
        }

        private static bool LongRunUPS()
        {
            RegistryKey regKeyHandle = PowerChuteData.regHandler.GetRegKeyHandle("Technical Data");
            return PowerChuteData.regHandler.GetRegKeyValue("LongrunUPS", regKeyHandle).ToString().Equals("1");
        }
    }
}
