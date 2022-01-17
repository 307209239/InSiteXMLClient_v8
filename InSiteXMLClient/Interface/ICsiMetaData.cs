using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiMetaData
    {
        ICsiCdoType GetCdoType();

        ICsiCdoDefinition GetCdoDefinition();

        ICsiLabel GetLabel();

        ICsiLabel GetFieldLabel();

        ICsiLabel GetCdoLabel();

        ICsiFieldDefinition GetCdoField();

        void RequestCdoSubTypesByName(string name, bool recurse);

        void RequestCdoSubTypesById(int Id, bool recurse);

        void RequestCdoDefinitionByName(string name);

        void RequestCdoDefinitionById(int Id);

        void RequestCdoDefinitionFieldByName(string name, string fieldName);

        void RequestLabelById(int labelId);

        void RequestLabelByName(string labelName);

        void RequestQueryParameters(string queryName);

        void RequestCdoDefinition();

        void RequestUserDefinedFields();

        void RequestFieldItem(string itemName);

        void RequestFieldLabel();

        void RequestCdoLabel();

        ICsiQueryParameters GetQueryParameters();

        Array GetUserDefinedFields();
    }
}