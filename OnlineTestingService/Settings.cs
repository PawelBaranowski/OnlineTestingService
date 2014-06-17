using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace OnlineTestingService
{
    [Serializable]
    public class Settings
    {
        private static string settingsPath;

        private static Settings settings;
        private static XmlSerializer serializer;

        static Settings()
        {
            settingsPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\settings.xml";
            serializer = new XmlSerializer(typeof(Settings));
            try
            {
                TextReader reader = new StreamReader(settingsPath);
                settings = (Settings)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (IOException)
            {
                settings = new Settings();
            }
        }

        private Settings() { }

        
        public int _NotificationDelay { get; set; }
        public string _CandidateNotificationSubject { get; set; }
        public string _CandidateNotificationTemplate { get; set; }
        public string _EmployeeNotificationSubject { get; set; }
        public string _EmployeeNotificationTemplate { get; set; }
        public string _SendEmailFromAddress { get; set; }
        public string _UrlPrefix { get; set; }

        private static void Save()
        {
            TextWriter writer = new StreamWriter(settingsPath);
            serializer.Serialize(writer, settings);
            writer.Close();
        }

        public static int NotificationDelay
        {
            get { return settings._NotificationDelay; }
            set
            {
                settings._NotificationDelay = value;
                Save();
            }
        }
        public static string CandidateNotificationSubject
        {
            get { return settings._CandidateNotificationSubject; }
            set
            {
                settings._CandidateNotificationSubject = value;
                Save();
            }
        }
        public static string CandidateNotificationTemplate
        {
            get { return settings._CandidateNotificationTemplate; }
            set
            {
                settings._CandidateNotificationTemplate = value;
                Save();
            }
        }
        public static string EmployeeNotificationSubject
        {
            get { return settings._EmployeeNotificationSubject; }
            set
            {
                settings._EmployeeNotificationSubject = value;
                Save();
            }
        }
        public static string EmployeeNotificationTemplate
        {
            get { return settings._EmployeeNotificationTemplate; }
            set
            {
                settings._EmployeeNotificationTemplate = value;
                Save();
            }
        }
        public static string SendEmailFromAddress
        {
            get { return settings._SendEmailFromAddress; }
            set
            {
                settings._SendEmailFromAddress = value;
                Save();
            }
        }
        public static string UrlPrefix
        {
            get { return settings._UrlPrefix; }
            set
            {
                settings._UrlPrefix = value;
                Save();
            }
        }
    }
}