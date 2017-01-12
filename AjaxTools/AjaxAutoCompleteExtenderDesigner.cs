using System.Web.UI.WebControls;
using System.Web.UI;
using AjaxControlToolkit.Design;

namespace AjaxTools
{
#if VS90
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2117:AptcaTypesShouldOnlyExtendAptcaBaseTypes", Justification = "Security handled by base class")]
#endif
    public class AjaxAutoCompleteExtenderDesigner : ExtenderControlBaseDesigner<AjaxAutoCompleteExtender>
    {
#if VS90
        [PageMethodSignature("AutoComplete", "ServicePath", "ServiceMethod", "UseContextKey")]
        private delegate string[] GetCompletionList(string prefixText, int count, string contextKey);
#endif
    }
}
