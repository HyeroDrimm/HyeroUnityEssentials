using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyeroUnityEssentials
{
    public class TimeController
    {
        private static List<TimeScaleModifier> timeScaleModifiers = new();

        public static void AddModifier(TimeScaleModifier modifier)
        {
            timeScaleModifiers.Add(modifier);
            timeScaleModifiers.Sort((x,y) => -x.priority.CompareTo(y.priority));
            UpdateTimeScale();
        }

        public static void RemoveModifier(TimeScaleModifier modifier)
        {
            timeScaleModifiers.Remove(modifier);
            UpdateTimeScale();
        }

        public static void RemoveModifier(string name)
        {
            timeScaleModifiers.RemoveAt(timeScaleModifiers.FindIndex(x => x.name == name));
            UpdateTimeScale();
        }

        public static void UpdateTimeScale()
        {
            Time.timeScale = timeScaleModifiers.Count != 0 ? timeScaleModifiers[0].scaleModifier : 1;
        }

        public static void Clear(float timeScale = 1)
        {
            timeScaleModifiers.Clear();
            Time.timeScale = timeScale;
        }
    }

    public class TimeScaleModifier
    {
        public float scaleModifier;
        public string name;
        public int priority;

        public TimeScaleModifier(string name, float scaleModifier, int priority)
        { 
            this.name = name;
            this.scaleModifier = scaleModifier;
            this.priority = priority;
        }
    }
}
