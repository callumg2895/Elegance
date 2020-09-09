using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Elegance.Core.Tests.Data.TestRepository;
using System.IO;
using System.Reflection;

namespace Elegance.Core.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private static readonly Queue<Action> _cleanupActions;

        protected static readonly ResourceRepository _resourceRepository;
        protected static readonly TestEntityARepository _testEntityARepository;

        static TestBase()
        {
            _cleanupActions = new Queue<Action>();

            _resourceRepository = new ResourceRepository();
            _testEntityARepository = new TestEntityARepository();
        }

        public TestBase()
        {

        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            _testEntityARepository.CreateTable();
            _resourceRepository.LoadStoredProcedure("GetTestEntityAItems");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _resourceRepository.UnloadStoredProcedures("GetTestEntityAItems");
            _testEntityARepository.DropTable();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestCleanup]
        public void TestCleanup()
        {
            while (_cleanupActions.TryDequeue(out Action action))
            {
                action();
            }
        }

        protected void AddCleanupAction(Action action)
        {
            _cleanupActions.Enqueue(action);
        }
    }
}
