using System;
using Moq;
using NUnit.Framework;
using QuikLauncher;

namespace QuikLauncherTests
{
    public class LaunchServiceTest
    {
        private Mock<IConfigurationService> _configurationMock;
        private Mock<IQuikApplicationManager> _quikAppMock;

        private IQuikLaunchService service;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfigurationService>();
            _quikAppMock = new Mock<IQuikApplicationManager>();

            _configurationMock
                .Setup(c => c.Delay)
                .Returns(0);

            service = new QuikLaunchService(
                _configurationMock.Object,
                _quikAppMock.Object);
        }

        [Test]
        public void Run_WhenQuikAuthorized_ShouldDoNothing()
        {
            _quikAppMock
                .Setup(q => q.GetAuthorizedWindow())
                .Returns(new IntPtr(1));

            service.Run();

            _quikAppMock.Verify(q => q.GetAuthorizedWindow(), Times.Once());
            _quikAppMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Run_WhenQuikExistsAndNotAuthorized_ShouldConnectAndLogin()
        {
            var quikWindow = new IntPtr(1);
            var loginPopup = new IntPtr(2);

            _quikAppMock
                .Setup(q => q.GetAuthorizedWindow())
                .Returns(IntPtr.Zero);

            _quikAppMock
                .Setup(q => q.GetNotAuthorizedWindow())
                .Returns(quikWindow);

            _quikAppMock
                .SetupSequence(q => q.GetLoginPopup())
                .Returns(IntPtr.Zero)
                .Returns(loginPopup);

            service.Run();

            _quikAppMock.Verify(q => q.GetAuthorizedWindow(), Times.Once());
            _quikAppMock.Verify(q => q.GetNotAuthorizedWindow(), Times.Once());
            _quikAppMock.Verify(q => q.GetLoginPopup(), Times.Exactly(2));
            _quikAppMock.Verify(q => q.ProceedConnect(quikWindow), Times.Once());
            _quikAppMock.Verify(q => q.ProceedLogin(loginPopup), Times.Once());
            _quikAppMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Run_WhenLoginPopupExists_ShouldLogin()
        {
            var loginPopup = new IntPtr(2);

            _quikAppMock
                .Setup(q => q.GetAuthorizedWindow())
                .Returns(IntPtr.Zero);

            _quikAppMock
                .Setup(q => q.GetLoginPopup())
                .Returns(loginPopup);

            service.Run();

            _quikAppMock.Verify(q => q.GetAuthorizedWindow(), Times.Once());
            _quikAppMock.Verify(q => q.GetLoginPopup(), Times.Once());
            _quikAppMock.Verify(q => q.ProceedLogin(loginPopup), Times.Once());
            _quikAppMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Run_WhenQuikNotFound_ShouldStartAndLogin()
        {
            var loginPopup = new IntPtr(2);

            _quikAppMock
                .Setup(q => q.GetAuthorizedWindow())
                .Returns(IntPtr.Zero);

            _quikAppMock
                .Setup(q => q.GetNotAuthorizedWindow())
                .Returns(IntPtr.Zero);

            _quikAppMock
                .SetupSequence(q => q.GetLoginPopup())
                .Returns(IntPtr.Zero)
                .Returns(loginPopup);

            service.Run();

            _quikAppMock.Verify(q => q.GetAuthorizedWindow(), Times.Once());
            _quikAppMock.Verify(q => q.GetLoginPopup(), Times.Exactly(2));
            _quikAppMock.Verify(q => q.GetNotAuthorizedWindow(), Times.Once());
            _quikAppMock.Verify(q => q.RunNewInstance(), Times.Once());
            _quikAppMock.Verify(q => q.ProceedLogin(loginPopup), Times.Once());
            _quikAppMock.VerifyNoOtherCalls();
        }
    }
}
