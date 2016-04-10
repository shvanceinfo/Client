using UnityEngine;
using System.Collections;
using model;

namespace manager
{
    public class Trigger
    {
        public TriggerType Type { get; set; }

        public string Param { get; set; }

        public Trigger()
        {
            
        }

        public Trigger(TriggerType t)
        {
            Type = t;
        }
        public Trigger(TriggerType t, string p)
        {
            Type = t;
            Param = p;
        }
        public Trigger(TriggerType t, int p)
        {
            Type = t;
            Param = p.ToString();
        }
    }
}
