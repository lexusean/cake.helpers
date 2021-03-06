using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Helpers.Nuget;
using Cake.Helpers.Settings;
using Cake.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Cake.Helpers.Tests.Unit.Nuget
{
  [TestClass]
  public class NugetHelperSettingsTests
  {
    #region Test Setup and Teardown

    [TestInitialize]
    public void TestInit()
    {
      SingletonFactory.ClearFactory();
    }

    #endregion

    #region Test Methods

    [TestMethod]
    [TestCategory(Global.TestType)]
    public void NugetHelperSettings_AddSource_ExistingFeed()
    {
      var feedName = "testfeed";
      var feedUrl = "testfeed";
      var user = "test";
      var pass = "pass";
      var context = this.GetMoqContext(new Dictionary<string, bool>(), new Dictionary<string, string>());
      var helperSetting = new NugetHelperSettings();

      var firstSource = helperSetting.AddSource(feedName, feedUrl);

      Assert.IsNotNull(firstSource);
      Assert.IsFalse(firstSource.IsSecure);

      var nextSource = helperSetting.AddSource(feedName, feedUrl, source =>
      {
        source.IsSecure = true;
        source.Username = user;
        source.Password = pass;
      });

      Assert.IsNotNull(nextSource);
      Assert.AreEqual(firstSource, nextSource);
      Assert.IsTrue(nextSource.IsSecure);
      Assert.AreEqual(feedName, nextSource.FeedName);
      Assert.AreEqual(feedUrl, nextSource.FeedSource);
      Assert.AreEqual(user, nextSource.Username);
      Assert.AreEqual(pass, nextSource.Password);

      var feedUrls = helperSetting.GetFeedUrls();
      Assert.IsNotNull(feedUrls);
      var enumerable = feedUrls as string[] ?? feedUrls.ToArray();
      Assert.AreEqual(2, enumerable.Count());
      Assert.IsTrue(enumerable.Any(t => t == feedUrl));
    }

    [TestMethod]
    [TestCategory(Global.TestType)]
    public void NugetHelperSettings_AddSource_FeedUrlOnly()
    {
      var feedUrl = "testfeed";
      var context = this.GetMoqContext(new Dictionary<string, bool>(), new Dictionary<string, string>());
      var helperSetting = new NugetHelperSettings();

      var firstSource = helperSetting.AddSource(feedUrl);

      Assert.IsNotNull(firstSource);
      Assert.IsFalse(firstSource.IsSecure);
      Assert.IsTrue(string.IsNullOrWhiteSpace(firstSource.FeedName));
      Assert.AreEqual(feedUrl, firstSource.FeedSource);
    }

    [TestMethod]
    [TestCategory(Global.TestType)]
    public void NugetHelperSettings_AddSource_NoFeedUrl()
    {
      var feedName = "testfeed";

      var context = this.GetMoqContext(new Dictionary<string, bool>(), new Dictionary<string, string>());
      var helperSetting = new NugetHelperSettings();
      Assert.ThrowsException<ArgumentNullException>(() => helperSetting.AddSource(feedName, string.Empty));
    }

    [TestMethod]
    [TestCategory(Global.TestType)]
    public void NugetHelperSettings_Default()
    {
      var feedName = NugetHelperSettings.DefaultNugetFeedName;
      var feedUrl = NugetHelperSettings.DefaultNugetFeedUrl;
      var context = this.GetMoqContext(new Dictionary<string, bool>(), new Dictionary<string, string>());
      var helperSetting = new NugetHelperSettings();

      Assert.IsNotNull(helperSetting.NugetSources);
      Assert.AreEqual(1, helperSetting.NugetSources.Count());
      Assert.IsTrue(helperSetting.NugetSources.All(t => t.FeedName == feedName && t.FeedSource == feedUrl));
    }

    [TestMethod]
    [TestCategory(Global.TestType)]
    public void NugetHelperSettings_GetFeedUrls_NullHelper()
    {
      INugetHelperSettings helperSetting = null;
      Assert.ThrowsException<ArgumentNullException>(() => helperSetting.GetFeedUrls());
    }

    #endregion

    #region Test Helpers

    private ICakeArguments GetMoqArguments(
      IDictionary<string, bool> hasArgs,
      IDictionary<string, string> argValues)
    {
      var argsMock = new Mock<ICakeArguments>();
      argsMock.Setup(t => t.HasArgument(It.IsAny<string>()))
        .Returns((string arg) =>
        {
          if (!hasArgs.ContainsKey(arg))
            return false;

          return hasArgs[arg];
        });

      argsMock.Setup(t => t.GetArgument(It.IsAny<string>()))
        .Returns((string arg) =>
        {
          if (!argValues.ContainsKey(arg))
            return string.Empty;

          return argValues[arg];
        });

      return argsMock.Object;
    }

    private ICakeContext GetMoqContext(
      IDictionary<string, bool> hasArgs,
      IDictionary<string, string> argValues)
    {
      var fixture = HelperFixture.CreateFixture();
      var args = this.GetMoqArguments(hasArgs, argValues);
      var globber = this.GetMoqGlobber(fixture.FileSystem, fixture.Environment);
      var reg = this.GetMoqRegistry();

      return this.GetMoqContext(fixture, globber, reg, args);
    }

    private ICakeContext GetMoqContext(
      HelperFixture fixture,
      IGlobber globber,
      IRegistry registry,
      ICakeArguments args)
    {
      var log = new FakeLog();

      var contextMock = new Mock<ICakeContext>();
      contextMock.SetupGet(t => t.FileSystem).Returns(fixture.FileSystem);
      contextMock.SetupGet(t => t.Environment).Returns(fixture.Environment);
      contextMock.SetupGet(t => t.Globber).Returns(globber);
      contextMock.SetupGet(t => t.Log).Returns(log);
      contextMock.SetupGet(t => t.Arguments).Returns(args);
      contextMock.SetupGet(t => t.ProcessRunner).Returns(fixture.ProcessRunner);
      contextMock.SetupGet(t => t.Registry).Returns(registry);
      contextMock.SetupGet(t => t.Tools).Returns(fixture.Tools);

      return contextMock.Object;
    }

    private IGlobber GetMoqGlobber(
      IFileSystem fs,
      ICakeEnvironment env)
    {
      return new Globber(fs, env);
    }

    private IRegistry GetMoqRegistry()
    {
      var regMock = new Mock<IRegistry>();
      regMock.SetupGet(t => t.LocalMachine).Returns((IRegistryKey) null);

      return regMock.Object;
    }

    #endregion
  }
}