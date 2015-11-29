using Microsoft.Win32;
using System;

namespace PowerChuteCS
{
    internal class RegistryHandler
    {
        private bool regWritable = false;

        private string BASE_REG_KEY = string.Empty;

        private static RegistryHandler registryHandlerObj = null;

        private RegistryHandler()
        {
            this.BASE_REG_KEY = "SOFTWARE\\APC\\PowerChute Personal Edition\\3.02.00";
        }

        public static RegistryHandler GetRegistryHandlerInstance()
        {
            if (RegistryHandler.registryHandlerObj == null)
            {
                RegistryHandler.registryHandlerObj = new RegistryHandler();
            }
            return RegistryHandler.registryHandlerObj;
        }

        public RegistryKey GetRegKeyHandle(string key)
        {
            key = this.BASE_REG_KEY + "\\" + key;
            RegistryKey result;
            //if (MainFrame.GetUIControlObject().MGDIsAdmin())
            //{
            //    this.regWritable = true;
            //    result = Registry.LocalMachine.OpenSubKey(key, true);
            //}
            //else
            {
                result = Registry.LocalMachine.OpenSubKey(key);
            }
            return result;
        }

        public RegistryKey GetBaseRegKeyHandle()
        {
            return Registry.LocalMachine.OpenSubKey(this.BASE_REG_KEY);
        }

        public object GetRegKeyValue(string attribute, RegistryKey rKey)
        {
            string text = string.Empty;
            object result;
            if (rKey != null)
            {
                object value = rKey.GetValue(attribute);
                text = (value as string);
                if (text == null)
                {
                    result = value;
                    return result;
                }
            }
            result = text;
            return result;
        }

        public string[] GetRegKeyValues(string[] attributes, RegistryKey rKey)
        {
            int num = attributes.Length;
            string[] array = new string[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = (string)this.GetRegKeyValue(attributes[i], rKey);
            }
            return array;
        }

        public void SetRegKeyValue(string key, object value, RegistryKey rKey)
        {
            if (rKey != null)
            {
                if (this.regWritable)
                {
                    rKey.SetValue(key, value);
                }
            }
        }
    }
}
