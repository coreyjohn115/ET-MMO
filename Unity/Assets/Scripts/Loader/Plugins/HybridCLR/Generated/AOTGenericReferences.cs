using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"MemoryPack.dll",
		"MongoDB.Bson.dll",
		"System.Core.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"System.dll",
		"Unity.Core.dll",
		"Unity.Loader.dll",
		"Unity.ThirdParty.dll",
		"UnityEngine.CoreModule.dll",
		"YooAsset.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// ET.AEvent<object,ET.AddItemEvent>
	// ET.AEvent<object,ET.AddUpdateTask>
	// ET.AEvent<object,ET.AfterCreateCurrentScene>
	// ET.AEvent<object,ET.AfterUnitCreate>
	// ET.AEvent<object,ET.AppStartInitFinish>
	// ET.AEvent<object,ET.BasicChangeEvent>
	// ET.AEvent<object,ET.ChangePosition>
	// ET.AEvent<object,ET.ChangeRotation>
	// ET.AEvent<object,ET.Client.AddBuffView>
	// ET.AEvent<object,ET.Client.ClientUseSkill>
	// ET.AEvent<object,ET.Client.DelGroup>
	// ET.AEvent<object,ET.Client.EquipUpdateEvent>
	// ET.AEvent<object,ET.Client.FightStateChange>
	// ET.AEvent<object,ET.Client.HotKeyEvent>
	// ET.AEvent<object,ET.Client.InitTask>
	// ET.AEvent<object,ET.Client.MenuSelectEvent>
	// ET.AEvent<object,ET.Client.NetError>
	// ET.AEvent<object,ET.Client.PlayActionEvent>
	// ET.AEvent<object,ET.Client.PopHurtHud>
	// ET.AEvent<object,ET.Client.PushActionEvent>
	// ET.AEvent<object,ET.Client.RemoveBuffView>
	// ET.AEvent<object,ET.Client.UpdateGroup>
	// ET.AEvent<object,ET.Client.UpdateMsg>
	// ET.AEvent<object,ET.Client.UpdateShield>
	// ET.AEvent<object,ET.DeleteTask>
	// ET.AEvent<object,ET.EnterMapFinish>
	// ET.AEvent<object,ET.EntryEvent1>
	// ET.AEvent<object,ET.EntryEvent3>
	// ET.AEvent<object,ET.LoadingProgress>
	// ET.AEvent<object,ET.MoveStart>
	// ET.AEvent<object,ET.MoveStop>
	// ET.AEvent<object,ET.NumericChange>
	// ET.AEvent<object,ET.RemoveItemEvent>
	// ET.AEvent<object,ET.SceneChangeFinish>
	// ET.AEvent<object,ET.SceneChangeStart>
	// ET.AEvent<object,ET.UpdateFashionEffectEvent>
	// ET.AInvokeHandler<ET.FiberInit,object>
	// ET.AInvokeHandler<ET.LanguageLoader.GetLanguageCfg,ET.Pair<UnityEngine.Color,object>>
	// ET.AInvokeHandler<ET.MailBoxInvoker>
	// ET.AInvokeHandler<ET.NetComponentOnRead>
	// ET.AInvokeHandler<ET.TimerCallback>
	// ET.AwakeSystem<object,byte>
	// ET.AwakeSystem<object,int,int>
	// ET.AwakeSystem<object,int>
	// ET.AwakeSystem<object,object,int>
	// ET.AwakeSystem<object,object>
	// ET.AwakeSystem<object>
	// ET.DestroySystem<object>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CloseConfirm>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>
	// ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>
	// ET.ETAsyncTaskMethodBuilder<System.ValueTuple<byte,long>>
	// ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>
	// ET.ETAsyncTaskMethodBuilder<byte>
	// ET.ETAsyncTaskMethodBuilder<int>
	// ET.ETAsyncTaskMethodBuilder<object>
	// ET.ETAsyncTaskMethodBuilder<uint>
	// ET.ETTask<ET.Client.Wait_CloseConfirm>
	// ET.ETTask<ET.Client.Wait_CreateMyUnit>
	// ET.ETTask<ET.Client.Wait_SceneChangeFinish>
	// ET.ETTask<ET.Client.Wait_UnitStop>
	// ET.ETTask<System.ValueTuple<byte,long>>
	// ET.ETTask<System.ValueTuple<uint,object>>
	// ET.ETTask<byte>
	// ET.ETTask<int>
	// ET.ETTask<object>
	// ET.ETTask<uint>
	// ET.EntityRef<object>
	// ET.IAwake<byte>
	// ET.IAwake<int,int>
	// ET.IAwake<int>
	// ET.IAwake<object,int>
	// ET.IAwake<object>
	// ET.IAwakeSystem<byte>
	// ET.IAwakeSystem<int,int>
	// ET.IAwakeSystem<int>
	// ET.IAwakeSystem<object,int>
	// ET.IAwakeSystem<object>
	// ET.LateUpdateSystem<object>
	// ET.ListComponent<Unity.Mathematics.float3>
	// ET.ListComponent<int>
	// ET.ListComponent<long>
	// ET.ListComponent<object>
	// ET.LoadSystem<object>
	// ET.Pair<UnityEngine.Color,object>
	// ET.Pair<long,object>
	// ET.Pair<object,object>
	// ET.Singleton<object>
	// ET.StateMachineWrap<ET.Client.A2NetClient_MessageHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.A2NetClient_RequestHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AAction.<OnExecute>d__1>
	// ET.StateMachineWrap<ET.Client.AI_Attack.<Execute>d__1>
	// ET.StateMachineWrap<ET.Client.AI_XunLuo.<Execute>d__1>
	// ET.StateMachineWrap<ET.Client.ActionComponentSystem.<PlayAction>d__3>
	// ET.StateMachineWrap<ET.Client.ActionComponentSystem.<PushAction>d__4>
	// ET.StateMachineWrap<ET.Client.ActionUnitSystem.<WaitFinishAsync>d__3>
	// ET.StateMachineWrap<ET.Client.AddBuffViewEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AddItemEvent_Watcher.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AddUpdateTask_Event.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterUnitCreate_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AnimationAction.<OnExecute>d__2>
	// ET.StateMachineWrap<ET.Client.AnimatorAction.<OnExecute>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_CreateHudUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_CreatePop.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_CreateTools.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.AppStartInitFinish_Menu.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.BasicChange_RefreshHud.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.BattleTextSystem.<PopHud>d__7>
	// ET.StateMachineWrap<ET.Client.C2C_GroupDelHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.C2C_GroupUpdateHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.C2C_UpdateChatHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChatEmojoMenuSelectEventHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ChatMenuSelectEventHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<Call>d__6>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<DisposeClientSender>d__3>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>
	// ET.StateMachineWrap<ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>
	// ET.StateMachineWrap<ET.Client.ClientSkillComponentSystem.<NormalAttack>d__6>
	// ET.StateMachineWrap<ET.Client.ClientSkillComponentSystem.<UseSkill>d__7>
	// ET.StateMachineWrap<ET.Client.ClientUseSkillEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.CommandComponentSystem.<RunAsync>d__3>
	// ET.StateMachineWrap<ET.Client.CommandComponentSystem.<RunAsync>d__4>
	// ET.StateMachineWrap<ET.Client.CommandOpenWin.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.CommandUnitSystem.<RunAsync>d__2>
	// ET.StateMachineWrap<ET.Client.DataSaveComponentSystem.<GetAsync>d__6<object>>
	// ET.StateMachineWrap<ET.Client.DataSaveComponentSystem.<SaveAsync>d__5<object>>
	// ET.StateMachineWrap<ET.Client.DelGroupHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.DeleteTask_Event.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ESItemSystem.<ItemClick>d__7>
	// ET.StateMachineWrap<ET.Client.ESItemSystem.<RefreShIcon>d__4>
	// ET.StateMachineWrap<ET.Client.ESItemSystem.<RefreshFrame>d__5>
	// ET.StateMachineWrap<ET.Client.ES_EquipItemSystem.<Show>d__3>
	// ET.StateMachineWrap<ET.Client.ES_NormalItemSystem.<Show>d__3>
	// ET.StateMachineWrap<ET.Client.EUIHelper.<>c__DisplayClass19_0.<<AddListenerAsyncWithId>g__clickActionAsync|1>d>
	// ET.StateMachineWrap<ET.Client.EUIHelper.<>c__DisplayClass20_0.<<AddListenerAsync>g__clickActionAsync|1>d>
	// ET.StateMachineWrap<ET.Client.EnterMapFinish_CreateMainUI.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.EnterMapFinish_InitUnit.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.EnterMapFinish_SendGameOk.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.EnterMapHelper.<EnterMapAsync>d__0>
	// ET.StateMachineWrap<ET.Client.EntryEvent3_InitClient.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.FiberInit_NetClient.<Handle>d__0>
	// ET.StateMachineWrap<ET.Client.HotKeyEvent_Handler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.HotKeyEvent_UseSkill.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.IconHelper.<LoadAtlas>d__3>
	// ET.StateMachineWrap<ET.Client.IconHelper.<LoadIconSpriteAsync>d__1>
	// ET.StateMachineWrap<ET.Client.IconHelper.<LoadSpriteAsync>d__4>
	// ET.StateMachineWrap<ET.Client.IconHelper.<LoadWidgetSpriteAsync>d__2>
	// ET.StateMachineWrap<ET.Client.IconHelper.<SetSprite>d__0>
	// ET.StateMachineWrap<ET.Client.InitTask_Event.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LoadingProgress_Update.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.LoginHelper.<GetRoles>d__3>
	// ET.StateMachineWrap<ET.Client.LoginHelper.<GetServerInfos>d__2>
	// ET.StateMachineWrap<ET.Client.LoginHelper.<Login>d__0>
	// ET.StateMachineWrap<ET.Client.LoginHelper.<QueryAccount>d__1>
	// ET.StateMachineWrap<ET.Client.M2C_BreakSkillHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_CreateUnitsHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_DelBuffHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_DelSkillListHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_DeleteTaskHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_HurtListHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_PathfindingResultHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_StopHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateBasicInfoHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateBuffHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateEquipHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateFashionEffectHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateFightDataInfoHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateHpHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateItemHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateNumericHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdatePlayerDataHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateShieldHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateSkillHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateSkillListHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UpdateTaskHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.M2C_UseSkillHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Main2NetClient_LoginHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.MainBottomMenuSelectHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.MenuSelectHandler.<OnMenuClick>d__2>
	// ET.StateMachineWrap<ET.Client.MenuSelectHandler.<Run>d__3>
	// ET.StateMachineWrap<ET.Client.MoveHelper.<MoveToAsync>d__0>
	// ET.StateMachineWrap<ET.Client.MoveHelper.<MoveToAsync>d__1>
	// ET.StateMachineWrap<ET.Client.MoveStart_Animator.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.MoveStop_Animator.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ParticleAction.<OnExecute>d__0>
	// ET.StateMachineWrap<ET.Client.PingComponentSystem.<PingAsync>d__2>
	// ET.StateMachineWrap<ET.Client.PlayActionEventHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.PopHurtHud_Handler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.PopMsg_EventHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.PushActionEventHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.RedDotComponentSystem.<PreLoadGameObject>d__11>
	// ET.StateMachineWrap<ET.Client.RemoveBuffViewEvent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.RemoveItemEvent_Watcher.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>>
	// ET.StateMachineWrap<ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<Init>d__1>
	// ET.StateMachineWrap<ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>
	// ET.StateMachineWrap<ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<Connect>d__2>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<CreateRouterSession>d__0>
	// ET.StateMachineWrap<ET.Client.RouterHelper.<GetRouterAddress>d__1>
	// ET.StateMachineWrap<ET.Client.SceneChangeFinish_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.SceneChangeFinish_CloseLoading.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.SceneChangeStart_AddComponent.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.Scroll_Item_BagSystem.<Refresh>d__2>
	// ET.StateMachineWrap<ET.Client.UIChatSystem.<LoadData>d__0>
	// ET.StateMachineWrap<ET.Client.UIComponentHelper.<CheckCloseUI>d__2>
	// ET.StateMachineWrap<ET.Client.UIComponentHelper.<OpenConfirm>d__3>
	// ET.StateMachineWrap<ET.Client.UIComponentHelper.<OpenConfirm>d__4>
	// ET.StateMachineWrap<ET.Client.UIComponentHelper.<PopItemTips>d__1>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<LoadBaseWindowsAsync>d__31>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<PreLoadWindowsItemAsync>d__32>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<PreloadUI>d__4>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<ShowBaseWindowAsync>d__23>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<ShowWindow>d__14<object>>
	// ET.StateMachineWrap<ET.Client.UIComponentSystem.<ShowWindowAsync>d__15>
	// ET.StateMachineWrap<ET.Client.UIItemTipSystem.<CreateTip>d__4>
	// ET.StateMachineWrap<ET.Client.UILoginSystem.<OnEnterGameClick>d__12>
	// ET.StateMachineWrap<ET.Client.UILoginSystem.<OnLoginClick>d__9>
	// ET.StateMachineWrap<ET.Client.UILoginSystem.<OnServerClick>d__10>
	// ET.StateMachineWrap<ET.Client.UILoginSystem.<QueryAccount>d__8>
	// ET.StateMachineWrap<ET.Client.UILoginSystem.<QueryServer>d__7>
	// ET.StateMachineWrap<ET.Client.UnitWeaponComponentSystem.<RefreshWeapon>d__2>
	// ET.StateMachineWrap<ET.Client.UpdateFashionEffectEvent_Handler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.UpdateGroupHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.UpdateMsgHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.Client.UpdateShield_RefreshHud.<Run>d__0>
	// ET.StateMachineWrap<ET.ConsoleComponentSystem.<Start>d__1>
	// ET.StateMachineWrap<ET.Entry.<StartAsync>d__2>
	// ET.StateMachineWrap<ET.EntryEvent1_InitShare.<Run>d__0>
	// ET.StateMachineWrap<ET.FiberInit_Main.<Handle>d__0>
	// ET.StateMachineWrap<ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>
	// ET.StateMachineWrap<ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>
	// ET.StateMachineWrap<ET.MessageHandler.<Handle>d__1<object,object,object>>
	// ET.StateMachineWrap<ET.MessageHandler.<Handle>d__1<object,object>>
	// ET.StateMachineWrap<ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>
	// ET.StateMachineWrap<ET.MessageSessionHandler.<HandleAsync>d__2<object>>
	// ET.StateMachineWrap<ET.MoveComponentSystem.<MoveToAsync>d__5>
	// ET.StateMachineWrap<ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>
	// ET.StateMachineWrap<ET.ObjectWaitSystem.<Wait>d__4<object>>
	// ET.StateMachineWrap<ET.ReloadConfigConsoleHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.ReloadDllConsoleHandler.<Run>d__0>
	// ET.StateMachineWrap<ET.RpcInfo.<Wait>d__7>
	// ET.StateMachineWrap<ET.Server.TestConsoleHanlder.<Run>d__0>
	// ET.StateMachineWrap<ET.SessionSystem.<Call>d__3>
	// ET.StateMachineWrap<ET.SessionSystem.<Call>d__4>
	// ET.StructBsonSerialize<ET.ActorId>
	// ET.StructBsonSerialize<ET.Address>
	// ET.StructBsonSerialize<ET.MessageReturn>
	// ET.StructBsonSerialize<ET.UnitHead>
	// ET.StructBsonSerialize<Unity.Mathematics.float2>
	// ET.StructBsonSerialize<Unity.Mathematics.float3>
	// ET.StructBsonSerialize<Unity.Mathematics.float4>
	// ET.StructBsonSerialize<Unity.Mathematics.quaternion>
	// ET.StructBsonSerialize<object>
	// ET.UpdateSystem<object>
	// MemoryPack.Formatters.ArrayFormatter<byte>
	// MemoryPack.Formatters.ArrayFormatter<object>
	// MemoryPack.Formatters.DictionaryFormatter<int,long>
	// MemoryPack.Formatters.ListFormatter<Unity.Mathematics.float3>
	// MemoryPack.Formatters.ListFormatter<int>
	// MemoryPack.Formatters.ListFormatter<long>
	// MemoryPack.Formatters.ListFormatter<object>
	// MemoryPack.Formatters.UnmanagedFormatter<int>
	// MemoryPack.IMemoryPackFormatter<Unity.Mathematics.float3>
	// MemoryPack.IMemoryPackFormatter<int>
	// MemoryPack.IMemoryPackFormatter<long>
	// MemoryPack.IMemoryPackFormatter<object>
	// MemoryPack.IMemoryPackable<object>
	// MemoryPack.MemoryPackFormatter<System.UIntPtr>
	// MemoryPack.MemoryPackFormatter<int>
	// MemoryPack.MemoryPackFormatter<object>
	// MongoDB.Bson.Serialization.IBsonSerializer<object>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<ET.ActorId>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<ET.Address>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<ET.MessageReturn>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<ET.UnitHead>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float2>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float3>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.float4>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<Unity.Mathematics.quaternion>
	// MongoDB.Bson.Serialization.Serializers.SerializerBase<object>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<ET.ActorId>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<ET.Address>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<ET.MessageReturn>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<ET.UnitHead>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float2>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float3>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.float4>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<Unity.Mathematics.quaternion>
	// MongoDB.Bson.Serialization.Serializers.StructSerializerBase<object>
	// System.Action<DotRecast.Detour.StraightPathItem>
	// System.Action<ET.Client.AtData>
	// System.Action<ET.Client.ConfirmBtn>
	// System.Action<ET.EntityRef<object>>
	// System.Action<ET.Pair<long,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<Unity.Mathematics.float3>
	// System.Action<UnityEngine.EventSystems.RaycastResult>
	// System.Action<byte>
	// System.Action<int>
	// System.Action<long,int>
	// System.Action<long,object>
	// System.Action<long>
	// System.Action<object,int>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<byte>
	// System.ByReference<byte>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__35<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary<object,object>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__28<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Generic.ArraySortHelper<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ArraySortHelper<ET.Client.AtData>
	// System.Collections.Generic.ArraySortHelper<ET.Client.ConfirmBtn>
	// System.Collections.Generic.ArraySortHelper<ET.EntityRef<object>>
	// System.Collections.Generic.ArraySortHelper<ET.Pair<long,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<Unity.Mathematics.float3>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.Comparer<ET.ActorId>
	// System.Collections.Generic.Comparer<ET.Client.AtData>
	// System.Collections.Generic.Comparer<ET.Client.ConfirmBtn>
	// System.Collections.Generic.Comparer<ET.EntityRef<object>>
	// System.Collections.Generic.Comparer<ET.Pair<long,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<Unity.Mathematics.float3>
	// System.Collections.Generic.Comparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<uint>
	// System.Collections.Generic.Comparer<ushort>
	// System.Collections.Generic.ComparisonComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ComparisonComparer<ET.ActorId>
	// System.Collections.Generic.ComparisonComparer<ET.Client.AtData>
	// System.Collections.Generic.ComparisonComparer<ET.Client.ConfirmBtn>
	// System.Collections.Generic.ComparisonComparer<ET.EntityRef<object>>
	// System.Collections.Generic.ComparisonComparer<ET.Pair<long,object>>
	// System.Collections.Generic.ComparisonComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ComparisonComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ComparisonComparer<byte>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<long>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.ComparisonComparer<uint>
	// System.Collections.Generic.ComparisonComparer<ushort>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ushort,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ushort,object>
	// System.Collections.Generic.Dictionary<int,ET.Pair<object,object>>
	// System.Collections.Generic.Dictionary<int,ET.RpcInfo>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,ET.Client.HotKey>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ushort,object>
	// System.Collections.Generic.EqualityComparer<ET.ActorId>
	// System.Collections.Generic.EqualityComparer<ET.Client.HotKey>
	// System.Collections.Generic.EqualityComparer<ET.Pair<object,object>>
	// System.Collections.Generic.EqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<uint>
	// System.Collections.Generic.EqualityComparer<ushort>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<long>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet.Enumerator<ushort>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<long>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSet<ushort>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<long>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.HashSetEqualityComparer<ushort>
	// System.Collections.Generic.ICollection<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ICollection<ET.Client.AtData>
	// System.Collections.Generic.ICollection<ET.Client.ConfirmBtn>
	// System.Collections.Generic.ICollection<ET.EntityRef<object>>
	// System.Collections.Generic.ICollection<ET.Pair<long,object>>
	// System.Collections.Generic.ICollection<ET.RpcInfo>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.Pair<object,object>>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,ET.Client.HotKey>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.ICollection<Unity.Mathematics.float3>
	// System.Collections.Generic.ICollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IComparer<ET.Client.AtData>
	// System.Collections.Generic.IComparer<ET.Client.ConfirmBtn>
	// System.Collections.Generic.IComparer<ET.EntityRef<object>>
	// System.Collections.Generic.IComparer<ET.Pair<long,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.IComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IEnumerable<ET.Client.AtData>
	// System.Collections.Generic.IEnumerable<ET.Client.ConfirmBtn>
	// System.Collections.Generic.IEnumerable<ET.EntityRef<object>>
	// System.Collections.Generic.IEnumerable<ET.Pair<long,object>>
	// System.Collections.Generic.IEnumerable<ET.RpcInfo>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.Pair<object,object>>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,ET.Client.HotKey>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerable<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerable<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IEnumerator<ET.Client.AtData>
	// System.Collections.Generic.IEnumerator<ET.Client.ConfirmBtn>
	// System.Collections.Generic.IEnumerator<ET.EntityRef<object>>
	// System.Collections.Generic.IEnumerator<ET.Pair<long,object>>
	// System.Collections.Generic.IEnumerator<ET.RpcInfo>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.Pair<object,object>>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,ET.Client.HotKey>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ushort,object>>
	// System.Collections.Generic.IEnumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.IEnumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ushort>
	// System.Collections.Generic.IList<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.IList<ET.Client.AtData>
	// System.Collections.Generic.IList<ET.Client.ConfirmBtn>
	// System.Collections.Generic.IList<ET.EntityRef<object>>
	// System.Collections.Generic.IList<ET.Pair<long,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<Unity.Mathematics.float3>
	// System.Collections.Generic.IList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IReadOnlyDictionary<int,int>
	// System.Collections.Generic.IReadOnlyDictionary<int,long>
	// System.Collections.Generic.IReadOnlyDictionary<int,object>
	// System.Collections.Generic.KeyValuePair<int,ET.Pair<object,object>>
	// System.Collections.Generic.KeyValuePair<int,ET.RpcInfo>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,ET.Client.HotKey>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ushort,object>
	// System.Collections.Generic.List.Enumerator<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.List.Enumerator<ET.Client.AtData>
	// System.Collections.Generic.List.Enumerator<ET.Client.ConfirmBtn>
	// System.Collections.Generic.List.Enumerator<ET.EntityRef<object>>
	// System.Collections.Generic.List.Enumerator<ET.Pair<long,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<Unity.Mathematics.float3>
	// System.Collections.Generic.List.Enumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.List<ET.Client.AtData>
	// System.Collections.Generic.List<ET.Client.ConfirmBtn>
	// System.Collections.Generic.List<ET.EntityRef<object>>
	// System.Collections.Generic.List<ET.Pair<long,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<Unity.Mathematics.float3>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<DotRecast.Detour.StraightPathItem>
	// System.Collections.Generic.ObjectComparer<ET.ActorId>
	// System.Collections.Generic.ObjectComparer<ET.Client.AtData>
	// System.Collections.Generic.ObjectComparer<ET.Client.ConfirmBtn>
	// System.Collections.Generic.ObjectComparer<ET.EntityRef<object>>
	// System.Collections.Generic.ObjectComparer<ET.Pair<long,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<Unity.Mathematics.float3>
	// System.Collections.Generic.ObjectComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<uint>
	// System.Collections.Generic.ObjectComparer<ushort>
	// System.Collections.Generic.ObjectEqualityComparer<ET.ActorId>
	// System.Collections.Generic.ObjectEqualityComparer<ET.Client.HotKey>
	// System.Collections.Generic.ObjectEqualityComparer<ET.Pair<object,object>>
	// System.Collections.Generic.ObjectEqualityComparer<ET.RpcInfo>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<uint>
	// System.Collections.Generic.ObjectEqualityComparer<ushort>
	// System.Collections.Generic.Queue.Enumerator<int>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<int,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<int,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<int,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<int,object>
	// System.Collections.Generic.SortedDictionary<int,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.SortedSetEqualityComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<DotRecast.Detour.StraightPathItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.Client.AtData>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.Client.ConfirmBtn>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.EntityRef<object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<ET.Pair<long,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<Unity.Mathematics.float3>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<DotRecast.Detour.StraightPathItem>
	// System.Comparison<ET.ActorId>
	// System.Comparison<ET.Client.AtData>
	// System.Comparison<ET.Client.ConfirmBtn>
	// System.Comparison<ET.EntityRef<object>>
	// System.Comparison<ET.Pair<long,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<Unity.Mathematics.float3>
	// System.Comparison<UnityEngine.EventSystems.RaycastResult>
	// System.Comparison<byte>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<uint>
	// System.Comparison<ushort>
	// System.Func<int,object>
	// System.Func<object,byte>
	// System.Func<object,int>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Buffer<ET.RpcInfo>
	// System.Linq.Enumerable.<SkipIterator>d__31<object>
	// System.Linq.GroupedEnumerable<object,int,object>
	// System.Linq.GroupedEnumerable<object,object,object>
	// System.Linq.IGrouping<int,object>
	// System.Linq.IGrouping<object,object>
	// System.Linq.IdentityFunction.<>c<object>
	// System.Linq.IdentityFunction<object>
	// System.Linq.Lookup.<GetEnumerator>d__12<int,object>
	// System.Linq.Lookup.<GetEnumerator>d__12<object,object>
	// System.Linq.Lookup.Grouping.<GetEnumerator>d__7<int,object>
	// System.Linq.Lookup.Grouping.<GetEnumerator>d__7<object,object>
	// System.Linq.Lookup.Grouping<int,object>
	// System.Linq.Lookup.Grouping<object,object>
	// System.Linq.Lookup<int,object>
	// System.Linq.Lookup<object,object>
	// System.Nullable<UnityEngine.Color>
	// System.Nullable<float>
	// System.Predicate<DotRecast.Detour.StraightPathItem>
	// System.Predicate<ET.Client.AtData>
	// System.Predicate<ET.Client.ConfirmBtn>
	// System.Predicate<ET.EntityRef<object>>
	// System.Predicate<ET.Pair<long,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<Unity.Mathematics.float3>
	// System.Predicate<UnityEngine.EventSystems.RaycastResult>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ushort>
	// System.ReadOnlySpan.Enumerator<byte>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span.Enumerator<byte>
	// System.Span<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass32_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<ET.ActorId,object>
	// System.ValueTuple<byte,long>
	// System.ValueTuple<int,object>
	// System.ValueTuple<uint,object>
	// System.ValueTuple<uint,uint>
	// System.ValueTuple<ushort,object>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// object ET.Client.TweenManager.CreateTweener<object>()
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.DataSaveComponentSystem.<SaveAsync>d__5<object>>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.DataSaveComponentSystem.<SaveAsync>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.Client.MenuSelectHandler.<OnMenuClick>d__2>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.Client.MenuSelectHandler.<OnMenuClick>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.ConsoleComponentSystem.<Start>d__1>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.A2NetClient_MessageHandler.<Run>d__0>(object&,ET.Client.A2NetClient_MessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.A2NetClient_RequestHandler.<Run>d__0>(object&,ET.Client.A2NetClient_RequestHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AAction.<OnExecute>d__1>(object&,ET.Client.AAction.<OnExecute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AI_Attack.<Execute>d__1>(object&,ET.Client.AI_Attack.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AI_XunLuo.<Execute>d__1>(object&,ET.Client.AI_XunLuo.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ActionComponentSystem.<PlayAction>d__3>(object&,ET.Client.ActionComponentSystem.<PlayAction>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ActionComponentSystem.<PushAction>d__4>(object&,ET.Client.ActionComponentSystem.<PushAction>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ActionUnitSystem.<WaitFinishAsync>d__3>(object&,ET.Client.ActionUnitSystem.<WaitFinishAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AddBuffViewEvent.<Run>d__0>(object&,ET.Client.AddBuffViewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AddItemEvent_Watcher.<Run>d__0>(object&,ET.Client.AddItemEvent_Watcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AddUpdateTask_Event.<Run>d__0>(object&,ET.Client.AddUpdateTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>(object&,ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AfterUnitCreate_AddComponent.<Run>d__0>(object&,ET.Client.AfterUnitCreate_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>(object&,ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AnimationAction.<OnExecute>d__2>(object&,ET.Client.AnimationAction.<OnExecute>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AnimatorAction.<OnExecute>d__0>(object&,ET.Client.AnimatorAction.<OnExecute>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_AddComponent.<Run>d__0>(object&,ET.Client.AppStartInitFinish_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_CreateHudUI.<Run>d__0>(object&,ET.Client.AppStartInitFinish_CreateHudUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>(object&,ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_CreatePop.<Run>d__0>(object&,ET.Client.AppStartInitFinish_CreatePop.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_CreateTools.<Run>d__0>(object&,ET.Client.AppStartInitFinish_CreateTools.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.AppStartInitFinish_Menu.<Run>d__0>(object&,ET.Client.AppStartInitFinish_Menu.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.BasicChange_RefreshHud.<Run>d__0>(object&,ET.Client.BasicChange_RefreshHud.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.BattleTextSystem.<PopHud>d__7>(object&,ET.Client.BattleTextSystem.<PopHud>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.C2C_GroupDelHandler.<Run>d__0>(object&,ET.Client.C2C_GroupDelHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.C2C_GroupUpdateHandler.<Run>d__0>(object&,ET.Client.C2C_GroupUpdateHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.C2C_UpdateChatHandler.<Run>d__0>(object&,ET.Client.C2C_UpdateChatHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>(object&,ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>(object&,ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ChatEmojoMenuSelectEventHandler.<Run>d__0>(object&,ET.Client.ChatEmojoMenuSelectEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ChatMenuSelectEventHandler.<Run>d__0>(object&,ET.Client.ChatMenuSelectEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<DisposeClientSender>d__3>(object&,ET.Client.ClientSenderComponentSystem.<DisposeClientSender>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>(object&,ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSkillComponentSystem.<NormalAttack>d__6>(object&,ET.Client.ClientSkillComponentSystem.<NormalAttack>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientSkillComponentSystem.<UseSkill>d__7>(object&,ET.Client.ClientSkillComponentSystem.<UseSkill>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ClientUseSkillEvent.<Run>d__0>(object&,ET.Client.ClientUseSkillEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.CommandComponentSystem.<RunAsync>d__3>(object&,ET.Client.CommandComponentSystem.<RunAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.CommandComponentSystem.<RunAsync>d__4>(object&,ET.Client.CommandComponentSystem.<RunAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.CommandOpenWin.<Run>d__0>(object&,ET.Client.CommandOpenWin.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.CommandUnitSystem.<RunAsync>d__2>(object&,ET.Client.CommandUnitSystem.<RunAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.DelGroupHandler.<Run>d__0>(object&,ET.Client.DelGroupHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.DeleteTask_Event.<Run>d__0>(object&,ET.Client.DeleteTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ESItemSystem.<ItemClick>d__7>(object&,ET.Client.ESItemSystem.<ItemClick>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ESItemSystem.<RefreShIcon>d__4>(object&,ET.Client.ESItemSystem.<RefreShIcon>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ESItemSystem.<RefreshFrame>d__5>(object&,ET.Client.ESItemSystem.<RefreshFrame>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ES_EquipItemSystem.<Show>d__3>(object&,ET.Client.ES_EquipItemSystem.<Show>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ES_NormalItemSystem.<Show>d__3>(object&,ET.Client.ES_NormalItemSystem.<Show>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EUIHelper.<>c__DisplayClass19_0.<<AddListenerAsyncWithId>g__clickActionAsync|1>d>(object&,ET.Client.EUIHelper.<>c__DisplayClass19_0.<<AddListenerAsyncWithId>g__clickActionAsync|1>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EUIHelper.<>c__DisplayClass20_0.<<AddListenerAsync>g__clickActionAsync|1>d>(object&,ET.Client.EUIHelper.<>c__DisplayClass20_0.<<AddListenerAsync>g__clickActionAsync|1>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapFinish_CreateMainUI.<Run>d__0>(object&,ET.Client.EnterMapFinish_CreateMainUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapFinish_InitUnit.<Run>d__0>(object&,ET.Client.EnterMapFinish_InitUnit.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapFinish_SendGameOk.<Run>d__0>(object&,ET.Client.EnterMapFinish_SendGameOk.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.EntryEvent3_InitClient.<Run>d__0>(object&,ET.Client.EntryEvent3_InitClient.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.FiberInit_NetClient.<Handle>d__0>(object&,ET.Client.FiberInit_NetClient.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.HotKeyEvent_Handler.<Run>d__0>(object&,ET.Client.HotKeyEvent_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.HotKeyEvent_UseSkill.<Run>d__0>(object&,ET.Client.HotKeyEvent_UseSkill.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.IconHelper.<LoadAtlas>d__3>(object&,ET.Client.IconHelper.<LoadAtlas>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.IconHelper.<SetSprite>d__0>(object&,ET.Client.IconHelper.<SetSprite>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.InitTask_Event.<Run>d__0>(object&,ET.Client.InitTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.LoadingProgress_Update.<Run>d__0>(object&,ET.Client.LoadingProgress_Update.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_BreakSkillHandler.<Run>d__0>(object&,ET.Client.M2C_BreakSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>(object&,ET.Client.M2C_CreateMyUnitHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_CreateUnitsHandler.<Run>d__0>(object&,ET.Client.M2C_CreateUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_DelBuffHandler.<Run>d__0>(object&,ET.Client.M2C_DelBuffHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_DelSkillListHandler.<Run>d__0>(object&,ET.Client.M2C_DelSkillListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_DeleteTaskHandler.<Run>d__0>(object&,ET.Client.M2C_DeleteTaskHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_HurtListHandler.<Run>d__0>(object&,ET.Client.M2C_HurtListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_PathfindingResultHandler.<Run>d__0>(object&,ET.Client.M2C_PathfindingResultHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>(object&,ET.Client.M2C_RemoveUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>(object&,ET.Client.M2C_StartSceneChangeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_StopHandler.<Run>d__0>(object&,ET.Client.M2C_StopHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateBasicInfoHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateBasicInfoHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateBuffHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateBuffHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateEquipHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateEquipHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateFashionEffectHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateFashionEffectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateFightDataInfoHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateFightDataInfoHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateHpHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateHpHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateItemHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateItemHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateNumericHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateNumericHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdatePlayerDataHandler.<Run>d__0>(object&,ET.Client.M2C_UpdatePlayerDataHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateShieldHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateShieldHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateSkillHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateSkillListHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateSkillListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UpdateTaskHandler.<Run>d__0>(object&,ET.Client.M2C_UpdateTaskHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.M2C_UseSkillHandler.<Run>d__0>(object&,ET.Client.M2C_UseSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.Main2NetClient_LoginHandler.<Run>d__0>(object&,ET.Client.Main2NetClient_LoginHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MainBottomMenuSelectHandler.<Run>d__0>(object&,ET.Client.MainBottomMenuSelectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MenuSelectHandler.<Run>d__3>(object&,ET.Client.MenuSelectHandler.<Run>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MoveHelper.<MoveToAsync>d__1>(object&,ET.Client.MoveHelper.<MoveToAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MoveStart_Animator.<Run>d__0>(object&,ET.Client.MoveStart_Animator.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.MoveStop_Animator.<Run>d__0>(object&,ET.Client.MoveStop_Animator.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>(object&,ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ParticleAction.<OnExecute>d__0>(object&,ET.Client.ParticleAction.<OnExecute>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PingComponentSystem.<PingAsync>d__2>(object&,ET.Client.PingComponentSystem.<PingAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PlayActionEventHandler.<Run>d__0>(object&,ET.Client.PlayActionEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PopHurtHud_Handler.<Run>d__0>(object&,ET.Client.PopHurtHud_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PopMsg_EventHandler.<Run>d__0>(object&,ET.Client.PopMsg_EventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.PushActionEventHandler.<Run>d__0>(object&,ET.Client.PushActionEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RedDotComponentSystem.<PreLoadGameObject>d__11>(object&,ET.Client.RedDotComponentSystem.<PreLoadGameObject>d__11&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RemoveBuffViewEvent.<Run>d__0>(object&,ET.Client.RemoveBuffViewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RemoveItemEvent_Watcher.<Run>d__0>(object&,ET.Client.RemoveItemEvent_Watcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>(object&,ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<Init>d__1>(object&,ET.Client.RouterAddressComponentSystem.<Init>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>(object&,ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>(object&,ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeFinish_AddComponent.<Run>d__0>(object&,ET.Client.SceneChangeFinish_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeFinish_CloseLoading.<Run>d__0>(object&,ET.Client.SceneChangeFinish_CloseLoading.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.SceneChangeStart_AddComponent.<Run>d__0>(object&,ET.Client.SceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.Scroll_Item_BagSystem.<Refresh>d__2>(object&,ET.Client.Scroll_Item_BagSystem.<Refresh>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIChatSystem.<LoadData>d__0>(object&,ET.Client.UIChatSystem.<LoadData>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentHelper.<CheckCloseUI>d__2>(object&,ET.Client.UIComponentHelper.<CheckCloseUI>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentHelper.<PopItemTips>d__1>(object&,ET.Client.UIComponentHelper.<PopItemTips>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<LoadBaseWindowsAsync>d__31>(object&,ET.Client.UIComponentSystem.<LoadBaseWindowsAsync>d__31&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<PreLoadWindowsItemAsync>d__32>(object&,ET.Client.UIComponentSystem.<PreLoadWindowsItemAsync>d__32&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<PreloadUI>d__4>(object&,ET.Client.UIComponentSystem.<PreloadUI>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UIItemTipSystem.<CreateTip>d__4>(object&,ET.Client.UIItemTipSystem.<CreateTip>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UILoginSystem.<OnEnterGameClick>d__12>(object&,ET.Client.UILoginSystem.<OnEnterGameClick>d__12&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UILoginSystem.<OnLoginClick>d__9>(object&,ET.Client.UILoginSystem.<OnLoginClick>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UILoginSystem.<OnServerClick>d__10>(object&,ET.Client.UILoginSystem.<OnServerClick>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UnitWeaponComponentSystem.<RefreshWeapon>d__2>(object&,ET.Client.UnitWeaponComponentSystem.<RefreshWeapon>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UpdateFashionEffectEvent_Handler.<Run>d__0>(object&,ET.Client.UpdateFashionEffectEvent_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UpdateGroupHandler.<Run>d__0>(object&,ET.Client.UpdateGroupHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UpdateMsgHandler.<Run>d__0>(object&,ET.Client.UpdateMsgHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Client.UpdateShield_RefreshHud.<Run>d__0>(object&,ET.Client.UpdateShield_RefreshHud.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ConsoleComponentSystem.<Start>d__1>(object&,ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Entry.<StartAsync>d__2>(object&,ET.Entry.<StartAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.EntryEvent1_InitShare.<Run>d__0>(object&,ET.EntryEvent1_InitShare.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.FiberInit_Main.<Handle>d__0>(object&,ET.FiberInit_Main.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>(object&,ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>(object&,ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageHandler.<Handle>d__1<object,object,object>>(object&,ET.MessageHandler.<Handle>d__1<object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageHandler.<Handle>d__1<object,object>>(object&,ET.MessageHandler.<Handle>d__1<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>(object&,ET.MessageSessionHandler.<HandleAsync>d__2<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.MessageSessionHandler.<HandleAsync>d__2<object>>(object&,ET.MessageSessionHandler.<HandleAsync>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>(object&,ET.NumericChangeEvent_NotifyWatcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ReloadConfigConsoleHandler.<Run>d__0>(object&,ET.ReloadConfigConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.ReloadDllConsoleHandler.<Run>d__0>(object&,ET.ReloadDllConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<object,ET.Server.TestConsoleHanlder.<Run>d__0>(object&,ET.Server.TestConsoleHanlder.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<byte,long>>.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>(object&,ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<GetRouterAddress>d__1>(object&,ET.Client.RouterHelper.<GetRouterAddress>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.UILoginSystem.<QueryAccount>d__8>(object&,ET.Client.UILoginSystem.<QueryAccount>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.Client.UILoginSystem.<QueryServer>d__7>(object&,ET.Client.UILoginSystem.<QueryServer>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<object,ET.MoveComponentSystem.<MoveToAsync>d__5>(object&,ET.MoveComponentSystem.<MoveToAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.EnterMapHelper.<EnterMapAsync>d__0>(object&,ET.Client.EnterMapHelper.<EnterMapAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.LoginHelper.<GetRoles>d__3>(object&,ET.Client.LoginHelper.<GetRoles>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.LoginHelper.<GetServerInfos>d__2>(object&,ET.Client.LoginHelper.<GetServerInfos>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.LoginHelper.<Login>d__0>(object&,ET.Client.LoginHelper.<Login>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.LoginHelper.<QueryAccount>d__1>(object&,ET.Client.LoginHelper.<QueryAccount>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.MoveHelper.<MoveToAsync>d__0>(object&,ET.Client.MoveHelper.<MoveToAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentHelper.<OpenConfirm>d__3>(object&,ET.Client.UIComponentHelper.<OpenConfirm>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentHelper.<OpenConfirm>d__4>(object&,ET.Client.UIComponentHelper.<OpenConfirm>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>>(System.Runtime.CompilerServices.TaskAwaiter&,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ET.Client.DataSaveComponentSystem.<GetAsync>d__6<object>>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ET.Client.DataSaveComponentSystem.<GetAsync>d__6<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ClientSenderComponentSystem.<Call>d__6>(object&,ET.Client.ClientSenderComponentSystem.<Call>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.IconHelper.<LoadIconSpriteAsync>d__1>(object&,ET.Client.IconHelper.<LoadIconSpriteAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.IconHelper.<LoadSpriteAsync>d__4>(object&,ET.Client.IconHelper.<LoadSpriteAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.IconHelper.<LoadWidgetSpriteAsync>d__2>(object&,ET.Client.IconHelper.<LoadWidgetSpriteAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>>(object&,ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<CreateRouterSession>d__0>(object&,ET.Client.RouterHelper.<CreateRouterSession>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<ShowBaseWindowAsync>d__23>(object&,ET.Client.UIComponentSystem.<ShowBaseWindowAsync>d__23&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<ShowWindow>d__14<object>>(object&,ET.Client.UIComponentSystem.<ShowWindow>d__14<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.UIComponentSystem.<ShowWindowAsync>d__15>(object&,ET.Client.UIComponentSystem.<ShowWindowAsync>d__15&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.ObjectWaitSystem.<Wait>d__4<object>>(object&,ET.ObjectWaitSystem.<Wait>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.RpcInfo.<Wait>d__7>(object&,ET.RpcInfo.<Wait>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.SessionSystem.<Call>d__3>(object&,ET.SessionSystem.<Call>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.SessionSystem.<Call>d__4>(object&,ET.SessionSystem.<Call>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.AwaitUnsafeOnCompleted<object,ET.Client.RouterHelper.<Connect>d__2>(object&,ET.Client.RouterHelper.<Connect>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.A2NetClient_MessageHandler.<Run>d__0>(ET.Client.A2NetClient_MessageHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.A2NetClient_RequestHandler.<Run>d__0>(ET.Client.A2NetClient_RequestHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AAction.<OnExecute>d__1>(ET.Client.AAction.<OnExecute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AI_Attack.<Execute>d__1>(ET.Client.AI_Attack.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AI_XunLuo.<Execute>d__1>(ET.Client.AI_XunLuo.<Execute>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ActionComponentSystem.<PlayAction>d__3>(ET.Client.ActionComponentSystem.<PlayAction>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ActionComponentSystem.<PushAction>d__4>(ET.Client.ActionComponentSystem.<PushAction>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ActionUnitSystem.<WaitFinishAsync>d__3>(ET.Client.ActionUnitSystem.<WaitFinishAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AddBuffViewEvent.<Run>d__0>(ET.Client.AddBuffViewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AddItemEvent_Watcher.<Run>d__0>(ET.Client.AddItemEvent_Watcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AddUpdateTask_Event.<Run>d__0>(ET.Client.AddUpdateTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0>(ET.Client.AfterCreateCurrentScene_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterUnitCreate_AddComponent.<Run>d__0>(ET.Client.AfterUnitCreate_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0>(ET.Client.AfterUnitCreate_CreateUnitView.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AnimationAction.<OnExecute>d__2>(ET.Client.AnimationAction.<OnExecute>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AnimatorAction.<OnExecute>d__0>(ET.Client.AnimatorAction.<OnExecute>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_AddComponent.<Run>d__0>(ET.Client.AppStartInitFinish_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_CreateHudUI.<Run>d__0>(ET.Client.AppStartInitFinish_CreateHudUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0>(ET.Client.AppStartInitFinish_CreateLoginUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_CreatePop.<Run>d__0>(ET.Client.AppStartInitFinish_CreatePop.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_CreateTools.<Run>d__0>(ET.Client.AppStartInitFinish_CreateTools.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.AppStartInitFinish_Menu.<Run>d__0>(ET.Client.AppStartInitFinish_Menu.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.BasicChange_RefreshHud.<Run>d__0>(ET.Client.BasicChange_RefreshHud.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.BattleTextSystem.<PopHud>d__7>(ET.Client.BattleTextSystem.<PopHud>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.C2C_GroupDelHandler.<Run>d__0>(ET.Client.C2C_GroupDelHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.C2C_GroupUpdateHandler.<Run>d__0>(ET.Client.C2C_GroupUpdateHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.C2C_UpdateChatHandler.<Run>d__0>(ET.Client.C2C_UpdateChatHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0>(ET.Client.ChangePosition_SyncGameObjectPos.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0>(ET.Client.ChangeRotation_SyncGameObjectRotation.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChatEmojoMenuSelectEventHandler.<Run>d__0>(ET.Client.ChatEmojoMenuSelectEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ChatMenuSelectEventHandler.<Run>d__0>(ET.Client.ChatMenuSelectEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSenderComponentSystem.<DisposeClientSender>d__3>(ET.Client.ClientSenderComponentSystem.<DisposeClientSender>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2>(ET.Client.ClientSenderComponentSystem.<RemoveFiberAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSkillComponentSystem.<NormalAttack>d__6>(ET.Client.ClientSkillComponentSystem.<NormalAttack>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientSkillComponentSystem.<UseSkill>d__7>(ET.Client.ClientSkillComponentSystem.<UseSkill>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ClientUseSkillEvent.<Run>d__0>(ET.Client.ClientUseSkillEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.CommandComponentSystem.<RunAsync>d__3>(ET.Client.CommandComponentSystem.<RunAsync>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.CommandComponentSystem.<RunAsync>d__4>(ET.Client.CommandComponentSystem.<RunAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.CommandOpenWin.<Run>d__0>(ET.Client.CommandOpenWin.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.CommandUnitSystem.<RunAsync>d__2>(ET.Client.CommandUnitSystem.<RunAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.DataSaveComponentSystem.<SaveAsync>d__5<object>>(ET.Client.DataSaveComponentSystem.<SaveAsync>d__5<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.DelGroupHandler.<Run>d__0>(ET.Client.DelGroupHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.DeleteTask_Event.<Run>d__0>(ET.Client.DeleteTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ESItemSystem.<ItemClick>d__7>(ET.Client.ESItemSystem.<ItemClick>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ESItemSystem.<RefreShIcon>d__4>(ET.Client.ESItemSystem.<RefreShIcon>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ESItemSystem.<RefreshFrame>d__5>(ET.Client.ESItemSystem.<RefreshFrame>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ES_EquipItemSystem.<Show>d__3>(ET.Client.ES_EquipItemSystem.<Show>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ES_NormalItemSystem.<Show>d__3>(ET.Client.ES_NormalItemSystem.<Show>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EUIHelper.<>c__DisplayClass19_0.<<AddListenerAsyncWithId>g__clickActionAsync|1>d>(ET.Client.EUIHelper.<>c__DisplayClass19_0.<<AddListenerAsyncWithId>g__clickActionAsync|1>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EUIHelper.<>c__DisplayClass20_0.<<AddListenerAsync>g__clickActionAsync|1>d>(ET.Client.EUIHelper.<>c__DisplayClass20_0.<<AddListenerAsync>g__clickActionAsync|1>d&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EnterMapFinish_CreateMainUI.<Run>d__0>(ET.Client.EnterMapFinish_CreateMainUI.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EnterMapFinish_InitUnit.<Run>d__0>(ET.Client.EnterMapFinish_InitUnit.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EnterMapFinish_SendGameOk.<Run>d__0>(ET.Client.EnterMapFinish_SendGameOk.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.EntryEvent3_InitClient.<Run>d__0>(ET.Client.EntryEvent3_InitClient.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.FiberInit_NetClient.<Handle>d__0>(ET.Client.FiberInit_NetClient.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.HotKeyEvent_Handler.<Run>d__0>(ET.Client.HotKeyEvent_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.HotKeyEvent_UseSkill.<Run>d__0>(ET.Client.HotKeyEvent_UseSkill.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.IconHelper.<LoadAtlas>d__3>(ET.Client.IconHelper.<LoadAtlas>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.IconHelper.<SetSprite>d__0>(ET.Client.IconHelper.<SetSprite>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.InitTask_Event.<Run>d__0>(ET.Client.InitTask_Event.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.LoadingProgress_Update.<Run>d__0>(ET.Client.LoadingProgress_Update.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_BreakSkillHandler.<Run>d__0>(ET.Client.M2C_BreakSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_CreateMyUnitHandler.<Run>d__0>(ET.Client.M2C_CreateMyUnitHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_CreateUnitsHandler.<Run>d__0>(ET.Client.M2C_CreateUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_DelBuffHandler.<Run>d__0>(ET.Client.M2C_DelBuffHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_DelSkillListHandler.<Run>d__0>(ET.Client.M2C_DelSkillListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_DeleteTaskHandler.<Run>d__0>(ET.Client.M2C_DeleteTaskHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_HurtListHandler.<Run>d__0>(ET.Client.M2C_HurtListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_PathfindingResultHandler.<Run>d__0>(ET.Client.M2C_PathfindingResultHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_RemoveUnitsHandler.<Run>d__0>(ET.Client.M2C_RemoveUnitsHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_StartSceneChangeHandler.<Run>d__0>(ET.Client.M2C_StartSceneChangeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_StopHandler.<Run>d__0>(ET.Client.M2C_StopHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateBasicInfoHandler.<Run>d__0>(ET.Client.M2C_UpdateBasicInfoHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateBuffHandler.<Run>d__0>(ET.Client.M2C_UpdateBuffHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateEquipHandler.<Run>d__0>(ET.Client.M2C_UpdateEquipHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateFashionEffectHandler.<Run>d__0>(ET.Client.M2C_UpdateFashionEffectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateFightDataInfoHandler.<Run>d__0>(ET.Client.M2C_UpdateFightDataInfoHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateHpHandler.<Run>d__0>(ET.Client.M2C_UpdateHpHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateItemHandler.<Run>d__0>(ET.Client.M2C_UpdateItemHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateNumericHandler.<Run>d__0>(ET.Client.M2C_UpdateNumericHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdatePlayerDataHandler.<Run>d__0>(ET.Client.M2C_UpdatePlayerDataHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateShieldHandler.<Run>d__0>(ET.Client.M2C_UpdateShieldHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateSkillHandler.<Run>d__0>(ET.Client.M2C_UpdateSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateSkillListHandler.<Run>d__0>(ET.Client.M2C_UpdateSkillListHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UpdateTaskHandler.<Run>d__0>(ET.Client.M2C_UpdateTaskHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.M2C_UseSkillHandler.<Run>d__0>(ET.Client.M2C_UseSkillHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Main2NetClient_LoginHandler.<Run>d__0>(ET.Client.Main2NetClient_LoginHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MainBottomMenuSelectHandler.<Run>d__0>(ET.Client.MainBottomMenuSelectHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MenuSelectHandler.<OnMenuClick>d__2>(ET.Client.MenuSelectHandler.<OnMenuClick>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MenuSelectHandler.<Run>d__3>(ET.Client.MenuSelectHandler.<Run>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MoveHelper.<MoveToAsync>d__1>(ET.Client.MoveHelper.<MoveToAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MoveStart_Animator.<Run>d__0>(ET.Client.MoveStart_Animator.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.MoveStop_Animator.<Run>d__0>(ET.Client.MoveStop_Animator.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0>(ET.Client.NetClient2Main_SessionDisposeHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ParticleAction.<OnExecute>d__0>(ET.Client.ParticleAction.<OnExecute>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PingComponentSystem.<PingAsync>d__2>(ET.Client.PingComponentSystem.<PingAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PlayActionEventHandler.<Run>d__0>(ET.Client.PlayActionEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PopHurtHud_Handler.<Run>d__0>(ET.Client.PopHurtHud_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PopMsg_EventHandler.<Run>d__0>(ET.Client.PopMsg_EventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.PushActionEventHandler.<Run>d__0>(ET.Client.PushActionEventHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RedDotComponentSystem.<PreLoadGameObject>d__11>(ET.Client.RedDotComponentSystem.<PreLoadGameObject>d__11&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RemoveBuffViewEvent.<Run>d__0>(ET.Client.RemoveBuffViewEvent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RemoveItemEvent_Watcher.<Run>d__0>(ET.Client.RemoveItemEvent_Watcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5>(ET.Client.ResourcesLoaderComponentSystem.<LoadSceneAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2>(ET.Client.RouterAddressComponentSystem.<GetAllRouter>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<Init>d__1>(ET.Client.RouterAddressComponentSystem.<Init>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3>(ET.Client.RouterAddressComponentSystem.<WaitTenMinGetAllRouter>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1>(ET.Client.RouterCheckComponentSystem.<CheckAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeFinish_AddComponent.<Run>d__0>(ET.Client.SceneChangeFinish_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeFinish_CloseLoading.<Run>d__0>(ET.Client.SceneChangeFinish_CloseLoading.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.SceneChangeStart_AddComponent.<Run>d__0>(ET.Client.SceneChangeStart_AddComponent.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.Scroll_Item_BagSystem.<Refresh>d__2>(ET.Client.Scroll_Item_BagSystem.<Refresh>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIChatSystem.<LoadData>d__0>(ET.Client.UIChatSystem.<LoadData>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIComponentHelper.<CheckCloseUI>d__2>(ET.Client.UIComponentHelper.<CheckCloseUI>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIComponentHelper.<PopItemTips>d__1>(ET.Client.UIComponentHelper.<PopItemTips>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIComponentSystem.<LoadBaseWindowsAsync>d__31>(ET.Client.UIComponentSystem.<LoadBaseWindowsAsync>d__31&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIComponentSystem.<PreLoadWindowsItemAsync>d__32>(ET.Client.UIComponentSystem.<PreLoadWindowsItemAsync>d__32&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIComponentSystem.<PreloadUI>d__4>(ET.Client.UIComponentSystem.<PreloadUI>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UIItemTipSystem.<CreateTip>d__4>(ET.Client.UIItemTipSystem.<CreateTip>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UILoginSystem.<OnEnterGameClick>d__12>(ET.Client.UILoginSystem.<OnEnterGameClick>d__12&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UILoginSystem.<OnLoginClick>d__9>(ET.Client.UILoginSystem.<OnLoginClick>d__9&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UILoginSystem.<OnServerClick>d__10>(ET.Client.UILoginSystem.<OnServerClick>d__10&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UnitWeaponComponentSystem.<RefreshWeapon>d__2>(ET.Client.UnitWeaponComponentSystem.<RefreshWeapon>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UpdateFashionEffectEvent_Handler.<Run>d__0>(ET.Client.UpdateFashionEffectEvent_Handler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UpdateGroupHandler.<Run>d__0>(ET.Client.UpdateGroupHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UpdateMsgHandler.<Run>d__0>(ET.Client.UpdateMsgHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Client.UpdateShield_RefreshHud.<Run>d__0>(ET.Client.UpdateShield_RefreshHud.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ConsoleComponentSystem.<Start>d__1>(ET.ConsoleComponentSystem.<Start>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Entry.<StartAsync>d__2>(ET.Entry.<StartAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EntryEvent1_InitShare.<Run>d__0>(ET.EntryEvent1_InitShare.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.AppStartInitFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.AppStartInitFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.Client.ClientUseSkill>>(ET.EventSystem.<PublishAsync>d__4<object,ET.Client.ClientUseSkill>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EnterMapFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EnterMapFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent1>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent1>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent2>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent2>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent3>>(ET.EventSystem.<PublishAsync>d__4<object,ET.EntryEvent3>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.LoginFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.LoginFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.EventSystem.<PublishAsync>d__4<object,ET.SceneChangeFinish>>(ET.EventSystem.<PublishAsync>d__4<object,ET.SceneChangeFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.FiberInit_Main.<Handle>d__0>(ET.FiberInit_Main.<Handle>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1>(ET.MailBoxType_OrderedMessageHandler.<HandleInner>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1>(ET.MailBoxType_UnOrderedMessageHandler.<HandleAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageHandler.<Handle>d__1<object,object,object>>(ET.MessageHandler.<Handle>d__1<object,object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageHandler.<Handle>d__1<object,object>>(ET.MessageHandler.<Handle>d__1<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageSessionHandler.<HandleAsync>d__2<object,object>>(ET.MessageSessionHandler.<HandleAsync>d__2<object,object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.MessageSessionHandler.<HandleAsync>d__2<object>>(ET.MessageSessionHandler.<HandleAsync>d__2<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.NumericChangeEvent_NotifyWatcher.<Run>d__0>(ET.NumericChangeEvent_NotifyWatcher.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ReloadConfigConsoleHandler.<Run>d__0>(ET.ReloadConfigConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.ReloadDllConsoleHandler.<Run>d__0>(ET.ReloadDllConsoleHandler.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder.Start<ET.Server.TestConsoleHanlder.<Run>d__0>(ET.Server.TestConsoleHanlder.<Run>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CloseConfirm>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CloseConfirm>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CloseConfirm>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_CreateMyUnit>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CreateMyUnit>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_CreateMyUnit>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_SceneChangeFinish>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_SceneChangeFinish>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_SceneChangeFinish>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<ET.Client.Wait_UnitStop>.Start<ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_UnitStop>>(ET.ObjectWaitSystem.<Wait>d__4<ET.Client.Wait_UnitStop>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<byte,long>>.Start<ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4>(ET.Client.ClientSenderComponentSystem.<LoginAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<System.ValueTuple<uint,object>>.Start<ET.Client.RouterHelper.<GetRouterAddress>d__1>(ET.Client.RouterHelper.<GetRouterAddress>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.UILoginSystem.<QueryAccount>d__8>(ET.Client.UILoginSystem.<QueryAccount>d__8&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.Client.UILoginSystem.<QueryServer>d__7>(ET.Client.UILoginSystem.<QueryServer>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<byte>.Start<ET.MoveComponentSystem.<MoveToAsync>d__5>(ET.MoveComponentSystem.<MoveToAsync>d__5&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.EnterMapHelper.<EnterMapAsync>d__0>(ET.Client.EnterMapHelper.<EnterMapAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.LoginHelper.<GetRoles>d__3>(ET.Client.LoginHelper.<GetRoles>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.LoginHelper.<GetServerInfos>d__2>(ET.Client.LoginHelper.<GetServerInfos>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.LoginHelper.<Login>d__0>(ET.Client.LoginHelper.<Login>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.LoginHelper.<QueryAccount>d__1>(ET.Client.LoginHelper.<QueryAccount>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.MoveHelper.<MoveToAsync>d__0>(ET.Client.MoveHelper.<MoveToAsync>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.UIComponentHelper.<OpenConfirm>d__3>(ET.Client.UIComponentHelper.<OpenConfirm>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<int>.Start<ET.Client.UIComponentHelper.<OpenConfirm>d__4>(ET.Client.UIComponentHelper.<OpenConfirm>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ClientSenderComponentSystem.<Call>d__6>(ET.Client.ClientSenderComponentSystem.<Call>d__6&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.DataSaveComponentSystem.<GetAsync>d__6<object>>(ET.Client.DataSaveComponentSystem.<GetAsync>d__6<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.IconHelper.<LoadIconSpriteAsync>d__1>(ET.Client.IconHelper.<LoadIconSpriteAsync>d__1&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.IconHelper.<LoadSpriteAsync>d__4>(ET.Client.IconHelper.<LoadSpriteAsync>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.IconHelper.<LoadWidgetSpriteAsync>d__2>(ET.Client.IconHelper.<LoadWidgetSpriteAsync>d__2&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>>(ET.Client.ResourcesLoaderComponentSystem.<LoadAllAssetsAsync>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>>(ET.Client.ResourcesLoaderComponentSystem.<LoadAssetAsync>d__3<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.RouterHelper.<CreateRouterSession>d__0>(ET.Client.RouterHelper.<CreateRouterSession>d__0&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.UIComponentSystem.<ShowBaseWindowAsync>d__23>(ET.Client.UIComponentSystem.<ShowBaseWindowAsync>d__23&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.UIComponentSystem.<ShowWindow>d__14<object>>(ET.Client.UIComponentSystem.<ShowWindow>d__14<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.Client.UIComponentSystem.<ShowWindowAsync>d__15>(ET.Client.UIComponentSystem.<ShowWindowAsync>d__15&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.ETCancellationTokenHelper.<TimeoutAsync>d__6<object>>(ET.ETCancellationTokenHelper.<TimeoutAsync>d__6<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.ETTaskHelper.<GetContextAsync>d__0<object>>(ET.ETTaskHelper.<GetContextAsync>d__0<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.ObjectWaitSystem.<Wait>d__4<object>>(ET.ObjectWaitSystem.<Wait>d__4<object>&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.RpcInfo.<Wait>d__7>(ET.RpcInfo.<Wait>d__7&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.SessionSystem.<Call>d__3>(ET.SessionSystem.<Call>d__3&)
		// System.Void ET.ETAsyncTaskMethodBuilder<object>.Start<ET.SessionSystem.<Call>d__4>(ET.SessionSystem.<Call>d__4&)
		// System.Void ET.ETAsyncTaskMethodBuilder<uint>.Start<ET.Client.RouterHelper.<Connect>d__2>(ET.Client.RouterHelper.<Connect>d__2&)
		// ET.ETTask<object> ET.ETCancellationTokenHelper.TimeoutAsync<object>(ET.ETTask<object>,long)
		// object ET.Entity.AddChild<object,int>(int,bool)
		// object ET.Entity.AddChild<object,object,int>(object,int,bool)
		// object ET.Entity.AddChild<object,object>(object,bool)
		// object ET.Entity.AddChild<object>(bool)
		// ET.Entity ET.Entity.AddChildByType<object>(object,System.Type,bool)
		// object ET.Entity.AddChildWithId<object,int>(long,int,bool)
		// object ET.Entity.AddChildWithId<object,object>(long,object,bool)
		// object ET.Entity.AddChildWithId<object>(long,bool)
		// object ET.Entity.AddComponent<object,byte>(byte,bool)
		// object ET.Entity.AddComponent<object,int,int>(int,int,bool)
		// object ET.Entity.AddComponent<object,int>(int,bool)
		// object ET.Entity.AddComponent<object,object,int>(object,int,bool)
		// object ET.Entity.AddComponent<object,object>(object,bool)
		// object ET.Entity.AddComponent<object>(bool)
		// object ET.Entity.AddComponentWithId<object,byte>(long,byte,bool)
		// object ET.Entity.AddComponentWithId<object,int,int>(long,int,int,bool)
		// object ET.Entity.AddComponentWithId<object,int>(long,int,bool)
		// object ET.Entity.AddComponentWithId<object,object,int>(long,object,int,bool)
		// object ET.Entity.AddComponentWithId<object,object>(long,object,bool)
		// object ET.Entity.AddComponentWithId<object>(long,bool)
		// object ET.Entity.GetChild<object>()
		// object ET.Entity.GetChild<object>(long)
		// object ET.Entity.GetComponent<object>()
		// object ET.Entity.GetParent<object>()
		// System.Void ET.Entity.RemoveComponent<object>()
		// System.Void ET.EntitySystemSingleton.Awake<byte>(ET.Entity,byte)
		// System.Void ET.EntitySystemSingleton.Awake<int,int>(ET.Entity,int,int)
		// System.Void ET.EntitySystemSingleton.Awake<int>(ET.Entity,int)
		// System.Void ET.EntitySystemSingleton.Awake<object,int>(ET.Entity,object,int)
		// System.Void ET.EntitySystemSingleton.Awake<object>(ET.Entity,object)
		// int ET.EnumHelper.FromString<int>(string)
		// long ET.EnumHelper.FromString<long>(string)
		// System.Void ET.EventSystem.Invoke<ET.NetComponentOnRead>(long,ET.NetComponentOnRead)
		// System.Void ET.EventSystem.Publish<object,ET.AddItemEvent>(object,ET.AddItemEvent)
		// System.Void ET.EventSystem.Publish<object,ET.AddUpdateTask>(object,ET.AddUpdateTask)
		// System.Void ET.EventSystem.Publish<object,ET.AfterCreateCurrentScene>(object,ET.AfterCreateCurrentScene)
		// System.Void ET.EventSystem.Publish<object,ET.AfterUnitCreate>(object,ET.AfterUnitCreate)
		// System.Void ET.EventSystem.Publish<object,ET.BasicChangeEvent>(object,ET.BasicChangeEvent)
		// System.Void ET.EventSystem.Publish<object,ET.ChangePosition>(object,ET.ChangePosition)
		// System.Void ET.EventSystem.Publish<object,ET.ChangeRotation>(object,ET.ChangeRotation)
		// System.Void ET.EventSystem.Publish<object,ET.Client.AddBuffView>(object,ET.Client.AddBuffView)
		// System.Void ET.EventSystem.Publish<object,ET.Client.ClientUseSkill>(object,ET.Client.ClientUseSkill)
		// System.Void ET.EventSystem.Publish<object,ET.Client.DelGroup>(object,ET.Client.DelGroup)
		// System.Void ET.EventSystem.Publish<object,ET.Client.EquipUpdateEvent>(object,ET.Client.EquipUpdateEvent)
		// System.Void ET.EventSystem.Publish<object,ET.Client.FightStateChange>(object,ET.Client.FightStateChange)
		// System.Void ET.EventSystem.Publish<object,ET.Client.HotKeyEvent>(object,ET.Client.HotKeyEvent)
		// System.Void ET.EventSystem.Publish<object,ET.Client.MenuSelectEvent>(object,ET.Client.MenuSelectEvent)
		// System.Void ET.EventSystem.Publish<object,ET.Client.NetError>(object,ET.Client.NetError)
		// System.Void ET.EventSystem.Publish<object,ET.Client.PopHurtHud>(object,ET.Client.PopHurtHud)
		// System.Void ET.EventSystem.Publish<object,ET.Client.RemoveBuffView>(object,ET.Client.RemoveBuffView)
		// System.Void ET.EventSystem.Publish<object,ET.Client.UpdateGroup>(object,ET.Client.UpdateGroup)
		// System.Void ET.EventSystem.Publish<object,ET.Client.UpdateMsg>(object,ET.Client.UpdateMsg)
		// System.Void ET.EventSystem.Publish<object,ET.Client.UpdateShield>(object,ET.Client.UpdateShield)
		// System.Void ET.EventSystem.Publish<object,ET.DeleteTask>(object,ET.DeleteTask)
		// System.Void ET.EventSystem.Publish<object,ET.LoadingProgress>(object,ET.LoadingProgress)
		// System.Void ET.EventSystem.Publish<object,ET.MoveStart>(object,ET.MoveStart)
		// System.Void ET.EventSystem.Publish<object,ET.MoveStop>(object,ET.MoveStop)
		// System.Void ET.EventSystem.Publish<object,ET.NumericChange>(object,ET.NumericChange)
		// System.Void ET.EventSystem.Publish<object,ET.RemoveItemEvent>(object,ET.RemoveItemEvent)
		// System.Void ET.EventSystem.Publish<object,ET.SceneChangeStart>(object,ET.SceneChangeStart)
		// System.Void ET.EventSystem.Publish<object,ET.UpdateFashionEffectEvent>(object,ET.UpdateFashionEffectEvent)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.AppStartInitFinish>(object,ET.AppStartInitFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.Client.ClientUseSkill>(object,ET.Client.ClientUseSkill)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EnterMapFinish>(object,ET.EnterMapFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent1>(object,ET.EntryEvent1)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent2>(object,ET.EntryEvent2)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.EntryEvent3>(object,ET.EntryEvent3)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.LoginFinish>(object,ET.LoginFinish)
		// ET.ETTask ET.EventSystem.PublishAsync<object,ET.SceneChangeFinish>(object,ET.SceneChangeFinish)
		// object ET.MiscHelper.Get<object>(System.Collections.Generic.IList<object>,int,object)
		// object ET.MongoHelper.Deserialize<object>(byte[])
		// object ET.MongoHelper.FromJson<object>(string)
		// ET.Pair<UnityEngine.Color,object> ET.Pair<UnityEngine.Color,object>.Create<UnityEngine.Color,object>(UnityEngine.Color,object)
		// ET.Pair<long,object> ET.Pair<long,object>.Create<long,object>(long,object)
		// System.Void ET.RandomGenerator.BreakRank<object>(System.Collections.Generic.List<object>)
		// object ET.World.AddSingleton<object>()
		// System.Collections.Generic.List<object> MemoryPack.Formatters.ListFormatter.DeserializePackable<object>(MemoryPack.MemoryPackReader&)
		// System.Void MemoryPack.Formatters.ListFormatter.DeserializePackable<object>(MemoryPack.MemoryPackReader&,System.Collections.Generic.List<object>&)
		// System.Void MemoryPack.Formatters.ListFormatter.SerializePackable<object>(MemoryPack.MemoryPackWriter&,System.Collections.Generic.List<object>&)
		// byte[] MemoryPack.Internal.MemoryMarshalEx.AllocateUninitializedArray<byte>(int,bool)
		// byte& MemoryPack.Internal.MemoryMarshalEx.GetArrayDataReference<byte>(byte[])
		// MemoryPack.MemoryPackFormatter<object> MemoryPack.MemoryPackFormatterProvider.GetFormatter<object>()
		// bool MemoryPack.MemoryPackFormatterProvider.IsRegistered<int>()
		// bool MemoryPack.MemoryPackFormatterProvider.IsRegistered<object>()
		// System.Void MemoryPack.MemoryPackFormatterProvider.Register<int>(MemoryPack.MemoryPackFormatter<int>)
		// System.Void MemoryPack.MemoryPackFormatterProvider.Register<object>(MemoryPack.MemoryPackFormatter<object>)
		// System.Void MemoryPack.MemoryPackReader.DangerousReadUnmanagedArray<byte>(byte[]&)
		// byte[] MemoryPack.MemoryPackReader.DangerousReadUnmanagedArray<byte>()
		// MemoryPack.IMemoryPackFormatter<object> MemoryPack.MemoryPackReader.GetFormatter<object>()
		// System.Void MemoryPack.MemoryPackReader.ReadPackable<object>(object&)
		// object MemoryPack.MemoryPackReader.ReadPackable<object>()
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.float3,Unity.Mathematics.float3>(Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.float3,int>(Unity.Mathematics.float3&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.float3>(Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.quaternion,int>(Unity.Mathematics.quaternion&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<Unity.Mathematics.quaternion>(Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,Unity.Mathematics.float3,int>(byte&,Unity.Mathematics.float3&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,Unity.Mathematics.float3>(byte&,int&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,int,int,int>(byte&,int&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,int,long,long,long>(byte&,int&,int&,long&,long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,int>(byte&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,long,Unity.Mathematics.float3,Unity.Mathematics.quaternion>(byte&,int&,long&,Unity.Mathematics.float3&,Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,long,int>(byte&,int&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int,long>(byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,int>(byte&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,Unity.Mathematics.float3>(byte&,long&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,int,int,Unity.Mathematics.float3,Unity.Mathematics.float3>(byte&,long&,int&,int&,Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,int,int>(byte&,long&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,int>(byte&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long,int,long,int>(byte&,long&,long&,int&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long,int>(byte&,long&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long,long,byte,byte,byte,byte,byte>(byte&,long&,long&,long&,byte&,byte&,byte&,byte&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long,long,int>(byte&,long&,long&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long,long,long>(byte&,long&,long&,long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long,long>(byte&,long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,long>(byte&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte,uint>(byte&,uint&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<byte>(byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,int,int>(int&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,int>(int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,long,int>(int&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int,long>(int&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<int>(int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,byte,int,long>(long&,byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,byte>(long&,byte&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,int>(long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,long,int,int>(long&,long&,int&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,long,int,long,long>(long&,long&,int&,long&,long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long,long,int>(long&,long&,int&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<long>(long&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanaged<uint>(uint&)
		// System.Void MemoryPack.MemoryPackReader.ReadUnmanagedArray<byte>(byte[]&)
		// byte[] MemoryPack.MemoryPackReader.ReadUnmanagedArray<byte>()
		// System.Void MemoryPack.MemoryPackReader.ReadValue<object>(object&)
		// object MemoryPack.MemoryPackReader.ReadValue<object>()
		// System.Void MemoryPack.MemoryPackWriter.DangerousWriteUnmanagedArray<byte>(byte[])
		// MemoryPack.IMemoryPackFormatter<object> MemoryPack.MemoryPackWriter.GetFormatter<object>()
		// System.Void MemoryPack.MemoryPackWriter.WritePackable<object>(object&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<Unity.Mathematics.float3,Unity.Mathematics.float3>(Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<Unity.Mathematics.float3,int>(Unity.Mathematics.float3&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<Unity.Mathematics.quaternion,int>(Unity.Mathematics.quaternion&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<byte,long>(byte&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,int,int>(int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,int>(int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,long,int>(int&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int,long>(int&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<int>(int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,byte>(long&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,int>(long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,long,int,int>(long&,long&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,long,int,long,long>(long&,long&,int&,long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long,long,int>(long&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanaged<long>(long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedArray<byte>(byte[])
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,Unity.Mathematics.float3,int>(byte,byte&,Unity.Mathematics.float3&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,Unity.Mathematics.float3>(byte,byte&,int&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,int,int,int>(byte,byte&,int&,int&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,int,long,long,long>(byte,byte&,int&,int&,long&,long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,int>(byte,byte&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,long,Unity.Mathematics.float3,Unity.Mathematics.quaternion>(byte,byte&,int&,long&,Unity.Mathematics.float3&,Unity.Mathematics.quaternion&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,long,int>(byte,byte&,int&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int,long>(byte,byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,int>(byte,byte&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,Unity.Mathematics.float3>(byte,byte&,long&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,int,int,Unity.Mathematics.float3,Unity.Mathematics.float3>(byte,byte&,long&,int&,int&,Unity.Mathematics.float3&,Unity.Mathematics.float3&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,int,int>(byte,byte&,long&,int&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,int>(byte,byte&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long,int,long,int>(byte,byte&,long&,long&,int&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long,int>(byte,byte&,long&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long,long,byte,byte,byte,byte,byte>(byte,byte&,long&,long&,long&,byte&,byte&,byte&,byte&,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long,long,int>(byte,byte&,long&,long&,long&,int&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long,long,long>(byte,byte&,long&,long&,long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long,long>(byte,byte&,long&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,long>(byte,byte&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte,uint>(byte,byte&,uint&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<byte>(byte,byte&)
		// System.Void MemoryPack.MemoryPackWriter.WriteUnmanagedWithObjectHeader<long,byte,int,long>(byte,long&,byte&,int&,long&)
		// System.Void MemoryPack.MemoryPackWriter.WriteValue<object>(object&)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(MongoDB.Bson.IO.IBsonReader,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// object MongoDB.Bson.Serialization.BsonSerializer.Deserialize<object>(string,System.Action<MongoDB.Bson.Serialization.BsonDeserializationContext.Builder>)
		// MongoDB.Bson.Serialization.IBsonSerializer<object> MongoDB.Bson.Serialization.BsonSerializer.LookupSerializer<object>()
		// object MongoDB.Bson.Serialization.IBsonSerializerExtensions.Deserialize<object>(MongoDB.Bson.Serialization.IBsonSerializer<object>,MongoDB.Bson.Serialization.BsonDeserializationContext)
		// object System.Activator.CreateInstance<object>()
		// byte[] System.Array.Empty<byte>()
		// object[] System.Array.Empty<object>()
		// int System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,int>(System.Collections.Generic.IReadOnlyDictionary<int,int>,int)
		// int System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,int>(System.Collections.Generic.IReadOnlyDictionary<int,int>,int,int)
		// long System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,long>(System.Collections.Generic.IReadOnlyDictionary<int,long>,int)
		// long System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,long>(System.Collections.Generic.IReadOnlyDictionary<int,long>,int,long)
		// object System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,object>(System.Collections.Generic.IReadOnlyDictionary<int,object>,int)
		// object System.Collections.Generic.CollectionExtensions.GetValueOrDefault<int,object>(System.Collections.Generic.IReadOnlyDictionary<int,object>,int,object)
		// int System.Enum.Parse<int>(string)
		// int System.Enum.Parse<int>(string,bool)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<System.Linq.IGrouping<int,object>> System.Linq.Enumerable.GroupBy<object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<System.Linq.IGrouping<object,object>> System.Linq.Enumerable.GroupBy<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Skip<object>(System.Collections.Generic.IEnumerable<object>,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.SkipIterator<object>(System.Collections.Generic.IEnumerable<object>,int)
		// ET.RpcInfo[] System.Linq.Enumerable.ToArray<ET.RpcInfo>(System.Collections.Generic.IEnumerable<ET.RpcInfo>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Span<byte> System.MemoryExtensions.AsSpan<byte>(byte[])
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<object,ET.Client.UIHelper.<LoadComUI>d__0>(object&,ET.Client.UIHelper.<LoadComUI>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<ET.Client.UIHelper.<LoadComUI>d__0>(ET.Client.UIHelper.<LoadComUI>d__0&)
		// byte& System.Runtime.CompilerServices.Unsafe.Add<byte>(byte&,int)
		// byte& System.Runtime.CompilerServices.Unsafe.As<byte,byte>(byte&)
		// object& System.Runtime.CompilerServices.Unsafe.AsRef<object>(object&)
		// Unity.Mathematics.float3 System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Unity.Mathematics.float3>(byte&)
		// Unity.Mathematics.quaternion System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Unity.Mathematics.quaternion>(byte&)
		// byte System.Runtime.CompilerServices.Unsafe.ReadUnaligned<byte>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>(byte&)
		// long System.Runtime.CompilerServices.Unsafe.ReadUnaligned<long>(byte&)
		// uint System.Runtime.CompilerServices.Unsafe.ReadUnaligned<uint>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<Unity.Mathematics.float3>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<Unity.Mathematics.quaternion>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<byte>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<int>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<long>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<uint>()
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<Unity.Mathematics.float3>(byte&,Unity.Mathematics.float3)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<Unity.Mathematics.quaternion>(byte&,Unity.Mathematics.quaternion)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<byte>(byte&,byte)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<int>(byte&,int)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<long>(byte&,long)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<uint>(byte&,uint)
		// byte& System.Runtime.InteropServices.MemoryMarshal.GetReference<byte>(System.Span<byte>)
		// System.Threading.Tasks.Task<object> System.Threading.Tasks.TaskFactory.StartNew<object>(System.Func<object>,System.Threading.CancellationToken)
		// object UnityEngine.Component.GetComponent<object>()
		// object[] UnityEngine.Component.GetComponents<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// YooAsset.AllAssetsHandle YooAsset.ResourcePackage.LoadAllAssetsAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string,uint)
	}
}