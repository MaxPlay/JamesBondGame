using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JamesBond
{
    public class Settings
    {
        public static float Audio;
        public static string Language;
        public static bool DisplayTimer;

        static Settings()
        {
            Load();
        }

        public static void Load()
        {
            if (!File.Exists("settings.txt"))
            {
                Audio = 1;
                Language = "de";
                DisplayTimer = true;
                Save();
            }

            try
            {
                using (FileStream stream = File.OpenRead("settings.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        if (!float.TryParse(reader.ReadLine(), out Audio))
                        {
                            Audio = 1;
                        }

                        string lang = reader.ReadLine();
                        if (IsPossibleLang(lang))
                        {
                            Language = lang;
                        }
                        else
                        {
                            Language = "de";
                        }

                        if (!bool.TryParse(reader.ReadLine(), out DisplayTimer))
                        {
                            DisplayTimer = true;
                        }
                    }
                }
                Debug.Log("Settings Loaded.");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Audio = 1;
                Language = "de";
                DisplayTimer = true;
            }
        }

        public static void Save()
        {
            using (FileStream stream = File.OpenWrite("settings.txt"))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(Audio);
                    writer.WriteLine(Language);
                    writer.WriteLine(DisplayTimer);
                }
            }
        }

        private static bool IsPossibleLang(string lang)
        {
            if (lang.Length > 2)
                return false;

            return File.Exists("strings." + lang + ".xml");
        }
    }
}
