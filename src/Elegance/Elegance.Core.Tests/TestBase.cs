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
        protected static readonly TestEntityBRepository _testEntityBRepository;
        protected static readonly TestEntityCRepository _testEntityCRepository;
        protected static readonly TestEntityDRepository _testEntityDRepository;
        protected static readonly TestEntityERepository _testEntityERepository;

        static TestBase()
        {
            _cleanupActions = new Queue<Action>();

            _resourceRepository = new ResourceRepository();
            _testEntityARepository = new TestEntityARepository();
            _testEntityBRepository = new TestEntityBRepository();
            _testEntityCRepository = new TestEntityCRepository();
            _testEntityDRepository = new TestEntityDRepository();
            _testEntityERepository = new TestEntityERepository();
        }

        public TestBase()
        {

        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            _resourceRepository.LoadStaticData("CreateTestEntityATable");
            _resourceRepository.LoadStaticData("CreateTestEntityBTable");
            _resourceRepository.LoadStaticData("CreateTestEntityCTable");
            _resourceRepository.LoadStaticData("CreateTestEntityDTable");
            _resourceRepository.LoadStaticData("CreateTestEntityETable");
            _resourceRepository.LoadStoredProcedure("GetTestEntityAItems");
            _resourceRepository.LoadStoredProcedure("CreateTestEntityAItem");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _resourceRepository.UnloadStoredProcedures("GetTestEntityAItems");
            _resourceRepository.UnloadStoredProcedures("CreateTestEntityAItem");
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            
        }

        [TestCleanup]
        public virtual void TestCleanup()
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
