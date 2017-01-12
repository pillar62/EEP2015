using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using EFWCFModule;
using System.Data;
using EFBase;
using EFBase.Design;

namespace EFServerTools.Design
{
    /// <summary>
    ///  Edm property dropdown editor
    /// </summary>
    internal class EdmPropertyDropDownEditor : PropertyDropDownEditor
    {
        /// <summary>
        /// Get related command of object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Related command</returns>
        private ICommand GetRelatedCommand(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (obj is IEFProperty)
            {
                var parentProperty = (obj as IEFProperty).ParentProperty;
                if (parentProperty == null)
                {
                    var component = (obj as IEFProperty).Component;
                    if (component == null)
                    {
                        throw new ArgumentNullException("Component");
                    }
                    return GetRelatedCommand(component);
                }
                else
                {
                    return GetRelatedCommand(parentProperty);
                }
            }
            else if (obj is ICommand)
            {
                return obj as ICommand;
            }
            else if (obj is IUseCommand)
            {
                var command = (obj as IUseCommand).Command;
                if (command == null)
                {
                    throw new ArgumentNullException("Command");
                }
                return command;
            }
            else if (obj is IUseUpdateComponent)
            {
                var updateComponent = (obj as IUseUpdateComponent).UpdateComponent;
                if (updateComponent == null)
                {
                    throw new ArgumentNullException("UpdateComponent");
                }
                return GetRelatedCommand(updateComponent);
            }
            else
            {
                throw new ArgumentException("Component not related to command.");
            }
        }

        /// <summary>
        /// Get related entity set of object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Related entity set</returns>
        private IUseEntitySet GetRelatedEntitySet(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (obj is IUseEntitySet)
            {
                return obj as IUseEntitySet;
            }   
            if (obj is IEFProperty)
            {
                var parentProperty = (obj as IEFProperty).ParentProperty;
                if (parentProperty == null)
                {
                    var component = (obj as IEFProperty).Component;
                    if (component == null)
                    {
                        throw new ArgumentNullException("Component");
                    }
                    return GetRelatedEntitySet(component);
                }
                else
                {
                    return GetRelatedEntitySet(parentProperty);
                }
            }
            else
            {
                throw new ArgumentException("Component not related to entity set.");
            }
        }

        /// <summary>
        /// Gets list of values
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>List of values</returns>
        public override List<string> GetListOfValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance != null && context.PropertyDescriptor != null)
            {
                var edmItemAttribute = context.PropertyDescriptor.Attributes[typeof(EdmItemAttribute)];
                if (edmItemAttribute == null)
                {
                    throw new ObjectNotFoundException(string.Format("EdmItemAttribute not found on property:{0}.", context.PropertyDescriptor.Name));
                }
                if (edmItemAttribute.Equals(EdmItemAttribute.EdmMetadataAttribute))
                {
                    return MetadataProvider.GetMetadataFiles(DTE.CurrentDirectory);
                }
                else
                {
                    var command = GetRelatedCommand(context.Instance);
                    var provider = new MetadataProvider(DTE.CurrentDirectory, command.MetadataFile);
                    if (edmItemAttribute.Equals(EdmItemAttribute.EdmEntitySetAttribute))
                    {
                        return provider.GetEntitySetNames(command.ContextName);
                    }
                    var entitySetName = context.PropertyDescriptor.Attributes[typeof(TargetEntitySetItemAttribute)] == null
                        ? command.EntitySetName : GetRelatedEntitySet(context.Instance).TargetEntitySet;
                    var entityTypeName = provider.GetEntitySetType(command.ContextName, entitySetName);
                    if (edmItemAttribute.Equals(EdmItemAttribute.EdmScalePropertyAttribute))
                    {
                        return provider.GetPropertyNames(command.ContextName, entityTypeName);
                    }
                    else if (edmItemAttribute.Equals(EdmItemAttribute.EdmNavigationPropertyAttribute))
                    {
                        return provider.GetNavgationPropertyNames(command.ContextName, entityTypeName);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("EdmItemAttribute:{0} not supported.", edmItemAttribute.TypeId));
                    }
                }
            }
            return new List<string>();
        }
    }

    /// <summary>
    /// Edm item attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EdmItemAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of edm item attribute
        /// </summary>
        /// <param name="itemType">Item type</param>
        public EdmItemAttribute(string itemType)
        {
            if (string.IsNullOrEmpty(itemType))
            {
                throw new ArgumentNullException("itemType");
            }
            this._itemType = itemType;
        }

        private string _itemType;
        /// <summary>
        /// Gets item type
        /// </summary>
        public string ItemType
        {
            get { return _itemType; }
        }

        /// <summary>
        /// Gets typeid
        /// </summary>
        public override object TypeId
        {
            get
            {
                return _itemType;
            }
        }

        #region Const String
        /// <summary>
        /// ScaleProperty
        /// </summary>
        public const string SCALE_PROPERTY = "ScaleProperty";
        /// <summary>
        /// NavigationProperty
        /// </summary>
        public const string NAVIGATION_PROPERTY = "NavigationProperty";
        /// <summary>
        /// EntitySet
        /// </summary>
        public const string ENTITYSET = "EntitySet";
        /// <summary>
        /// Metadata
        /// </summary>
        public const string METADATA = "Metadata"; 
        #endregion
        /// <summary>
        /// Scale property item attribute
        /// </summary>
        internal static EdmItemAttribute EdmScalePropertyAttribute = new EdmItemAttribute(SCALE_PROPERTY);
        /// <summary>
        /// Navigation property item attribute
        /// </summary>
        internal static EdmItemAttribute EdmNavigationPropertyAttribute = new EdmItemAttribute(NAVIGATION_PROPERTY);
        /// <summary>
        /// EntitySet item attribute
        /// </summary>
        internal static EdmItemAttribute EdmEntitySetAttribute = new EdmItemAttribute(ENTITYSET);
        /// <summary>
        /// Metadata item attribute
        /// </summary>
        internal static EdmItemAttribute EdmMetadataAttribute = new EdmItemAttribute(METADATA);

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns> true if the specified System.Object is equal to the current System.Object</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is EdmItemAttribute)
            {
                return this.ItemType.Equals((obj as EdmItemAttribute).ItemType);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns> A hash code for the current System.Object</returns>
        public override int GetHashCode()
        {
            return this.ItemType.GetHashCode();
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class TargetEntitySetItemAttribute : Attribute
    {

    }
}
