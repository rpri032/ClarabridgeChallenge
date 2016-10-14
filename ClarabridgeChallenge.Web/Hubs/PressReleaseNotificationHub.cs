using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Ngp.Oberon.Web
{
    [HubName("pressReleaseNotification")]
    public class PressReleaseNotificationHub : Hub
    {
        private static IHubContext _instance;
        private static IHubContext Instance
        {
            get { return _instance = _instance ??  GlobalHost.ConnectionManager.GetHubContext<PressReleaseNotificationHub>(); }
        }
        public static void PressReleaseListRefresh()
        {
            Instance.Clients.All.onPressReleaseListRefresh();
        }
    }
}