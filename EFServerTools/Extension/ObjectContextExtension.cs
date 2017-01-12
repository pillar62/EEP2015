//Reference by http://blogs.msdn.com/cesardelatorre/archive/2008/09/04/updating-data-using-entity-framework-in-n-tier-and-n-layer-applications-short-lived-ef-contexts.aspx
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Reflection;

namespace System.Data.Objects
{
    public static class ObjectContextExtension
    {
        public static void AttachUpdated(this ObjectContext context, EntityObject objectDetached)
        {
            if (objectDetached.EntityState == EntityState.Detached)
            {
                object currentEntityInDb = null;
                if (context.TryGetObjectByKey(objectDetached.EntityKey, out currentEntityInDb))
                {
                    context.ApplyCurrentValues(objectDetached.EntityKey.EntitySetName, objectDetached);
                    //context.ApplyPropertyChanges(objectDetached.EntityKey.EntitySetName, objectDetached);
                    //(CDLTLL)Apply property changes to all referenced entities in context 
                    context.ApplyReferencePropertyChanges((IEntityWithRelationships)objectDetached,
                        (IEntityWithRelationships)currentEntityInDb); //Custom extensor method 
                }
                else
                {
                    throw new ObjectNotFoundException();
                }
            }
        }

        public static void AttachInserted(this ObjectContext context, EntityObject objectDetached, string entitySetName)
        {
            if (objectDetached.EntityState == EntityState.Detached)
            {
                context.AddObject(entitySetName, objectDetached);
            }
        }
        
        public static void ApplyReferencePropertyChanges(this ObjectContext context,
                                                IEntityWithRelationships newEntity,
                                                IEntityWithRelationships oldEntity)
        {
            if (oldEntity != null)
            {
                foreach (var relatedEnd in oldEntity.RelationshipManager.GetAllRelatedEnds())
                {
                    var oldRef = relatedEnd as EntityReference;
                    if (oldRef != null)
                    {
                        // this related end is a reference not a collection 
                        var newRef = newEntity.RelationshipManager.GetRelatedEnd(oldRef.RelationshipName, oldRef.TargetRoleName) as EntityReference;
                        if (newRef.EntityKey != null)
                        {
                            oldRef.EntityKey = newRef.EntityKey;
                        }
                    }
                }
            }
        }


        public static EntityObject CreateObject(this ObjectContext context, string entitySetName)
        {
            var property = context.GetType().GetProperty(entitySetName);
            if (property == null)
            {
                throw new ObjectNotFoundException(string.Format("EntitySet:{0} not found.", entitySetName));
            }
            if (!property.PropertyType.IsGenericType)
            {
                throw new EntityException(string.Format("EntitySet:{0} type is not generic type.", entitySetName));
            }
            var type = property.PropertyType.GetGenericArguments()[0];
            var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
            if (constructor == null)
            {
                throw new MissingMemberException(type.Name, type.Name);
            }
            return (EntityObject)constructor.Invoke(null);
        }

        public static object GetObjectCurrentValue(this ObjectContext context, EntityObject entityObject, string propertyName)
        {
            EntityObject entityObjectInDb = (EntityObject)context.GetObjectByKey(entityObject.EntityKey);
            return context.ObjectStateManager.GetObjectStateEntry(entityObjectInDb).CurrentValues[propertyName];
        }

        public static object GetObjectOriginalValue(this ObjectContext context, EntityObject entityObject, string propertyName)
        {
            EntityObject entityObjectInDb = (EntityObject)context.GetObjectByKey(entityObject.EntityKey);
            return context.ObjectStateManager.GetObjectStateEntry(entityObjectInDb).OriginalValues[propertyName];
        }

        public static void ExecuteNonQuery(this ObjectContext context, string commandText)
        {
            var connection = (context.Connection as System.Data.EntityClient.EntityConnection).StoreConnection;
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }


    }
}
