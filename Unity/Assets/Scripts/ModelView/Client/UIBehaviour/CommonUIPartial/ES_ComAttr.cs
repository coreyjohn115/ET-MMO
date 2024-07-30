using System.Collections.Generic;

namespace ET.Client
{
    public partial class ES_ComAttr: IUILogic, IUICom
    {
        void IUICom.SetActive(bool active)
        {
            this.uiTransform.SetActive(active);
        }
    }
}