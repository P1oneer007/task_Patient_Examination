using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    class Configuration
    {
        public static int ElementsOnPage
        {
            get
            {
                int res;
                try
                {
                    res = Convert.ToInt32(ReadSetting("ElementsOnPage"));
                }
                catch (Exception)
                {
                    res = 20;
                    return res;
                }
                if (res <= 0) res = 20;
                return res;
            }
        }

        public static void SetElementsOnPage(int value)
        {
            AddUpdateAppSettings("ElementsOnPage", value.ToString());
        }

        public static int ConsoleHeight
        {
            get
            {
                int resHeight = MainPanelHeight + InfoPanelHeight + ComandPanelHeight;
                return resHeight;
            }
        }

        public const int ConsoleWidth = 70;//

        public static int MainPanelHeight
        {
            get { return ElementsOnPage + 2; }
        }

        public const int InfoPanelHeight = 3;//размер информационной панели

        public const int ComandPanelHeight = 3;

        public static int MessagesPosition
        {
            get { return Configuration.MainPanelHeight + Configuration.InfoPanelHeight + 1; }
        }

        public static int CommandPosition
        {
            get { return Configuration.MainPanelHeight + Configuration.InfoPanelHeight; }
        }


        private static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }
    }

}
