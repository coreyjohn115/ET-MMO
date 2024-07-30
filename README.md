### English: please use your browser to translate to english

### 1. 登录创建保存流程

### 2. 缓存服

### 3. 聊天服

### 4. 道具和任务(服务端)

### 5. 账号服

#### 1. 存储服务器, 账号和角色数据

#### 2. 数据保存和代码重载

* 利用Http请求实现数据保存

```csharp
[HttpHandler(SceneType.Account, "/save")]
public class HttpSaveDataRequestHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        var set = new HashSet<string>() { "Cache", "Chat", "Map", "Rank" };
        foreach (StartSceneConfig config in StartSceneConfigCategory.Instance.GetAll().Values)
        {
            if (set.Contains(config.Name))
            {
                IResponse resp = null;
                try
                {
                    resp = await scene.GetComponent<MessageSender>().Call(config.ActorId, W2Other_SaveDataRequest.Create());
                }
                catch (Exception)
                {
                    HttpHelper.Response(context, $"保存数据错误: {resp.Error}, {config.Name} - {config.Id}");
                    return;
                }
            }
        }

        HttpHelper.Response(context, "Success");
        await ETTask.CompletedTask;
    }
}
```

#### 3. 数据查询

* 利用Http请求实现数据查询

```csharp
[HttpHandler(SceneType.Account, "/query_info")]
    public class HttpQueryInfoHandler: IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            string sceneName = context.Request.QueryString["scene"];
            if (sceneName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "scene type is null");
                return;
            }

            string componentName = context.Request.QueryString["component"];
            if (componentName.IsNullOrEmpty())
            {
                HttpHelper.Response(context, "component name is null");
                return;
            }

            var actorId = StartSceneConfigCategory.Instance.GetBySceneName(scene.Zone(), sceneName).ActorId;
            var rep = await scene.GetComponent<MessageSender>().Call(actorId, new ConmponentQueryRequest() { ComponentName = componentName });
            if (rep == null)
            {
                HttpHelper.Response(context, "component not found");
                return;
            }

            byte[] data = (rep as ComponentQueryResponse).Entity;
            var entity = MongoHelper.Deserialize<Entity>(data);
            string field = context.Request.QueryString["field"];
            if (!field.IsNullOrEmpty())
            {
                var f = entity.GetType().GetField(field,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.NonPublic);
                if (f == null)
                {
                    HttpHelper.Response(context, "field not found");
                    return;
                }

                HttpHelper.Response(context, f.GetValue(entity));
                return;
            }

            HttpHelper.Response(context, entity);
        }
    }
```
交流群: 951617541
## 友情链接

- [ET 框架](https://github.com/egametang/ET) 强大的基于C#的双端框架
- [xasset](https://github.com/xasset/xasset) 致力于为 Unity 项目提供了一套 精简稳健 的资源管理环境
- [ET UI框架](https://github.com/zzjfengqing/ET-EUI) 字母哥实现的UI框架，ET风格，各种事件分发
