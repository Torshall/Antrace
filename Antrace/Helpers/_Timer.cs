using System.Collections.Generic;
using System.Text;
using System;
using AntRace.Interfaces;
using AntRace.Containers;

namespace AntRace.Helpers
{ 
    public class _Timer
    {
        public List<TimeContainer> ActiveTimecontainersList = new List<TimeContainer>();
        List<TimeContainer> InactiveTimecontainersList = new List<TimeContainer>();

        #region Instance
        private static _Timer _Instance;
        public static _Timer Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new _Timer();

                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }
        #endregion
        public _Timer()
        {
            _Instance = this;
        }
        
        /// <summary>
        /// Creates a TimeContainer and adds it to the active timers list
        /// </summary>
        /// <param name="_Trigger">Event to trigger</param>
        /// <param name="_Delay">Time until trigger</param>
        /// <param name="_Loop">Loops the timer</param>
        /// <param name="_Linked">Linked Object</param>
        /// <param name="_Tick">Event call on each tick</param>
        /// <param name="_TickTime">Time between ticks</param>
        /// <param name="_Start">Event on timer start</param>
        /// <param name="_delayedStart">Time to wait before starting timer</param>
        /// <returns></returns>
        public static TimeContainer AddDelegate(onFinishedReturn _Trigger = null, float _Delay = 1, bool _Loop = false, object _Linked = null, Tick _Tick = null, float _TickTime = 0, Start _Start = null, float _delayedStart = 0)
        {
            TimeContainer TimeDelegate = new TimeContainer(_Start, _Tick, _Trigger, _Linked, _Delay, _Loop, _TickTime, _delayedStart);
            Instance.ActiveTimecontainersList.Add(TimeDelegate);

            if (_delayedStart > 0)
            {
                TimeDelegate.isPaused = true;
                AddDelegate((object o) =>
                {
                    if (_Start != null)
                        _Start();
                    TimeDelegate.isPaused = false;
                }, _delayedStart);
            }
            else if (_Start != null)
                _Start();

            return TimeDelegate;
        }

        public void RemoveListener(TimeContainer del)
        {
            if (ActiveTimecontainersList.Contains(del))
            {
                ActiveTimecontainersList.Remove(del);
                del.LinkedObject = null;
            }
        }

        public void ClearAllTimers()
        {
            for (int i = 0; i < ActiveTimecontainersList.Count; ++i)
            {
                ActiveTimecontainersList[i].Remove();
            }
        }

        public static void Update(float dt)
        {
            for (int i = 0; i < Instance.ActiveTimecontainersList.Count; ++i)
            {
                if (Instance.ActiveTimecontainersList[i].RemoveNext)
                {
                    Instance.RemoveListener(Instance.ActiveTimecontainersList[i]);
                    i--;
                    continue;
                }

                if (Instance.ActiveTimecontainersList[i].isPaused)
                    continue;

                TimeContainer timeDelegate = Instance.ActiveTimecontainersList[i];
                timeDelegate.Timer += dt;
                timeDelegate.procentsFinished = timeDelegate.Timer / timeDelegate.WaitTime;
                timeDelegate.StepSize = dt / timeDelegate.WaitTime;
                timeDelegate.deltaTime = dt;


                //Check if more or less than target value
                if (timeDelegate.Timer > timeDelegate.WaitTime)
                {
                    if (timeDelegate.OnTrigger != null)
                        timeDelegate.OnTrigger(timeDelegate.LinkedObject);

                    //Reset if looping, delete if not
                    if (timeDelegate.isLooping)
                    {
                        timeDelegate.Timer = 0;
                        timeDelegate.TickTimer = 0;
                        if (timeDelegate.OnStart != null)
                            timeDelegate.OnStart();
                    }
                    else
                    {
                        if (Instance.ActiveTimecontainersList.Contains(timeDelegate))
                        {
                            Instance.RemoveListener(timeDelegate);
                            i--;
                            continue;
                        }
                    }
                }
                else if (timeDelegate.OnTick != null)
                {
                    timeDelegate.TickTimer += dt;
                    if (timeDelegate.TickTimer > timeDelegate.TickTimeDelay)
                    {
                        timeDelegate.OnTick(timeDelegate);
                        timeDelegate.TickTimer -= timeDelegate.TickTimeDelay;
                    }
                }
            }
        }
    }
}