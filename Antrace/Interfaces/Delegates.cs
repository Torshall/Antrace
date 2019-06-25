using AntRace.Containers;
using AntRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Interfaces
{
    public delegate bool onEvaluateEvent();

    public delegate void Start();
    public delegate void Tick(TimeContainer delegator);
    public delegate void onFinishedReturn(object obj = null);    

    public delegate void OnDataSync(string key, DataContainer data);
}
