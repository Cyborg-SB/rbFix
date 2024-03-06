using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using QuickFix;
using RbFix.Application.Services.Interfaces;
using RbFix.Application.UseCases;
using RbFix.Tests.Shared;

namespace RbFix.Tests.UseCases
{
    public class GetFixSessionUseCaseTests
    {
        private readonly GetSessionUseCase sut;
        private readonly ILogger<GetSessionUseCase> nMockLogger;
        private readonly IFixSessionService nMockSessionService;
        public GetFixSessionUseCaseTests()
        {
            nMockSessionService = Substitute.For<IFixSessionService>();
            nMockLogger = Substitute.For<ILogger<GetSessionUseCase>>();
            sut = new GetSessionUseCase(nMockLogger,nMockSessionService);
            CommomMocks.InitializeFixEngineWithDefaultTestingSettings();
        }

        [Fact]
        void Teste()
        {
            var sessions = new List<Session>()
            {
                CommomMocks.SessionAceptor,
                CommomMocks.SessionInitiator
            };

            nMockSessionService.GetSessions().Returns(sessions);

            var response = sut.GetSessions();

            response.Should().BeSameAs(sessions);
        }
        [Fact]
        void Teste2()
        {
            nMockSessionService.GetSessions().ReturnsNull();

            var response = sut.GetSessions();
            nMockLogger.Received(1).LogInformation("");
            response.Should().BeSameAs(null);
        }

    }
}
