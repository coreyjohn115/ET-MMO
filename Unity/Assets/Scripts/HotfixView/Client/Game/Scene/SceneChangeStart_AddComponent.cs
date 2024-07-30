using System;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();

                ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();

                // 加载场景资源
                string path = $"Assets/Bundles/Scenes/{currentScene.Name}.unity";
                var handler = resourcesLoaderComponent.LoadScene(path, LoadSceneMode.Single);
                while (true)
                {
                    EventSystem.Instance.Publish(root, new LoadingProgress() { Progress = handler.Progress });
                    await root.GetComponent<TimerComponent>().WaitFrameAsync();
                    if (handler.IsDone)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}