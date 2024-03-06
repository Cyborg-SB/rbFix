using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix.Transport;
using RbFix.Application.Services.Interfaces;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Helpers;
using RbFix.Infrastructure.Providers;
using RbFix.Infrastructure.Providers.Interfaces;
using System.Collections.Concurrent;

namespace RbFix.Application.Services
{

    /// <summary>
    /// Criar session config com as inforamçòes da sessão
    /// GatewayName
    /// DictionarioFix
    /// IpAddress
    /// HeartBeatInterval
    /// Port
    /// RawData
    /// RawDataLength
    /// Host
    /// CustomKey
    /// shouldLogSchedulerMessages
    /// CustomLogonMessage
    /// startTime
    /// Endtime
    /// 
    /// session config deve fazer parte de session entity
    /// que deve extender sessionId
    ///
    /// 
    ///
    /// </summary>
    public class FixSchedulerService : IFixSchedulerService
    {
        private readonly ISessionSettingsProvider sessionSettingsProvider;
        private static readonly object _locker = new (); 
        internal static ConcurrentDictionary<string, Session> SessionsDictionary { get; private set; } = new ConcurrentDictionary<string, Session>();
        public  IReadOnlyCollection<Session> Sessions { get { return SessionsDictionary.Values.ToList(); } }
        private SocketInitiator initiator;
        private ThreadedSocketAcceptor acceptor;
        public FixSchedulerService(ISessionSettingsProvider sessionSettingsProvider)
        {
            this.sessionSettingsProvider = sessionSettingsProvider;
        }
        public HashSet<SessionID> SessionsInScheduler { get { return sessionSettingsProvider.Instance.GetSessions(); } }
        public void LoadConfiguration(string content)
        {
            sessionSettingsProvider.Instance.Load(content);
        }


        public void RegisterSessions(IFixApplication app)
        {
            lock (_locker)
            {
                if (!FixEngine.IsInitialized)
                    throw new Exception("Fix engine is not initialized");

                IMessageStoreFactory storeFactory = new FixFileStoreFactory(sessionSettingsProvider.Instance);
                ILogFactory logFactory = new FixFileLogFactory(sessionSettingsProvider.Instance);
                IMessageFactory messageFactory = new MessageFactory();

                initiator = new SocketInitiator(app, storeFactory, sessionSettingsProvider.Instance, logFactory, messageFactory);
                acceptor = new ThreadedSocketAcceptor(app, storeFactory, sessionSettingsProvider.Instance, logFactory, messageFactory);

                initiator?.Start();
                acceptor?.Start();

                SetupSessionParameters();

                //MessageGenerationMock();
            }

        }

        private void SetupSessionParameters()
        {
            foreach (SessionID session in SessionsInScheduler)
            {
                var currentSessionDictinary = sessionSettingsProvider.Instance.Get(session);

                var sessionEntity = SessionsDictionary.GetOrAdd(session.SenderCompID, Session.LookupSession(session));

                sessionEntity.ApplicationDataDictionary.AllowUnknownMessageFields = currentSessionDictinary.GetBoolSafe(SessionSettings.ALLOW_UNKNOWN_MSG_FIELDS) ?? FixEngine.Instance.Settings.AllowUnknownMsgFields;
                sessionEntity.ApplicationDataDictionary.CheckFieldsHaveValues = currentSessionDictinary.GetBoolSafe(SessionSettings.VALIDATE_FIELDS_HAVE_VALUES) ?? FixEngine.Instance.Settings.CheckFieldsHaveValues;
                sessionEntity.ApplicationDataDictionary.CheckFieldsOutOfOrder = currentSessionDictinary.GetBoolSafe(SessionSettings.VALIDATE_FIELDS_OUT_OF_ORDER) ?? FixEngine.Instance.Settings.CheckFieldsOutOfOrder;
                sessionEntity.ApplicationDataDictionary.CheckUserDefinedFields = currentSessionDictinary.GetBoolSafe(SessionSettings.VALIDATE_USER_DEFINED_FIELDS) ?? FixEngine.Instance.Settings.CheckUserDefinedFields;
                sessionEntity.CheckLatency = currentSessionDictinary.GetBoolSafe(SessionSettings.CHECK_LATENCY) ?? FixEngine.Instance.Settings.CheckLatency;
                sessionEntity.MaxLatency = currentSessionDictinary.GetIntSsafe(SessionSettings.MAX_LATENCY) ?? FixEngine.Instance.Settings.MaxLatency;
                sessionEntity.MaxMessagesInResendRequest = currentSessionDictinary.GetUlongSafe(SessionSettings.MAX_MESSAGES_IN_RESEND_REQUEST) ?? FixEngine.Instance.Settings.MaxMessagesInResendRequest;
                sessionEntity.ValidateLengthAndChecksum = currentSessionDictinary.GetBoolSafe(SessionSettings.VALIDATE_LENGTH_AND_CHECKSUM) ?? FixEngine.Instance.Settings.ValidateLengthAndChecksum;
                sessionEntity.MillisecondsInTimeStamp = currentSessionDictinary.GetBoolSafe(SessionSettings.MILLISECONDS_IN_TIMESTAMP) ?? FixEngine.Instance.Settings.MillisecondsInTimeStamp;
                sessionEntity.SendRedundantResendRequests = currentSessionDictinary.GetBoolSafe(SessionSettings.SEND_REDUNDANT_RESENDREQUESTS) ?? FixEngine.Instance.Settings.SendRedundantResendRequests;
                sessionEntity.EnableLastMsgSeqNumProcessed = currentSessionDictinary.GetBoolSafe(SessionSettings.ENABLE_LAST_MSG_SEQ_NUM_PROCESSED) ?? FixEngine.Instance.Settings.EnableLastMsgSeqNumProcessed;
                sessionEntity.RequiresOrigSendingTime = currentSessionDictinary.GetBoolSafe(SessionSettings.RESETSEQUENCE_MESSAGE_REQUIRES_ORIGSENDINGTIME) ?? FixEngine.Instance.Settings.RequiresOrigSendingTime;

            }
        }

        private void MessageGenerationMock()
        {
            while (true)
            {

                var sesseion = Session.LookupSession(sessionSettingsProvider.Instance.GetSessions()?.FirstOrDefault(x => x.SenderCompID == "ARCAA"));

                IMessageFactory messageFactory = new MessageFactory();
                var msg = messageFactory.Create(sesseion.SessionID.BeginString, MsgType.EXECUTIONREPORT); ;

                msg.SetField(new DecimalField(QuickFix.Fields.Tags.Price, 15));
                msg.SetField(new ClOrdID($"{new Random().Next(1000000, 9000000)}"));
                msg.SetField(new ExecID($"{new Random().Next(1000000, 9000000)}"));
                msg.SetField(new Symbol($"PETR4"));
                msg.SetField(new Side(Side.SELL));
                msg.SetField(new TransactTime(DateTime.Now));
                msg.SetField(new OrdType(OrdType.LIMIT));
                msg.SetField(new OrdType(OrdType.LIMIT));
                msg.SetField(new Account($"{new Random().Next(1000000, 9000000)}"));
                msg.SetField(new OrderID($"{new Random().Next(1000000, 9000000)}"));

                Group group = new (NoPartyIDs.TAG, PartyID.TAG);
                Group group2 = new (NoPartyIDs.TAG, PartyID.TAG);

                group.SetField(new PartyID("12345678901"), false);
                group.SetField(new PartyIDSource('D'), false);
                group.SetField(new PartyRole(7), false);

                group2.SetField(new PartyID("444345678901"), false);
                group2.SetField(new PartyIDSource('D'), false);
                group2.SetField(new PartyRole(11), false);

                Group subGroup = new(NoPartySubIDs.TAG, PartySubID.TAG);
                Group subGroup2 = new(NoPartySubIDs.TAG, PartySubID.TAG);

                subGroup.SetField(new PartySubID("PartySubID1"), false);
                subGroup.SetField(new PartySubIDType(14), false);

                subGroup2.SetField(new PartySubID("PartySubID2"), false);
                subGroup2.SetField(new PartySubIDType(15), false);

                group.AddGroup(subGroup);
                group.AddGroup(subGroup2);
                group2.AddGroup(subGroup);
                group2.AddGroup(subGroup2);


                msg.AddGroup(group);
                msg.AddGroup(group2);
                msg.SetField(new ExecType(ExecType.NEW));
                msg.SetField(new OrdStatus(OrdStatus.NEW));
                msg.SetField(new LeavesQty(500));
                msg.SetField(new CumQty(500));
                msg.SetField(new Quantity(3500));
                msg.SetField(new AvgPx(12));
                msg.SetField(new Price(12));
                msg.SetField(new TradeDate($"{DateTime.Now:yyyyMMdd}"));

                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                //_ = msg.GetJson();
                sesseion.Send(msg);

            }
        }
    }
}
