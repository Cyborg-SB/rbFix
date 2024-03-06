using FluentAssertions;
using RbFix.Infrastructure.Helpers;
using RbFix.Tests.Shared;

namespace RbFix.Tests.Infrastructure.Helpers
{
    public class QuickFixMessagesExtensionsTests
    {
        public QuickFixMessagesExtensionsTests()
        {
            CommomMocks.InitializeFixEngineWithDefaultTestingSettings();
        }
        [Fact]
        public void GetJsonShouldReturnIndentedSerializeMessageWithPropertyNames()
        {
            //ARRANGE            
            var msg = CommomMocks.GetExecutionReportTesteMessageWithSubGroups(CommomMocks.SessionInitiator);

            //ACT
            var msgJson = msg.GetJson();

            //ASSERT
            msgJson.Should().NotBeNullOrWhiteSpace();
            var assertList = new List<string>();

            CommomMocks.GetMessagePropertiesNames(CommomMocks.SessionInitiator.ApplicationDataDictionary, msg, assertList);
          
            msgJson.Should().ContainAll(assertList);
        }

        [Fact]
        public void GetJsonShouldReturnIndentedSerializeMessageWithNoProperties()
        {
            //ARRANGE
            var msg = CommomMocks.GetExecutionReportTesteMessageWithSubGroups(CommomMocks.SessionInitiator, false);

            //ACT
            var msgJson = msg.GetJson();

            //ASSERT
            msgJson.Should().NotBeNullOrWhiteSpace();
            var assertList = new List<string>();
            CommomMocks.GetMessagePropertiesNames(CommomMocks.SessionInitiator.ApplicationDataDictionary, msg, assertList);

            msgJson.Should().NotContainAll(assertList);
        }

    }
}