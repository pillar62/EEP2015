using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFClientTools.EFServerReference;

namespace JQClientTools
{
    public class JQMessageProvider
    {
        public EFBase.MessageProvider UserMessageProvider { get; set; }
        public EFBase.MessageProvider MessageProvider { get; set; }

        public JQMessageProvider(string currentDirectory, ClientInfo clientInfo, string locale)
        {
            if (clientInfo != null && !string.IsNullOrEmpty(clientInfo.SDDeveloperID))
            {
                UserMessageProvider = new EFBase.MessageProvider(System.IO.Path.Combine(currentDirectory, string.Format("preview{0}", clientInfo.SDDeveloperID)), locale);
            }
            MessageProvider = new EFBase.MessageProvider(currentDirectory, locale);
        }

        public string this[string key]
        {
            get
            {
                var message = string.Empty;
                if (UserMessageProvider != null)
                {
                    message = UserMessageProvider[key];
                }
                if (string.IsNullOrEmpty(message))
                {
                    message = MessageProvider[key];
                }
                return message;
            }
        }
    }

    public class JQPlaceholderProvider
    {
        public EFBase.PlaceholderProvider PlaceholderProvider { get; set; }
        public JQPlaceholderProvider(string currentDirectory, ClientInfo clientInfo, string locale)
        {
            PlaceholderProvider = new EFBase.PlaceholderProvider(currentDirectory, locale);
        }

        public string this[string key]
        {
            get
            {
                var message = string.Empty;
                if (string.IsNullOrEmpty(message))
                {
                    message = PlaceholderProvider[key];
                }
                return message;
            }
        }

    }
}
