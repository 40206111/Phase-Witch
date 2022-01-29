using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static int VolumeMusice = 50;
    public static int VolumeSound = 50;
    public static int VolumeMaster = 50;

    public static float HoverTipsTime = 1.0f;
    public static bool HoverTipsShow = true;

    public static class TextSpeed
    {
        public const float Slow = 0.1f;
        public const float Medium = 0.05f;
        public const float Fast = 0.02f;
        public const float VeryFast = 0.01f;

        public static string GetSpeedName(float speed)
        {
            return speed switch
            {
                Slow => "Slow",
                Medium => "Medium",
                Fast => "Fast",
                VeryFast => "Very Fast",
                _ => "Not a text speed.",
            };
        }
    }
    public static float TimePerChar = TextSpeed.Fast;
}
