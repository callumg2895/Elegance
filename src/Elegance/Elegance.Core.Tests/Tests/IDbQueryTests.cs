﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// Test Method: Test001_WhenReadingResultFromRawSQL_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method, the results should match
        /// the results produced by the manually written reader code.
        /// </summary>
        [TestMethod]
        public void Test001_WhenReadingReferenceTypeResultFromRawSQL_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                GenerateTestEntity();
            }

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
        /// Test Method: Test002_WhenReadingResultFromRawSQLWithParameters_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method with parameters specified, the results 
        /// should match the results produced by the manually written reader code using the same parameters.
        /// </summary>
        [TestMethod]
        public void Test002_WhenReadingReferenceTypeResultFromRawSQLWithParameters_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var expectedResult = _testEntityARepository.GetByAllProperties_Standard(testEntity);
            var actualResult = _testEntityARepository.GetByAllProperties_Query(testEntity);

            // Assert
            Assert.AreEqual(expectedResult.GetType(), actualResult.GetType(), $"Expected type '{expectedResult.GetType().Name}', but got type '{actualResult.GetType().Name}'");
            Assert.IsTrue(TestEntityA.AreEqual(expectedResult, actualResult), "Expected entities to be equal");
        }

        /// <summary>
        /// Test Method: Test003_WhenReadingResultFromRawSQLWithInvalidParameters_ResultShouldBeNull:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method with invalid parameters specified,
        /// the result should be a null.
        /// </summary>
        [TestMethod]
        public void Test003_WhenReadingReferenceTypeResultFromRawSQLWithInvalidParameters_ResultShouldBeNull()
        {
            // Arrange
            var testEntity = GenerateTestEntity(insert: true);
            var testEntity2 = GenerateTestEntity(insert: false);

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var result = _testEntityARepository.GetByAllProperties_Query(testEntity2);

            // Assert
            Assert.IsNull(result, "Expected result to be null");
        }

        /// <summary>
        /// Test Method: Test004_WhenReadingScalarTypeResultFromRawSQL_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateScalarQuery method, the results should match
        /// the results produced by the manually written reader code.
        /// </summary>
        [TestMethod]
        public void Test004_WhenReadingScalarTypeResultFromRawSQL_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var expectedResult = _testEntityARepository.GetAllCount_Standard();
            var actualResult = _testEntityARepository.GetAllCount_Query();

            // Assert
            Assert.AreEqual(expectedResult, actualResult, $"Expected '{expectedResult}', but got '{actualResult}'");
        }

        /// <summary>
        /// Test Method: Test005_WhenReadingReferenceTypeResultFromStoredProcedureWithOutParameters_OutParametersShouldBeReturnedAlongsideResult:
        /// 
        /// When a stored procedure is passed to the CreateQuery method with out parameters specified, the values 
        /// of the out parameters should be accessible from the query object, as well as the results of the query 
        /// itself.
        /// </summary>
        [TestMethod]
        public void Test005_WhenReadingReferenceTypeResultFromStoredProcedureWithOutParameters_OutParametersShouldBeReturnedAlongsideResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var results = _testEntityARepository.GetAll_Query_StoredProcedure(out int status, out string message);

            // Assert
            Assert.AreEqual(0, status, $"Expected {nameof(status)} to be '{0}', but got '{status}'");
            Assert.AreEqual("success", message, $"Expected {nameof(message)} to be 'success', but got '{message}'");
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(TestEntityA.AreEqual(testEntity, results[0]), "Expected entities to be equal");
        }

        /// <summary>
        /// Test Method: Test006_WhenReadingReferenceTypeResultFromStoredProcedureWithInAndOutParameters_OutParametersShouldBeReturnedAlongsideResult:
        /// 
        /// When a stored procedure is passed to the CreateQuery method with in and out parameters specified, the 
        /// values of the out parameters should be accessible from the query object, as well as the results of the
        /// query itself.
        /// </summary>
        [TestMethod]
        public void Test006_WhenReadingReferenceTypeResultFromStoredProcedureWithInAndOutParameters_OutParametersShouldBeReturnedAlongsideResult()
        {
            // Arrange
            var testEntity = GenerateTestEntity();
            _testEntityARepository.InsertTestEntity(testEntity);

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            var result = _testEntityARepository.GetByAllProperties_Query_StoredProcedure(testEntity, out int status, out string message);

            // Assert
            Assert.AreEqual(0, status, $"Expected {nameof(status)} to be '{0}', but got '{status}'");
            Assert.AreEqual("success", message, $"Expected {nameof(message)} to be 'success', but got '{message}'");
            Assert.IsTrue(TestEntityA.AreEqual(testEntity, result), "Expected entities to be equal");
        }

        private TestEntityA GenerateTestEntity(bool insert = true)
        {
            seed++;

            var testEntity = new TestEntityA()
            {
                PropertyBigInt = seed,
                PropertyInt = seed,
                PropertySmallInt = (short)(seed % short.MaxValue),
                PropertyTinyInt = (byte)(seed % byte.MaxValue),
                PropertyVarChar = $"{seed}",
                PropertyDateTime = DateTime.Now.AddDays(seed)
            };

            if (insert)
            {
                _testEntityARepository.InsertTestEntity(testEntity);
            }

            return testEntity;
        }
    }
}
