using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Managers
{
    class SettingsManager
    {
        public static String getDataPath()
        {
            return ConfigurationManager.AppSettings["DataFolder"];
        }

        public static String getHouseName()
        {
            ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings["HouseName"];
        }

        public static String getRefreshML()
        {
            return ConfigurationManager.AppSettings["WindowRefresh"];
        }

        public static String getWindowSize(int i)
        {
            ConfigurationManager.RefreshSection("appSettings");
            if (i == 0)
            {
                return ConfigurationManager.AppSettings["MainFormDefaultSize"];
            }
            if (i == 1)
            {
                return ConfigurationManager.AppSettings["FormsDefaultSize"];
            }
            return null;
        }

        public static void setDataPath(String path)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DataFolder"].Value = path;
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static void setHouseName(String name)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["HouseName"].Value = name;
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static void setWindowSize(int i, String path)
        {
            if (i == 0)
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["MainFormDefaultSize"].Value = path;
                config.Save(ConfigurationSaveMode.Modified);
            }
            if (i == 1)
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["FormsDefaultSize"].Value = path;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        internal static void setRefresh(double v)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["WindowRefresh"].Value = v + "";
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
