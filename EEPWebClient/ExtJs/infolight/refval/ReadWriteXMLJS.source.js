// JScript File

var xmlDoc;

/*
//Javascript读 XMl,用于Ext.Grid的MultiLanguage的机制
//目前支持ie和firfox
    var moz = (typeof document.implementation != 'undefined') && (typeof document.implementation.createDocument != 'undefined');
    var ie = (typeof window.ActiveXObject != 'undefined');

    //加载XML
    if (moz) 
    {
        xmlDoc = document.implementation.createDocument("", "doc", null);
    } 
    else if (ie) 
    {
        xmlDoc = new ActiveXObject("MSXML2.DOMDocument.3.0");
        xmlDoc.async = false;
        while(xmlDoc.readyState != 4) {};
    }
    xmlDoc.load(multi_XmlUrl);
    //上面这部分要写在function外，是因为xmlDoc.load()需要一個過程，提前load
*/

/*改进新的写法，支持ie,firefox,google Chrome,Apple Safari*/
var xmlhttp = null;
if (window.ActiveXObject)
{
	try
	{
		xmlhttp=new ActiveXObject("Msxml2.XMLHTTP");
	}
	catch (e)
	{
		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
	}
}
else if (window.XMLHttpRequest)
{
	xmlhttp=new XMLHttpRequest();
}

//loadxmldoc(multi_XmlUrl);

function getxml(x)
{
    if (xmlhttp!=null)
    {  
        xmlhttp.open("GET",x,false);
        xmlhttp.send(null);
    }
    else
    {
        alert("Your browser does not support XMLHTTP.11");
        return false;
    }
	return(xmlhttp);
}

function loadxmldoc(x)
{	
    if (window.ActiveXObject)
    {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async=false; //同步加载，意思是等xml文件全部装载完成之后才进行操作  
        xmlDoc.load(x);
    }
    else if (window.XMLHttpRequest)
    {
        xmlDoc = getxml(x).responseXML;
    }
    else
    {
        alert("Your browser does not support XMLHTTP.11 ! Multilanguage is invalid !");
       //return false;
    }
}

function genieReadXml()
{
    /*
    如：
    multi_XmlUrl = "bOPOM02.aspx.xml"; 
    multi_languageType = "CHT";
    multi_ExtGridIDs = "wgvDetail;wgvView";
    multi_fileds = "PRODENAME|PRODSTRUCTURE;PRODENAME|PRODSTRUCTURE";
    參數解析：
    multi_XmlUrl：每支程式對應的Mulitlanguage設定檔。
    multi_languageType：預設的語言類型，后面load后會重新抓取當前的。
    multi_ExtGridIDs：頁面中有多個Ext.Grid時，用;分隔，這里是Grid的ID
    multi_fileds:每個Grid要顯示的欄位，可以為空，多個Grid是以';'分隔，每個Grid的欄位是以'|'分隔。可以為空，為空表示全部顯示。
    */
    
    //String.prototype.Trim = function() { return this.replace(/(^\s*)|(\s*$)/g, ""); }
    
    var MultiStr = "";
    var root;
    var nodeList;
    var Multijson;
    var splitStr1 = ";";
    var splitStr2 = "|";
    try
    {
        multi_languageType = document.form1[curMultiLanHiddenID].value;
        //解析Xml
        if(window.ActiveXObject)
        {
             root = xmlDoc.selectSingleNode("/Infolight/"+multi_languageType);
             nodeList = root.childNodes; 
        }
        else if (document.implementation && document.implementation.createDocument)
        {
            root = xmlDoc.getElementsByTagName("Infolight")[0].getElementsByTagName(multi_languageType)[0];
            nodeList = root.getElementsByTagName("Language");
        }
        
        var nodeLen = nodeList.length;

        for(var j=0;j<nodeLen;j++) {
            if (typeof (multi_fileds) != 'undefined' && multi_fileds != "") {
                var gridAry = multi_ExtGridIDs.split(splitStr1);
                var filedAry = multi_fileds.split(splitStr1);
                for (var p = 0; p < gridAry.length; p++) {
                    var filedAry2 = filedAry[p].split(splitStr2);
                    for (var k = 0; k < filedAry2.length; k++) {
                        var captionName = gridAry[p] + "." + filedAry2[k] + ".HeadText";
                        if (nodeList[j].attributes[0].nodeName == "field" && nodeList[j].attributes[0].nodeValue == captionName) {
                            if (nodeList[j].attributes[1].nodeName == "value")
                                MultiStr += "'" + gridAry[p] + "_" + filedAry2[k] + "':'" + nodeList[j].attributes[1].nodeValue + "',";
                        }
                    }
                }
            }
            else if (typeof (multi_ExtGridIDs) != 'undefined' && typeof (splitStr1) != 'undefined' && splitStr1 != "") {
                var gridAry = multi_ExtGridIDs.split(splitStr1);
                for (var p = 0; p < gridAry.length; p++) {
                    if (nodeList[j].attributes[0].nodeName == "field") {
                        var captionName = nodeList[j].attributes[0].nodeValue;
                        var captionAry = captionName.split('.');
                        if (captionAry[0] == gridAry[p]) {
                            if (nodeList[j].attributes[1].nodeName == "value")
                                MultiStr += "'" + captionAry[0] + "_" + captionAry[1] + "':'" + nodeList[j].attributes[1].nodeValue + "',";
                        }
                    }
                }
            }

            if (nodeList[j].attributes[0].nodeName == "field") {
                var captionName = nodeList[j].attributes[0].nodeValue;
                var captionAry = captionName.split('.');
                if (captionName.indexOf("GexRefVal") > -1) {
                    if (nodeList[j].attributes[1].nodeName == "value")
                        MultiStr += "'" + captionAry[0] + "_" + captionAry[1] + "':'" + nodeList[j].attributes[1].nodeValue + "',";
                }
            }
        }

        MultiStr = MultiStr.substr(0,MultiStr.length -1);
        Multijson = eval("({"+MultiStr+"})");
    }
    catch(e)
    {
        alert("error:Multilanguage is invalid !");
        Multijson = "";
    }
    return Multijson;
}

/*
注意XML的转移字符
<	&lt;
>	&gt;
&	&amp;
"	&quot;
'	&apos;
*/
function getAnyQueryDefault()
{
    //debugger;
    //解析Xml
    var root;
    var nodeList;
    var MultiStr='';
    var Multijson;
    
    try
    {
        if(window.ActiveXObject)
        {
             root = xmlDoc.selectSingleNode("/Infolight/AnyQueryDefault");
             nodeList = root.childNodes; 
        }
        else if (document.implementation && document.implementation.createDocument)
        {
            root = xmlDoc.getElementsByTagName("AnyQueryDefault")[0];
            nodeList = root.getElementsByTagName("QueryField");
        }
        
        var nodeLen = nodeList.length;
        
        for(var x=0;x<nodeLen;x++)
        {
            if(MultiStr != '' || MultiStr == undefined)
                MultiStr += ",";
            
            var idx = nodeList[x].attributes.getNamedItem("idx").nodeValue;
            var relation = nodeList[x].attributes.getNamedItem("relation").nodeValue;
            var leftParenthesis = nodeList[x].attributes.getNamedItem("leftParenthesis").nodeValue;
            var fieldname = nodeList[x].attributes.getNamedItem("fieldname").nodeValue;
            var operator = nodeList[x].attributes.getNamedItem("operator").nodeValue;
            var value = nodeList[x].attributes.getNamedItem("value").nodeValue;
            var rightParenthesis = nodeList[x].attributes.getNamedItem("rightParenthesis").nodeValue;
            
            MultiStr += "{'idx':'"+idx+"','relation':'"+relation+"','leftParenthesis':'"+leftParenthesis+"','fieldname':'"+fieldname+"','operator':'"+operator+"','value':'"+value+"','rightParenthesis':'"+rightParenthesis+"'}";
        }
        
        Multijson = eval("(["+MultiStr+"])");
    }
    catch(e)
    {
        alert("error:Multilanguage is invalid !");
        Multijson = "";
    }
    return Multijson;
}
