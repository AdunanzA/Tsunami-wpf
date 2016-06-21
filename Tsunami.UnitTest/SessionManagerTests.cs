using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsunami;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Tsunami.Gui.Wpf;
using System.Collections;

namespace Tsunami.UnitTest
{
   
 [TestClass()]
    public class SessionManagerTests
    {
        const string TORRENT_LIBRE_OFFICE = "LibreOffice_5.1.1_Win_x86.msi.torrent";
        const string TORRENT_FOLDER = @"Torrent\";
        Boolean bSessionManagerStarted = false;
        Boolean bStartWeb = false;
        int listSize = 0;
        List<TorrentItem> torrentList = null;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance;}
            set { testContextInstance = value; }
        }
        /*
        Result Message:	Method Tsunami.UnitTest.SessionManagerTests.ClassInit 
        has wrong signature. 
        The method must be static, public, does not return a value 
        and should take a single parameter of type TestContext.
        */

/*        [ClassInitialize]
            static public void ClassInit(TestContext)
            {
                torrentList = new List<TorrentItem>();
            }
*/
        [TestMethod()]
        public void InitializeTest()
        {
            Boolean bResult = true;
            if (bSessionManagerStarted == false)
            {
                SessionManager.Initialize();
                bSessionManagerStarted = true;
                torrentList = new List<TorrentItem>();
                setListeners();
            }

            Assert.IsTrue(bResult) ;
        }

        [TestMethod()]
        public void startWebTest() {
            InitializeTest();
// Tests nothing because SessionManager.startWeb() is a void Method
            Boolean bResult = true;
            if (bStartWeb == false) { 
                SessionManager.startWeb();
                bStartWeb = true;
            }
            Assert.IsTrue(bResult);
        }

        [TestMethod()]
        public void stopWebTest()
        {
// Tests nothing because SessionManager.stopWeb() is a void Method
            Boolean bResult = true;
            if (bStartWeb == true) {
                SessionManager.stopWeb();
                bStartWeb = false;
            }
            Assert.IsTrue(bResult);
        }

        [TestMethod()]
        public void TerminateTest()
        {
            InitializeTest();
            // Tests nothing because SessionManager.terminate() is a void Method
            Boolean bResult = true;
            if (bSessionManagerStarted == true)
            {
                SessionManager.Terminate();
                bSessionManagerStarted = false;
            }
            Assert.IsTrue(bResult);
        }

        [TestMethod()]
        public void getTorrentStatusTest()
        {
            InitializeTest();
        }

        [TestMethod()]
        public void getTorrentFilesTest()
        {
            InitializeTest();
        }

        [TestMethod()]
        public void getTorrentStatusListTest()
        {
            InitializeTest();
        }

        [TestMethod()]
        public void addTorrentTest()
        {
            InitializeTest();
            string path = Path.GetDirectoryName(
                     Assembly.GetExecutingAssembly().Location);
            string path2 = Path.GetFullPath(path);
            path = Path.Combine(path2, TORRENT_FOLDER, TORRENT_LIBRE_OFFICE);
            if (File.Exists(path))
            {
                SessionManager.addTorrent(path);
            } else
            {
                Assert.Fail("File " + path + " does not exists");
            }
        }

        [TestMethod()]
        public void addTorrentTest1()
        {
            InitializeTest();
        }

        [TestMethod()]
        public void deleteTorrentTest()
        {

        }

        [TestMethod()]
        public void pauseTorrentTest()
        {

        }

        [TestMethod()]
        public void resumeTorrentTest()
        {

        }

        [TestMethod()]
        public void GiveMeStateFromEnumTest()
        {
            InitializeTest();
        }

        [TestMethod()]
        public void GiveMeStorageModeFromEnumTest()
        {
            InitializeTest();
        }
        private void setListeners()
        {
            SessionManager.TorrentAdded += new EventHandler<EventsArgs.OnTorrentAddedEventArgs>(AddTorrentListener);
            SessionManager.TorrentUpdated += new EventHandler<EventsArgs.OnTorrentUpdatedEventArgs>(UpdateTorrentListener);
//            SessionManager.TorrentRemoved += new EventHandler<EventsArgs.OnTorrentRemovedEventArgs>(RemovedTorrentListener);
//            SessionManager.SessionStatisticsUpdate += new EventHandler<EventsArgs.OnSessionStatisticsEventArgs>(UpdateFromSessionStatistics);
        }
        [TestMethod()]
        public void AddTorrentListener(object sender, EventsArgs.OnTorrentAddedEventArgs e)
        {
            TorrentItem item = new TorrentItem(e.QueuePosition, e.Name, e.Hash, 0, 0, e.Status, e.Progress, 0, 0, 0);
            torrentList.Add(item);
            listSize++;
            Assert.AreEqual(listSize, torrentList.Count);
        }
//        [TestMethod()]
        private void UpdateTorrentListener(object sender, EventsArgs.OnTorrentUpdatedEventArgs e)
        {
            TorrentItem item = torrentList.FirstOrDefault(o => o.Hash == e.InfoHash);
            item.DownloadRate = e.DownloadRate;
// TO DO: refresh all attributes 
            TorrentItem updatedItem = torrentList.FirstOrDefault(o => o.Hash == e.InfoHash);
            Assert.AreEqual(item, updatedItem);

        }
    }
}