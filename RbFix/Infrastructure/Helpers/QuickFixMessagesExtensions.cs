
using QuickFix;
using QuickFix.DataDictionary;
using QuickFix.Fields;
using System.Text;
using System.Text.Json;

namespace RbFix.Infrastructure.Helpers
{
    public static class QuickFixMessagesExtensions
    {

        private static JsonWriterOptions jsonWriterOptions = new JsonWriterOptions() { Indented = true };

        public static string GetJson(this Message message)
        {
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, jsonWriterOptions);


            writer.WriteStartObject();
            writer.WriteStartObject(message.GetType().Name);

            WriteTagsToStream(writer, message.Header,message.ApplicationDataDictionary);
            WriteTagsToStream(writer, message,message.ApplicationDataDictionary);
            WriteTagsToStream(writer, message.Trailer,message.ApplicationDataDictionary);

            writer.WriteEndObject();
            writer.WriteEndObject();

            writer.Flush();

            string json = Encoding.UTF8.GetString(stream.ToArray());

            return json;

        }


        private static void WriteTagsToStream(Utf8JsonWriter writer, FieldMap fieldMap,DataDictionary dictionary)
        {
            List<int> groupsTag = fieldMap.GetGroupTags();

            foreach (KeyValuePair<int, IField> field in fieldMap) 
            {
                if (groupsTag.Contains(field.Value.Tag))
                    WriteRepeatingGroupsTagsToStream(writer, fieldMap, field.Value.Tag, dictionary);                           

                else
                    writer.WriteString(GetFieldPropertyNameFromDictionaryElseTag(field.Value.Tag, dictionary), field.Value.ToString());
            }
        }

        private static void WriteRepeatingGroupsTagsToStream(Utf8JsonWriter writer, FieldMap fieldMap, int tagNumber, DataDictionary dictioany)
        {
            writer.WriteStartArray(GetFieldPropertyNameFromDictionaryElseTag(tagNumber, dictioany));

            for (int x = 1; x <= fieldMap.GetInt(tagNumber); x++)
            {
                var group = fieldMap.GetGroup(x, tagNumber);
                List<int> subGroupsInGroupTag = group.GetGroupTags();
                writer.WriteStartObject();
                foreach (var groupdField in group)
                {
                    if (subGroupsInGroupTag.Contains(groupdField.Value.Tag))
                        WriteRepeatingGroupsTagsToStream(writer, group, groupdField.Value.Tag, dictioany);
                    else
                        writer.WriteString(GetFieldPropertyNameFromDictionaryElseTag(groupdField.Value.Tag, dictioany), groupdField.Value.ToString());
                }
                writer.WriteEndObject();
            }                 

            writer.WriteEndArray();
        }

        private static string GetFieldPropertyNameFromDictionaryElseTag(int tagNumber, DataDictionary dictioany)
        {
            if (dictioany is not null &&
                                dictioany.FieldsByTag.ContainsKey(tagNumber) &&
                                dictioany.FieldsByTag.TryGetValue(tagNumber, out DDField dictionaryField))
                return dictionaryField.Name;

            else
                return
                    tagNumber.ToString();
           
        }
    }
}
