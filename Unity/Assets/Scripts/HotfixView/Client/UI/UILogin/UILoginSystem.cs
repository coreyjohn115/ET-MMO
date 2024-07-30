using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (UILogin))]
    [FriendOf(typeof (Account))]
    [FriendOf(typeof (ServerInfoComponent))]
    [EntitySystemOf(typeof (UILogin))]
    public static partial class UILoginSystem
    {
        [Invoke(TimerInvokeType.ClientServerCheck)]
        private class ServerCheckTimer: ATimer<UILogin>
        {
            protected override void Run(UILogin self)
            {
                self.QueryServer().NoContext();
            }
        }

        [EntitySystem]
        private static void Awake(this UILogin self)
        {
            self.TimerId = self.Fiber().Root.GetComponent<TimerComponent>().NewRepeatedTimer(10 * 1000, TimerInvokeType.ClientServerCheck, self);
        }

        [EntitySystem]
        private static void Destroy(this UILogin self)
        {
            self.Fiber().Root.GetComponent<TimerComponent>().Remove(ref self.TimerId);
        }

        public static void RegisterUIEvent(this UILogin self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UILogin self)
        {
            self.View.E_LoginBtnButton.AddListenerAsync(self.OnLoginClick);
            self.View.E_ServerBtnButton.AddListenerAsync(self.OnServerClick);
            self.View.E_BackBtnButton.AddListener(self.OnBackClick);
            self.View.E_EnterGameBtnButton.AddListenerAsync(self.OnEnterGameClick);
        }

        public static void OnFocus(this UILogin self)
        {
            var server = self.Scene().GetComponent<ServerInfoComponent>().GetCurServer();
            if (server == null)
            {
                return;
            }

            self.View.E_ServerTxtExtendText.text = server.ServerName;
            switch (server.Status)
            {
                case ServerStatus.Normal:
                    self.View.E_ServerTxtExtendText.color = Color.green;
                    break;
                case ServerStatus.Stop:
                    self.View.E_ServerTxtExtendText.color = Color.gray;
                    break;
            }
        }

        public static void ShowWindow(this UILogin self, Entity contextData = null)
        {
            self.View.E_AccountInputInputField.text = string.Empty;
            self.View.E_PasswordInputInputField.text = string.Empty;
            using (var entity = self.Scene().GetComponent<DataSaveComponent>().Get<Account>("LoginAccount"))
            {
                if (entity != null)
                {
                    self.View.E_AccountInputInputField.text = entity.AccountName;
                    self.View.E_PasswordInputInputField.text = entity.Password;
                    self.QueryAccount().NoContext();

                    var server = self.Scene().GetComponent<DataSaveComponent>().Get<ServerInfo>("LoginServer");
                    if (server != null)
                    {
                        self.View.E_ServerTxtExtendText.text = server.ServerName;
                        self.Scene().GetComponent<ServerInfoComponent>().CurrentServerId = (int)server.Id;
                    }

                    self.QueryServer().NoContext();
                }

                self.View.EG_AccountRectTransform.SetActive(entity == null);
                self.View.EG_ServerRectTransform.SetActive(entity != null);
            }
        }

        private static async ETTask<bool> QueryServer(this UILogin self)
        {
            var errno = await LoginHelper.GetServerInfos(self.Scene(), self.View.E_AccountInputInputField.text);
            if (errno != ErrorCode.ERR_Success)
            {
                EventSystem.Instance.Publish(self.Scene(), new NetError() { Error = errno });
                Log.Error($"获取服务器列表失败: {errno}");
                return false;
            }

            SelectDefServer();
            var info = self.Scene().GetComponent<ServerInfoComponent>().GetCurServer();
            if (info == null)
            {
                SelectDefServer();
                info = self.Scene().GetComponent<ServerInfoComponent>().GetCurServer();
            }

            await self.Scene().GetComponent<DataSaveComponent>().SaveAsync("LoginServer", info);
            switch (info.Status)
            {
                case ServerStatus.Normal:
                    self.View.E_ServerTxtExtendText.color = Color.green;
                    break;
                case ServerStatus.Stop:
                    self.View.E_ServerTxtExtendText.color = Color.gray;
                    break;
            }

            return true;

            void SelectDefServer()
            {
                //默认选择服务器
                if (self.Scene().GetComponent<ServerInfoComponent>().CurrentServerId == 0)
                {
                    var server = self.Scene().GetComponent<ServerInfoComponent>().GetDefault();
                    self.View.E_ServerTxtExtendText.text = server.ServerName;
                    self.Scene().GetComponent<ServerInfoComponent>().CurrentServerId = (int)server.Id;
                }
            }
        }

        private static async ETTask<bool> QueryAccount(this UILogin self)
        {
            var errno = await LoginHelper.QueryAccount(self.Scene(), self.View.E_AccountInputInputField.text,
                self.View.E_PasswordInputInputField.text);
            if (errno != ErrorCode.ERR_Success)
            {
                Log.Error($"获取账号失败: {errno}");
                return false;
            }

            var acc = self.Scene().GetChild<Account>();
            await self.Scene().GetComponent<DataSaveComponent>().SaveAsync("LoginAccount", acc);

            return true;
        }

        private static async ETTask OnLoginClick(this UILogin self)
        {
            var ok = await self.QueryAccount();
            if (!ok)
            {
                return;
            }

            ok = await self.QueryServer();
            if (!ok)
            {
                return;
            }

            self.View.EG_AccountRectTransform.SetActive(false);
            self.View.EG_ServerRectTransform.SetActive(true);
        }

        private static async ETTask OnServerClick(this UILogin self)
        {
            await self.Scene().GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UIServerList);
        }

        private static void OnBackClick(this UILogin self)
        {
            self.View.EG_AccountRectTransform.SetActive(true);
            self.View.EG_ServerRectTransform.SetActive(false);
        }

        private static async ETTask OnEnterGameClick(this UILogin self)
        {
            var account = self.Scene().GetChild<Account>();
            if (!account)
            {
                Log.Error($"获取账号信息失败!");
                return;
            }

            var errno = await LoginHelper.Login(self.Scene(), account.AccountName, account.Password, account.Id);
            if (errno != ErrorCode.ERR_Success)
            {
                Log.Error($"登录失败: {errno}");
                return;
            }

            await self.Root().GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UILoading);
            errno = await EnterMapHelper.EnterMapAsync(self.Root());
            if (errno != ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<UIComponent>().CloseWindow(WindowID.Win_UILoading);
            }
        }
    }
}