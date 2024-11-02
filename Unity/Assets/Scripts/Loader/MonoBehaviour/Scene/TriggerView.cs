using UnityEngine;

namespace ET.Client
{
    public struct TriggerEnter
    {
        public string Name;
        public string Args;
        public GameObject TriggerGo;
    }

    public struct TriggerExit
    {
        public string Name;
        public string Args;
        public GameObject TriggerGo;
    }

    public class TriggerView: MonoBehaviour
    {
        public string Name;

        public string Args;

        private void OnTriggerEnter(Collider other)
        {
            EventSystem.Instance.Invoke(new TriggerEnter() { Name = this.Name, TriggerGo = other.gameObject, Args = this.Args });
        }

        private void OnTriggerExit(Collider other)
        {
            EventSystem.Instance.Invoke(new TriggerExit() { Name = this.Name, TriggerGo = other.gameObject, Args = this.Args });
        }
    }
}