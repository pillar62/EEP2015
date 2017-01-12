using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;

public class CodeHelper
{
    public CodeHelper()
    {
        TabPostion = 0;
        CodeWriter = new StringBuilder();
        LocalScript = true;
    }

    private int TabPostion { get; set; }
    private StringBuilder CodeWriter { get; set; }

    public bool LocalScript { get; set; }

    public string ScriptVersion { get; set; }

    public string UseMap { get; set; }

    #region Component.cs
    private string[] ComponentUsing = new string[] { 
        "System",
        "System.Collections.Generic",
        "System.ComponentModel",
        "System.Text",
        "Srvtools"
    };
    public string CreateComponentCode(string name, JArray components, string code)
    {
        ValidateCode(code, null);
        AppendUsingPart(ComponentUsing);
        AppendLine();
        AppendLine("namespace {0}", name);                          //  namespace {0}
        AppendBeginBracket();                                       //  {
        AppendLine("public partial class Component : DataModule");  //      public partial class Component : DataModule
        AppendBeginBracket();                                       //      {
        AppendComponentMembers();                                   //
        AppendInitializeComponent(components);                      //
        AppendLine();                                               //
        AppendFields(components);                                   //
        AppendLine();                                               //
        if (!string.IsNullOrEmpty(code))                            //
        {                                                           //
            foreach (var line in code.Split('\n'))                  //
            {                                                       //
                AppendLine(line);                                   //
            }                                                       //
        }                                                           //
        AppendEndBracket();                                         //      }
        AppendEndBracket();                                         //  }
        return CodeWriter.ToString();
    }

    private void AppendComponentMembers()
    {
        AppendLine("public Component()");                               //  public Component()
        AppendBeginBracket();                                           //  {
        AppendLine("InitializeComponent();");                           //      InitializeComponent();
        AppendEndBracket();                                             //  }
        AppendLine();
        AppendLine("public Component(IContainer container)");           //  public Component(IContainer container)  
        AppendBeginBracket();                                           //  {
        AppendLine("container.Add(this);");                             //      container.Add(this);
        AppendLine("InitializeComponent();");                           //      InitializeComponent();
        AppendEndBracket();                                             //  }
        AppendLine();
        AppendLine("protected override void Dispose(bool disposing)");  //  protected override void Dispose(bool disposing)
        AppendBeginBracket();                                           //  {
        AppendLine("if (disposing && (components != null))");           //      if (disposing && (components != null))
        AppendBeginBracket();                                           //      {
        AppendLine("components.Dispose();");                            //          components.Dispose();
        AppendEndBracket();                                             //      }
        AppendLine("base.Dispose(disposing);");                         //      base.Dispose(disposing);
        AppendEndBracket();                                             //  }
        AppendLine();
        AppendLine("private System.ComponentModel.IContainer components = null;");  //  private System.ComponentModel.IContainer components = null;
    }

    private void AppendFields(JArray components)
    {
        foreach (var component in components)
        {
            var type = (string)component["type"];
            var id = (string)(component["properties"] as JObject)["ID"];
            AppendLine("private {0} {1};", type, id);
        }
    }

    private List<string> CollectionItems = new List<string>();
    private Dictionary<string, Type> Members = new Dictionary<string, Type>();
    private void AppendInitializeComponent(JArray components)
    {
        var memberPart = new List<string>();

        foreach (var component in components)
        {
            var componentType = (string)component["type"];
            var ct = componentType.Split('.');
            var type = JsonHelper.GetType(ct[0], componentType);
            var properties = component["properties"] as JObject;
            var id = (string)properties["ID"];
            //add name 
            properties["Name"] = id;
            memberPart.AddRange(AppendMember(id, type, properties, true));
        }
        AppendLine("private void InitializeComponent()");
        AppendBeginBracket();
        AppendLine("this.components = new System.ComponentModel.Container();");
        foreach (var member in Members.Where(c => !typeof(Component).IsAssignableFrom(c.Value)))
        {
            AppendLine("{0} {1} = new {0}();", member.Value.FullName, member.Key);
        }
        foreach (var member in Members.Where(c => typeof(Component).IsAssignableFrom(c.Value)))
        {
            AppendLine("this.{0} = new {1}(this.components);", member.Key, member.Value.FullName);
        }
        foreach (var member in Members.Where(c => typeof(ISupportInitialize).IsAssignableFrom(c.Value)))
        {
            AppendLine("((System.ComponentModel.ISupportInitialize)(this.{0})).BeginInit();", member.Key);
        }
        foreach (var part in memberPart)
        {
            AppendLine(part);
        }
        foreach (var member in Members.Where(c => typeof(ISupportInitialize).IsAssignableFrom(c.Value)))
        {
            AppendLine("((System.ComponentModel.ISupportInitialize)(this.{0})).EndInit();", member.Key);
        }

        AppendEndBracket();
    }

    private string GetNewMemberID(Type type)
    {
        for (int i = 1; ; i++)
        {
            var newMemberID = string.Format("{0}{1}", type.Name, i);
            if (!Members.ContainsKey(newMemberID))
            {
                return newMemberID;
            }
        }
    }

    private List<string> AppendMember(string id, Type type, JObject properties, bool isField)
    {
        var list = new List<string>();
        var MemberName = isField ? string.Format("this.{0}", id) : id;
        Members.Add(id, type);
        if (isField)
        {
            list.Add("//");
            list.Add(string.Format("// {0}", id));
            list.Add("//");
        }
        foreach (var p in properties.Properties().OrderBy(c => c.Name))
        {
            if (p.Name == "ID")
            {
                continue;
            }
            var property = type.GetProperty(p.Name);
            if (property != null)
            {
                if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                {
                    var value = (JArray)properties[p.Name];
                    if (value != null && value.Count > 0)
                    {
                        var itemType = property.PropertyType.GetProperty("Item"
                            , System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).PropertyType;

                        foreach (JObject item in value)
                        {
                            var itemID = GetNewMemberID(itemType);
                            item["ID"] = itemID;
                            list.AddRange(AppendMember(itemID, itemType, item, false));
                        }
                        foreach (JObject item in value)
                        {
                            var itemID = (string)item["ID"];
                            list.Add(string.Format("{0}.{1}.Add({2});", MemberName, p.Name, itemID));
                        }
                    }
                }
                else
                {
                    var value = ((JValue)properties[p.Name]).ToString();
                    if (property.PropertyType.IsEnum)
                    {
                        value = string.Format("{0}.{1}", property.PropertyType.FullName, value);
                    }
                    else if (property.PropertyType == typeof(TimeSpan))
                    {
                        value = string.Format("System.TimeSpan.Parse(\"{0}\")", value);
                    }
                    else if (typeof(Component).IsAssignableFrom(property.PropertyType))
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            value = "null";
                        }
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        value = string.Format("\"{0}\"", value.Replace("\n", "\\n"));
                    }
                    list.Add(string.Format("{0}.{1} = {2};", MemberName, p.Name, value));
                }
            }
            else
            {
                var eventInfo = type.GetEvent(p.Name);
                if (eventInfo != null)
                {
                    var value = ((JValue)properties[p.Name]).ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        list.Add(string.Format("{0}.{1} += new {2}({3});", MemberName, p.Name, eventInfo.EventHandlerType.FullName, value));
                    }
                }
            }
        }
        return list;
    }

    #endregion

    #region aspx.cs
    private string[] AspxUsing = new string[] { 
        "System", 
        "System.Collections.Generic", 
        "System.Web", 
        "System.Web.UI", 
        "System.Web.UI.WebControls"
    };
    public string CreateAspxCode(string name, string code)
    {
        AppendUsingPart(AspxUsing);
        AppendLine();
        AppendLine("public partial class {0} : System.Web.UI.Page", name);  //  public partial class {0}_{1} : System.Web.UI.Page
        AppendBeginBracket();                                               //  {
        AppendAspxMembers();                                                //
        AppendLine();                                                       //
        if (!string.IsNullOrEmpty(code))                                    //
        {                                                                   //
            foreach (var line in code.Split('\n'))                          //
            {                                                               //
                AppendLine(line);                                           //
            }                                                               //
        }                                                                   //
        AppendEndBracket();                                                 //  }
        return CodeWriter.ToString();
    }

    private void AppendAspxMembers()
    {
        //AppendLine("protected void Page_Load(object sender, EventArgs e)");     //  protected void Page_Load(object sender, EventArgs e)
        //AppendBeginBracket();                                                   //  {
        //AppendEndBracket();                                                     //  }
        //AppendLine();
        AppendLine("public override void ProcessRequest(HttpContext context)"); //  public override void ProcessRequest(HttpContext context)
        AppendBeginBracket();                                                   //  {
        AppendLine("if (!JqHttpHandler.ProcessRequest(context))");              //      if (!JqHttpHandler.ProcessRequest(context))
        AppendBeginBracket();                                                   //      {
        AppendLine("base.ProcessRequest(context);");                            //          base.ProcessRequest(context);
        AppendEndBracket();                                                     //      }
        AppendEndBracket();                                                     //  }
    }

    #endregion

    #region aspx
    public string CreateAspx(string name, JArray controls, string script, bool mobile)
    {
        AppendLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"{0}.aspx.cs\" Inherits=\"{0}\" %>", name); //  <%@ Page Language="C#" AutoEventWireup="true" CodeFile="{0}.aspx.cs" Inherits="{0}" %>
        AppendLine();
        if (mobile)
        {
            AppendLine("<%@ Register Assembly=\"JQMobileTools\" Namespace=\"JQMobileTools\" TagPrefix=\"JQMobileTools\" %>");   //  <%@ Register Assembly=\"JQMobileTools\" Namespace=\"JQMobileTools\" TagPrefix=\"JQMobileTools\" %>

        }
        else
        {
            AppendLine("<%@ Register Assembly=\"JQClientTools\" Namespace=\"JQClientTools\" TagPrefix=\"JQClientTools\" %>");   //  <%@ Register Assembly=\"JQClientTools\" Namespace=\"JQClientTools\" TagPrefix=\"JQClientTools\" %>
        }
        AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");    //  <html xmlns="http://www.w3.org/1999/xhtml">
        AppendAspxHeader(name, script);
        AppendAspxBody(controls, mobile);
        AppendLine("</html>");                                          //  </html>
        return CodeWriter.ToString();
    }

    private void AppendAspxHeader(string name, string script)
    {
        AppendBeginTag("head", "runat=\"server\"");
        AppendLine("<title>{0}</title>", name);
        //if (!string.IsNullOrEmpty(styles))
        //{
        //    AppendBeginTag("style");
        //    AppendLine(styles);
        //    AppendEndTag();
        //}
        if (!string.IsNullOrEmpty(script))
        {
            if (!string.IsNullOrEmpty(ScriptVersion))
            {
                AppendBeginTag("script", string.Format("src=\"{0}.js\"", ScriptVersion));
                AppendEndTag();
            }
            else
            {
                AppendBeginTag("script");
                foreach (var line in script.Split('\n'))
                {
                    AppendLine(line);
                }
                AppendEndTag();
            }
        }
        AppendEndTag();
    }

    private void AppendAspxBody(JArray controls, bool mobile)
    {
        AppendBeginTag("body");
        AppendBeginTag("form", "id=\"form1\" runat=\"server\"");
        if (!HasControl(controls, new Type[] { typeof(JQClientTools.JQScriptManager), typeof(JQMobileTools.JQScriptManager) }))
        {
            //append scriptmanager
            var scriptManager = new JObject();
            scriptManager["ID"] = "ScriptManager";
            scriptManager["LocalScript"] = LocalScript.ToString().ToLower();
            
            if (HasControl(controls, new Type[] { typeof(JQChartTools.JQBarChart), typeof(JQChartTools.JQPieChart), typeof(JQChartTools.JQLineChart) }))
            {
                scriptManager["UseChartJS"] = "true";
            }
            if (mobile)
            {
                scriptManager["Theme"] = "b";
                if (!string.IsNullOrEmpty(UseMap))
                {
                    scriptManager["UseMap"] = UseMap;
                }
                AppendControl(typeof(JQMobileTools.JQScriptManager), scriptManager, null);
            }
            else
            {
                AppendControl(typeof(JQClientTools.JQScriptManager), scriptManager, null);
            }
        }
        foreach (JObject control in controls)
        {
            AppendControl(control);
        }
        AppendEndTag();
        AppendEndTag();
    }

    private bool HasControl(JArray controls, Type[] controlTypes)
    {
        foreach (JObject control in controls)
        {
            var type = (string)control["type"];
            if (controlTypes.Count(c => string.Compare(c.FullName, type, true) == 0) > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void AppendControl(JObject control)
    {
        var controlType = (string)control["type"];
        var ct = controlType.Split('.');
        var type = JsonHelper.GetType(ct[0], controlType);
        var properties = control["properties"] as JObject;
        var childControls = control["children"] as JArray;
        AppendControl(type, properties, childControls);
    }

    private void AppendControl(Type type, JObject properties, JArray childControls)
    {
        var obj = type.GetConstructor(new Type[] { }).Invoke(null);
        var list = new List<string>();
        if (typeof(Control).IsAssignableFrom(type))
        {
            list.Add("runat=\"server\"");
        }
        var collectionProperties = new List<PropertyInfo>();
        foreach (var p in properties.Properties())
        {
            var property = type.GetProperty(p.Name);
            if (property != null && JsonHelper.CanEdit(property))
            {
                if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                {
                    collectionProperties.Add(property);
                }
                else
                {
                    if (properties[property.Name] is JValue)
                    {
                        var value = (JValue)properties[property.Name];
                        if (value.Type == JTokenType.String && string.IsNullOrEmpty((string)value.Value))
                        {
                        }
                        else
                        {
                            list.Add(string.Format("{0}=\"{1}\"", property.Name, value));
                        }
                    }
                    else if (properties[property.Name] is JObject)
                    {
                        var attribute = (EditorAttribute)Attribute.GetCustomAttribute(property, typeof(EditorAttribute), false);
                        if (attribute != null)
                        {
                            var editorType = Type.GetType(attribute.EditorTypeName);
                            if (editorType == typeof(JQClientTools.EditorOptionsEditor) || editorType == typeof(JQMobileTools.EditorOptionsEditor))
                            {
                                //edtioroption
                                var editor = (string)properties["Editor"];

                                var editorObjType = JsonHelper.GetEditControlType(editor, type.FullName);
                                if (editorObjType != null)
                                {
                                    var editorObj = CreateItem(editorObjType, properties[property.Name] as JObject);
                                    var optionProperty = editorObjType.GetProperty("InfolightOptions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (optionProperty == null)
                                    {
                                        optionProperty = editorObjType.GetProperty("DataOptions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    }
                                    if (optionProperty == null)
                                    {
                                        optionProperty = editorObjType.GetProperty("Options", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    }
                                    var value = optionProperty.GetValue(editorObj, null);
                                    list.Add(string.Format("{0}=\"{1}\"", property.Name, value));
                                }
                            }
                        }
                    }
                }
            }
        }
        if (collectionProperties.Count > 0)
        {
            AppendBeginTag(string.Format("{0}:{1}", type.Namespace, type.Name), string.Join(" ", list));
            foreach (var property in collectionProperties)
            {
                AppendBeginTag(property.Name);
                var value = (JArray)properties[property.Name];
                if (value != null && value.Count > 0)
                {
                    var itemType = property.PropertyType.GetGenericArguments()[0];
                    foreach (JObject item in value)
                    {
                        AppendControl(itemType, item, null);
                    }
                }
                AppendEndTag();
            }
            AppendEndTag();
        }
        else if (childControls != null)
        {
            AppendBeginTag(string.Format("{0}:{1}", type.Namespace, type.Name), string.Join(" ", list));
            foreach (JObject childControl in childControls)
            {
                AppendControl(childControl);
            }
            AppendEndTag();
        }
        else
        {
            AppendLine("<{0}:{1} {2}/>", type.Namespace, type.Name, string.Join(" ", list));
        }
    }

    private object CreateItem(Type type, JObject properties)
    {
        var obj = type.GetConstructor(new Type[] { }).Invoke(null);
        foreach (var p in properties.Properties())
        {
            var property = type.GetProperty(p.Name);
            if (property != null && JsonHelper.CanEdit(property))
            {
                if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                {
                    var list = (System.Collections.IList)property.GetValue(obj, null);
                    var itemType = property.PropertyType.GetGenericArguments()[0];
                    var items = (JArray)properties[property.Name];
                    foreach (JObject item in items)
                    {
                        list.Add(CreateItem(itemType, item));
                    }
                }
                else
                {
                    var value = ((JValue)properties[property.Name]).Value;
                    if (value.ToString().Length > 0)
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            property.SetValue(obj, Enum.Parse(property.PropertyType, value.ToString()), null);
                        }
                        else if (property.PropertyType == typeof(Unit))
                        {
                            property.SetValue(obj, Unit.Parse(value.ToString()), null);
                        }
                        else
                        {
                            property.SetValue(obj, Convert.ChangeType(value, property.PropertyType), null);
                        }
                    }
                }
            }
        }
        return obj;
    }

    #endregion

    public string CreateXoml(string name, JArray controls)
    {
        //save xoml
        System.IO.FileStream fStream = new System.IO.FileStream(name, System.IO.FileMode.OpenOrCreate);
        try
        {
            XmlTextWriter writer = new XmlTextWriter(fStream, new System.Text.UTF8Encoding());
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.WriteStartDocument();// ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            writer.WriteStartElement("ns0", "FLSequentialWorkflow", "clr-namespace:FLTools;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            writer.WriteEndElement();
            writer.Close();
        }
        finally
        {
            fStream.Close();
        }
        fStream = new System.IO.FileStream(name, System.IO.FileMode.Open);

        try
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(fStream);
            XmlNode root = doc.SelectSingleNode("FLSequentialWorkflow");
            XmlElement xeRoot = doc.DocumentElement;
            JObject mainProperty = null;
            foreach (JObject item in controls)
            {
                if (item["type"] != null && item["type"].ToString() == "mainProperty")
                {
                    mainProperty = item;
                    break;
                }
            }
            foreach (var item in mainProperty["properties"]["rows"])
            {
                if (item["name"] != null && item["value"] != null)
                {
                    if (item["name"].ToString() == "Description")
                    {
                        XmlAttribute aXmlAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                        aXmlAttribute.Value = "FLSequentialWorkflow";// item["value"].ToString();
                        xeRoot.Attributes.Append(aXmlAttribute);

                        aXmlAttribute = doc.CreateAttribute("Description");
                        aXmlAttribute.Value = item["value"].ToString();
                        xeRoot.Attributes.Append(aXmlAttribute);
                    }
                    if (item["name"].ToString() == "Keys")
                    {
                        var keys = "";
                        foreach (var key in item["value"])
                        {
                            keys += key["KeyName"].ToString() + ",";
                        }
                        if (!String.IsNullOrEmpty(keys))
                            keys = keys.Substring(0, keys.Length - 1);
                        xeRoot.SetAttribute(item["name"].ToString(), keys);
                    }
                    else
                        xeRoot.SetAttribute(item["name"].ToString(), item["value"].ToString());
                }
            }
            //XmlAttribute xmlnsAttribute = doc.CreateAttribute("xmlns");
            //xmlnsAttribute.Value = "http://schemas.microsoft.com/winfx/2006/xaml/workflow";
            //xeRoot.Attributes.Append(xmlnsAttribute);

            JToken currentProperty = FindNextActivity("start_node", controls);
            while (currentProperty != null && currentProperty["type"].ToString() != "flowend")
            {
                if (currentProperty["type"].ToString().LastIndexOf("_S") != -1)
                {
                    String type = "";
                    String branchType = "";
                    if (currentProperty["type"].ToString().IndexOf("flowIfElseActivity") != -1)
                    {
                        type = "IfElseActivity";
                        branchType = "IfElseBranchActivity";
                    }
                    else
                    {
                        type = "ParallelActivity";
                        branchType = "SequenceActivity";
                    }
                    XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "", type, "http://schemas.microsoft.com/winfx/2006/xaml/workflow");
                    XmlAttribute newAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                    newAttribute.Value = currentProperty["id"].ToString();
                    newNode.Attributes.Append(newAttribute);
                    if (currentProperty["properties"] != null)
                    {
                        for (var j = 0; j < currentProperty["properties"].Count(); j++)
                        {
                            if (currentProperty["properties"][j]["name"] != null && currentProperty["properties"][j]["name"].ToString() == "Description")
                            {
                                XmlAttribute dAttribute = doc.CreateAttribute("Description");
                                dAttribute.Value = currentProperty["properties"][j]["value"].ToString();
                                newNode.Attributes.Append(dAttribute);
                            }
                        }
                    }

                    xeRoot.AppendChild(newNode);

                    var branchCount = 0;
                    foreach (var item in controls)
                    {
                        if (item["id"] != null && item["id"].ToString().IndexOf(currentProperty["classify"] + "_") != -1 &&
                            item["id"].ToString() != currentProperty["classify"] + "_S" && item["id"].ToString() != currentProperty["classify"] + "_E" &&
                            item["type"].ToString() != "sl" && item["type"].ToString() != "lr")
                        {
                            branchCount++;
                        }
                    }
                    for (var i = 0; i < branchCount; i++)
                    {
                        var nId = currentProperty["classify"] + "_" + (i + 1).ToString();
                        var node = FindCurrentActivity(nId, controls);
                        XmlNode aNode = doc.CreateNode(XmlNodeType.Element, "", branchType, "http://schemas.microsoft.com/winfx/2006/xaml/workflow");
                        XmlAttribute aAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                        if (node["name"] != null && node["name"].ToString() != "")
                            aAttribute.Value = node["name"].ToString();
                        else
                            aAttribute.Value = nId;
                        aNode.Attributes.Append(aAttribute);

                        for (var j = 0; j < node["properties"].Count(); j++)
                        {
                            if (node["properties"][j]["name"] != null && node["properties"][j]["name"].ToString() == "Description")
                            {
                                XmlAttribute dAttribute = doc.CreateAttribute("Description");
                                dAttribute.Value = node["properties"][j]["value"].ToString();
                                aNode.Attributes.Append(dAttribute);
                            }
                        }

                        newNode.AppendChild(aNode);

                        if (i == branchCount - 1)
                            currentProperty = WriteNestedActivity(doc, aNode, nId, controls);
                        else
                            WriteNestedActivity(doc, aNode, nId, controls);
                    }
                }
                else
                {
                    XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "ns0", currentProperty["type"].ToString().Replace("FLTools.", String.Empty), "clr-namespace:FLTools;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                    foreach (var item in currentProperty["properties"])
                    {
                        if (item["name"] != null && item["value"] != null)
                        {
                            XmlAttribute newAttribute = null;
                            if (item["name"].ToString() == "name" || item["name"].ToString() == "ID")
                            {
                                newAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                                newAttribute.Value = item["value"].ToString();
                                newNode.Attributes.Append(newAttribute);
                            }
                            else if (item["name"].ToString() == "ApproveRights")
                            {
                                //<ns0:FLApprove.ApproveRights>
                                XmlNode xnApproveRights = doc.CreateNode(XmlNodeType.Element, "ns0", "FLApprove.ApproveRights", "clr-namespace:FLTools;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                                //<ns1:ApproveRightCollection xmlns:ns1="clr-namespace:FLCore;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
                                XmlNode xnApproveRightCollection = doc.CreateNode(XmlNodeType.Element, "ns1", "ApproveRightCollection", "clr-namespace:FLCore;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                                for (var i = 0; i < item["value"].Count(); i++)
                                {
                                    XmlNode xnApproveRight = doc.CreateNode(XmlNodeType.Element, "ns1", "ApproveRight", "clr-namespace:FLCore;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                                    XmlAttribute xaGrade = doc.CreateAttribute("Grade");
                                    xaGrade.Value = item["value"][i]["Grade"].ToString();
                                    xnApproveRight.Attributes.Append(xaGrade);
                                    XmlAttribute xaExpression = doc.CreateAttribute("Expression");
                                    xaExpression.Value = item["value"][i]["Expression"].ToString();
                                    xnApproveRight.Attributes.Append(xaExpression);
                                    xnApproveRightCollection.AppendChild(xnApproveRight);
                                }
                                xnApproveRights.AppendChild(xnApproveRightCollection);
                                newNode.AppendChild(xnApproveRights);
                            }
                            else
                            {
                                newAttribute = doc.CreateAttribute(item["name"].ToString());
                                newAttribute.Value = item["value"].ToString();
                                newNode.Attributes.Append(newAttribute);
                            }
                        }
                    }
                    xeRoot.AppendChild(newNode);
                }

                currentProperty = FindNextActivity(currentProperty["id"].ToString(), controls);
            }
            fStream.Close();
            var sXml = doc.OuterXml;
            CodeWriter = new StringBuilder(sXml);
        }
        finally
        {
            fStream.Close();
            System.IO.File.Delete(name);
        }
        return CodeWriter.ToString();
    }

    private JToken FindCurrentActivity(String id, JArray controls)
    {
        JToken returnValue = null;
        foreach (JObject item in controls)
        {
            if (item["id"] != null && item["id"].ToString().Equals(id))
                returnValue = item;
        }
        return returnValue;
    }

    private JToken FindNextActivity(String pA, JArray controls)
    {
        JToken returnValue = null;
        foreach (JObject item in controls)
        {
            if (item["from"] != null && item["from"].ToString().StartsWith(pA))
            {
                foreach (JObject itemNext in controls)
                {
                    if (itemNext["id"] != null && itemNext["id"].ToString().Equals(item["to"].ToString()))
                    {
                        returnValue = itemNext;
                        break;
                    }
                }
                break;
            }
        }
        return returnValue;
    }

    private JToken WriteNestedActivity(XmlDocument doc, XmlNode parent, String currentId, JArray controls)
    {
        JToken currentProperty = FindNextActivity(currentId, controls);
        while (currentProperty != null && currentProperty["type"].ToString().LastIndexOf("_E") == -1)
        {
            if (currentProperty["type"].ToString().LastIndexOf("_S") != -1)
            {
                String type = "";
                String branchType = "";
                if (currentProperty["type"].ToString().IndexOf("flowIfElseActivity") != -1)
                {
                    type = "IfElseActivity";
                    branchType = "IfElseBranchActivity";
                }
                else
                {
                    type = "ParallelActivity";
                    branchType = "SequenceActivity";
                }

                XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "", type, "http://schemas.microsoft.com/winfx/2006/xaml/workflow");
                XmlAttribute newAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                newAttribute.Value = currentProperty["id"].ToString();
                newNode.Attributes.Append(newAttribute);
                parent.AppendChild(newNode);

                var branchCount = 0;
                foreach (var item in controls)
                {
                    if (item["id"] != null && item["id"].ToString().IndexOf(currentProperty["classify"] + "_") != -1 &&
                        item["id"].ToString() != currentProperty["classify"] + "_S" && item["id"].ToString() != currentProperty["classify"] + "_E" &&
                        item["type"].ToString() != "sl" && item["type"].ToString() != "lr")
                    {
                        branchCount++;
                    }
                }
                for (var i = 0; i < branchCount; i++)
                {
                    var nId = currentProperty["classify"] + "_" + (i + 1).ToString();
                    var node = FindCurrentActivity(nId, controls);
                    XmlNode aNode = doc.CreateNode(XmlNodeType.Element, "", branchType, "http://schemas.microsoft.com/winfx/2006/xaml/workflow");
                    XmlAttribute aAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                    if (node["name"] != null && node["name"].ToString() != "")
                        aAttribute.Value = node["name"].ToString();
                    else
                        aAttribute.Value = nId;
                    aNode.Attributes.Append(aAttribute);
                    for (var j = 0; j < node["properties"].Count(); j++)
                    {
                        if (node["properties"][j]["name"] != null && node["properties"][j]["name"].ToString() == "Description")
                        {
                            XmlAttribute dAttribute = doc.CreateAttribute("Description");
                            dAttribute.Value = node["properties"][j]["value"].ToString();
                            aNode.Attributes.Append(dAttribute);
                        }
                    }
                    newNode.AppendChild(aNode);

                    if (i == branchCount - 1)
                        currentProperty = WriteNestedActivity(doc, aNode, nId, controls);
                    else
                        WriteNestedActivity(doc, aNode, nId, controls);
                }
            }
            else
            {
                XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "ns0", currentProperty["type"].ToString().Replace("FLTools.", String.Empty), "clr-namespace:FLTools;Assembly=FLTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                foreach (var item in currentProperty["properties"])
                {
                    if (item["name"] != null && item["value"] != null)
                    {
                        XmlAttribute newAttribute = null;
                        if (item["name"].ToString() == "name" || item["name"].ToString() == "ID")
                        {
                            newAttribute = doc.CreateAttribute("x", "Name", "http://schemas.microsoft.com/winfx/2006/xaml");
                            newAttribute.Value = item["value"].ToString();
                        }
                        else
                        {
                            newAttribute = doc.CreateAttribute(item["name"].ToString());
                            newAttribute.Value = item["value"].ToString();
                        }
                        newNode.Attributes.Append(newAttribute);
                    }
                }
                parent.AppendChild(newNode);
            }

            currentProperty = FindNextActivity(currentProperty["id"].ToString(), controls);
        }
        return currentProperty;
    }

    private void AppendUsingPart(string[] usings)
    {
        foreach (var u in usings)
        {
            AppendLine("using {0};", u);
        }
    }


    const char BLANK_CHAR = ' ';
    const int BLANK_CHAR_COUNT = 4;

    private void AppendLine()
    {
        CodeWriter.AppendLine();
    }

    private void AppendLine(string value)
    {
        CodeWriter.Append(BLANK_CHAR, TabPostion * BLANK_CHAR_COUNT);
        CodeWriter.AppendLine(value);
    }

    private void AppendLine(string format, params object[] args)
    {
        CodeWriter.Append(BLANK_CHAR, TabPostion * BLANK_CHAR_COUNT);
        CodeWriter.AppendLine(string.Format(format, args));
    }

    private void AppendComment(string comment)
    {
        AppendLine("//");
        AppendLine("// {0}", comment);
        AppendLine("//");
    }

    private void AppendBeginBracket()
    {
        AppendLine("{");
        TabPostion++;
    }

    private void AppendEndBracket()
    {
        TabPostion--;
        AppendLine("}");
    }

    private Stack<string> Tags = new Stack<string>();

    private void AppendBeginTag(string tag)
    {
        AppendBeginTag(tag, null);
    }

    private void AppendBeginTag(string tag, string attributes)
    {
        Tags.Push(tag);
        if (!string.IsNullOrEmpty(attributes))
        {
            AppendLine("<{0} {1}>", tag, attributes);
        }
        else
        {
            AppendLine("<{0}>", tag);
        }
        TabPostion++;
    }

    private void AppendEndTag()
    {
        var tag = Tags.Pop();
        TabPostion--;
        AppendLine("</{0}>", tag);
    }

    private static string[] CodeProhibited = new string[] { 
        "System.IO",
        "System.Diagnostics",
        "System.CodeDom.Compiler"
    };

    public static void ValidateCode(string code, string script)
    {
        if (!string.IsNullOrEmpty(code))
        {
            code = RemoveMark(code);
            foreach (var codeP in CodeProhibited)
            {
                var pattern = codeP.Replace(".", @"\s*\.\s*");
                if (Regex.IsMatch(code, pattern))
                {
                    throw new Exception(string.Format("The type or namespace name '{0}' does not exist in the namespace 'System'", codeP));
                }
            }
        }
        if (!string.IsNullOrEmpty(script))
        {
            if (Regex.IsMatch(script, "(<%|%>)"))
            {
                throw new Exception("script can not contain code blocks (i.e. <% ... %>).");
            }
        }
    }

    private static string RemoveMark(string code)
    {
        var builder = new StringBuilder(code);
        while (true)
        {
            var startIndex = builder.ToString().IndexOf("/*");
            if (startIndex < 0 || startIndex + 2 == builder.Length)
            {
                break;
            }
            var endIndex = builder.ToString().IndexOf("*/", startIndex + 2);
            if (endIndex < 0)
            {
                break;
            }
            builder.Remove(startIndex, endIndex + 2 - startIndex);
        }
        var lines = builder.ToString().Split('\n');
        builder = new StringBuilder();
        foreach (var line in lines)
        {
            var index = line.IndexOf("//");
            if (index > 0)
            {
                builder.AppendLine(line.Substring(0, index));
            }
            else if (index < 0)
            {

                builder.AppendLine(line);
            }
        }
        return builder.ToString(); ;
    }
}

public class RdlcHelper
{
    #region Const

    const string DEFAULT_PREFIX = "xmlns";
    const string DEFAULT_URI = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
    const string RD_PREFIX = "rd";
    const string RD_URI = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";
    const string REPORT_ELEMENT = "Report";

    const string DATASOURCES_ELEMENT = "DataSources";
    const string DATASOURCE_ELEMENT = "DataSource";
    const string CONNECTIONPROPERTIES_ELEMENT = "ConnectionProperties";
    const string DATAPROVIDER_ELEMENT = "DataProvider";
    const string CONNECTSTRING_ELEMENT = "ConnectString";
    const string DATASOURCE_ID_ELEMENT = "DataSourceID";

    const string DATASETS_ELEMENT = "DataSets";
    const string DATASET_ELEMENT = "DataSet";
    const string FIELDS_ELEMENT = "Fields";
    const string FIELD_ELEMENT = "Field";
    const string DATAFIELD_ELEMENT = "DataField";
    const string QUERY_ELEMENT = "Query";
    const string DATASOURCENAME_ELEMENT = "DataSourceName";
    const string COMMANDTEXT_ELEMENT = "CommandText";

    const string BODY_ELEMENT = "Body";
    const string REPORTITEMS_ELEMENT = "ReportItems";

    const string REPORTPARAMETERS_ELEMENT = "ReportParameters";
    const string REPORTPARAMETER_ELEMENT = "ReportParameter";

    const string PAGE_ELEMENT = "Page";
    const string PAGEHEADER_ELEMENT = "PageHeader";
    const string PAGEFOOTER_ELEMENT = "PageFooter";

    const string TABLIX_ELEMENT = "Tablix";
    const string TABLIXBODY_ELEMENT = "TablixBody";
    const string TABLIXHEADER_ELEMENT = "TablixHeader";
    const string TABLIXCOLUMNS_ELEMENT = "TablixColumns";
    const string TABLIXCOLUMN_ELEMENT = "TablixColumn";
    const string TABLIXROWS_ELEMENT = "TablixRows";
    const string TABLIXROW_ELEMENT = "TablixRow";
    const string TABLIXCELLS_ELEMENT = "TablixCells";
    const string TABLIXCELL_ELEMENT = "TablixCell";
    const string RECTANGLE_ELEMENT = "Rectangle";
    const string PARAGRAPHS_ELEMENT = "Paragraphs";
    const string PARAGRAPH_ELEMENT = "Paragraph";
    const string TEXTRUNS_ELEMENT = "TextRuns";
    const string TEXTRUN_ELEMENT = "TextRun";
    const string TABLIXCOLUMNHIERARCHY_ELEMENT = "TablixColumnHierarchy";
    const string TABLIXROWHIERARCHY_ELEMENT = "TablixRowHierarchy";
    const string TABLIXMEMBERS_ELEMENT = "TablixMembers";
    const string TABLIXMEMBER_ELEMENT = "TablixMember";
    const string GROUPEXPRESSIONS_ELEMENT = "GroupExpressions";
    const string GROUPEXPRESSION_ELEMENT = "GroupExpression";
    const string SORTEXPRESSIONS_ELEMENT = "SortExpressions";
    const string SORTEXPRESSION_ELEMENT = "SortExpression";
    const string PAGEBREAK_ELEMENT = "PageBreak";

    const string TEXTBOX_ELEMENT = "Textbox";
    const string IMAGE_ELEMENT = "Image";

    const string STYLE_ELEMENT = "Style";
    const string BORDER_ELEMENT = "Border";
    const string TOPBORDER_ELEMENT = "TopBorder";
    const string LEFTBORDER_ELEMENT = "LeftBorder";
    const string RIGHTBORDER_ELEMENT = "RightBorder";
    const string BOTTOMBORDER_ELEMENT = "BottomBorder";

    const string DEFAULT_NAME = "DefaultName";
    const string TYPENAME_ELEMENT = "TypeName";
    const string TOP_ELEMENT = "Top";
    const string LEFT_ELEMENT = "Left";
    const string WIDTH_ELEMENT = "Width";
    const string HEIGHT_ELEMENT = "Height";
    const string CELLCONTENTS_ELEMENT = "CellContents";
    const string CANGROW_ELEMENT = "CanGrow";
    const string KEEPTOGETHER_ELEMENT = "KeepTogether";
    const string SOURCE_ELEMENT = "Source";
    const string VALUE_ELEMENT = "Value";
    const string FONTFAMILY_ELEMENT = "FontFamily";
    const string FONTSIZE_ELEMENT = "FontSize";
    const string FONTWEIGHT_ELEMENT = "FontWeight";
    const string FONTSTYLE_ELEMENT = "FontStyle";
    const string TEXTDECORATION_ELEMENT = "TextDecoration";
    const string FORMAT_ELEMENT = "Format";
    const string COLOR_ELEMENT = "Color";
    const string BACKGROUNDCOLOR_ELEMENT = "BackgroundColor";
    const string PADDINGLEFT_ELEMENT = "PaddingLeft";
    const string PADDINGRIGHT_ELEMENT = "PaddingRight";
    const string PADDINGTOP_ELEMENT = "PaddingTop";
    const string PADDINGBOTTOM_ELEMENT = "PaddingBottom";
    const string KEEPWITHGROUP_ELEMENT = "KeepWithGroup";
    const string REPEATONNEWPAGE_ELEMENT = "RepeatOnNewPage";
    const string GROUP_ELEMENT = "Group";
    const string DATASETNAME_ELEMENT = "DataSetName";
    const string DATATYPE_ELEMENT = "DataType";
    const string ALLOWBLANK_ELEMENT = "AllowBlank";
    const string NULLABLE_ELEMENT = "Nullable";
    const string PROMPT_ELEMENT = "Prompt";
    const string PRINTONFIRSTPAGE_ELEMENT = "PrintOnFirstPage";
    const string PRINTONLASTPAGE_ELEMENT = "PrintOnLastPage";
    const string SIZING_ELEMENT = "Sizing";
    const string SIZE_ELEMENT = "Size";
    const string BREAKLOCATION_ELEMENT = "BreakLocation";


    const string TEXTALIGN_ELEMENT = "TextAlign";
    //const string VERTICALALIGN_ELEMENT = "VerticalAlign";

    const string NAME_PROPERTY = "Name";

    const double COLUMN_WIDTH = 2.5;
    const double ROW_HEIGHT = 0.6;
    const double REPORT_WIDTH = 21;

    #endregion

    public JQClientTools.JQReport Report { get; set; }

    public System.Data.DataSet DataSet { get; set; }

    public string RemoteName { get; set; }

    public RdlcHelper()
    {
        var report = new JQClientTools.JQReport();
    }

    private void InitialReport(JObject control)
    {
        Report = CreateItem(control["properties"] as JObject, typeof(JQClientTools.JQReport)) as JQClientTools.JQReport;
    }

    private object CreateItem(JObject properties, Type itemType)
    {
        var item = itemType.GetConstructor(new Type[] { }).Invoke(null);
        foreach (var p in properties.Properties())
        {
            var property = itemType.GetProperty(p.Name);
            if (property != null)
            {
                if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                {
                    if (properties[property.Name] is JArray)
                    {
                        foreach (JObject childProperties in properties[property.Name] as JArray)
                        {
                            var type = string.Empty;
                            if (childProperties["type"] != null)
                            {
                                type = childProperties["type"].ToString();
                            }
                            else
                            {
                                type = property.PropertyType.GetGenericArguments()[0].ToString();
                            }
                            var ct = type.Split('.');
                            var childItemType = JsonHelper.GetType(ct[0], type);
                            (property.GetValue(item, null) as System.Collections.IList).Add(CreateItem(childProperties, childItemType));
                        }
                    }
                }
                else
                {
                    if (properties[property.Name] is JValue)
                    {
                        var value = (JValue)properties[property.Name];
                        if (value.Type == JTokenType.String && string.IsNullOrEmpty((string)value.Value))
                        {
                        }
                        else if (property.PropertyType.IsEnum)
                        {
                            property.SetValue(item, Enum.Parse(property.PropertyType, value.ToString()), null);
                        }
                        else if (property.PropertyType == typeof(System.Drawing.Font))
                        {
                            var fontValues = value.ToString().Split(',');
                            var familyName = fontValues[0];
                            var emSize = float.Parse(fontValues[1].Replace("pt", string.Empty));

                            var fontStyle = 0;
                            foreach (var fi in typeof(System.Drawing.FontStyle).GetFields(BindingFlags.Static | BindingFlags.Public))
                            {
                                if (value.ToString().Contains(fi.Name))
                                {
                                    fontStyle += (int)fi.GetValue(null);
                                }
                            }
                            var font = new System.Drawing.Font(familyName, emSize, (System.Drawing.FontStyle)fontStyle);
                            property.SetValue(item, font, null);
                        }
                        else
                        {
                            property.SetValue(item, Convert.ChangeType(value.ToString(), property.PropertyType), null);
                        }
                    }

                }
            }
        }
        return item;
    }

    private void InitialDataSource()
    {
        RemoteName = Report.RemoteName;
        var remoteNames = RemoteName.Split('.');
        DataSet = DataSetHelper.GetDataSet(remoteNames[0], remoteNames[1], true);
    }

    private bool HasTotal()
    {
        return Report.ReportStyle == JQClientTools.ReportStyle.Report && Report.FieldItems.Where(c => !c.Group).Count(c => !string.IsNullOrEmpty(c.Total)) > 0;
    }

    private bool HasGroupTotal()
    {
        //多个group 分组不汇总,line feed也不汇总
        return Report.ReportStyle == JQClientTools.ReportStyle.Report && Report.FieldItems.Count(c => c.Group) == 1 && Report.FieldItems.Where(c => !c.Group).Count(c => !string.IsNullOrEmpty(c.Total) && c.GroupTotal) > 0;
    }

    private bool HasGroupLineFeed()
    {
        return Report.ReportStyle == JQClientTools.ReportStyle.Report && Report.GroupStyle == JQClientTools.GroupStyle.LineFeed && Report.FieldItems.Count(c => c.Group) > 0;
    }

    public int DetailRowCount
    {
        get
        {
            if (Report.ReportStyle == JQClientTools.ReportStyle.Report)
            {
                return Report.FieldItems.Where(c => !c.Group).Max(c => c.RowIndex) + 1;
            }
            else
            {
                return Report.FieldItems.Max(c => c.RowIndex) + 1;
            }
        }
    }

    public int DetailColumnCount
    {
        get
        {
            var maxColumnIndex = Report.FieldItems.Where(c => !c.Group).Max(c => c.ColumnIndex);
            if (Report.ReportStyle == JQClientTools.ReportStyle.Report)
            {
                return maxColumnIndex + 1 - Report.FieldItems.Where(c => c.Group && c.ColumnIndex < maxColumnIndex).Select(c => c.ColumnIndex).Distinct().Count();
            }
            else
            {
                return maxColumnIndex + 1;
            }
        }
    }

    public string CreateRdlc(string name, JArray controls)
    {
        var control = controls[0] as JObject;
        InitialReport(control);
        if (!string.IsNullOrEmpty(Report.RDLCFileNames))
        {
            return string.Empty;
        }
        InitialDataSource();

        var xmlBuilder = new StringBuilder();
        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true, Encoding = Encoding.UTF8 };
        using (var writer = XmlWriter.Create(xmlBuilder, xmlWriterSettings))
        {
            writer.WriteRaw(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            writer.WriteStartElement(REPORT_ELEMENT, DEFAULT_URI); //<Report>
            writer.WriteAttributeString(DEFAULT_PREFIX, RD_PREFIX, null, RD_URI);

            AppendDataSources(writer);
            AppendDataSets(writer);
            AppendBody(writer);
            WriteParameters(writer);
            AppendString(writer, WIDTH_ELEMENT, string.Format("{0}cm", REPORT_WIDTH));
            AppendPage(writer);

            writer.WriteEndElement(); //</Report>
        }
        return xmlBuilder.ToString();
    }

    private void AppendDataSources(XmlWriter writer)
    {
        writer.WriteStartElement(DATASOURCES_ELEMENT); //<DataSources>
        AppendDataSource(Report.DataSetName, writer);
        if (!string.IsNullOrEmpty(Report.HeaderDataMember))
        {
            AppendDataSource(Report.HeaderDataSetName, writer);
        }
        writer.WriteEndElement(); //</DataSources>
    }

    private void AppendDataSource(string dataSetName, XmlWriter writer)
    {
        writer.WriteStartElement(DATASOURCE_ELEMENT); //<DataSource>
        writer.WriteAttributeString(NAME_PROPERTY, dataSetName);
        writer.WriteStartElement(CONNECTIONPROPERTIES_ELEMENT); //<ConnectionProperties>

        AppendString(writer, DATAPROVIDER_ELEMENT, "System.Data.DataSet");//<DataProvider/>
        AppendString(writer, CONNECTSTRING_ELEMENT, "/* Local Connection */");//<DataProvider/>

        writer.WriteEndElement(); //</ConnectionProperties>

        AppendString(writer, RD_PREFIX, DATASOURCE_ID_ELEMENT, RD_URI, Guid.NewGuid().ToString()); //<rd:DefaultName/>

        writer.WriteEndElement(); //</DataSource>
    }

    private void AppendDataSets(XmlWriter writer)
    {
        writer.WriteStartElement(DATASETS_ELEMENT); //<DataSets>
        AppendDataSet(Report.DataSetName, Report.DataMember, writer);
        if (!string.IsNullOrEmpty(Report.HeaderDataMember))
        {
            AppendDataSet(Report.HeaderDataSetName, Report.HeaderDataMember, writer);
        }
        writer.WriteEndElement(); //</DataSets>
    }

    private void AppendDataSet(string dataSetName, string dataMember, XmlWriter writer)
    {
        
        writer.WriteStartElement(DATASET_ELEMENT); //<DataSet>
        writer.WriteAttributeString(NAME_PROPERTY, dataSetName);
        writer.WriteStartElement(FIELDS_ELEMENT); //<Fields>

        var dataTable = !string.IsNullOrEmpty(dataMember) ? DataSet.Tables[dataMember] : DataSet.Tables[0];

        foreach (System.Data.DataColumn column in dataTable.Columns)
        {
            writer.WriteStartElement(FIELD_ELEMENT); //<Field>
            writer.WriteAttributeString(NAME_PROPERTY, column.ColumnName);

            AppendString(writer, DATAFIELD_ELEMENT, column.ColumnName);//<DataField/>
            //WriteString(writer, RD_PREFIX, DATAFIELD_ELEMENT, RD_URI, column.DataType.FullName);//<rd:TypeName/>

            writer.WriteEndElement(); //</Field>
        }
        writer.WriteEndElement(); //</Fields>

        writer.WriteStartElement(QUERY_ELEMENT); //<Query>
        AppendString(writer, DATASOURCENAME_ELEMENT, dataSetName); //<DataSourceName/>
        AppendString(writer, COMMANDTEXT_ELEMENT, "/* Local Query */"); //<CommandText/>
        writer.WriteEndElement(); //</Query>
        writer.WriteEndElement(); //</DataSet>
    }

    private void AppendBody(XmlWriter writer)
    {
        writer.WriteStartElement(BODY_ELEMENT); //<Body>
        if (Report.ReportStyle == JQClientTools.ReportStyle.Report)
        {
            AppendTableDetail(Report.FieldItems, writer);
        }
        else
        {
            AppendLabelDetail(Report.FieldItems, writer);
        }
        AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", 2 * ROW_HEIGHT));//<Height/>
        AppendDefaultStyle(writer);
        writer.WriteEndElement(); //</Body>
    }

    private List<double> ColumnWidths = new List<double>();

    private void AppendTableDetail(List<JQClientTools.JQReportFieldItem> items, XmlWriter writer)
    {
        writer.WriteStartElement(REPORTITEMS_ELEMENT); //<ReportItems>
        writer.WriteStartElement(TABLIX_ELEMENT); //<Tablix>
        writer.WriteAttributeString(NAME_PROPERTY, "Main");
        writer.WriteStartElement(TABLIXBODY_ELEMENT); //<TablixBody>
        writer.WriteStartElement(TABLIXCOLUMNS_ELEMENT); //<TablixColumns>
        foreach (var item in items.Where(c => c.Group))
        {
            var width = item.Width * 0.039;
            //var width = item.Width * Report.FieldFont.Size / 2 * 0.039;
            ColumnWidths.Add(width);
        }

        //多行

        Dictionary<string, JQClientTools.JQReportFieldItem> itemPositions = new Dictionary<string, JQClientTools.JQReportFieldItem>();
        foreach (var item in items.Where(c => !c.Group))
        {
            if (items.Exists(c => c.Group && c.ColumnIndex == item.ColumnIndex))
            {
                continue;//和group同列的不打印
            }

            var rowIndex = item.RowIndex;
            var columnIndex = item.ColumnIndex - items.Where(c => c.Group && c.ColumnIndex < item.ColumnIndex).Select(c => c.ColumnIndex).Distinct().Count();
            itemPositions.Add(string.Format("{0}.{1}", rowIndex, columnIndex), item);
        }
        var detailRowCount = DetailRowCount;
        var detailColumnCount = DetailColumnCount;
        for (int i = 0; i < detailColumnCount; i++)
        {
            writer.WriteStartElement(TABLIXCOLUMN_ELEMENT); //<TablixColumn>
            double width = 100;
            if (itemPositions.Where(c => c.Key.EndsWith(string.Format(".{0}", i))).Count() > 0)
            {
                width = itemPositions.Where(c => c.Key.EndsWith(string.Format(".{0}", i))).Max(c => c.Value.Width);
            }
            width = width * 0.039;
            ColumnWidths.Add(width);
            AppendString(writer, WIDTH_ELEMENT, string.Format("{0}cm", width)); //<Width/>
            writer.WriteEndElement(); //</TablixColumn>
        }

        writer.WriteEndElement(); //</TablixColumns>

        writer.WriteStartElement(TABLIXROWS_ELEMENT); //<TablixRows>

        #region Header
        writer.WriteStartElement(TABLIXROW_ELEMENT); //<TablixRow>

        AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>

        writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
        for (int i = 0; i < detailColumnCount; i++)
        {
            writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
            writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
            var key = string.Format("0.{0}", i);
            if (itemPositions.ContainsKey(key))
            {
                var item = itemPositions[key];
                AppendTextBox(writer, string.Format("DetailLabel{0}{1}", item.Field, i), item.Caption
                    , GetItemStyle(item.HeaderFont, item.HeaderColor, item.BackColor, item.HeaderAlignment, null, Report.HeaderBorderWidth), null);
            }
            else
            {
                AppendTextBox(writer, string.Format("DetailLabel{0}", i), string.Empty
                  , GetItemStyle(Report.HeaderFont, "Black", string.Empty, string.Empty, null, Report.HeaderBorderWidth), null);
            }
            writer.WriteEndElement(); //</CellContents>
            writer.WriteEndElement(); //</TablixCell>
        }
        writer.WriteEndElement(); //</TablixCells>
        writer.WriteEndElement(); //</TablixRow> 
        #endregion

        #region Data

        for (int i = 0; i < detailRowCount; i++)
        {
            writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>
            writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
            for (int j = 0; j < detailColumnCount; j++)
            {
                writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
                writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
                var key = string.Format("{0}.{1}", i, j);
                if (itemPositions.ContainsKey(key))
                {
                    var item = itemPositions[key];
                    var itemStyle = GetItemStyle(item.Font, item.Color, item.BackColor, item.Alignment, item.Format, Report.DetailBorderWidth);
                    if (detailRowCount > 1)
                    {
                        itemStyle.SetBorder(detailRowCount, detailColumnCount, i, j);
                    }
                    AppendTextBox(writer, string.Format("DetailText{0}{1}{2}", item.Field, i, j), GetFormatExpression(string.Format("Fields!{0}.Value", item.Field), string.Empty, true)
                        , itemStyle, null);
                }
                else
                {
                    var itemStyle = GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, Report.DetailBorderWidth);
                    if (detailRowCount > 1)
                    {
                        itemStyle.SetBorder(detailRowCount, detailColumnCount, i, j);
                    }
                    AppendTextBox(writer, string.Format("DetailText{0}{1}", i, j), string.Empty
                        , itemStyle, null);
                }
                writer.WriteEndElement(); //</CellContents>
                writer.WriteEndElement(); //</TablixCell>
            }
            writer.WriteEndElement(); //</TablixCells>
            writer.WriteEndElement(); //</TablixRow> 
        }
        #endregion

        #region GroupTotal
        if (HasGroupTotal())
        {
            writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>

            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>

            writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
            for (int i = 0; i < detailColumnCount; i++)
            {
                writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
                writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
                var key = string.Format("0.{0}", i);
                if (itemPositions.ContainsKey(key))
                {
                    var item = itemPositions[key];
                    var expression = string.Empty;
                    if (!string.IsNullOrEmpty(item.Total))
                    {
                        if (item.Total != JQClientTools.JQReportTotal.Count && !string.IsNullOrEmpty(item.Format))
                        {
                            expression = string.Format("Format({0}(Fields!{1}.Value), \"{2}\")", item.Total, item.Field, item.Format);
                        }
                        else
                        {
                            expression = string.Format("{0}(Fields!{1}.Value)", item.Total, item.Field);
                        }
                    }

                    AppendTextBox(writer, string.Format("GroupTotalText{0}{1}", item.Field, i)
                    , !string.IsNullOrEmpty(item.Total) && item.GroupTotal ? GetFormatExpression(expression, item.GroupTotalFormat, true) : string.Empty
                    , GetItemStyle(item.Font, item.Color, item.BackColor, item.Alignment, null, Report.DetailBorderWidth), null);

                }
                else
                {
                    AppendTextBox(writer, string.Format("GroupTotalText{0}", i), string.Empty
                      , GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, Report.DetailBorderWidth), null);
                }
                writer.WriteEndElement(); //</CellContents>
                writer.WriteEndElement(); //</TablixCell>
            }
            writer.WriteEndElement(); //</TablixCells>
            writer.WriteEndElement(); //</TablixRow> 
        }
        #endregion

        #region GroupLineFeed

        if (HasGroupLineFeed())
        {
            writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>
            writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
            for (int i = 0; i < detailColumnCount; i++)
            {
                writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
                writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
                AppendTextBox(writer, string.Format("GroupLineFeedText{0}", i), string.Empty
                      , GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, 0), null);
                writer.WriteEndElement(); //</CellContents>
                writer.WriteEndElement(); //</TablixCell>
            }

            writer.WriteEndElement(); //</TablixCells>
            writer.WriteEndElement(); //</TablixRow> 
        }

        #endregion

        var groupItems = items.Where(c => c.Group).ToList();

        #region Total
        if (HasTotal())
        {
            writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>

            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>

            writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>


            for (int i = 0; i < detailColumnCount; i++)
            {
                writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
                writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
                var key = string.Format("0.{0}", i);
                if (i == 0 && groupItems.Count == 0 && !string.IsNullOrEmpty(Report.TotalCaption))
                {
                    AppendTextBox(writer, string.Format("TotalText{0}", i), Report.TotalCaption
                      , GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, Report.DetailBorderWidth), null);
                }
                else if (itemPositions.ContainsKey(key))
                {
                    var item = itemPositions[key];
                    var expression = string.Empty;
                    if (!string.IsNullOrEmpty(item.Total))
                    {
                        if (item.Total != JQClientTools.JQReportTotal.Count && !string.IsNullOrEmpty(item.Format))
                        {
                            expression = string.Format("Format({0}(Fields!{1}.Value), \"{2}\")", item.Total, item.Field, item.Format);
                        }
                        else
                        {
                            expression = string.Format("{0}(Fields!{1}.Value)", item.Total, item.Field);
                        }
                    }
                    AppendTextBox(writer, string.Format("TotalText{0}{1}", item.Field, i)
                       , !string.IsNullOrEmpty(item.Total) ? GetFormatExpression(expression, item.TotalFormat, true) : string.Empty
                       , GetItemStyle(item.Font, item.Color, item.BackColor, item.Alignment, null, Report.DetailBorderWidth), null);
                }
                else
                {
                    AppendTextBox(writer, string.Format("TotalText{0}", i), string.Empty
                      , GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, Report.DetailBorderWidth), null);
                }
                writer.WriteEndElement(); //</CellContents>
                writer.WriteEndElement(); //</TablixCell>
            }
            writer.WriteEndElement(); //</TablixCells>
            writer.WriteEndElement(); //</TablixRow> 
        }
        #endregion

        writer.WriteEndElement(); //</TablixRows>

        writer.WriteEndElement(); //</TablixBody>

        writer.WriteStartElement(TABLIXCOLUMNHIERARCHY_ELEMENT); //<TablixColumnHierarchy>
        writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
        for (int i = 0; i < detailColumnCount; i++)
        {
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            writer.WriteEndElement(); //</TablixMember>
        }
        writer.WriteEndElement(); //</TablixMembers>
        writer.WriteEndElement(); //</TablixColumnHierarchy>

        AppendGroup(groupItems, writer);

        AppendString(writer, DATASETNAME_ELEMENT, Report.DataSetName); //<DataSetName/>

        AppendDefaultStyle(writer);

        writer.WriteEndElement(); //</Tablix>

        writer.WriteEndElement(); //</ReportItems>
    }

    private void AppendLabelDetail(List<JQClientTools.JQReportFieldItem> items, XmlWriter writer)
    {
        writer.WriteStartElement(REPORTITEMS_ELEMENT); //<ReportItems>
        writer.WriteStartElement(TABLIX_ELEMENT); //<Tablix>
        writer.WriteAttributeString(NAME_PROPERTY, "Main");
        writer.WriteStartElement(TABLIXBODY_ELEMENT); //<TablixBody>

        writer.WriteStartElement(TABLIXCOLUMNS_ELEMENT); //<TablixColumns>
        Dictionary<string, JQClientTools.JQReportFieldItem> itemPositions = new Dictionary<string, JQClientTools.JQReportFieldItem>();
        foreach (var item in items)
        {
            var rowIndex = item.RowIndex;
            var columnIndex = item.ColumnIndex;
            itemPositions.Add(string.Format("{0}.{1}", rowIndex, columnIndex), item);
        }
        var detailRowCount = DetailRowCount;
        var detailColumnCount = DetailColumnCount;
        for (int i = 0; i < detailColumnCount; i++)
        {
            double width = 100;
            if (itemPositions.Where(c => c.Key.EndsWith(string.Format(".{0}", i))).Count() > 0)
            {
                width = itemPositions.Where(c => c.Key.EndsWith(string.Format(".{0}", i))).Max(c => c.Value.Width);
                width += Report.LabelCaptionWidth;
            }

            if (i > 0)
            {
                width += Report.LabelHorizontalGap;
            }

            width = width * 0.039;
            ColumnWidths.Add(width);
        }
        writer.WriteStartElement(TABLIXCOLUMN_ELEMENT); //<TablixColumn>
        AppendString(writer, WIDTH_ELEMENT, string.Format("{0}cm", ColumnWidths.Sum())); //<Width/>
        writer.WriteEndElement(); //</TablixColumn>
        writer.WriteEndElement(); //</TablixColumns>

        writer.WriteStartElement(TABLIXROWS_ELEMENT); //<TablixRows>

        writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>
        AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>
        writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
        writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
        writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>

        writer.WriteStartElement(RECTANGLE_ELEMENT);//<Rectangle>
        writer.WriteAttributeString(NAME_PROPERTY, "RecMain");
        writer.WriteStartElement(REPORTITEMS_ELEMENT); //<ReportItems>

        #region Data
        for (int i = 0; i < detailRowCount; i++)
        {
            for (int j = 0; j < detailColumnCount; j++)
            {
                var key = string.Format("{0}.{1}", i, j);
                if (itemPositions.ContainsKey(key))
                {
                    var item = itemPositions[key];
                    var rectangle = GetItemRectangle(item);
                    if (j > 0)
                    {
                        rectangle.Left += Report.LabelHorizontalGap * 0.039;
                    }
                    if (Report.LabelCaptionWidth > 0)
                    {
                        rectangle.Width = Report.LabelCaptionWidth * 0.039;
                        AppendTextBox(writer, string.Format("DetailLabel{0}{1}{2}", item.Field, i, j), item.Caption
                            , GetItemStyle(item.HeaderFont, item.HeaderColor, item.BackColor, item.HeaderAlignment, item.Format, 0), rectangle);
                    }
                    rectangle = GetItemRectangle(item);
                    if (j > 0)
                    {
                        rectangle.Left += Report.LabelHorizontalGap * 0.039;
                    }
                    rectangle.Left += Report.LabelCaptionWidth * 0.039;
                    if (j > 0)
                    {
                        rectangle.Width -= Report.LabelHorizontalGap * 0.039;
                    }
                    rectangle.Width -= Report.LabelCaptionWidth * 0.039;
                    AppendTextBox(writer, string.Format("DetailText{0}{1}{2}", item.Field, i, j), GetFormatExpression(string.Format("Fields!{0}.Value", item.Field), string.Empty, true)
                        , GetItemStyle(item.Font, item.Color, item.BackColor, item.Alignment, item.Format, 0), rectangle);
                }
            }
        }
        #endregion

        writer.WriteEndElement(); //</ReportItems>
        
        if (Report.DetailBorderWidth > 0)
        {
            AppendTextBoxStyle(writer, new TextBoxStyle() { BorderWidth = Report.DetailBorderWidth });
        }
        writer.WriteEndElement(); //</Rectangle>
        writer.WriteEndElement(); //</CellContents>
        writer.WriteEndElement(); //</TablixCell>
        writer.WriteEndElement(); //</TablixCells>
        writer.WriteEndElement(); //</TablixRow>

        if (Report.LabelStyle == JQClientTools.LabelStyle.LineFeed)
        {
            writer.WriteStartElement(TABLIXROW_ELEMENT);  //<TablixRow>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", ROW_HEIGHT)); //<Height/>
            writer.WriteStartElement(TABLIXCELLS_ELEMENT); //<TablixCells>
            writer.WriteStartElement(TABLIXCELL_ELEMENT); //<TablixCell>
            writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
            AppendTextBox(writer, "LineFeedText", string.Empty
                            , GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, 0), null);
            writer.WriteEndElement(); //</CellContents>
            writer.WriteEndElement(); //</TablixCell>
            writer.WriteEndElement(); //</TablixCells>
            writer.WriteEndElement(); //</TablixRow>
        }
       

        writer.WriteEndElement(); //</TablixRows>
        writer.WriteEndElement(); //</TablixBody>

        writer.WriteStartElement(TABLIXCOLUMNHIERARCHY_ELEMENT); //<TablixColumnHierarchy>
        writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
        writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
        writer.WriteEndElement(); //</TablixMember>
        writer.WriteEndElement(); //</TablixMembers>
        writer.WriteEndElement(); //</TablixColumnHierarchy>

        //AppendGroup(new List<JQClientTools.JQReportFieldItem>(), writer);
        writer.WriteStartElement(TABLIXROWHIERARCHY_ELEMENT); //<TablixRowHierarchy>
        writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
        writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
        writer.WriteStartElement(GROUP_ELEMENT); //<Group>
        writer.WriteAttributeString(NAME_PROPERTY, "Detail");
        if (Report.LabelStyle == JQClientTools.LabelStyle.ChangePage)
        {
            writer.WriteStartElement(PAGEBREAK_ELEMENT); //<PageBreak>
            AppendString(writer, BREAKLOCATION_ELEMENT, "Between");//<Value/>
            writer.WriteEndElement(); //</PageBreak>
        }
        writer.WriteEndElement(); //</Group>

        if (Report.LabelStyle == JQClientTools.LabelStyle.LineFeed)
        {
            writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            writer.WriteEndElement(); //</TablixMember>
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            writer.WriteEndElement(); //</TablixMember>
            writer.WriteEndElement(); //</TablixMembers>
            AppendString(writer, KEEPTOGETHER_ELEMENT, bool.TrueString.ToLower()); //<KeepTogether/>
        }

        writer.WriteEndElement(); //</TablixMember>
        writer.WriteEndElement(); //</TablixMembers>
        writer.WriteEndElement(); //</TablixRowHierarchy>
        AppendString(writer, DATASETNAME_ELEMENT, Report.DataSetName); //<DataSetName/>

        AppendDefaultStyle(writer);

        writer.WriteEndElement(); //</Tablix>

        writer.WriteEndElement(); //</ReportItems>
    }

    private void AppendGroup(List<JQClientTools.JQReportFieldItem> groupItems, XmlWriter writer)
    {
        writer.WriteStartElement(TABLIXROWHIERARCHY_ELEMENT); //<TablixRowHierarchy>

        writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>

        writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>

        if (groupItems.Count > 0)
        {
            if (Report.GroupStyle == JQClientTools.GroupStyle.LineFeed)
            {
                groupItems.Insert(0, new JQClientTools.JQReportFieldItem() { Field = groupItems[0].Field, GroupFor = "LineFeed", Width = 0 });
            }

            AppendGroupCaption(groupItems, groupItems[0], writer);
        }
        else if (Report.DetailCount > 0)
        {
            Report.GroupStyle = JQClientTools.GroupStyle.ChangePage;
            AppendGroupCaption(groupItems, new JQClientTools.JQReportFieldItem() { Field = "GroupForCount", GroupFor = "Count", Width = 0 }, writer);
        }

        AppendString(writer, KEEPWITHGROUP_ELEMENT, "After"); //<KeepWithGroup/>
        AppendString(writer, REPEATONNEWPAGE_ELEMENT, bool.TrueString.ToLower()); //<RepeatOnNewPage/>
        writer.WriteEndElement(); //</TablixMember>

        writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
        if (groupItems.Count > 0)
        {
            AppendGroupContent(groupItems, groupItems[0], writer);
        }
        else if (Report.DetailCount > 0)
        {
            AppendGroupContent(groupItems, new JQClientTools.JQReportFieldItem() { Field = "GroupForCount", GroupFor = "Count", Width = 0 }, writer);
        }
        else
        {
            writer.WriteStartElement(GROUP_ELEMENT); //<Group>
            writer.WriteAttributeString(NAME_PROPERTY, "Detail");
            writer.WriteEndElement(); //</Group>
            var detailRowCount = DetailRowCount;
            if (detailRowCount > 1)
            {
                writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
                for (int i = 0; i < detailRowCount; i++)
                {
                    writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
                    writer.WriteEndElement(); //</TablixMember>
                }
                writer.WriteEndElement(); //</TablixMembers>
            }
        }

        writer.WriteEndElement(); //</TablixMember>

        if (HasTotal())
        {
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            if (groupItems.Count > 0)
            {
                AppendTotalCaption(groupItems, groupItems[0], writer);
            }
            else if (Report.DetailCount > 0)
            {
                AppendTotalCaption(groupItems, new JQClientTools.JQReportFieldItem() { Field = "GroupForCount", GroupFor = "Count", Width = 0 }, writer);
            }
            AppendString(writer, KEEPWITHGROUP_ELEMENT, "Before"); //<KeepWithGroup/>
            writer.WriteEndElement(); //</TablixMember>
        }

        writer.WriteEndElement(); //</TablixMembers>
        writer.WriteEndElement(); //</TablixRowHierarchy>
    }

    private void AppendGroupCaption(List<JQClientTools.JQReportFieldItem> groupItems, JQClientTools.JQReportFieldItem groupItem, XmlWriter writer)
    {
        writer.WriteStartElement(TABLIXHEADER_ELEMENT); //<TablixHeader>
        var width = groupItem.Width * 0.039;
        AppendString(writer, SIZE_ELEMENT, string.Format("{0}cm", width));//<Size/>
        writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
        if (groupItem.GroupFor == "Count" || groupItem.GroupFor == "LineFeed")
        {
            AppendTextBox(writer, string.Format("GroupLabel{0}", groupItem.GroupFor), string.Empty, GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, 0), null);
        }
        else
        {
            AppendTextBox(writer, string.Format("GroupLabel{0}", groupItem.Field), groupItem.Caption, GetItemStyle(groupItem.HeaderFont, groupItem.HeaderColor, groupItem.BackColor, groupItem.HeaderAlignment, null, Report.HeaderBorderWidth), null);
        }
       
        writer.WriteEndElement(); //</CellContents>
        writer.WriteEndElement(); //</TablixHeader>

        var childgroup = GetChildGroup(groupItems, groupItem);
        if (childgroup != null)
        {
            writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            AppendGroupCaption(groupItems, childgroup, writer);
            writer.WriteEndElement(); //</TablixMember>
            writer.WriteEndElement(); //</TablixMembers>
        }
    }

    private void AppendGroupContent(List<JQClientTools.JQReportFieldItem> groupItems, JQClientTools.JQReportFieldItem groupItem, XmlWriter writer)
    {
        writer.WriteStartElement(GROUP_ELEMENT); //<Group>
        writer.WriteAttributeString(NAME_PROPERTY, string.Format("Group{0}", groupItem.GroupFor == null? groupItem.Field : groupItem.GroupFor));
        writer.WriteStartElement(GROUPEXPRESSIONS_ELEMENT); //<GroupExpressions>
        if (groupItem.GroupFor == "Count")
        {
            AppendString(writer, GROUPEXPRESSION_ELEMENT, string.Format("=Int((RowNumber(Nothing)-1)/{0})", Report.DetailCount));//<GroupExpression/>
        }
        else
        {
            AppendString(writer, GROUPEXPRESSION_ELEMENT, string.Format("=Fields!{0}.Value", groupItem.Field));//<GroupExpression/>
        }
        writer.WriteEndElement(); //</GroupExpressions>
        if (Report.GroupStyle == JQClientTools.GroupStyle.ChangePage && groupItems.IndexOf(groupItem) <= 0)
        {
            writer.WriteStartElement(PAGEBREAK_ELEMENT); //<PageBreak>
            AppendString(writer, BREAKLOCATION_ELEMENT, "Between");//<Value/>
            writer.WriteEndElement(); //</PageBreak>
        }

        writer.WriteEndElement(); //</Group>
        if (groupItem.GroupFor == "Count") { }
        else
        {
            writer.WriteStartElement(SORTEXPRESSIONS_ELEMENT); //<SortExpressions>
            writer.WriteStartElement(SORTEXPRESSION_ELEMENT); //<SortExpression>
            AppendString(writer, VALUE_ELEMENT, string.Format("=Fields!{0}.Value", groupItem.Field));//<Value/>
            writer.WriteEndElement(); //</SortExpression>
            writer.WriteEndElement(); //</SortExpressions>
        }

        writer.WriteStartElement(TABLIXHEADER_ELEMENT); //<TablixHeader>
        var width = groupItem.Width * 0.039;
        AppendString(writer, SIZE_ELEMENT, string.Format("{0}cm", width));//<Size/>
        writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
        if (groupItem.GroupFor == "Count" || groupItem.GroupFor == "LineFeed")
        {
            AppendTextBox(writer, string.Format("GroupText{0}", groupItem.GroupFor), string.Empty, GetItemStyle(Report.DetailFont, "Black", string.Empty, string.Empty, null, 0), null);
        }
        else
        {
            AppendTextBox(writer, string.Format("GroupText{0}", groupItem.Field), GetFormatExpression(string.Format("Fields!{0}.Value", groupItem.Field), null, true), GetItemStyle(groupItem.Font, groupItem.Color, groupItem.BackColor, groupItem.Alignment, groupItem.Format, Report.DetailBorderWidth), null);
        }
       
        writer.WriteEndElement(); //</CellContents>
        writer.WriteEndElement(); //</TablixHeader>

        writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
        writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
        var childgroup = GetChildGroup(groupItems, groupItem);
        if (childgroup != null)
        {
            AppendGroupContent(groupItems, childgroup, writer);
        }
        else
        {
            writer.WriteStartElement(GROUP_ELEMENT); //<Group>
            writer.WriteAttributeString(NAME_PROPERTY, "Detail");
            writer.WriteEndElement(); //</Group>
            var detailRowCount = DetailRowCount;
            if (detailRowCount > 1)
            {
                writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
                for (int i = 0; i < detailRowCount; i++)
                {
                    writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
                    writer.WriteEndElement(); //</TablixMember>
                }
                writer.WriteEndElement(); //</TablixMembers>
            }
        }
        writer.WriteEndElement(); //</TablixMember>

        if (HasGroupTotal())
        {
            if ((groupItems.IndexOf(groupItem) == 0 && groupItems[0].GroupFor != "LineFeed") 
                || (groupItems.IndexOf(groupItem) == 1 && groupItems[0].GroupFor == "LineFeed"))
            {
                writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
                AppendString(writer, KEEPWITHGROUP_ELEMENT, "Before"); //<KeepWithGroup/>
                writer.WriteEndElement(); //</TablixMember>

            }
        }
        if (groupItem.GroupFor == "LineFeed")
        {
            childgroup = GetChildGroup(groupItems, groupItem);
            if (childgroup != null)
            {
                writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
                AppendGroupLineFeed(groupItems, childgroup, writer);
                AppendString(writer, KEEPWITHGROUP_ELEMENT, "Before"); //<KeepWithGroup/>
                writer.WriteEndElement(); //</TablixMember>
            }
        }

        writer.WriteEndElement(); //</TablixMembers>
    }

    private void AppendGroupLineFeed(List<JQClientTools.JQReportFieldItem> groupItems, JQClientTools.JQReportFieldItem groupItem, XmlWriter writer)
    {
        writer.WriteStartElement(TABLIXHEADER_ELEMENT); //<TablixHeader>
        var width = groupItem.Width * 0.039;
        AppendString(writer, SIZE_ELEMENT, string.Format("{0}cm", width));//<Size/>
        writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>

        AppendTextBox(writer, string.Format("GroupLineFeedText{0}", groupItem.Field), string.Empty, GetItemStyle(groupItem.HeaderFont, "Black", string.Empty, string.Empty, null, 0), null);
        
        writer.WriteEndElement(); //</CellContents>
        writer.WriteEndElement(); //</TablixHeader>

        var childgroup = GetChildGroup(groupItems, groupItem);
        if (childgroup != null)
        {
            writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            AppendGroupLineFeed(groupItems, childgroup, writer);
            writer.WriteEndElement(); //</TablixMember>
            writer.WriteEndElement(); //</TablixMembers>
        }
    }

    private void AppendTotalCaption(List<JQClientTools.JQReportFieldItem> groupItems, JQClientTools.JQReportFieldItem groupItem, XmlWriter writer)
    {
        writer.WriteStartElement(TABLIXHEADER_ELEMENT); //<TablixHeader>
        var width = groupItem.Width * 0.039;
        AppendString(writer, SIZE_ELEMENT, string.Format("{0}cm", width));//<Size/>
        writer.WriteStartElement(CELLCONTENTS_ELEMENT); //<CellContents>
        if (groupItem.GroupFor == "Count" || groupItem.GroupFor == "LineFeed")
        {
            AppendTextBox(writer, string.Format("TotalLabel{0}", groupItem.GroupFor), string.Empty, GetItemStyle(groupItem.HeaderFont, "Black", string.Empty, string.Empty, null, 0), null);
        }
        else
        {
            AppendTextBox(writer, string.Format("TotalLabel{0}", groupItem.Field), Report.TotalCaption, GetItemStyle(groupItem.HeaderFont, groupItem.HeaderColor, groupItem.BackColor, groupItem.HeaderAlignment, null, Report.DetailBorderWidth), null);
        }
        writer.WriteEndElement(); //</CellContents>
        writer.WriteEndElement(); //</TablixHeader>

        var childgroup = GetChildGroup(groupItems, groupItem);
        if (childgroup != null)
        {
            if (groupItem.GroupFor == "Count" || groupItem.GroupFor == "LineFeed") { }
            else
            {
                Report.TotalCaption = string.Empty;
            }
            writer.WriteStartElement(TABLIXMEMBERS_ELEMENT); //<TablixMembers>
            writer.WriteStartElement(TABLIXMEMBER_ELEMENT); //<TablixMember>
            AppendTotalCaption(groupItems, childgroup, writer);
            writer.WriteEndElement(); //</TablixMember>
            writer.WriteEndElement(); //</TablixMembers>
        }
    }

    private JQClientTools.JQReportFieldItem GetChildGroup(List<JQClientTools.JQReportFieldItem> groupItems, JQClientTools.JQReportFieldItem parentGroupItem)
    {
        var index = groupItems.IndexOf(parentGroupItem);
        if (groupItems.Count > index + 1)
        {
            return groupItems[index + 1];
        }
        else
        {
            return null;
        }
    }

    private void WriteParameters(XmlWriter writer)
    {
        writer.WriteStartElement(REPORTPARAMETERS_ELEMENT); //<ReportParameters>
        var parameterNames = new string[] { "CompanyName", "QueryCondition", "ReportDate", "ReportDateTime", "UserID", "UserName", "Logo" };
        foreach (var name in parameterNames)
        {
            writer.WriteStartElement(REPORTPARAMETER_ELEMENT); //<ReportParameter>
            writer.WriteAttributeString(NAME_PROPERTY, name); //<Name/>
            AppendString(writer, DATATYPE_ELEMENT, typeof(string).Name); //<DataType/>
            AppendString(writer, NULLABLE_ELEMENT, bool.TrueString.ToLower()); //<Nullable/>
            AppendString(writer, ALLOWBLANK_ELEMENT, bool.TrueString.ToLower()); //<AllowBlank/>
            AppendString(writer, PROMPT_ELEMENT, name); //<Prompt/>
            writer.WriteEndElement(); //</ReportParameter>
        }

        writer.WriteEndElement(); //<ReportParameters>
    }

    private void AppendPage(XmlWriter writer)
    {
        writer.WriteStartElement(PAGE_ELEMENT); //<Page>

        if (Report.HeaderItems.Count > 0)
        {
            //page header
            writer.WriteStartElement(PAGEHEADER_ELEMENT); //<PageHeader>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", (Report.HeaderItems.Max(c => c.RowIndex) + 1) * ROW_HEIGHT)); //<Height/>
            AppendString(writer, PRINTONFIRSTPAGE_ELEMENT, bool.TrueString.ToLower()); //<PrintOnFirstPage/>
            AppendString(writer, PRINTONLASTPAGE_ELEMENT, bool.TrueString.ToLower()); //<PrintOnLastPage/>

            AppendItems(Report.HeaderItems, writer, "Header");
            writer.WriteEndElement(); //</PageHeader>
        }

        if (Report.FooterItems.Count > 0)
        {
            //page footer
            writer.WriteStartElement(PAGEFOOTER_ELEMENT); //<PageFooter>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", (Report.HeaderItems.Max(c => c.RowIndex) + 1) * ROW_HEIGHT)); //<Height/>
            AppendString(writer, PRINTONFIRSTPAGE_ELEMENT, bool.TrueString.ToLower()); //<PrintOnFirstPage/>
            AppendString(writer, PRINTONLASTPAGE_ELEMENT, bool.TrueString.ToLower()); //<PrintOnLastPage/>

            AppendItems(Report.FooterItems, writer, "Footer");



            if (Report.FooterBorderWidth > 0)
            {
                writer.WriteStartElement(STYLE_ELEMENT); //<Style>

                writer.WriteStartElement(BORDER_ELEMENT); //<Border>
                AppendString(writer, STYLE_ELEMENT, "None"); //<Style/>
                writer.WriteEndElement(); //</Border>

                writer.WriteStartElement(TOPBORDER_ELEMENT); //<TopBorder>
                AppendString(writer, STYLE_ELEMENT, "Solid"); //<Style/>
                AppendString(writer, WIDTH_ELEMENT, string.Format("{0}pt", Report.FooterBorderWidth)); //<Width/>
                writer.WriteEndElement(); //</TopBorder>

                writer.WriteEndElement(); //</Style>
            }


            writer.WriteEndElement(); //</PageFooter>
        }

        AppendDefaultStyle(writer);
        writer.WriteEndElement(); //</Page>
    }

    private void AppendItems(List<JQClientTools.JQReportItem> items, XmlWriter writer, string region)
    {
        writer.WriteStartElement(REPORTITEMS_ELEMENT); //<ReportItems>

        foreach (var item in items)
        {
            if (item is JQClientTools.JQReportConstantItem && (item as JQClientTools.JQReportConstantItem).Style == JQClientTools.JQReportConstantItem.StyleType.Logo)
            {
                AppendImage(writer, string.Format("{0}Item{1}", region, items.IndexOf(item)), GetItemExpression(item), GetItemRectangle(item));
            }
            else
            {
                AppendTextBox(writer, string.Format("{0}Item{1}", region, items.IndexOf(item)), GetItemExpression(item), GetItemStyle(item.Font, item.Color, item.BackColor, item.Alignment, null, 0), GetItemRectangle(item));
            }
        }

        writer.WriteEndElement();//</ReportItems>
    }

    private string GetItemExpression(JQClientTools.JQReportItem item)
    {
        var format = item.Format;
        var caption = item.Caption;
        if (item is JQClientTools.JQReportConstantItem)
        {
            var expression = string.Empty;
            switch ((item as JQClientTools.JQReportConstantItem).Style)
            {
                case JQClientTools.JQReportConstantItem.StyleType.CompanyName:
                case JQClientTools.JQReportConstantItem.StyleType.QueryCondition:
                case JQClientTools.JQReportConstantItem.StyleType.ReportDate:
                case JQClientTools.JQReportConstantItem.StyleType.ReportDateTime:
                case JQClientTools.JQReportConstantItem.StyleType.UserID:
                case JQClientTools.JQReportConstantItem.StyleType.UserName:
                case JQClientTools.JQReportConstantItem.StyleType.Logo:
                    expression = string.Format("Parameters!{0}.Value", (item as JQClientTools.JQReportConstantItem).Style);
                    break;
                case JQClientTools.JQReportConstantItem.StyleType.PageIndex:
                    expression = "Globals!PageNumber.ToString()";
                    break;
                case JQClientTools.JQReportConstantItem.StyleType.PageIndexAndTotal:
                    expression = @"Globals!PageNumber.ToString()+""/""+Globals!TotalPages.ToString()";
                    break;
                case JQClientTools.JQReportConstantItem.StyleType.Description:
                    expression = "\"" + Report.Description + "\"";
                    break;
                case JQClientTools.JQReportConstantItem.StyleType.ReportID:
                    expression = "\"" + Report.ReportID + "\"";
                    break;
                case JQClientTools.JQReportConstantItem.StyleType.ReportName:
                    expression = "\"" + Report.ReportName + "\"";
                    break;
            }
            
            if ((item as JQClientTools.JQReportConstantItem).Style == JQClientTools.JQReportConstantItem.StyleType.Logo)
            {
                format = string.Empty;
                caption = string.Empty;
            }
            if (!string.IsNullOrEmpty(format) && format.Contains("{0}"))
            {
                return GetFormatExpression(expression, format, false);
            }
            else
            {
                if (!string.IsNullOrEmpty(caption))
                {
                    format = caption + "{0}";
                }
                return GetFormatExpression(expression, format, false);
            }

            //return GetFormatExpression(expression, format, false);

        }
        else if (item is JQClientTools.JQReportFieldItem)
        {
            var expression = string.Format("First(Fields!{0}.Value, \"{1}\")", (item as JQClientTools.JQReportFieldItem).Field, Report.HeaderDataSetName);
            if (!string.IsNullOrEmpty(format) && format.Contains("{0}"))
            {
                return GetFormatExpression(expression, format, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(format))
                {
                    expression = string.Format("Format({0}, \"{1}\")", expression, format);
                }

                if (!string.IsNullOrEmpty(caption))
                {
                    format = caption + "{0}";
                }
                return GetFormatExpression(expression, format, true);
            }
        }
        return string.Empty;
    }

    private string GetFormatExpression(string expression, string format, bool toString)
    {
        var formatExpression = new StringBuilder();
        if (string.IsNullOrEmpty(format))
        {
            formatExpression.AppendFormat("={0}", expression);
        }
        else
        {
            if (format.Contains("{0}"))
            {
                var parts = Regex.Split(format, @"\{0\}");
                formatExpression.Append("=");
                if (parts[0].Length > 0)
                {

                    formatExpression.AppendFormat("\"{0}\"+", parts[0]);
                }
                formatExpression.Append(expression);
                if (toString)
                {
                    formatExpression.Append(".ToString()");
                }
                if (parts[1].Length > 0)
                {
                    formatExpression.AppendFormat("+\"{0}\"", parts[1]);
                }
            }
            else
            {
                formatExpression.AppendFormat("=\"{0}\"", format);
            }
        }
        return formatExpression.ToString();
    }

    private TextBoxStyle GetItemStyle(System.Drawing.Font font, string color, string backColor, string alignment, string format, int borderWidth)
    {
        return new TextBoxStyle()
        {
            FontName = font.Name,
            FontSize = string.Format("{0}pt", font.Size),
            Color = color,
            BackgroudColor = backColor,
            TextAlign = string.IsNullOrEmpty(alignment) ? string.Empty : alignment.ToString(),
            FontWeight = font.Bold ? "Bold" : string.Empty,
            FontStyle = font.Italic ? "Italic" : string.Empty,
            TextDecoration = font.Underline ? "Underline" : (font.Strikeout ? "Strikethrough" : string.Empty),
            Format = format,
            Padding = "2pt",
            BorderWidth = borderWidth
        };
    }

    private ItemRectangle GetItemRectangle(JQClientTools.JQReportItem item)
    {
        var left = 0.0;
        for (int i = 0; i < item.ColumnIndex; i++)
        {
            left += GetColumnWidth(i);
        }

        var top = 0.0;
        for (int i = 0; i < item.RowIndex; i++)
        {
            top += ROW_HEIGHT;
        }

        var width = 0.0;
        var cells = item.Cells;
        if (cells == 0)
        {
            cells = 1;
        }
        for (int i = item.ColumnIndex; i < item.ColumnIndex + cells; i++)
        {
            width += GetColumnWidth(i);
        }

        var height = ROW_HEIGHT;
        return new ItemRectangle() { Height = height, Left = left, Top = top, Width = width };
    }

    private double GetColumnWidth(int columnIndex)
    {
        if (columnIndex < ColumnWidths.Count)
        {
            return ColumnWidths[columnIndex];
        }
        else
        {
            return COLUMN_WIDTH;
        }
    }

    private void AppendTextBox(XmlWriter writer, string name, string expression, TextBoxStyle style, ItemRectangle rectangle)
    {
        writer.WriteStartElement(TEXTBOX_ELEMENT); //<Textbox>
        writer.WriteAttributeString(NAME_PROPERTY, name);

        AppendString(writer, CANGROW_ELEMENT, bool.TrueString.ToLower()); //<CanGrow/>
        AppendString(writer, KEEPTOGETHER_ELEMENT, bool.TrueString.ToLower()); //<KeepTogether/>

        writer.WriteStartElement(PARAGRAPHS_ELEMENT); //<Paragraphs>
        writer.WriteStartElement(PARAGRAPH_ELEMENT); //<Paragraph>
        writer.WriteStartElement(TEXTRUNS_ELEMENT); //<TextRuns>
        writer.WriteStartElement(TEXTRUN_ELEMENT); //<TextRun>

        AppendString(writer, VALUE_ELEMENT, expression); //<Value/>

        AppendTextRunStyle(writer, style);

        writer.WriteEndElement(); //</TextRun>
        writer.WriteEndElement(); //</TextRuns>

        AppendParagraphStyle(writer, style);

        writer.WriteEndElement(); //</Paragraph>
        writer.WriteEndElement(); //</Paragraphs>

        if (rectangle != null)
        {
            AppendString(writer, TOP_ELEMENT, string.Format("{0}cm", rectangle.Top)); //<Top/>
            AppendString(writer, LEFT_ELEMENT, string.Format("{0}cm", rectangle.Left)); //<Left/>
            AppendString(writer, WIDTH_ELEMENT, string.Format("{0}cm", rectangle.Width)); //<Width/>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", rectangle.Height)); //<Height/>
        }

        AppendString(writer, RD_PREFIX, DEFAULT_NAME, RD_URI, name); //<rd:DefaultName/>

        AppendTextBoxStyle(writer, style);

        writer.WriteEndElement(); //</Textbox>
    }

    private void AppendImage(XmlWriter writer, string name, string expression, ItemRectangle rectangle)
    {
        writer.WriteStartElement(IMAGE_ELEMENT); //<Image>
        writer.WriteAttributeString(NAME_PROPERTY, name);

        AppendString(writer, SOURCE_ELEMENT, "External");
        AppendString(writer, VALUE_ELEMENT, expression);
        //WriteString(writer, SIZING_ELEMENT, "FitProportional");

        if (rectangle != null)
        {
            AppendString(writer, TOP_ELEMENT, string.Format("{0}cm", rectangle.Top)); //<Top/>
            AppendString(writer, LEFT_ELEMENT, string.Format("{0}cm", rectangle.Left)); //<Left/>
            AppendString(writer, WIDTH_ELEMENT, string.Format("{0}cm", rectangle.Width)); //<Width/>
            AppendString(writer, HEIGHT_ELEMENT, string.Format("{0}cm", rectangle.Height)); //<Height/>
        }

        writer.WriteEndElement(); //</Image>
    }

    private void AppendString(XmlWriter writer, string elementName, string value)
    {
        writer.WriteStartElement(elementName);
        writer.WriteString(value);
        writer.WriteEndElement();
    }

    private void AppendString(XmlWriter writer, string prefix, string elementName, string ns, string value)
    {
        writer.WriteStartElement(prefix, elementName, ns);
        writer.WriteString(value);
        writer.WriteEndElement();
    }

    private void AppendTextRunStyle(XmlWriter writer, TextBoxStyle style)
    {
        writer.WriteStartElement(STYLE_ELEMENT); //<Style>

        AppendString(writer, FONTFAMILY_ELEMENT, style.FontName); //<FontFamily/>
        AppendString(writer, FONTSIZE_ELEMENT, style.FontSize); //<FontSize/>
        if (!string.IsNullOrEmpty(style.Format))
        {
            AppendString(writer, FORMAT_ELEMENT, style.Format); //<FontWeight/>
        }
        if (!string.IsNullOrEmpty(style.FontWeight))
        {
            AppendString(writer, FONTWEIGHT_ELEMENT, style.FontWeight); //<FontWeight/>
        }
        if (!string.IsNullOrEmpty(style.FontStyle))
        {
            AppendString(writer, FONTSTYLE_ELEMENT, style.FontStyle); //<FontStyle/>
        }
        if (!string.IsNullOrEmpty(style.TextDecoration))
        {
            AppendString(writer, TEXTDECORATION_ELEMENT, style.TextDecoration); //<TextDecoration/>
        }
        AppendString(writer, COLOR_ELEMENT, style.Color); //<Color/>

        writer.WriteEndElement(); //</Style>
    }

    private void AppendParagraphStyle(XmlWriter writer, TextBoxStyle style)
    {
        writer.WriteStartElement(STYLE_ELEMENT);//<Style>
        if (!string.IsNullOrEmpty(style.TextAlign))
        {
            AppendString(writer, TEXTALIGN_ELEMENT, style.TextAlign); //<TextAlign/>
        }

        writer.WriteEndElement();//</Style>
    }

    private void AppendTextBoxStyle(XmlWriter writer, TextBoxStyle style)
    {
        writer.WriteStartElement(STYLE_ELEMENT); //<Style>
        if (style.BorderWidth > 0)
        {
            writer.WriteStartElement(BORDER_ELEMENT); //<Border>
            //AppendString(writer, COLOR_ELEMENT, style.BorderColor); //<Color/>
            AppendString(writer, STYLE_ELEMENT, "Solid"); //<Style/>
            AppendString(writer, WIDTH_ELEMENT, string.Format("{0}pt", style.BorderWidth)); //<Style/>
            writer.WriteEndElement(); //</Border>

            if (!style.TopBorder)
            {
                writer.WriteStartElement(TOPBORDER_ELEMENT); //<TopBorder>
                AppendString(writer, STYLE_ELEMENT, "None"); //<Style/>
                writer.WriteEndElement(); //</TopBorder>
            }
            if (!style.LeftBorder)
            {
                writer.WriteStartElement(LEFTBORDER_ELEMENT); //<LeftBorder>
                AppendString(writer, STYLE_ELEMENT, "None"); //<Style/>
                writer.WriteEndElement(); //</LeftBorder>
            }
            if (!style.RightBorder)
            {
                writer.WriteStartElement(RIGHTBORDER_ELEMENT); //<RightBorder>
                AppendString(writer, STYLE_ELEMENT, "None"); //<Style/>
                writer.WriteEndElement(); //</RightBorder>
            }
            if (!style.BottomBorder)
            {
                writer.WriteStartElement(BOTTOMBORDER_ELEMENT); //<BottomBorder>
                AppendString(writer, STYLE_ELEMENT, "None"); //<Style/>
                writer.WriteEndElement(); //</BottomBorder>
            }
        }

        if (!string.IsNullOrEmpty(style.BackgroudColor))
        {
            AppendString(writer, BACKGROUNDCOLOR_ELEMENT, style.BackgroudColor); //<BackgroundColor/>
        }

        //if (!string.IsNullOrEmpty(style.VerticalAlign))
        //{
        //    WriteString(writer, VERTICALALIGN_ELEMENT, style.VerticalAlign);//<VerticalAlign/>
        //}

        AppendString(writer, PADDINGLEFT_ELEMENT, style.Padding); //<PaddingLeft/>
        AppendString(writer, PADDINGRIGHT_ELEMENT, style.Padding); //<PaddingRight/>
        AppendString(writer, PADDINGTOP_ELEMENT, style.Padding); //<PaddingTop/>
        AppendString(writer, PADDINGBOTTOM_ELEMENT, style.Padding); //<PaddingBottom/>

        writer.WriteEndElement(); //</Style>
    }

    private void AppendDefaultStyle(XmlWriter writer)
    {
        writer.WriteStartElement(STYLE_ELEMENT);//<Style>
        writer.WriteEndElement();//</Style>
    }

    public class TextBoxStyle
    {
        public TextBoxStyle()
        {
            LeftBorder = true;
            RightBorder = true;
            TopBorder = true;
            BottomBorder = true;
        }

        public string FontName { get; set; }

        public string FontSize { get; set; }

        public string FontWeight { get; set; }

        public string FontStyle { get; set; }

        public string TextDecoration { get; set; }

        public string Color { get; set; }

        public string TextAlign { get; set; }

        //public string BorderColor { get; set; }

        //public string BorderStyle { get; set; }

        public int BorderWidth { get; set; }

        public bool LeftBorder { get; set; }
        public bool RightBorder { get; set; }
        public bool TopBorder { get; set; }
        public bool BottomBorder { get; set; }

        public string BackgroudColor { get; set; }

        //public string VerticalAlign { get; set; }

        public string Padding { get; set; }

        public string Format { get; set; }

        public void SetBorder(int rowCount, int columnCount, int rowIndex, int columnIndex)
        {
            LeftBorder = columnIndex == 0;
            RightBorder = columnIndex == columnCount - 1;
            TopBorder = rowIndex == 0;
            BottomBorder = rowIndex == rowCount - 1;

        }
    }

    public class ItemRectangle
    {
        public double Left { get; set; }

        public double Width { get; set; }

        public double Top { get; set; }

        public double Height { get; set; }
    }
}

public class CodeParser
{
    public string AppVirtualDir { get; set; }

    public string AppPhysicalTargetDir { get; set; }

    public string Folder { get; set; }

    public string Namespace { get; set; }

    public string GetPageContent(string pageName, string pagePath)
    {
        var buildManager = new System.Web.Compilation.ClientBuildManager(AppVirtualDir, AppPhysicalTargetDir);
        try
        {
            var obj = new JObject();
            //script
            using (var reader = new System.IO.StreamReader(pagePath, true))
            {
                var content = reader.ReadToEnd();
                var startIndex = content.IndexOf("<script>", StringComparison.OrdinalIgnoreCase);
                if (startIndex > 0)
                {
                    startIndex += 8;
                    var endIndex = content.IndexOf("</script>", startIndex, StringComparison.OrdinalIgnoreCase);
                    if (endIndex > startIndex)
                    {
                        var script = content.Substring(startIndex, endIndex - startIndex);
                        obj["script"] = script;
                        content = content.Replace(script, string.Empty);
                        reader.Close();
                        using (var writer = new System.IO.StreamWriter(pagePath, false, new UTF8Encoding(true)))
                        {
                            writer.Write(content);
                        }
                    }
                }
            }
            obj["controls"] = new JArray();
            var pageType = buildManager.GetCompiledType(string.Format("{0}/{1}/{2}.aspx", AppVirtualDir, Folder, pageName));
            var page = Activator.CreateInstance(pageType) as Page;
            var buildControlMethod = pageType.GetMethod("__BuildControlTree", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            buildControlMethod.Invoke(page, new object[] { page });

            foreach (var control in page.Controls)
            {
                if (control.GetType().Namespace == Namespace)
                {
                    if (FilteredControls.Contains(control.GetType().FullName))
                    {
                        continue;
                    }
                    var objControl = GetItemProperties(control);
                    (obj["controls"] as JArray).Add(objControl);
                }
                else if (control is System.Web.UI.HtmlControls.HtmlForm)
                {
                    foreach (var childControl in (control as System.Web.UI.HtmlControls.HtmlForm).Controls)
                    {
                        if (childControl.GetType().Namespace == Namespace)
                        {
                            if (FilteredControls.Contains(childControl.GetType().FullName))
                            {
                                continue;
                            }
                            var objControl = GetItemProperties(childControl);
                            (obj["controls"] as JArray).Add(objControl);
                        }
                    }
                }

            }
            //code
            var code = new StringBuilder();
            using (var reader = new System.IO.StreamReader(string.Format("{0}.cs", pagePath), true))
            {
                var content = reader.ReadToEnd();
                var medthods = pageType.BaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in medthods)
                {
                    if (FilteredMethods.Contains(method.Name))
                    {
                        continue;
                    }
                    var match = Regex.Match(content, string.Format(@"{0}\s*\(.*\)", method.Name));
                    if (match.Success)
                    {
                        var startIndex = content.LastIndexOf("\n", match.Index);
                        startIndex += 1;

                        var bracket = 0;
                        var quot = 0;
                        for (int i = match.Index; i < content.Length; i++)
                        {
                            if (content[i] == '"')
                            {
                                quot++;
                            }
                            if (quot % 2 == 0)
                            {
                                if (content[i] == '{')
                                {
                                    bracket++;
                                }
                                else if (content[i] == '}')
                                {
                                    bracket--;
                                    if (bracket == 0)
                                    {
                                        code.AppendLine(content.Substring(startIndex, i + 1 - startIndex));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            obj["code"] = code.ToString(); ;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }
        finally
        {
            buildManager.Unload();
        }
    }

    public string GetXomlContent(string pageName, string pagePath)
    {
        var buildManager = new System.Web.Compilation.ClientBuildManager(AppVirtualDir, AppPhysicalTargetDir);
        var obj = new JObject();
        obj["controls"] = new JArray();
        //script
        using (var reader = new System.IO.StreamReader(pagePath, true))
        {
            var content = reader.ReadToEnd();
            System.IO.StringReader sr = new System.IO.StringReader(content);
            XmlDocument doc = new XmlDocument();
            doc.Load(sr);
            XmlElement xeRoot = doc.DocumentElement;

            JObject main = new JObject();
            main["type"] = "mainProperty";
            //main["tabName"]
            JObject mainPropertoes = new JObject();
            mainPropertoes["total"] = 14;
            mainPropertoes["rows"] = new JArray();
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "Description", xeRoot.Attributes["Description"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "EEPAlias", xeRoot.Attributes["EEPAlias"].Value, "dbalias"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "ExpTime", xeRoot.Attributes["ExpTime"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "ExpTimeField", xeRoot.Attributes["ExpTimeField"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "FormName", xeRoot.Attributes["FormName"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "Keys", xeRoot.Attributes["Keys"].Value, "collection", "Srvtools.KeyItem", "KeyFields", "KeyName"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "NotifySendMail", xeRoot.Attributes["NotifySendMail"].Value, "checkbox"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "OrgKind", xeRoot.Attributes["OrgKind"].Value, "orgkind"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "PresentFields", xeRoot.Attributes["PresentFields"].Value, "collection", "Srvtools.KeyItem", "PresentFields", "PresentField"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "SkipForSameUser", xeRoot.Attributes["SkipForSameUser"].Value, "checkbox"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "TableName", xeRoot.Attributes["TableName"].Value, "tablename"));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "TimeUnit", xeRoot.Attributes["TimeUnit"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "UrgentTime", xeRoot.Attributes["UrgentTime"].Value));
            (mainPropertoes["rows"] as JArray).Add(CreateFlowProperty("Infolight", "WebFormName", xeRoot.Attributes["WebFormName"].Value, "webformname"));
            main["properties"] = mainPropertoes;
            (obj["controls"] as JArray).Add(main);

            String postfix = "_node_flow_" + pageName;
            //{"type":"flowstart","id":"start_node_node_flow_f6","name":"start","left":262,"top":10,"height":26,"width":26,"movable":false,"editable":false,"row":0}
            JObject flowstart = new JObject();
            flowstart["type"] = "flowstart";
            flowstart["id"] = "start_node" + postfix;
            flowstart["name"] = "start";
            flowstart["row"] = 0;
            (obj["controls"] as JArray).Add(flowstart);
            JObject currentObject = flowstart;

            currentObject = CreateFlow(obj, xeRoot.ChildNodes, currentObject, postfix);

            //{"type":"flowend","id":"end_node_node_flow_f6","name":"end","left":262,"top":130,"height":26,"width":26,"movable":false,"editable":false,"row":0},
            JObject flowend = new JObject();
            flowend["type"] = "flowend";
            flowend["id"] = "end_node" + postfix;
            flowend["name"] = "end";
            flowend["row"] = 0;
            (obj["controls"] as JArray).Add(flowend);
            (obj["controls"] as JArray).Add(CreateFlowLine(currentObject["id"].ToString(), flowend["id"].ToString(), "sl"));
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        return json;
    }

    private JObject CreateFlow(JObject obj, XmlNodeList children, JObject currentObject, String postfix, String c = "")
    {
        foreach (XmlNode node in children)
        {
            if (node.Name == "IfElseActivity" || node.Name == "ParallelActivity")
            {
                String aId = node.Attributes["x:Name"].Value + postfix + "_S";
                String classify = node.Attributes["x:Name"].Value + postfix;
                JArray properties = new JArray();
                properties.Add(CreateFlowProperty("Default", "Name", aId));
                properties.Add(CreateFlowProperty("Default", "Description", node.Attributes["Description"] == null ? String.Empty : node.Attributes["Description"].Value));
                JObject flowActivityS = CreateFlowNode(aId, "", "flow" + node.Name + "_S", properties);
                flowActivityS["movable"] = false;
                flowActivityS["classify"] = classify;
                flowActivityS["width"] = flowActivityS["height"] = 16;
                (obj["controls"] as JArray).Add(flowActivityS);
                (obj["controls"] as JArray).Add(CreateFlowLine(currentObject["id"].ToString(), flowActivityS["id"].ToString(), "sl"));
                currentObject = flowActivityS;

                aId = node.Attributes["x:Name"].Value + postfix + "_E";
                properties = new JArray();
                properties.Add(CreateFlowProperty("Default", "Name", aId));
                properties.Add(CreateFlowProperty("Default", "Description", node.Attributes["Description"] == null ? String.Empty : node.Attributes["Description"].Value));
                JObject flowActivityE = CreateFlowNode(aId, "", "flow" + node.Name + "_E", properties);
                flowActivityE["movable"] = false;
                flowActivityE["classify"] = classify;
                flowActivityE["width"] = flowActivityE["height"] = 16;
                (obj["controls"] as JArray).Add(flowActivityE);

                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    String branchId = node.Attributes["x:Name"].Value + postfix + "_" + (i + 1).ToString();
                    JArray branchProperties = new JArray();
                    branchProperties.Add(CreateFlowProperty("Default", "Name", branchId));
                    branchProperties.Add(CreateFlowProperty("Default", "Description", node.ChildNodes[i].Attributes["Description"] == null ? String.Empty : node.ChildNodes[i].Attributes["Description"].Value));
                    JObject branchActivity = CreateFlowNode(branchId, node.ChildNodes[i].Attributes["x:Name"].Value, "flow" + branchId, branchProperties);
                    branchActivity["movable"] = false;
                    branchActivity["classify"] = classify;
                    (obj["controls"] as JArray).Add(branchActivity);
                    (obj["controls"] as JArray).Add(CreateFlowLine(currentObject["id"].ToString(), branchActivity["id"].ToString(), "lr"));
                    currentObject = branchActivity;

                    currentObject = CreateFlow(obj, node.ChildNodes[i].ChildNodes, currentObject, postfix, classify);
                    (obj["controls"] as JArray).Add(CreateFlowLine(currentObject["id"].ToString(), flowActivityE["id"].ToString(), "lr"));

                    if (i != node.ChildNodes.Count - 1) currentObject = flowActivityS;
                }
                currentObject = flowActivityE;
            }
            else
            {
                object flowNode = CreateFlowActivity(node);
                var flowItemProperties = GetFlowItemProperties(flowNode);
                JObject flowActivity = CreateFlowNode(node.Attributes["x:Name"].Value + postfix, node.Attributes["x:Name"].Value, flowNode.GetType().FullName, flowItemProperties);
                if (!String.IsNullOrEmpty(c)) flowActivity["classify"] = c;
                (obj["controls"] as JArray).Add(flowActivity);
                (obj["controls"] as JArray).Add(CreateFlowLine(currentObject["id"].ToString(), flowActivity["id"].ToString(), "sl"));

                currentObject = flowActivity;
            }
        }

        return currentObject;
    }

    private JObject CreateFlowNode(String id, String name, String type, JArray flowItemProperties)
    {
        JObject flowNode = new JObject();
        flowNode["id"] = id;
        flowNode["name"] = name;
        flowNode["type"] = type;
        flowNode["properties"] = flowItemProperties;
        return flowNode;
    }

    private JObject CreateFlowLine(String from, String to, String type)
    {
        JObject flowLine = new JObject();
        flowLine["id"] = from + "_to_" + to;
        flowLine["from"] = from;
        flowLine["to"] = to;
        flowLine["type"] = type;
        flowLine["marked"] = false;
        return flowLine;
    }

    private JObject CreateFlowProperty(String group, String propertyName, object propertyValue, String editorType = "validatebox", String type = "", String parentPropertyName = "", String captionfield = "")
    {
        JObject property = new JObject();
        property["group"] = group;
        property["name"] = propertyName;
        if (propertyValue is JArray)
            property["value"] = (JArray)propertyValue;
        else
            property["value"] = new JValue(propertyValue);
        JObject editor = new JObject();
        editor["type"] = editorType;
        if (!String.IsNullOrEmpty(type))
        {
            JObject options = new JObject();
            options["assembly"] = "Srvtools";
            options["type"] = type;
            options["parentPropertyName"] = parentPropertyName;
            options["captionfield"] = captionfield;
            editor["options"] = options;
        }
        property["editor"] = editor;

        return property;
    }

    private object CreateFlowActivity(XmlNode node)
    {
        object flowActivity = new object();
        Assembly a = Assembly.Load("FLTools");
        Type t = a.GetType("FLTools." + node.LocalName);
        if (t != null)
        {
            flowActivity = a.CreateInstance(t.FullName);
            foreach (XmlAttribute item in node.Attributes)
            {
                PropertyInfo property = t.GetProperties().Where(p => p.Name == item.Name).FirstOrDefault();
                if (property != null)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(flowActivity, Enum.Parse(property.PropertyType, item.Value.ToString()), null);
                    }
                    else if (property.PropertyType == typeof(Unit))
                    {
                        property.SetValue(flowActivity, Unit.Parse(item.Value.ToString()), null);
                    }
                    else
                    {
                        property.SetValue(flowActivity, Convert.ChangeType(item.Value, property.PropertyType), null);
                    }
                }
            }
            if (node.ChildNodes.Count > 0)
            {
                if (node.ChildNodes[0].Name == "ns0:FLApprove.ApproveRights")
                {
                    FLCore.ApproveRightCollection approveRights = new FLCore.ApproveRightCollection();
                    foreach (XmlElement item in node.ChildNodes[0].ChildNodes)
                    {
                        for (int i = 0; i < item.ChildNodes.Count; i++)
                        {
                            FLCore.ApproveRight aRight = new FLCore.ApproveRight();
                            aRight.Name = item.ChildNodes[i].Attributes["Name"].Value;
                            aRight.Grade = item.ChildNodes[i].Attributes["Grade"].Value;
                            aRight.Expression = item.ChildNodes[i].Attributes["Expression"].Value;
                            approveRights.Add(aRight);
                        }
                    }
                    t.GetProperty("ApproveRights").SetValue(flowActivity, approveRights, null);
                }
            }
        }
        return flowActivity;
    }

    private static string[] FilteredControls = new string[] { 
        "JQClientTools.JQScriptManager",
        "JQMobileTools.JQScriptManager"
    };

    private static string[] FilteredMethods = new string[] { 
        "Page_Load",
        "ProcessRequest"
    };

    public JObject GetItemProperties(object item)
    {
        var objProperties = new JObject();
        foreach (var property in item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            if (JsonHelper.CanEdit(property))
            {
                var value = property.GetValue(item, null);
                if (value != null)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        objProperties[property.Name] = new JValue((int)value);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        objProperties[property.Name] = new JValue(value.ToString().ToLower());
                    }
                    else if (property.PropertyType == typeof(System.Drawing.Font))
                    {
                        var font = (System.Drawing.Font)value;
                        objProperties[property.Name] = new JValue(string.Format("{0},{1}pt", font.FontFamily.Name, font.Size));
                    }
                    else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                    {
                        var objItems = new JArray();
                        foreach (var collectionItem in (IEnumerable)value)
                        {
                            objItems.Add(GetItemProperties(collectionItem));
                        }
                        objProperties[property.Name] = objItems;
                    }
                    else
                    {
                        objProperties[property.Name] = new JValue(value.ToString());
                    }
                }
            }
        }

        if (item is Control)
        {
            var objControl = new JObject();
            objProperties["ID"] = (item as Control).ID;
            objControl["properties"] = objProperties;
            objControl["type"] = item.GetType().FullName;
            if (((item as Control).Controls).Count > 0)
            {
                var objChildrent = new JArray();
                foreach (var childControl in (item as Control).Controls)
                {
                    if (childControl.GetType().Namespace == typeof(JQClientTools.JQDataGrid).Namespace)
                    {
                        objChildrent.Add(GetItemProperties(childControl));
                    }
                }
                objControl["children"] = objChildrent;
            }
            return objControl;
        }
        else
        {
            return objProperties;
        }
    }

    public JArray GetFlowItemProperties(object item)
    {
        Dictionary<string, object> objProperties = new Dictionary<string, object>();
        foreach (var property in item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            if (JsonHelper.CanEdit(property))
            {
                var value = property.GetValue(item, null);
                if (value != null)
                {
                    //String controlType = String.Empty;
                    //if (property.PropertyType == typeof(bool))
                    //{
                    //    controlType = "checkbox";
                    //}
                    //else if (typeof(System.Collections.IList).IsAssignableFrom(property.PropertyType))
                    //{
                    //    var objItems = new JArray();
                    //    foreach (var collectionItem in (IEnumerable)value)
                    //    {
                    //        objItems.Add(GetItemProperties(collectionItem));
                    //    }
                    //    value = objItems;
                    //}
                    //else
                    //{
                    //    controlType = "validatebox";
                    //}
                    objProperties.Add(property.Name, value);
                }
            }
        }
        String TypeName = item.GetType().FullName;
        return JsonHelper.CreatePropertyItems(TypeName.Split('.')[0], TypeName, null, objProperties);
    }
}