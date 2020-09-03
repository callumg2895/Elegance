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
        private int seed;

        public IDbQueryTests()
            : base()
        {

        }

        /// <summary>
        /// Test Method: Test001_WhenReadingResultFromRawSQL_ResultShouldMatchStandardReaderResult
        /// 
        /// When a raw SQL select statement is passed to the Query class, the results should match
        /// the results produced by the manually written reader code.
        /// </summary>
        [TestMethod]
        public void Test001_WhenReadingResultFromRawSQL_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();
            _testEntityARepository.InsertTestEntity(testEntity);

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

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

        /// <summary>
        /// Test Method: Test002_WhenReadingResultFromRawSQLWithParameters_ResultShouldMatchStandardReaderResult
        /// 
        /// When a raw SQL select statement is passed to the Query class with parameters specified, the results 
        /// should match the results produced by the manually written reader code using the same parameters.
        /// </summary>
        [TestMethod]
        public void Test002_WhenReadingResultFromRawSQLWithParameters_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();
            _testEntityARepository.InsertTestEntity(testEntity);

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var expectedResult = _testEntityARepository.GetByAllProperties_Standard(testEntity);
            var actualResult = _testEntityARepository.GetByAllProperties_Query(testEntity);

            // Assert
            Assert.AreEqual(expectedResult.GetType(), actualResult.GetType(), $"Expected type '{expectedResult.GetType().Name}', but got type '{actualResult.GetType().Name}'");
            Assert.IsTrue(TestEntityA.AreEqual(expectedResult, actualResult), "Expected entities to be equal");
        }

        private TestEntityA GenerateTestEntity()
        {
            seed++;

            return new TestEntityA()
            {
                PropertyBigInt = seed,
                PropertyInt = seed,
                PropertySmallInt = (short)(seed % short.MaxValue),
                PropertyTinyInt = (byte)(seed % byte.MaxValue),
                PropertyVarChar = $"{seed}",
                PropertyDateTime = DateTime.Now.AddDays(seed)
            };
        }
    }
}
