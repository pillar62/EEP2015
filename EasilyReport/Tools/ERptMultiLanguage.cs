using System;
using System.Collections.Generic;
using System.Text;
using Infolight.EasilyReportTools.Properties;
using Srvtools;
using System.Reflection;
using System.ComponentModel;
using System.Resources;

namespace Infolight.EasilyReportTools.Tools
{
    internal class ERptMultiLanguage
    {

        private static Type resourcesType;
        static ERptMultiLanguage()
        {
            resourcesType = Type.GetType("EasilyReport.Properties.Resources");
        }
        
        public static string GetLanValue(string key)
        {
            string lanValue;
            string resValue;

            resValue = GetResourcesValue(key);
            switch (CliUtils.fClientLang)
            {
                case SYS_LANGUAGE.TRA:
                    lanValue = resValue.Split(',')[1];
                    break;

                case SYS_LANGUAGE.SIM:
                    lanValue = resValue.Split(',')[2];
                    break;
                
                default:
                    lanValue = resValue.Split(',')[0];
                    break;
            }

            return lanValue;
        }

        public static string GetResourcesValue(string resKey)
        {
            string resValue;
            Resources resources;
            ResourceManager resourceManager;
            resources = new Resources();
            resourceManager = new ResourceManager(resources.GetType());
            resValue = resourceManager.GetString(resKey);
            return resValue;
        }
    }
}
