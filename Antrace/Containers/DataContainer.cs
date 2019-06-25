using Microsoft.Xna.Framework;
using AntRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Containers
{
    /// <summary>
    /// Datacontainer used by EventEvaluation
    /// Made with network calls in mind. Will trigger on server and client at the same time
    /// </summary>
    public class DataContainer
    {
        #region ValueArrays
        public float[] FloatValues = new float[] { };
        public int FloatIndex;

        public int[] IntValues = new int[] { };
        public int IntIndex;

        public string[] StringValues = new string[] { };
        public int StringIndex;

        public bool[] BoolValues = new bool[] { };
        public int BoolIndex;

        public Vector2[] VectorValues = new Vector2[] { };
        public int VectorIndex;
        #endregion

        public string EventKey;
        public double NetworkTimeSinceRecived = 0;
        public float TriggerTime = 0.2f;

        void ResetIndex()
        {
            FloatIndex = 0;
            IntIndex = 0;
            StringIndex = 0;
            BoolIndex = 0;
            VectorIndex = 0;
        }

        /// <summary>
        /// Starts the callback in sync with sender and client using timer
        /// </summary>
        /// <param name="isSending"></param>
        public void Start(bool isSending)
        {
            ResetIndex();

            float time = (float)(TriggerTime - NetworkTimeSinceRecived);            
            time = MathHelper.Clamp(time, 0, Math.Abs(time));
            _Timer.AddDelegate((object o) =>
            {
                EventTrigger.Instance.ForceTrigger(EventKey);
            }, time > 0 ? time : 0);
        }
       
        #region GetValues
        public int NextInt()
        {
            if (IntIndex >= IntValues.Length)
                IntIndex = 0;

            int ret = IntValues[IntIndex];
            IntIndex++;

            return ret;
        }

        public float NextFloat()
        {
            if (FloatIndex >= FloatValues.Length)
                FloatIndex = 0;

            float ret = FloatValues[FloatIndex];
            FloatIndex++;

            return ret;
        }

        public bool NextBool()
        {
            if (BoolIndex >= BoolValues.Length)
                BoolIndex = 0;

            bool ret = BoolValues[BoolIndex];
            BoolIndex++;

            return ret;
        }

        public string NextString()
        {
            if (StringIndex >= StringValues.Length)
                StringIndex = 0;

            string ret = StringValues[StringIndex];
            StringIndex++;

            return ret;
        }

        public Vector2 NextVector2()
        {
            if (VectorIndex >= VectorValues.Length)
                VectorIndex = 0;

            Vector2 ret = VectorValues[VectorIndex];
            VectorIndex++;

            return ret;
        }
        #endregion

    }
}
