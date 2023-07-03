using System.Runtime.Serialization;
using System;

namespace GiftStoreMVC.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string name, double y)
        {
            this.Name = name;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "name")]
        public string Name = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}
