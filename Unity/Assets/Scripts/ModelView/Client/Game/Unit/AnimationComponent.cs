using Animancer;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class AnimationComponent: Entity, IAwake, IDestroy
    {
        public AnimancerComponent animancer;
        public EntityRef<UnitGoComponent> unitGo;
    }
}