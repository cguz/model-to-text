/**
* @file PreferenceRenderJSONImp.cs
*
* @brief This file contains the JSON implementation of the render. 
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Render
{
    using Newtonsoft.Json;
    using Preference.IntelliText.Template;
    using System.Text.Json;

    class PreferenceRenderJSONImp : IPreferenceRender
    {
        public string Run(IDescriptionModel description)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(description,  
                Formatting.Indented, 
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }

    /*
    class ListOfItemsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<Item>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject item = new JObject();

            foreach (var val in (value as List<Item>))
            {
                var internalItem = new JObject { new JProperty("prop1", val.Prop1) };
                item.Add(new JProperty(val.Name, internalItem));
            }

            item.WriteTo(writer);
        }
    }
    */
}