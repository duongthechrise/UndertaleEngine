using System;
using System.IO;
using System.Text.Json;


namespace UndertaleEngine.Game1
{
    public class Settings
    {
        public int WindowWidth { get; set; } = 640;
        public int WindowHeight { get; set; } = 480;

        public bool FullScreen { get; set; } = false;
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int WindowPosX { get; set; }
        public int WindowPosY { get; set; }
    }

    public static class GameSettings
    {
        public static Settings Current { get; private set; }

        public static void LoadSettings(string path = "settings.json")
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    Current = JsonSerializer.Deserialize<Settings>(json);
                }
                else
                {
                    Current = new Settings();
                    SaveSettings(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading settings: " + ex.Message);
                Current = new Settings();
                SaveSettings(path);
            }

            if (Current == null)
                Current = new Settings();
        }

        public static void SaveSettings(string path = "settings.json")
        {
            try
            {
                string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error saving settings: " + ex.Message);
            }
        }
    }
}