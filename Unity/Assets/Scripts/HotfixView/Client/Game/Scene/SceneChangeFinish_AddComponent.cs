using System;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinish_AddComponent: AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene root, SceneChangeFinish args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();
                currentScene.AddComponent<CameraComponent>();
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.WindowsEditor:
                        currentScene.AddComponent<PcInputComponent>();
                        break;
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.LinuxPlayer:
                        currentScene.AddComponent<PcInputComponent>();
                        break;
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.Android:
                        currentScene.AddComponent<PhoneComponent>();
                        break;
                    case RuntimePlatform.WebGLPlayer:
                        break;
                    case RuntimePlatform.PS4:
                        break;
                    case RuntimePlatform.XboxOne:
                        break;
                    case RuntimePlatform.tvOS:
                        break;
                    case RuntimePlatform.Switch:
                        break;
                    case RuntimePlatform.Stadia:
                        break;
                    case RuntimePlatform.GameCoreXboxSeries:
                        break;
                    case RuntimePlatform.GameCoreXboxOne:
                        break;
                    case RuntimePlatform.PS5:
                        break;
                    case RuntimePlatform.EmbeddedLinuxArm64:
                        break;
                    case RuntimePlatform.EmbeddedLinuxArm32:
                        break;
                    case RuntimePlatform.EmbeddedLinuxX64:
                        break;
                    case RuntimePlatform.EmbeddedLinuxX86:
                        break;
                    case RuntimePlatform.LinuxServer:
                        break;
                    case RuntimePlatform.WindowsServer:
                        break;
                    case RuntimePlatform.OSXServer:
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            await ETTask.CompletedTask;
        }
    }
}