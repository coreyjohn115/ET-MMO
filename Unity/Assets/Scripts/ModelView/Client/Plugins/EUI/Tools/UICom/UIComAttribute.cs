namespace ET.Client
{
    public class UIComAttribute: BaseAttribute
    {
        public string Name { get; }

        public UIComAttribute(string name)
        {
            this.Name = name;
        }
    }

    public abstract class AUIComHandler
    {
        public abstract void Show(Entity uiCom);
    }
}