namespace MusicPlayerApp.Data
{
    public class AppConfig
    {
        private static AppConfig instance;

        public bool DebugMode { get; set; }

        private AppConfig()
        {
            DebugMode = false;
        }

        /// <summary>
        /// Returns the single instance of AppConfig, initializing if necessary.
        /// </summary>
        public static AppConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppConfig();
                }
                return instance;
            }
        }
    }
}
