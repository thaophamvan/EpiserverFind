using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using EPiServer;
using EPiServer.Core;
using EPiServer.Events.Clients;
using EPiServer.Framework.Cache;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Electrolux.DataCache.Test
{
    [TestClass]
    public class DataCacheTest
    {
        private static IDataCacheHandler cache;

        private static Random rnd;

        private string key1;
        private string key2;
        private string key3;
        private string resource;
        private string o1;
        private string o2;
        private string o3;
        private string o4;
        private DateTime testStart;

        public DataCacheTest()
        {
            
        }

        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            rnd = new Random((int)DateTime.Now.Ticks);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            InitializationModule.FrameworkInitialization(HostType.TestFramework);
            // Create a mock repository
            var mockRepository = new Mock<IContentRepository>();
            var mocksynchronizedObjectInstanceCache = new Mock<ISynchronizedObjectInstanceCache>();
            var abc = new Mock<IEventRegistry>();
            var abc1 = new Mock<IObjectInstanceCache>();

            // Setup the repository to return a start page with a preset property value
            mockRepository.Setup(r => r.Get<HomePageForTest>(ContentReference.StartPage)).Returns(new HomePageForTest { CompanyName = "My company name" });

            // Create a mock service locator
            var mockLocator = new Mock<IServiceLocator>();

            // Setup the service locator to return our mock repository when an IContentRepository is requested
            mockLocator.Setup(l => l.GetInstance<IContentRepository>()).Returns(mockRepository.Object);
            mockLocator.Setup(c => c.GetInstance<IEventRegistry>()).Returns(abc.Object);
            mockLocator.Setup(c => c.GetInstance<IObjectInstanceCache>("RemoteCacheSynchronization")).Returns(abc1.Object);
            mockLocator.Setup(c => c.GetInstance<ISynchronizedObjectInstanceCache>()).Returns(mocksynchronizedObjectInstanceCache.Object);
            

            // Make use of our mock objects throughout EPiServer
            ServiceLocator.SetLocator(mockLocator.Object);

            cache = DataCacheHandler.Instance;
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 0, 100);
            cache.FailCountTimeout = new TimeSpan(0, 0, 0, 0, 100);
            cache.FailCountLimit = 3;
            var str = rnd.Next().ToString();
            key1 = "key1 " + str;
            key2 = "key2 " + str;
            key3 = "key3 " + str;
            resource = "resource " + str;
            o1 = "o1 " + key1;
            o2 = "o2 " + key1;
            o3 = "o3 " + key1;
            o4 = "o4 " + key1;
            testStart = DateTime.Now;

        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #endregion

        /// <summary>
        /// Tests that data can be stored and fetched from cache before expiration.
        /// </summary>
        [TestMethod]
        public void FetchResult()
        {
            var res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 1, 0, 0, 90000), resource);
            Thread.Sleep(20);
            var res2 = cache.ExecuteAndCache(key1, () => o2, o3, new TimeSpan(0, 1, 0, 0, 50), resource);
            Assert.AreSame(res1, o1, "First result is not fetched OK.");
            Assert.AreSame(res2, o1, "Second result is not from cache.");
        }

        /// <summary>
        /// Tests that data expires correctly and that new data is returned after expiration.
        /// </summary>
        [TestMethod]
        public void FetchResultAfterTimeout()
        {
            var res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            Thread.Sleep(600);
            var res2 = cache.ExecuteAndCache(key1, () => o2, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            Assert.AreSame(res1, o1);
            Assert.AreSame(res2, o2);
        }

        /// <summary>
        /// Tests that data expires correctly with CacheDependency object and that new data is 
        /// returned after expiration.
        /// </summary>
        [TestMethod]
        public void FetchResultAfterDependency()
        {
            List<string> cacheKeys = new List<string>(){ key2 };
            List<string> masterKeys = new List<string>() { key3 };
            var res1 = cache.ExecuteAndCache(key1, cacheKeys, masterKeys, () => o1, o4, new TimeSpan(0, 0, 0, 0, 5000), resource);
            Thread.Sleep(200);
            var res2 = cache.ExecuteAndCache(key2, cacheKeys, masterKeys, () => o2, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            //var resMaster = cache.ExecuteAndCache(key3, cacheKeys, masterKeys, () => o2, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            Thread.Sleep(1000);
            var res11 = cache.ExecuteAndCache(key1, cacheKeys, masterKeys, () => o3, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            // Invalid dependency
            var res3 = cache.ExecuteAndCache(key3, cacheKeys, masterKeys, () => o2, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            
            Thread.Sleep(200);
            var res4 = cache.ExecuteAndCache(key1, () => o3, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            Assert.AreSame(res1, o1, "First exec returned wrong data.");
            Assert.AreSame(res2, o1, "Cached data not returned.");
            Assert.AreSame(res3, o3, "Default data not returned after expire.");
        }

        private static string CreateTempFileWithContent(string content)
        {
            var fileName = Path.GetTempFileName();
            using (var textFile = new StreamWriter(fileName))
            {
                textFile.Write(content);
            }
            return fileName;
        }

        private static void DeleteTempFile(string fileName)
        {
            File.Delete(fileName);
        }

        /// <summary>
        /// 1. Consecutive execs that fails for a resource should automatically disables the resource.
        /// 2. A resource that is automatically disabled should be enbled after FailCountTimeout.
        /// </summary>
        [TestMethod]
        public void FetchDefaultResultAfterFail()
        {
            cache.FailCountTimeout = new TimeSpan(0, 0, 0, 0, 1000);
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 0, 200);

            // 1.
            // Time 0.000 s
            GenerateFail(resource, "Key for fail. " + key1, 3, cache.ExecTimeOut.Milliseconds * 2);

            // Time 1.200 s
            // Disable is true at this time. After third fail!
            Assert.AreEqual(3, cache.GetResourceFailCount(resource));
            Assert.IsTrue(cache.ResourceIsDisabled(resource));

            var res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            var res2 = cache.ExecuteAndCache(key2, () => o2, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            Assert.AreSame(res1, o3);
            Assert.AreSame(res2, o3);

            // 2.
            // Wait for fail to timeout
            Thread.Sleep(1400);

            // Time 2.600 s
            Assert.AreEqual(0, cache.GetResourceFailCount(resource));
            Assert.IsFalse(cache.ResourceIsDisabled(resource));

            res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            res2 = cache.ExecuteAndCache(key2, () => o2, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            Assert.AreSame(res1, o1);
            Assert.AreSame(res2, o2);
        }

        /// <summary>
        /// Consecutive execs that fails for a resource should automatically disables the resource.
        /// Old data should be returned from the cache if it exists.
        /// </summary>
        [TestMethod]
        public void FetchOldResultAfterFail()
        {
            cache.FailCountTimeout = new TimeSpan(0, 0, 0, 0, 1000);
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 0, 200);
            cache.OldDataMaxTimeout = new TimeSpan(0, 0, 0, 0, 3000);

            CheckTime(0);
            var res3 = cache.ExecuteAndCache(key1, () => o4, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            GenerateFail(resource, "Key for fail. " + key1, 3, cache.ExecTimeOut.Milliseconds * 2);

            // Disable is true at this time. After third fail!
            CheckTime(1200);
            Assert.AreEqual(3, cache.GetResourceFailCount(resource));
            Assert.IsTrue(cache.ResourceIsDisabled(resource));

            var res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            var res2 = cache.ExecuteAndCache(key2, () => o2, o3, new TimeSpan(0, 0, 0, 0, 50), resource);
            Assert.AreSame(res1, o4, "Old data is not returned for fail when data is in cache");
            Assert.AreSame(res2, o3, "Default data is not returned for fail when item not in cache.");
            Assert.AreSame(res3, o4);
        }

        private static void GenerateFail(string resource, string key, int noFails, int delayPerFailMs)
        {
            for (var i = 0; i < noFails; i++)
            {
                Thread.Sleep(delayPerFailMs);
                var res1 = cache.ExecuteAndCache(
                    key,
                    () =>
                    {
                        var x = int.Parse("This should fail");
                        return x;
                    },
                    new object(),
                    new TimeSpan(0, 0, 0, 0, 40),
                    resource);
            }
        }

        /// <summary>
        /// Tests that manual enable and disable works and that deafult data is returned when disabled.
        /// </summary>
        [TestMethod]
        public void FetchResultAfterDisableAndEnable()
        {
            var res1 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            cache.DisableResource(resource);
            var res2 = cache.ExecuteAndCache(key2, () => o2, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            var res3 = cache.ExecuteAndCache(key1, () => o1, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            Assert.AreSame(res1, o1, "Wrong data returned on first exec for key.");
            Assert.AreSame(res2, o3, "Default not returned when manually disabled for key not in cache.");
            Assert.AreSame(res3, o3, "Default not returned when manually disabled for key in cache.");

            cache.EnableResource(resource);
            cache.Remove(key2);
            res1 = cache.ExecuteAndCache(key2, () => o1, o3, new TimeSpan(0, 0, 0, 0, 500), resource);
            Assert.AreSame(res1, o1);
        }

        /// <summary>
        /// Multi threaded test where one thread makes en execute that is slow and
        /// the other thread fetches the same key. Default data should be returned while
        /// the slow thread is executing.
        /// </summary>
        [TestMethod]
        public void FetchDefaultResultSlowFirstThread()
        {
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 1);
            var result = new List<object>();

            var thr1 = ExecuteInNewThread(key1, o1, o3, 500, resource, result);
            thr1.Start();

            Thread.Sleep(200);  // Allow other thread to start

            var res2 = cache.ExecuteAndCache(key1, () => o2, o3, new TimeSpan(0, 0, 0, 0, 1000), resource);  // Returns default because other thread is executing.
            thr1.Join();  // Wait until thread has finished

            Assert.AreSame(result[0], o1, "Slow thread result not returned.");
            Assert.AreSame(res2, o3, "Default not returned while executing.");
        }

        /// <summary>
        /// Multi threaded test where one thread makes en execute that is slow and
        /// the other thread fetches the same key. Old data should be returned while
        /// the slow thread is executing.
        /// </summary>
        [TestMethod]
        public void FetchOldResultSlowFirstThread()
        {
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 1);
            var def = "def " + key1;
            var result = new List<object>();

            // Time 0.000 s
            var res1 = cache.ExecuteAndCache(key1, () => o1, def, new TimeSpan(0, 0, 0, 0, 500), resource);

            // Wait for data to expire, but still be present as old data
            Thread.Sleep(600);

            // Time 0.600 s
            var thr1 = ExecuteInNewThread(key1, o2, def, 500, resource, result);
            thr1.Start();

            Thread.Sleep(200);  // Allow other thread to start

            // Time 0.800 s
            var res3 = cache.ExecuteAndCache(key1, () => o3, def, new TimeSpan(0, 0, 0, 0, 1000), resource);  // Returns old data because other thread is executing.
            thr1.Join();  // Wait until thread has finished

            // Time 1.100 s
            var res2 = result[0];

            Assert.AreSame(o1, res1, "Exec result not returned from empty cache.");
            Assert.AreSame(o2, res2, "Exec result not returned from slow thread.");
            Assert.AreSame(o1, res3, "Old result not returned while executing.");

            Thread.Sleep(500);  // Wait for old data to be removed

            // Time 1.600 s
            var res4 = cache.ExecuteAndCache(key1, () => o3, def, new TimeSpan(0, 0, 0, 0, 1000), resource);

            Assert.AreSame(o2, res4, "Slow thread exec result not returned after thread finished.");
        }

        /// <summary>
        /// Multi threaded test where one thread makes an execute that is slow and
        /// the other thread fetches the same key. The other thread should wait for
        /// the slow thread to finish and use the slow threads result.
        /// </summary>
        [TestMethod]
        public void WaitForSlowFirstThreadResult()
        {
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 1);
            cache.MaxWaitForOtherThreadExec = new TimeSpan(0, 0, 0, 1, 0);
            var result = new List<object>();

            var thr1 = ExecuteInNewThread(key1, o1, o3, 500, resource, result);
            thr1.Start();

            Thread.Sleep(200);  // Allow other thread to start

            var res2 = cache.ExecuteAndCache(key1, () => o2, o3, new TimeSpan(0, 0, 0, 0, 1000), resource);
            thr1.Join();  // Wait until thread has finished

            Assert.AreSame(result[0], o1, "Slow thread result not returned.");
            Assert.AreSame(res2, o1, "Slow thread result not returned after waiting.");
        }

        /// <summary>
        /// Multi threaded test where one thread makes an execute that is slow and
        /// the other thread fetches the same key. The other thread should wait for
        /// the slow thread to finish and finally get a wait timeout and return default.
        /// </summary>
        [TestMethod]
        public void WaitForSlowFirstThreadResultTimeout()
        {
            cache.ExecTimeOut = new TimeSpan(0, 0, 0, 1);
            cache.MaxWaitForOtherThreadExec = new TimeSpan(0, 0, 0, 0, 500);
            var result = new List<object>();

            var thr1 = ExecuteInNewThread(key1, o1, o3, 1000, resource, result);
            thr1.Start();

            CheckTime(0);
            Thread.Sleep(200);  // Allow other thread to start

            CheckTime(200);
            var res2 = cache.ExecuteAndCache(key1, () => o2, o4, new TimeSpan(0, 0, 0, 0, 1000), resource);
            CheckTime(700);

            thr1.Join();  // Wait until thread has finished

            Assert.AreSame(result[0], o1, "Slow thread result not returned.");
            Assert.AreSame(res2, o4, "Default result not returned after wait timeout.");
        }

        private static Thread ExecuteInNewThread<TResult>(string key, TResult execResult, TResult defaultResult, int sleepMs, string resourceKey, List<object> result)
        {
            return new Thread(() => ThreadProc(
                () => cache.ExecuteAndCache(
                    key,
                    () =>
                    {
                        Thread.Sleep(sleepMs);
                        return execResult;
                    },
                    defaultResult,
                    new TimeSpan(0, 0, 0, 0, sleepMs * 2),
                    resourceKey),
                result));
        }

        private static void ThreadProc<TResult>(Func<TResult> methodToExecute, List<object> result)
        {
            result.Add(methodToExecute());
        }

        private void CheckTime(int expectedTimeMs)
        {
            CheckTime(expectedTimeMs, 50);
        }

        private void CheckTime(int expectedTimeMs, int maxDiffMs)
        {
            var elapsedTimeMs = DateTime.Now.Subtract(testStart).TotalMilliseconds;
            Assert.IsTrue(Math.Abs(elapsedTimeMs - expectedTimeMs) < maxDiffMs, "Expected elapsed time incorrect with over {0}ms. Expected: {1}, Actual: {2}", maxDiffMs, expectedTimeMs, elapsedTimeMs);
        }
    }
}

