namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class G2C_KickHandler: MessageHandler<Scene, G2C_Kick>
    {
        protected override async ETTask Run(Scene scene, G2C_Kick message)
        {
            scene.RemoveComponent<SessionComponent>();
            await UIComponentHelper.OpenConfirm(scene, 200003, 200004, default, new ConfirmBtn(UIConfirmType.Ok));
            Init.ExitGame();
        }
    }
}