using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace JamesBond
{
    public static class Localization
    {
        public static readonly string DefaultLanguage = "en";
        private static Dictionary<string, Dictionary<string, string>> languageSets;
        public static string errorText = "MISSING ENTRY.";
        public static string[] Languages;
        public static string[] ShortLanguages;

        public static void LoadLanguages()
        {
            string[] localizationfiles = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "strings.*.xml").ToArray();

            languageSets = new Dictionary<string, Dictionary<string, string>>();

            List<string> langs = new List<string>();
            List<string> shortlangs = new List<string>();

            for (int i = 0; i < localizationfiles.Length; i++)
            {
                string name = Path.GetFileName(localizationfiles[i]).Substring(8, 2);
                Dictionary<string, string> LanguageSet = LoadLanguageSet(localizationfiles[i], langs);
                languageSets.Add(name, LanguageSet);
                shortlangs.Add(name);
            }

            Languages = langs.ToArray();
            ShortLanguages = shortlangs.ToArray();
        }

        private static Dictionary<string, string> LoadLanguageSet(string file, List<string> langs)
        {
            Dictionary<string, string> languageSet = new Dictionary<string, string>();
            using (FileStream stream = File.OpenRead(file))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "strings")
                        {
                            langs.Add(reader.GetAttribute("name"));

                            ReadLanguageSet(languageSet, reader);
                        }
                    }
                }
            }

            return languageSet;
        }

        private static void ReadLanguageSet(Dictionary<string, string> languageSet, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "string")
                {
                    string id = reader.GetAttribute("identifier");
                    reader.Read();
                    string text = reader.Value;

                    languageSet.Add(id, text);
                }
            }
        }

        public static string GetString(string lang, string id)
        {
            if (languageSets.ContainsKey(lang))
            {
                if (languageSets[lang].ContainsKey(id))
                    return languageSets[lang][id];
                else
                    return errorText;
            }
            else if (languageSets.ContainsKey(DefaultLanguage))
            {
                if (languageSets[DefaultLanguage].ContainsKey(id))
                    return languageSets[DefaultLanguage][id];
                else
                    return errorText;
            }
            else
                return errorText;
        }
    }
}
