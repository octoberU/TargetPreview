using System;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPreview.Scripts
{
    public static class TimeController
    {
        public static float Time;
        
        static List<IReceiveTimeUpdates> updatesArray = new List<IReceiveTimeUpdates>(10);

        public static void SetTime(float time)
        {
            Time = time;
            foreach (var update in updatesArray) 
                update.OnTimeUpdated(time);
        }

        public static void AddListener(IReceiveTimeUpdates receiveTimeUpdates)
        {
            updatesArray.Add(receiveTimeUpdates);
            receiveTimeUpdates.OnTimeUpdated(Time);
        }
        public static void RemoveListener(IReceiveTimeUpdates receiveTimeUpdates) =>
            updatesArray.Remove(receiveTimeUpdates);
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void StaticReset()
        {
            updatesArray.Clear();
            Time = 0;
        }
    }

    public interface IReceiveTimeUpdates
    {
        public void OnTimeUpdated(float time);
    }
    

}