using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public partial class Scroll_Item_Menu
    {
        public MeunData MeunData => this.meunData;

        public EntityRef<MeunData> meunData;
    }
}