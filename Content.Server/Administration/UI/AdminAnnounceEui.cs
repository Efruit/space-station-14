using Content.Server.Administration.Managers;
using Content.Server.Chat.Managers;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Robust.Shared.IoC;

namespace Content.Server.Administration.UI
{
    public sealed class AdminAnnounceEui : BaseEui
    {
        [Dependency] private readonly IAdminManager _adminManager = default!;
        [Dependency] private readonly IChatManager _chatManager = default!;

        public AdminAnnounceEui()
        {
            IoCManager.InjectDependencies(this);
        }

        public override void Opened()
        {
            StateDirty();
        }

        public override EuiStateBase GetNewState()
        {
            return new AdminAnnounceEuiState();
        }

        public override void HandleMessage(EuiMessageBase msg)
        {
            switch (msg)
            {
                case AdminAnnounceEuiMsg.Close:
                    Close();
                    break;
                case AdminAnnounceEuiMsg.DoAnnounce doAnnounce:
                    if (!_adminManager.HasAdminFlag(Player, AdminFlags.Fun))
                    {
                        Close();
                        break;
                    }

                    switch (doAnnounce.AnnounceType)
                    {
                        case AdminAnnounceType.Server:
                            _chatManager.DispatchServerAnnouncement(doAnnounce.Announcement);
                            break;
                        case AdminAnnounceType.Station:
                            _chatManager.DispatchStationAnnouncement(doAnnounce.Announcement, doAnnounce.Announcer);
                            break;
                    }

                    StateDirty();

                    if (doAnnounce.CloseAfter)
                        Close();

                    break;
            }
        }
    }
}
