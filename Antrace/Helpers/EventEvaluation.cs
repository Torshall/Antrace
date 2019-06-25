using System.Collections;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using AntRace.Interfaces;
using AntRace.Containers;

namespace AntRace.Helpers
{
    /// <summary>
    /// Register and trigger event calls to functions.
    /// EventEvaluation - Checks for a function be true before triggering callbacks
    /// TriggerListener - Triggers all listeners with 
    /// Support for online sync
    /// </summary>
    public class EventTrigger
    {
        Dictionary<string, onEvaluateEvent> EventListener;
        Dictionary<string, DataContainer> DataContainers;
        Dictionary<string, onEvaluateEvent> GCSafeEvents;

        public static OnDataSync OnDataSyncCallback;

        #region Instance
        private static EventTrigger _Instance;
        public static EventTrigger Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new EventTrigger();

                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }
        #endregion

        public EventTrigger()
        {
            EventListener = new Dictionary<string, onEvaluateEvent>();
            DataContainers = new Dictionary<string, DataContainer>();

            GCSafeEvents = new Dictionary<string, onEvaluateEvent>();
        }

        /// <summary>
        /// Mainly used by the Datacontatiners to trigger over network
        /// </summary>
        /// <param name="key"></param>
        public void ForceTrigger(string key)
        {
            if (EventTrigger.Instance.EventListener.ContainsKey(key) && EventTrigger.Instance.EventListener[key] != null)
                EventTrigger.Instance.EventListener[key]();
        }

        /// <summary>
        /// Trigger event when a given key is invoked
        /// </summary>
        /// <param name="name">Key</param>
        /// <param name="EventToBeEvaluated"> Callback</param>
        /// <param name="disableGC"> If true, ignores cleanup call</param>
        public static void AddListener(string name, onEvaluateEvent EventToBeEvaluated, bool disableGC = false)
        {
            if (EventTrigger.Instance.EventListener.ContainsKey(name))
                EventTrigger.Instance.EventListener[name] += EventToBeEvaluated;
            else
                EventTrigger.Instance.EventListener.Add(name, EventToBeEvaluated);

            if (disableGC)
            {
                if (EventTrigger.Instance.GCSafeEvents.ContainsKey(name))
                    EventTrigger.Instance.GCSafeEvents[name] += EventToBeEvaluated;
                else
                    EventTrigger.Instance.GCSafeEvents.Add(name, EventToBeEvaluated);
            }
        }

        /// <summary>
        /// Return the DataContainer sent with the call, returns empty DataContainer if none sent
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataContainer GetData(string name)
        {
            if (EventTrigger.Instance.DataContainers.ContainsKey(name))
                return EventTrigger.Instance.DataContainers[name];

            return default(DataContainer);
        }

        protected static void SetData(DataContainer datavalue, bool Senddata = true)
        {
            if (EventTrigger.Instance.DataContainers.ContainsKey(datavalue.EventKey))
                EventTrigger.Instance.DataContainers[datavalue.EventKey] = datavalue;
            else
                EventTrigger.Instance.DataContainers.Add(datavalue.EventKey, datavalue);

            NetworkSyncData(datavalue.EventKey, datavalue);
            datavalue.Start(Senddata);
        }

        /// <summary>
        /// Triggers the callbacks
        /// </summary>
        /// <param name="name">Key</param>
        /// <param name="data">Data to send</param>
        public static void TriggerListener(string name, DataContainer data = null)
        {
            if (EventTrigger.Instance.EventListener.ContainsKey(name))
            {
                if (data != null)
                {
                    data.EventKey = name;
                    SetData(data);
                }
                else
                {
                    EventTrigger.Instance.EventListener[name]();
                }
            }
        }

        /// <summary>
        /// Gives callback to the network, if connected, to send the data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        protected static void NetworkSyncData(string name, DataContainer data = null)
        {
            if (OnDataSyncCallback != null)
                OnDataSyncCallback(name, data);
        }
        
        public static void RemoveListener(string name, onEvaluateEvent EventToBeEvaluated)
        {
            if (EventTrigger.Instance.EventListener.ContainsKey(name))
                EventTrigger.Instance.EventListener[name] -= EventToBeEvaluated;

            if (EventTrigger.Instance.GCSafeEvents.ContainsKey(name))
                EventTrigger.Instance.GCSafeEvents[name] -= EventToBeEvaluated;
        }

        public static void ClearAllListeners()
        {
            EventTrigger.Instance.EventListener.Clear();

            foreach (KeyValuePair<string, onEvaluateEvent> kvp in EventTrigger.Instance.GCSafeEvents)
            {
                AddListener(kvp.Key, kvp.Value);
            }
        }

    }
}
