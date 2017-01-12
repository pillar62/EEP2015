using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace System.Data.Objects.DataClasses
{
    public static class EntityObjectExtension
    {
        /// <summary>
        /// Gets value of property of entity object
        /// </summary>
        /// <param name="entityObject">Entity object</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Value of property</returns>
        public static object GetValue(this EntityObject entityObject, string propertyName)
        {
            if (entityObject == null)
            {
                throw new ArgumentNullException("entityObject");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            var index = propertyName.IndexOf('.');
            if (index > 0 && index < propertyName.Length - 1)
            {
                var navigationProperty = propertyName.Substring(0, index);
                var fieldName = propertyName.Substring(index + 1);
                var navigation = entityObject.GetValue(navigationProperty);
                if (navigation != null && navigation is EntityObject)
                {
                    return (navigation as EntityObject).GetValue(fieldName);
                }
                return null;
            }
            else
            {
                var property = GetProperty(entityObject.GetType(), propertyName);
                return property.GetValue(entityObject, null);
            }
        }

        /// <summary>
        /// Sets value of property of entity object
        /// </summary>
        /// <param name="entityObject">Entity object</param>
        /// <param name="propertyName">Name of property</param>
        /// <param name="value">Value of property</param>
        public static void SetValue(this EntityObject entityObject, string propertyName, object value)
        {
            if (entityObject == null)
            {
                throw new ArgumentNullException("entityObject");
            }
            var property = GetProperty(entityObject.GetType(), propertyName);
            object changedValue = null;
            if (value != null)
            {
                var type = property.PropertyType;
                if (!type.IsByRef && type.IsGenericType)
                {
                    type = type.GetGenericArguments()[0];
                }
                changedValue = Convert.ChangeType(value, type);
            }
            property.SetValue(entityObject, changedValue, null);
        }

        /// <summary>
        /// Load navigation reference of entity object
        /// </summary>
        /// <param name="entityObject">Entity object</param>
        /// <param name="referenceName">Name of reference</param>
        public static void LoadReference(this EntityObject entityObject, string referenceName)
        {
            if (entityObject == null)
            {
                throw new ArgumentNullException("entityObject");
            }
            if (string.IsNullOrEmpty(referenceName))
            {
                throw new ArgumentNullException("referenceName");
            }
            var index = referenceName.IndexOf('.');
            if (index > 0 && index < referenceName.Length - 1)
            {
                var navigationProperty = referenceName.Substring(0, index);
                var fieldName = referenceName.Substring(index + 1);

                LoadReference(entityObject, navigationProperty);
                var navigationObjects = entityObject.GetValue(navigationProperty);
                if (navigationObjects != null)
                {
                    if (navigationObjects is EntityObject)
                    {
                        LoadReference(navigationObjects as EntityObject, fieldName);
                    }
                    else if (navigationObjects is IEnumerable)
                    {
                        foreach (EntityObject obj in (IEnumerable)navigationObjects)
                        {
                            LoadReference(obj, fieldName);
                        }
                    }
                }
            }
            else
            {
                var property = GetProperty(entityObject.GetType(), referenceName);
                if (!typeof(IRelatedEnd).IsAssignableFrom(property.PropertyType))
                {
                    property = GetProperty(entityObject.GetType(), string.Format("{0}Reference", referenceName));
                }
                if (property != null && typeof(IRelatedEnd).IsAssignableFrom(property.PropertyType))
                {
                    var relatedEnd = (IRelatedEnd)property.GetValue(entityObject, null);
                    if (relatedEnd != null && !relatedEnd.IsLoaded)
                    {
                        relatedEnd.Load();
                    }
                }
            }
        }

        /// <summary>
        /// Gets property information of type
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Property information</returns>
        private static PropertyInfo GetProperty(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new MissingMemberException(type.Name, propertyName);
            }
            return property;
        }

    }
}
