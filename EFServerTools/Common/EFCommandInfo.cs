using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFServerTools.Common
{
    internal class EFCommandInfo
    {
        public const string CommandText = "Gets or sets the text command to run against the data source.";
        public const string CommandType = "Gets or sets the type of command to run against the data source.";
        public const string CommandTimeout = "The wait time before terminating the attempt to execute a command and generating an error.";
        public const string MultiSetWhere = "A value indicating style of apply where.";
        public const string SecStyle = "The style of security of command.";
        public const string SecFieldName = "The field of security of command.";
        public const string SecExcept = "The value of group not apply security of query command.";
        public const string SelectTop = "The amount of data in 'select top' string.";
        public const string Parameters = "The parameters of store procedure.";
        public const string ForeignKeyRelations = "Foreign key relations.";
        public const string ObjectName = "Object name of foreign key relation.";
        public const string DataBase = "The alias of database used by command.";
        public const string MetadataFile = "Metadata file.";
        public const string UseSystemDB = "Indicates whether use system database.";
    }
}
