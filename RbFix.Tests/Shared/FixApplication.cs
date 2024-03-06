using QuickFix;
using RbFix.Application.Services.Interfaces;

namespace RbFix.Tests.Shared
{
    internal class FixApplication : IFixApplication
    {
        internal FixApplication()
        {

        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
        }

        public void FromApp(Message message, SessionID sessionID)
        {
        }

        public void OnCreate(SessionID sessionID)
        {
        }

        public void OnLogon(SessionID sessionID)
        {
        }

        public void OnLogout(SessionID sessionID)
        {
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
        }

        public void ToApp(Message message, SessionID sessionID)
        {
        }
    }
}
