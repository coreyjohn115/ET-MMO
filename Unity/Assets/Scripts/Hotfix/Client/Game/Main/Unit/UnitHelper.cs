namespace ET.Client
{
    public static partial class UnitHelper
    {
        public static bool IsMainUnit(Entity entity, long id)
        {
            long myId = entity.Root().GetComponent<ClientPlayerComponent>().MyId;
            return myId == id;
        }

        public static Unit GetMyUnitFromClientScene(Scene root)
        {
            ClientPlayerComponent playerComponent = root.GetComponent<ClientPlayerComponent>();
            Scene currentScene = root.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene?.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetUnitFromClientScene(Scene root, long id)
        {
            Scene currentScene = root.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(id);
        }

        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            ClientPlayerComponent playerComponent = currentScene.Root().GetComponent<ClientPlayerComponent>();
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetUnitFromCurrentScene(Scene currentScene, long id)
        {
            return currentScene.GetComponent<UnitComponent>().Get(id);
        }

        /// <summary>
        /// 是否在战斗状态
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsBattle(this Unit self)
        {
            return self.GetComponent<ClientAbilityComponent>().IsBattle;
        }
    }
}