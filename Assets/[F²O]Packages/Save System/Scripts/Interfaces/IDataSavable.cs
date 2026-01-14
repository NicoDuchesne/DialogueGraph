using System;

namespace F2O.SaveSystem
{
    public interface IDataSavable
    {
        public static string PrefixID { get; }
        public string ID { get; }
        public string Label { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
