using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Elegance.Core.Tests.Data.TestEntities;
using Elegance.Core.Tests.Data.TestRepository;

namespace Elegance.Core.Tests.Tests
{
    [TestClass]
    public class IDbQueryTests : TestBase
    {
        /// <summary>
        /// Test Method: Test001_MatchesBehaviourOfBoilerplatCode
        /// </summary>
        [TestMethod]
        public void InsertAndReadSimpleEntity()
        {
            // Arrange

            // Act
            var expectedResults = _testEntityARepository.GetAll_Standard();
            var actualResults = _testEntityARepository.GetAll_Query();

            // Assert
            Assert.AreEqual(expectedResults.GetType(), actualResults.GetType(), $"Expected type '{expectedResults.GetType().Name}', but got type '{actualResults.GetType().Name}'");
            Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Expected {expectedResults.Count} results, but got {actualResults.Count}");

            for (int i = 0; i < expectedResults.Count; i++)
            {
                Assert.IsTrue(TestEntityA.AreEqual(expectedResults[i], actualResults[i]), "Expected entities to be equal");
            }
        }
    }
}
