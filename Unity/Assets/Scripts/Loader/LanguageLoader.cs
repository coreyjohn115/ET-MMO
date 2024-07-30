using UnityEngine;

namespace ET
{
    [Code]
    public class LanguageLoader: Singleton<LanguageLoader>, ISingletonAwake
    {
        public struct GetLanguageCfg
        {
            public int Id;
        }

        public void Awake()
        {
        }

        public Pair<Color, string> GetLanguage(int id)
        {
            var r = EventSystem.Instance.Invoke<GetLanguageCfg, Pair<Color, string>>(new GetLanguageCfg() { Id = id });
            return r;
        }
    }
}