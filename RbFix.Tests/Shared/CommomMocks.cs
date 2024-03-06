using QuickFix;
using QuickFix.DataDictionary;
using QuickFix.Fields;
using RbFix.Application.Services;
using RbFix.Application.Services.Interfaces;
using RbFix.Infrastructure.Configuration;
using RbFix.Infrastructure.Configuration.Setings;
using RbFix.Infrastructure.Providers;
using RbFix.Infrastructure.Providers.Interfaces;

namespace RbFix.Tests.Shared
{
    internal static class CommomMocks
    {
        internal static string UnitTestingSessionsSettings { get;} = "\r\n# default settings for sessions\r\n[DEFAULT]\r\nFileStorePath=store\r\nFileLogPath=log\r\nReconnectInterval=60\r\n\r\n# session definition\r\n[SESSION]\r\n# inherit FileStorePath, FileLogPath, ConnectionType, \r\n#    ReconnectInterval and SenderCompID from default\r\nBeginString=FIX.4.4\r\nConnectionType=acceptor\r\nSenderCompID=ARCAA\r\nTargetCompID=TW1\r\nStartTime=00:00:01\r\nEndTime=23:59:59\r\nHeartBtInt=20\r\nSocketAcceptPort=9823\r\nSocketAcceptHost=127.0.0.1\r\nDataDictionary= Infrastructure/Dictionaries/FIX44EntrypointGatewayEquities.xml\r\n\r\n\r\n\r\n# session definition\r\n[SESSION]\r\n# inherit FileStorePath, FileLogPath, ConnectionType, \r\n#    ReconnectInterval and SenderCompID from default\r\nBeginString=FIX.4.4\r\nTargetCompID=ARCAA\r\nSenderCompID=TW1\r\nConnectionType=initiator\r\nStartTime=00:00:01\r\nEndTime=23:59:59\r\nHeartBtInt=20\r\nSocketConnectPort=9823\r\nSocketConnectHost=127.0.0.1\r\nDataDictionary= Infrastructure/Dictionaries/FIX44EntrypointGatewayEquities.xml\r\n";
        internal static Session SessionInitiator { get; private set; }
        internal static Session SessionAceptor { get; private set; }
        internal static IFixSchedulerService FixSchedulerService { get; private set; } 
        private static readonly object _locker = new();
        internal static ISessionSettingsProvider SessionSettingsProvider { get; private set; }
        public static void InitializeFixEngineWithDefaultTestingSettings()
        {
            lock (_locker)
            {
                if (FixEngine.IsInitialized)
                    return;

                SessionSettingsProvider = new SessionSettingsProvider(UnitTestingSessionsSettings);
                FixEngine.Initialize(new FixEngineSettings());
                FixSchedulerService = new FixSchedulerService(SessionSettingsProvider.Instance);
                RegisterSessions(new FixApplication());

                SessionInitiator = GetUnitTestingSession(SessionSettingsProvider.Instance.GetSessions().First().SenderCompID);
                SessionAceptor = GetUnitTestingSession(SessionSettingsProvider.Instance.GetSessions().First().TargetCompID);
            }

            
        }

        private static Session GetUnitTestingSession(string sessionName)
        {
            var session = Session.LookupSession(SessionSettingsProvider.Instance.GetSessions()?.FirstOrDefault(x => x.SenderCompID == sessionName));
            return session; 
        }

        private static void RegisterSessions(IFixApplication app)
        {
            FixSchedulerService.RegisterSessions(app);
        }
        public static Message GetExecutionReportTesteMessageWithSubGroups(Session session, bool shouldUseSessionDicionary = true)
        {
            Message msg;

            if (shouldUseSessionDicionary)
                msg = new Message("", session.SessionDataDictionary, session.ApplicationDataDictionary, false);
            else
                msg = new Message();


            msg.Header.SetField(new StringField(Tags.MsgType,MsgType.EXECUTION_REPORT));
            msg.Header.SetField(new StringField(Tags.BeginString,session.SessionID.BeginString));

            msg.SetField(new ClOrdID($"{new Random().Next(1000000, 9000000)}"));
            msg.SetField(new ExecID($"{new Random().Next(1000000, 9000000)}"));
            msg.SetField(new Symbol($"PETR4"));
            msg.SetField(new Side(Side.SELL));
            msg.SetField(new TransactTime(DateTime.Now));
            msg.SetField(new OrdType(OrdType.LIMIT));
            msg.SetField(new OrdType(OrdType.LIMIT));
            msg.SetField(new Account($"{new Random().Next(1000000, 9000000)}"));
            msg.SetField(new OrderID($"{new Random().Next(1000000, 9000000)}"));

            Group group = new(NoPartyIDs.TAG, PartyID.TAG);
            Group group2 = new(NoPartyIDs.TAG, PartyID.TAG);

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

            return msg;
        }

        internal static void GetMessagePropertiesNames(DataDictionary dataDictionary, Message msg, List<string> assertList)
        {
            foreach (KeyValuePair<int, IField> field in msg.Header)
            {
                dataDictionary.FieldsByTag.TryGetValue(field.Value.Tag, out DDField dicionaryField);
                assertList.Add(dicionaryField.Name);
            }
            foreach (KeyValuePair<int, IField> field in msg)
            {
                dataDictionary.FieldsByTag.TryGetValue(field.Value.Tag, out DDField dicionaryField);
                assertList.Add(dicionaryField.Name);
            }           

            foreach (KeyValuePair<int, IField> field in msg.Trailer)
            {
                dataDictionary.FieldsByTag.TryGetValue(field.Value.Tag, out DDField dicionaryField);
                assertList.Add(dicionaryField.Name);
            }


            List<int> groupsTag = msg.GetGroupTags();

            foreach (var groupTag in groupsTag)
            {
                RecoverFieldPropertyName(dataDictionary, msg, assertList, groupTag);
            }

        }


        internal static void RecoverFieldPropertyName(DataDictionary dataDictionary, FieldMap msg, List<string> propertiesNameList, int fieldTag)
        {
            var currentGroup = msg.GetGroup(1, fieldTag);
            var groupsTag = currentGroup.GetGroupTags();
            foreach (KeyValuePair<int, IField> field in currentGroup)
            {
                if (groupsTag.Contains(field.Value.Tag))
                    RecoverFieldPropertyName(dataDictionary, currentGroup, propertiesNameList, field.Value.Tag);
                else
                {
                    dataDictionary.FieldsByTag.TryGetValue(field.Value.Tag, out DDField dicionaryField);
                    propertiesNameList.Add(dicionaryField.Name);
                }

            }
        }

        internal static void RecoverFieldPropertyName(DataDictionary dataDictionary, Group group, List<string> propertiesNameList, int groupTag)
        {
            var currentGroup = group.GetGroup(1, groupTag);
            dataDictionary.FieldsByTag.TryGetValue(groupTag, out DDField groupField);
            propertiesNameList.Add(groupField.Name);
            foreach (KeyValuePair<int, IField> field in currentGroup)
            {
                if (currentGroup.GetGroupTags().Count > 0)
                    RecoverFieldPropertyName(dataDictionary, group, propertiesNameList, field.Value.Tag);
                else
                {
                    dataDictionary.FieldsByTag.TryGetValue(field.Value.Tag, out DDField dicionaryField);
                    propertiesNameList.Add(dicionaryField.Name);
                }

            }
        }        
    }
}
