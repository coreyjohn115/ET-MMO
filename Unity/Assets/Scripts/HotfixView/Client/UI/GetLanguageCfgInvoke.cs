using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class GetLanguageCfgInvoke: AInvokeHandler<LanguageLoader.GetLanguageCfg, Pair<Color, string>>
    {
        public override Pair<Color, string> Handle(LanguageLoader.GetLanguageCfg args)
        {
            var config = LanguageCategory.Instance.Get(args.Id);
            return Pair<Color, string>.Create(config.ColorBytes.BytesColor(), config.Msg);
        }
    }
}