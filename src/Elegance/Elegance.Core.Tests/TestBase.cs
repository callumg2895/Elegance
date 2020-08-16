using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Elegance.Core.Tests.Data.TestRepository;

namespace Elegance.Core.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        protected static readonly string _connectionString;

        protected readonly TestEntityARepository _testEntityARepository;


        static TestBase()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"];
        }

        public TestBase()
        {
            _testEntityARepository = new TestEntityARepository();
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {

        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testEntityARepository.CreateTable();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _testEntityARepository.DropTable();
        }
    }
}
