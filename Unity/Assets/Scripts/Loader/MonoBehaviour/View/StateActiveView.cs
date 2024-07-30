using System;
using UnityEngine;

namespace ET.Client
{
    public class StateActiveView: MonoBehaviour
    {
        [Serializable]
        private class StateActive
        {
            public int State;
            public GameObject Go;
        }

        public void SetStateActive(int state)
        {
            foreach (StateActive ss in this.states)
            {
                ss.Go.SetActive(ss.State == state);
            }
        }

        [SerializeField]
        private StateActive[] states;
    }
}