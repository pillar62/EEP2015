using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.Design;

namespace AjaxTools
{
    public class ExtResourceFile : InfoOwnerCollectionItem
    {
        public ExtResourceFile()
        {
        }

        public ExtResourceFile(string fileUrl)
        {
            _fileUrl = fileUrl;
        }

        string _fileUrl = "";
        ResourceFileType _fileType = ResourceFileType.Javascript;

        [DefaultValue("")]
        [NotifyParentProperty(true)]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
        }

        [DefaultValue(typeof(ResourceFileType), "Javascript")]
        [NotifyParentProperty(true)]
        public ResourceFileType FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
        }

        public override string ToString()
        {
            return _fileUrl;
        }
    }

}
