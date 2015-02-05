using NUnit.Framework;
using System;
using System.IO;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using System.Threading;
using System.Linq;
using Xamarin.UITest.Queries;

namespace SearchableDictionaryTest
{
    [TestFixture()]
    public class SearchableDictionaryTest
    {
        private AndroidApp app;

        [SetUp()]
        public void SetUpTest()
        {
            string strApkPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            app = ConfigureApp.Android.ApkFile (strApkPath+"/bin/Release/SearchableDictionary.SearchableDictionary-Signed.apk").StartApp ();
        }

        [Test()]
        public void Search_Exists_Word()
        {
            app.WaitForElement(c => c.Text("Use the search box in the Action Bar to look up a word"),
                "Time out on application Initilized",
                TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
            app.EnterText(c => c.Id("search_src_text"), "mag");
            //Note: 10 second interval for display word in the list.
            Thread.Sleep(10000);
            app.Tap(c=>c.Id("search_src_text"));
            app.WaitForElement(c => c.Text("magnificent"),"Waiting for word list",TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
            app.Tap(c => c.Text("magnificent"));

            app.WaitForElement(c => c.Id("word"), "Waiting for word description",TimeSpan.FromSeconds(30));
            Assert.That(app.Query(c => c.Id("word")).Any());
            // Assert.AreEqual();
        }

        [Test()]
        public void Search_NonExists_Word()
        {
           
            app.WaitForElement(c => c.Text("Use the search box in the Action Bar to look up a word"),
                "Time out on application Initilized", TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
            app.EnterText(c => c.Id("search_src_text"), "magi");
            //Note: 10 second interval for display suggestion word int the list.
            Thread.Sleep(10000);
            app.Tap(c => c.Id("search_src_text"));
            bool valuesExitsOrNot = CheckValueExitsOrNotInDictonary();
            Assert.IsFalse(valuesExitsOrNot);
        }


        /// <summary>
        /// This function is used to check that the value exits or not in dictonary.
        /// </summary>
        /// <returns><c>true</c>, if value exits in dictonary <c>false</c> otherwise.</returns>

        public bool CheckValueExitsOrNotInDictonary()
        {
            bool valueExits=true;
            try
            {
                Assert.That(app.Query(c => c.Id("word")).Any());
            }
            catch(Exception ex)
            {
                valueExits=false;
            }
            return valueExits;
        }

        [TearDown()]
        public void TearDownTest()
        {
            app = null;
        }
    }
}

