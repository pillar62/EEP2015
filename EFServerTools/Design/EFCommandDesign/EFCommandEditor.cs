using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCFModule;

namespace EFServerTools.Design.EFCommandDesign
{
    public partial class EFCommandEditor : Form
    {
        public EFCommandEditor(string directroyName, string commandText, string metadataFile)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(directroyName))
            {
                throw new ArgumentNullException("directroyName");
            }
            _directroyName = directroyName;
            _commandText = commandText;
            _metadataFile = metadataFile;
        }

        private string _directroyName;
        private string DirectoryName
        {
            get
            {
                return _directroyName;
            }
        }

        private MetadataProvider _provider;

        private MetadataProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = new MetadataProvider(_directroyName, (string)comboBoxMeatadataFile.SelectedItem);
                }
                return _provider;
            }
        }

        private string _metadataFile;
        public string MetadataFile
        {
            get 
            {
                return _metadataFile;
            }
        }

        private string _commandText;
        public string CommandText
        {
            get
            {
                return _commandText;
            }
        }

        private void FormCommandEditor_Load(object sender, EventArgs e)
        {
            RefreshMetadataFiles();
            
            var ContainerName = EntityProvider.GetContainerName(CommandText);
            if (!string.IsNullOrEmpty(ContainerName) && comboBoxContainerName.Items.Contains(ContainerName))
            {
                comboBoxContainerName.SelectedItem = ContainerName;
            }
            var EntitySetName = EntityProvider.GetEntitySetName(CommandText);
            if (!string.IsNullOrEmpty(EntitySetName) && listBoxEntitySets.Items.Contains(EntitySetName))
            {
                listBoxEntitySets.SelectedItem = EntitySetName;
            }
            textBoxCommandText.Text = CommandText;
            textBoxCommandText.SelectionFont = textBoxCommandText.Font;
        }

        private void comboBoxMeatadataFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            _provider = null;
            _metadataFile = (string)comboBoxMeatadataFile.SelectedItem;
            RefreshEntityContainers();
        }

        private void comboBoxContainerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshEntitySets();
        }

        private void listBoxEntitySets_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshProperties();
        }

        private void RefreshMetadataFiles()
        {
            comboBoxMeatadataFile.Items.AddRange(MetadataProvider.GetMetadataFiles(DirectoryName).ToArray());
            if (comboBoxMeatadataFile.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(MetadataFile) && comboBoxMeatadataFile.Items.Contains(MetadataFile))
                {
                    comboBoxMeatadataFile.SelectedItem = MetadataFile;
                }
                else
                {
                    comboBoxMeatadataFile.SelectedIndex = 0;
                }
            }
        }

        private void RefreshEntityContainers()
        {
            comboBoxContainerName.Items.Clear();
            if (Provider != null)
            {
                comboBoxContainerName.Items.AddRange(Provider.GetEntityContainerNames().ToArray());
                if (comboBoxContainerName.Items.Count > 0)
                {
                    comboBoxContainerName.SelectedIndex = 0;
                }
            }
        }

        private void RefreshEntitySets()
        {
            listBoxEntitySets.Items.Clear();
            var containerName = (string)comboBoxContainerName.SelectedItem;
            if (Provider != null && !string.IsNullOrEmpty(containerName))
            {
                listBoxEntitySets.Items.AddRange(Provider.GetEntitySetNames(containerName).ToArray());
                if (listBoxEntitySets.Items.Count > 0)
                {
                    listBoxEntitySets.SelectedIndex = 0;
                }
            }
        }

        private void RefreshProperties()
        {
            listViewProperties.Items.Clear();
            var containerName = (string)comboBoxContainerName.SelectedItem;
            var entitySetName = (string)listBoxEntitySets.SelectedItem;

            if (Provider != null && !string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(entitySetName))
            {
                var entityTypeName = Provider.GetEntitySetType(containerName, entitySetName);
                var properties = Provider.GetPropertyNames(containerName, entityTypeName);
                foreach (var property in properties)
                {
                    listViewProperties.Items.Add(new ListViewItem(property, "property"));
                }
                var navproperties = Provider.GetNavgationPropertyNames(containerName, entityTypeName);
                foreach (var navproperty in navproperties)
                {
                    listViewProperties.Items.Add(new ListViewItem(navproperty, "navproperty"));
                }
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            var entitySet = (string)listBoxEntitySets.SelectedItem;
            if (!string.IsNullOrEmpty(entitySet))
            {
                var alias = entitySet.ToLower()[0];
                var sql = string.Format("Select value {0} from {1}.{2} as {0}"
                    , alias, comboBoxContainerName.SelectedItem, listBoxEntitySets.SelectedItem);
                textBoxCommandText.Text = sql;
            }
        }

        private void textBoxCommandText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (true)
            {
                if (Provider != null)
                {
                    var text = textBoxCommandText.Text.Substring(0, textBoxCommandText.SelectionStart).TrimEnd(e.KeyChar);
                    if (e.KeyChar.Equals(' ')) //按下空格
                    {
                        if (textBoxCommandText.Text.Trim().Length == 0) //sql是空的
                        {
                            ShowContextMenu(contextMenuStripScript);//select
                        }
                        else if (EndsWith(text, "from", true))//光标在from后
                        {
                            ShowContextMenu(contextMenuStripEntityContainer);
                        }
                        else
                        {
                            var alias = EntityProvider.GetAliasName(text);
                            if (EndsWith(text, "as " + alias, false))//光标在as c后
                            {
                                ShowContextMenu(contextMenuStripScript);//where and order
                            }
                            else if (EndsWith(text, "where", true) || (EndsWith(text, "and", true) && EntityProvider.ContainWhere(text) ))//光标在where c后
                            {
                                ShowContextMenu(contextMenuStripScript);//alias
                            }
                            else if (EndsWith(text, "order by", true) || (text.EndsWith(",") && EntityProvider.ContainOrder(text)))//光标在where c后
                            {
                                ShowContextMenu(contextMenuStripScript);//alias
                            }
                        }
                    }
                    else if (e.KeyChar.Equals('.'))//按下点
                    {
                        if (Provider.GetEntityContainerNames().Count(c => EndsWith(text, c, true)) > 0)//光标在Contanier后
                        {
                            ShowContextMenu(contextMenuStripEntitySet);
                        }
                        else
                        {
                            var alias = EntityProvider.GetAliasName(text);
                            if (EndsWith(text, alias, false))//光标在c后
                            {
                                ShowContextMenu(contextMenuStripProperty);
                            }
                        }
                    }
                }
            }
        }

#warning 正则要整理
        private bool EndsWith(string sql, string word, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            var option = ignoreCase ? System.Text.RegularExpressions.RegexOptions.IgnoreCase : System.Text.RegularExpressions.RegexOptions.None;
            return System.Text.RegularExpressions.Regex.IsMatch(sql, string.Format(@"\s+{0}$", word), option);
        }

        private void ShowContextMenu(ContextMenuStrip contextMenu)
        {
            InitContextMenu(contextMenu);
            var point = textBoxCommandText.GetPositionFromCharIndex(textBoxCommandText.SelectionStart);
            point.Offset(0, 15);
            contextMenu.Show(textBoxCommandText, point);
        }

        private void InitContextMenu(ContextMenuStrip contextMenu)
        {
            if (contextMenu == contextMenuStripScript)
            {
                InitcontextMenuStripScript();
            }
            else if (contextMenu == contextMenuStripEntityContainer)
            {
                InitcontextMenuStripEntityContainer();
            }
            else if (contextMenu == contextMenuStripEntitySet)
            {
                InitcontextMenuStripEntitySet();
            }
            else if (contextMenu == contextMenuStripProperty)
            {
                InitcontextMenuStripProperty();
            }
        }

        private void InitcontextMenuStripScript()
        {
            contextMenuStripScript.Items.Clear();

            if (!EntityProvider.ContainSelect(textBoxCommandText.Text))
            {
                var menuItemSelect = new ToolStripMenuItem("select", EFServerTools.Properties.Resources.script);
                menuItemSelect.Tag = "Select value c from";
                contextMenuStripScript.Items.Add(menuItemSelect);
            }
            else if (!EntityProvider.ContainWhere(textBoxCommandText.Text))
            {
                var menuItemWhere = new ToolStripMenuItem("where", EFServerTools.Properties.Resources.script);
                menuItemWhere.Tag = "Where";
                contextMenuStripScript.Items.Add(menuItemWhere);

                var menuItemOrderBy = new ToolStripMenuItem("orderby", EFServerTools.Properties.Resources.script);
                menuItemOrderBy.Tag = "Order by";
                contextMenuStripScript.Items.Add(menuItemOrderBy);
            }
            else
            {
                var alias = EntityProvider.GetAliasName(textBoxCommandText.Text);
                if (!string.IsNullOrEmpty(alias))
                {
                    var menuItemAlias = new ToolStripMenuItem(alias, EFServerTools.Properties.Resources.script);
                    menuItemAlias.Tag = alias;
                    contextMenuStripScript.Items.Add(menuItemAlias);
                }
            }
        }

        private void InitcontextMenuStripEntityContainer()
        {
            contextMenuStripEntityContainer.Items.Clear();
            if (Provider != null)
            {
                var containerNames = Provider.GetEntityContainerNames();
                foreach (var containerName in containerNames)
                {
                    var menuItem = new ToolStripMenuItem(containerName, EFServerTools.Properties.Resources.container);
                    menuItem.Tag = containerName;
                    contextMenuStripEntityContainer.Items.Add(menuItem);
                }
            }
        }

        private void InitcontextMenuStripEntitySet()
        {
            contextMenuStripEntitySet.Items.Clear();
            var containerName = EntityProvider.GetContainerName(textBoxCommandText.Text);
            if (Provider != null && !string.IsNullOrEmpty(containerName))
            {
                var entitySets = Provider.GetEntitySetNames(containerName);
                var alias = EntityProvider.GetAliasName(textBoxCommandText.Text);
                foreach (var entitySet in entitySets)
                {
                    var menuItem = new ToolStripMenuItem(entitySet, EFServerTools.Properties.Resources.entityset);
                    menuItem.Tag = entitySet + " as " + alias;
                    contextMenuStripEntitySet.Items.Add(menuItem);
                }
            }
        }

        private void InitcontextMenuStripProperty()
        {
            contextMenuStripProperty.Items.Clear();

            var containerName = EntityProvider.GetContainerName(textBoxCommandText.Text);
            var entitySetName = EntityProvider.GetEntitySetName(textBoxCommandText.Text);
            if (Provider != null && !string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(entitySetName))
            {
                var entityTypeName = Provider.GetEntitySetType(containerName, entitySetName);
                var properties = Provider.GetPropertyNames(containerName, entityTypeName);
                foreach (var property in properties)
                {
                    var menuItem = new ToolStripMenuItem(property, EFServerTools.Properties.Resources.property);
                    menuItem.Tag = property;
                    contextMenuStripProperty.Items.Add(menuItem);
                }
            }
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null)
            {
                var insertText = e.ClickedItem.Tag.ToString();
                var position = textBoxCommandText.SelectionStart;
                textBoxCommandText.Text = textBoxCommandText.Text.Insert(position, insertText);
                textBoxCommandText.SelectionStart = position + insertText.Length;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            var text = textBoxCommandText.Text;
            if(!string.IsNullOrEmpty(EntityProvider.GetContainerName(text)) && !string.IsNullOrEmpty(EntityProvider.GetEntitySetName(text))
                && EntityProvider.ContainSelect(text))
            {
                _commandText = text;
            }
            else
            {
                MessageBox.Show(this, "Command text is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
