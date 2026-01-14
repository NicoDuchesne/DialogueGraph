using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace F2O.SaveSystem
{
    [Serializable]
    public struct SettingDataSave : IDataSavable
    {
        public static string PrefixID => "SDS_";
        public string ID { get; private set; }
        public string Label { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public bool fullScreenOn;
        public int idResolution;

        public int idLanguage;
        public int idQuality;

        public int musicVolume;
        public int sfxVolume;

        public SettingDataSave(string label)
        {
            var datas = SaveSystem.GetAllSettingSaves();

            if (datas.Count != 0)
            {
                var IDs = datas.Select(x => int.Parse(x.ID.Split('_')[1])).Max();
                ID = PrefixID + ++IDs;
            }
            else
                ID = PrefixID + 0;

            Label = label;
            DateCreated = DateTime.Now;
            DateUpdated = DateCreated;

            fullScreenOn = true;
            idResolution = 0;
            idLanguage = 0;
            idQuality = 0;
            musicVolume = 1;
            sfxVolume = 1;
        }

        public SettingDataSave(string label, bool fullScreenOn, int idResolution, int idLanguage, int idQuality, int musicVolume, int sfxVolume)
        {
            this = new SettingDataSave(label)
            {
                fullScreenOn = fullScreenOn,
                idResolution = idResolution,
                idLanguage = idLanguage,
                idQuality = idQuality,
                musicVolume = musicVolume,
                sfxVolume = sfxVolume
            };
        }

        public override string ToString() => $"<b>{Label}</b> <i>({ID + SaveSystem.Extension})</i>";
    }
}
