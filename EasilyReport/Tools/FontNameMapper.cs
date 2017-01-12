using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Win32;

namespace Infolight.EasilyReportTools.Tools
{
    internal class FontNameMapper
    {
        public const string Mingliu = "mingliu";
        public const string PMingliu = "pmingliu";
        public const string Kaiu = "kaiu";

        public static string GetFontFileName(Font font)
        {
            string fontFileName = String.Empty;
            string fontName = String.Empty;

            fontName = font.FontFamily.GetName(1033);

            switch (fontName.ToLower())
            {
                #region TTC
                case "pmingliu"://ÐÂ¼šÃ÷ów
                    fontFileName = Mingliu + FontFileType.ttc + ",1";
                    break;

                case "dfkai-sb"://˜Ë¿¬ów
                    fontFileName = Kaiu + FontFileType.ttf;
                    break;

                case "simsun":
                case "mingliu"://¼šÃ÷ów
                case "cambria":
                    fontFileName = fontName + FontFileType.ttc + ",0";
                    break;

                case "nsimsun":
                case "nmingliu":
                    fontFileName = fontName.Substring(1) +FontFileType.ttc + ",1";
                    break;

                case "cambria math":
                    fontFileName = "cambria" + FontFileType.ttc + ",1";
                    break;

                case "meiryo":
                    fontFileName = fontName + FontFileType.ttc + ",0";
                    break;

                case "gulim":
                case "batang":
                    fontFileName = fontName + FontFileType.ttc + ",0";
                    break;

                case "ms gothic":
                    fontFileName = "msgothic" + FontFileType.ttc + ",0";
                    break;

                case "gulimche":
                    fontFileName = fontName + FontFileType.ttc + ",1";
                    break;

                case "batangche":
                    fontFileName = "batang" + FontFileType.ttc + ",1";
                    break;

                case "ms pgothic":
                    fontFileName = "msgothic" + FontFileType.ttc + ",1";
                    break;

                case "dotum":
                    fontFileName = "gulim" + FontFileType.ttc + ",2";
                    break;

                case "gungsuh":
                    fontFileName = "batang" + FontFileType.ttc + ",2";
                    break;

                case "ms ui gothic":
                    fontFileName = "msgothic" + FontFileType.ttc + ",2";
                    break;

                case "dotumche":
                    fontFileName = "gulim" + FontFileType.ttc + ",3";
                    break;

                case "gungsuhche":
                    fontFileName = "batang" + FontFileType.ttc + ",3";
                    break;

                case "ms mincho":
                    fontFileName = "msmincho" + FontFileType.ttc + ",0";
                    break;

                case "ms pmincho":
                    fontFileName = "msmincho" + FontFileType.ttc + ",1";
                    break;
                #endregion

                #region TTF
                //case "basemic symbol":
                //    fontFileName = "BASES___" + FontFileType.ttf;
                //    break;

                //case "basemic times":
                //    fontFileName = "BASET___" + FontFileType.ttf;
                //    break;

                //case "basemicnew":
                //    fontFileName = "BASEN___" + FontFileType.ttf;
                //    break;

                //case "kingsoft phonetic":
                //    fontFileName = "Ksphonet" + FontFileType.ttf;
                //    break;

                #endregion

                default:
                    fontFileName = GetRegValue(fontName + " (TrueType)");
                    break;
            }

            return fontFileName;
        }

        private static string GetRegValue(string key)
        {
            string value = "";
            RegistryKey rk = null;
            rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\MicroSoft\\Windows NT\\CurrentVersion\\Fonts");

            if (rk != null)
            {
                value = rk.GetValue(key).ToString();
            }

            return value;
        }
    }

    internal class FontFileType
    {
        public const string ttc = ".ttc";
        public const string ttf = ".ttf";
        public const string fon = ".fon";
    }
}
