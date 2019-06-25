using AntRace.Helpers;
using AntRace.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Containers
{
    public class TimeContainer
    {
        public Start OnStart;
        public Tick OnTick;
        public onFinishedReturn OnTrigger;
        public float Timer;
        public object LinkedObject;
        public float WaitTime;
        public bool isLooping;
        public float TickTimeDelay;
        public float TickTimer;
        public float deltaTime;
        public float procentsFinished;
        public float StepSize;
        public bool isPaused;
        public bool RemoveNext;
        public float DelayedStart;

        public TimeContainer(Start _Start = null, Tick _Tick = null, onFinishedReturn _Trigger = null, object _Linked = null, float _Delay = 1, bool _Loop = false, float _TickTime = 0, float _delayedStart = 0)
        {
            OnStart = _Start;
            OnTick = _Tick;
            OnTrigger = _Trigger;
            isLooping = _Loop;
            WaitTime = _Delay;
            Timer = 0;
            LinkedObject = _Linked;
            TickTimeDelay = _TickTime;
            DelayedStart = _delayedStart;
            procentsFinished = 0;
            StepSize = 0;
        }

        public TimeContainer SetValues(Start _Start = null, Tick _Tick = null, onFinishedReturn _Trigger = null, object _Linked = null, float _Delay = 1, bool _Loop = false, float _TickTime = 0, float _delayedStart = 0)
        {
            OnStart = _Start;
            OnTick = _Tick;
            OnTrigger = _Trigger;
            isLooping = _Loop;
            WaitTime = _Delay;
            Timer = 0;
            LinkedObject = _Linked;
            TickTimeDelay = _TickTime;
            DelayedStart = _delayedStart;
            procentsFinished = 0;
            StepSize = 0;

            return this;
        }

        public void Remove()
        {
            RemoveNext = true;
        }

        public void ClearValues()
        {
            TickTimer = 0;
            deltaTime = 0;
            isPaused = false;
            RemoveNext = false;
            OnStart = null;
            OnTick = null;
            OnTrigger = null;
            LinkedObject = null;
        }
    }
}
