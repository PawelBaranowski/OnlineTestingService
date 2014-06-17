using OnlineTestingService.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OnlineTestingServiceTests
{
    
    
    /// <summary>
    ///This is a test class for PasswordGeneratorTest and is intended
    ///to contain all PasswordGeneratorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PasswordGeneratorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for MakePassword
        ///</summary>
        [TestMethod()]
        public void MakePasswordTest()
        {
            string expected = string.Empty;
            string actual;
            Assert.AreEqual(expected, string.Empty);
            actual = PasswordGenerator.MakePassword();
            Assert.AreNotEqual(actual, string.Empty);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for Generate
        ///</summary>
        [TestMethod()]
        public void GenerateTest()
        {
            int length = 8; 
            string expected = string.Empty; 
            string actual;
            actual = PasswordGenerator.Generate(length);
            Assert.AreNotEqual(expected, actual);
            Assert.IsTrue(actual.Length == length);
        }

        /// <summary>
        ///A test for Generate
        ///</summary>
        [TestMethod()]
        public void GenerateTest1()
        {
            int minLength = 8;
            int maxLength = 10; 
            string expected = string.Empty; 
            string actual;
            actual = PasswordGenerator.Generate(minLength, maxLength);
            Assert.AreNotEqual(expected, actual);
            Assert.IsTrue(actual.Length <= maxLength && actual.Length >= minLength);
        }

        /// <summary>
        ///A test for PasswordGenerator Constructor
        ///</summary>
        [TestMethod()]
        public void PasswordGeneratorConstructorTest()
        {
            PasswordGenerator target = new PasswordGenerator();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for checkPassword
        ///</summary>
        [TestMethod()]
        public void checkPasswordTest()
        {
            string password = "passtest2"; 
            string hashPassword1 = "35b7967901948f9dc408af213ff37f4e";
            string hashPassword2 = "15b1111111111111c408af213ff37f41";
            bool expected1 = true;
            bool expected2 = false;
            bool actual1 = PasswordGenerator.checkPassword(password, hashPassword1);
            bool actual2 = PasswordGenerator.checkPassword(password, hashPassword2);
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        /// <summary>
        ///A test for encodeToMD5
        ///</summary>
        [TestMethod()]
        public void encodeToMD5Test()
        {
            string password = "passtest";
            string expected = "2f3bc18c0d3e6b1b8a445075535d26e9"; // TODO: Initialize to an appropriate value
            string actual;
            actual = PasswordGenerator.encodeToMD5(password);
            Assert.AreEqual(expected, actual);
        }
    }
}
