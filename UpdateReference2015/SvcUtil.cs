using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UpdateReference2015
{
    public class SvcUtil 
    {
        public SvcUtil(string path)
        {
            Path = path;
        }

        private string Path { get; set; }

        protected virtual string FileName
        {
            get
            {
                return "SvcUtil.exe";
            }
        }

        public string Directory { get; set; }

        public string Out { get; set; }

        public string Config { get; set; }

        public const string Language_VB = "vb";

        public const string Language_CS = "cs";

        public string Language { get; set; }

        public string NameSpace { get; set; }

        public const string CollectionType_List = "System.Collections.Generic.List`1";

        public string CollectionType { get; set; }

        public bool Async { get; set; }

        public bool NoLogo { get; set; }

        public bool NoConfig { get; set; }

        public bool MergeConfig { get; set; }

        public bool Serializable { get; set; }

        public bool EnableDataBinding { get; set; }

        public string ServiceURI { get; set; }

        private string GetArgumentString(string argumentName, string value)
        {
            return !string.IsNullOrEmpty(value) ? string.Format("/{0}:{1} ", argumentName, value): string.Empty;
        }

        private string GetArgumentString(string argumentName, bool value)
        {
            return value ? string.Format("/{0} ", argumentName) : string.Empty;
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            var argumentsBuilder = new StringBuilder();
            argumentsBuilder.Append(GetArgumentString("Directory", Directory));
            argumentsBuilder.Append(GetArgumentString("Out", Out));
            argumentsBuilder.Append(GetArgumentString("Config", Config));
            argumentsBuilder.Append(GetArgumentString("Language", Language));
            argumentsBuilder.Append(GetArgumentString("NameSpace", NameSpace));
            argumentsBuilder.Append(GetArgumentString("CollectionType", CollectionType));
            argumentsBuilder.Append(GetArgumentString("Async", Async));
            argumentsBuilder.Append(GetArgumentString("NoLogo", NoLogo));
            argumentsBuilder.Append(GetArgumentString("NoConfig", NoConfig));
            argumentsBuilder.Append(GetArgumentString("MergeConfig", MergeConfig));
            argumentsBuilder.Append(GetArgumentString("Serializable", Serializable));
            argumentsBuilder.Append(GetArgumentString("EnableDataBinding", EnableDataBinding));
            argumentsBuilder.Append(ServiceURI);

            return new ProcessStartInfo()
            {
                FileName = System.IO.Path.Combine(Path, FileName),
                Arguments = argumentsBuilder.ToString(),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
        }

        public string Execute()
        {
            var process = new Process() { StartInfo = CreateProcessStartInfo() };
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
    }

    public class SlSvcUtil : SvcUtil
    {
        public SlSvcUtil(string path) : base(path) { }

        protected override string FileName
        {
            get
            {
                return "SlSvcUtil.exe";
            }
        }
    }
}
