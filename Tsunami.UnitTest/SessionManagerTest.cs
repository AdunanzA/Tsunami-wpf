using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsunami;
using Tsunami.Models;

namespace Tsunami.UnitTest
{
    [TestClass]
    public class SessionManagerTest
    {
        [TestMethod]
        public void testGetTorrentStatusList()
        {
            SessionManager.Initialize();
            List<Models.TorrentStatus> statusList = SessionManager.getTorrentStatusList();
            Assert.IsNotNull(statusList);
            Assert.AreEqual(statusList.Count, 0);
        }
    }
}
