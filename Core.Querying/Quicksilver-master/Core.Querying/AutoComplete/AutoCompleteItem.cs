using System;
using System.Runtime.Serialization;

namespace Core.Querying.AutoComplete
{
    [Serializable]
    [DataContract]
    public class AutoCompleteItem
    {
        public AutoCompleteItem()
        {
        }

        public AutoCompleteItem(string id, string text)
        {
            Id = id;
            Text = text;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}