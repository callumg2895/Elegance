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

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });
            AddCleanupAction(() => { _testEntityBRepository.DeleteTestEntities(); });
            AddCleanupAction(() => { _testEntityCRepository.DeleteTestEntities(); });
            AddCleanupAction(() => { _testEntityDRepository.DeleteTestEntities(); });
            AddCleanupAction(() => { _testEntityERepository.DeleteTestEntities(); });
        }

        /// <summary>
        /// Test Method: Test001a_WhenReadingReferenceTypeResultFromRawSQLAliased_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method, the results should match
        /// the results produced by the manually written reader code when the select statement is aliased.
        /// </summary>
        [TestMethod]
        public void Test001a_WhenReadingReferenceTypeResultFromRawSQLAliased_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                GenerateTestEntity();
            }

            // Act
            var expectedResults = _testEntityARepository.GetAll_Standard_Aliased();
            var actualResults = _testEntityARepository.GetAll_Query_Aliased();

            // Assert
            Assert.AreEqual(expectedResults.GetType(), actualResults.GetType(), $"Expected type '{expectedResults.GetType().Name}', but got type '{actualResults.GetType().Name}'");
            Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Expected {expectedResults.Count} results, but got {actualResults.Count}");

            for (int i = 0; i < expectedResults.Count; i++)
            {
                AssertAreEqual(expectedResults[i], actualResults[i]);
            }
        }

        /// <summary>
        /// Test Method: Test001b_WhenReadingReferenceTypeResultFromRawSQLNotAliased_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method, the results should match
        /// the results produced by the manually written reader code when the select statement is not aliased.
        /// </summary>
        [TestMethod]
        public void Test001b_WhenReadingReferenceTypeResultFromRawSQLNotAliased_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                GenerateTestEntity();
            }

            // Act
            var expectedResults = _testEntityARepository.GetAll_Standard_NonAliased();
            var actualResults = _testEntityARepository.GetAll_Query_NonAliased();

            // Assert
            Assert.AreEqual(expectedResults.GetType(), actualResults.GetType(), $"Expected type '{expectedResults.GetType().Name}', but got type '{actualResults.GetType().Name}'");
            Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Expected {expectedResults.Count} results, but got {actualResults.Count}");

            for (int i = 0; i < expectedResults.Count; i++)
            {
                AssertAreEqual(expectedResults[i], actualResults[i]);
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

            // Act
            var expectedResult = _testEntityARepository.GetByAllProperties_Standard(testEntity);
            var actualResult = _testEntityARepository.GetByAllProperties_Query(testEntity);

            // Assert
            Assert.AreEqual(expectedResult.GetType(), actualResult.GetType(), $"Expected type '{expectedResult.GetType().Name}', but got type '{actualResult.GetType().Name}'");
            AssertAreEqual(expectedResult, actualResult);
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

            // Act
            var results = _testEntityARepository.GetAll_Query_StoredProcedure(out int status, out string message);

            // Assert
            Assert.AreEqual(0, status, $"Expected {nameof(status)} to be '{0}', but got '{status}'");
            Assert.AreEqual("success", message, $"Expected {nameof(message)} to be 'success', but got '{message}'");
            Assert.AreEqual(1, results.Count);
            AssertAreEqual(testEntity, results[0]);
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
            _testEntityARepository.InsertTestEntity_Standard(testEntity);

            // Act
            var result = _testEntityARepository.GetByAllProperties_Query_StoredProcedure(testEntity, out int status, out string message);

            // Assert
            Assert.AreEqual(0, status, $"Expected {nameof(status)} to be '{0}', but got '{status}'");
            Assert.AreEqual("success", message, $"Expected {nameof(message)} to be 'success', but got '{message}'");
            AssertAreEqual(testEntity, result);
        }

        /// <summary>
        /// Test Method: Test007_WhenReadingReferenceTypeResultFromRawSQLWithReader_ResultShouldMatchStandardReaderResult:
        /// 
        /// When a raw SQL select statement is passed to the CreateQuery method, the reader should match
        /// the reader produced by the manually written command code.
        /// </summary>
        [TestMethod]
        public void Test007_WhenReadingReferenceTypeResultFromRawSQLWithReader_ResultShouldMatchStandardReaderResult()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
            {
                GenerateTestEntity();
            }

            // Act
            var expectedResults = _testEntityARepository.GetAll_Standard_Aliased();
            var actualResults = _testEntityARepository.GetAll_Query_Reader();

            // Assert
            Assert.AreEqual(expectedResults.GetType(), actualResults.GetType(), $"Expected type '{expectedResults.GetType().Name}', but got type '{actualResults.GetType().Name}'");
            Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Expected {expectedResults.Count} results, but got {actualResults.Count}");

            for (int i = 0; i < expectedResults.Count; i++)
            {
                AssertAreEqual(expectedResults[i], actualResults[i]);
            }
        }

        private TestEntityA GenerateTestEntity(bool insert = true)
        {
            seed++;

            var testEntityB = new TestEntityB()
            {
                PropertyBigInt = seed,
            };

            var testEntityC = new TestEntityC()
            {
                PropertyBigInt = seed,
            };

            var testEntityA = new TestEntityA()
            {
                PropertyBigInt = seed,
                PropertyInt = seed,
                PropertySmallInt = (short)(seed % short.MaxValue),
                PropertyTinyInt = (byte)(seed % byte.MaxValue),
                PropertyReal = seed / float.MaxValue,
                PropertyFloat = seed / double.MaxValue,
                PropertyDecimal = seed / decimal.MaxValue,
                PropertyVarChar = $"{seed}",
                PropertyDateTime = DateTime.Now.AddDays(seed),

                PropertyNullableBigInt = seed,
                PropertyNullableInt = seed,
                PropertyNullableSmallInt = (short)(seed % short.MaxValue),
                PropertyNullableTinyInt = (byte)(seed % byte.MaxValue),
                PropertyNullableReal = seed / float.MaxValue,
                PropertyNullableFloat = seed / double.MaxValue,
                PropertyNullableDecimal = seed / decimal.MaxValue,
                PropertyNullableDateTime = DateTime.Now.AddDays(seed),

                TestEntityB = testEntityB,
                TestEntityC = testEntityC,

                TestEntityDList = new List<TestEntityD>(),
                TestEntityEList = new List<TestEntityE>(),
            };

            if (seed % 2 == 0)
            {
                testEntityA.PropertyNullableBigInt = null;
                testEntityA.PropertyNullableInt = null;
                testEntityA.PropertyNullableSmallInt = null;
                testEntityA.PropertyNullableTinyInt = null;
                testEntityA.PropertyNullableReal = null;
                testEntityA.PropertyNullableFloat = null;
                testEntityA.PropertyNullableDecimal = null;
                testEntityA.PropertyNullableDateTime = null;
            }
            else
            {
                testEntityA.PropertyNullableBigInt = seed;
                testEntityA.PropertyNullableInt = seed;
                testEntityA.PropertyNullableSmallInt = (short)(seed % short.MaxValue);
                testEntityA.PropertyNullableTinyInt = (byte)(seed % byte.MaxValue);
                testEntityA.PropertyNullableReal = seed / float.MaxValue;
                testEntityA.PropertyNullableFloat = seed / double.MaxValue;
                testEntityA.PropertyNullableDecimal = seed / decimal.MaxValue;
                testEntityA.PropertyNullableDateTime = DateTime.Now.AddDays(seed);
            }

            for (int i = 0; i < 5; i++)
            {
                var testEntityD = new TestEntityD()
                {
                    PropertyBigInt = seed,
                    PropertyVarChar = $"{seed}_{i}",
                };

                var testEntityE = new TestEntityE()
                {
                    PropertyBigInt = seed,
                    PropertyVarChar = $"{seed}_{i}",
                };

                testEntityA.TestEntityDList.Add(testEntityD);
                testEntityA.TestEntityEList.Add(testEntityE);

                if (insert)
                {
                    _testEntityERepository.InsertTestEntity_Standard(testEntityE);
                    _testEntityDRepository.InsertTestEntity_Standard(testEntityD);
                }
            }

            if (insert)
            {
                _testEntityCRepository.InsertTestEntity_Standard(testEntityC);
                _testEntityBRepository.InsertTestEntity_Standard(testEntityB);
                _testEntityARepository.InsertTestEntity_Standard(testEntityA);
            }

            return testEntityA;
        }

        private void AssertAreEqual(TestEntityA expected, TestEntityA actual)
        {
            Assert.AreEqual(expected.PropertyBigInt, actual.PropertyBigInt, $"Expected {expected.PropertyBigInt}, but got {actual.PropertyBigInt} for {nameof(TestEntityA.PropertyBigInt)}");
            Assert.AreEqual(expected.PropertyInt, actual.PropertyInt, $"Expected {expected.PropertyInt}, but got {actual.PropertyInt} for {nameof(TestEntityA.PropertyInt)}");
            Assert.AreEqual(expected.PropertySmallInt, actual.PropertySmallInt, $"Expected {expected.PropertySmallInt}, but got {actual.PropertySmallInt} for {nameof(TestEntityA.PropertySmallInt)}");
            Assert.AreEqual(expected.PropertyTinyInt, actual.PropertyTinyInt, $"Expected {expected.PropertyTinyInt}, but got {actual.PropertyTinyInt} for {nameof(TestEntityA.PropertyTinyInt)}");
            Assert.AreEqual(expected.PropertyReal, actual.PropertyReal, $"Expected {expected.PropertyReal}, but got {actual.PropertyReal} for {nameof(TestEntityA.PropertyReal)}");
            Assert.AreEqual(expected.PropertyFloat, actual.PropertyFloat, $"Expected {expected.PropertyFloat}, but got {actual.PropertyFloat} for {nameof(TestEntityA.PropertyFloat)}");
            Assert.AreEqual(expected.PropertyDecimal, actual.PropertyDecimal, $"Expected {expected.PropertyDecimal}, but got {actual.PropertyDecimal} for {nameof(TestEntityA.PropertyDecimal)}");
            Assert.AreEqual(expected.PropertyVarChar, actual.PropertyVarChar, $"Expected {expected.PropertyVarChar}, but got {actual.PropertyVarChar} for {nameof(TestEntityA.PropertyVarChar)}");
            Assert.AreEqual(expected.PropertyDateTime, actual.PropertyDateTime, $"Expected {expected.PropertyDateTime}, but got {actual.PropertyDateTime} for {nameof(TestEntityA.PropertyDateTime)}");
            Assert.AreEqual(expected.PropertyEnum, actual.PropertyEnum, $"Expected {expected.PropertyEnum}, but got {actual.PropertyEnum} for {nameof(TestEntityA.PropertyEnum)}");

            Assert.AreEqual(expected.PropertyNullableBigInt, actual.PropertyNullableBigInt, $"Expected {expected.PropertyNullableBigInt}, but got {actual.PropertyNullableBigInt} for {nameof(TestEntityA.PropertyNullableBigInt)}");
            Assert.AreEqual(expected.PropertyNullableInt, actual.PropertyNullableInt, $"Expected {expected.PropertyNullableInt}, but got {actual.PropertyNullableInt} for {nameof(TestEntityA.PropertyNullableInt)}");
            Assert.AreEqual(expected.PropertyNullableSmallInt, actual.PropertyNullableSmallInt, $"Expected {expected.PropertyNullableSmallInt}, but got {actual.PropertyNullableSmallInt} for {nameof(TestEntityA.PropertyNullableSmallInt)}");
            Assert.AreEqual(expected.PropertyNullableTinyInt, actual.PropertyNullableTinyInt, $"Expected {expected.PropertyNullableTinyInt}, but got {actual.PropertyNullableTinyInt} for {nameof(TestEntityA.PropertyNullableTinyInt)}");
            Assert.AreEqual(expected.PropertyNullableReal, actual.PropertyNullableReal, $"Expected {expected.PropertyNullableReal}, but got {actual.PropertyNullableReal} for {nameof(TestEntityA.PropertyNullableReal)}");
            Assert.AreEqual(expected.PropertyNullableFloat, actual.PropertyNullableFloat, $"Expected {expected.PropertyNullableFloat}, but got {actual.PropertyNullableFloat} for {nameof(TestEntityA.PropertyNullableFloat)}");
            Assert.AreEqual(expected.PropertyNullableDecimal, actual.PropertyNullableDecimal, $"Expected {expected.PropertyNullableDecimal}, but got {actual.PropertyNullableDecimal} for {nameof(TestEntityA.PropertyNullableDecimal)}");
            Assert.AreEqual(expected.PropertyNullableDateTime, actual.PropertyNullableDateTime, $"Expected {expected.PropertyNullableDateTime}, but got {actual.PropertyNullableDateTime} for {nameof(TestEntityA.PropertyNullableDateTime)}");
            Assert.AreEqual(expected.PropertyNullableEnum, actual.PropertyNullableEnum, $"Expected {expected.PropertyNullableEnum}, but got {actual.PropertyNullableEnum} for {nameof(TestEntityA.PropertyNullableEnum)}");

            AssertAreEqual(expected.TestEntityB, actual.TestEntityB);
            AssertAreEqual(expected.TestEntityC, actual.TestEntityC);

            Assert.AreEqual(expected.TestEntityDList?.Count, actual.TestEntityDList?.Count);
            Assert.AreEqual(expected.TestEntityEList?.Count, actual.TestEntityEList?.Count);

            if (expected.TestEntityDList != null)
            {
                for (int i = 0; i < expected.TestEntityDList.Count; i++)
                {
                    AssertAreEqual(expected.TestEntityDList[i], actual.TestEntityDList[i]);
                }
            }

            if (expected.TestEntityEList != null)
            {
                for (int i = 0; i < expected.TestEntityEList.Count; i++)
                {
                    AssertAreEqual(expected.TestEntityEList[i], actual.TestEntityEList[i]);
                }
            }
        }

        private void AssertAreEqual(TestEntityB expected, TestEntityB actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, $"Expected actual value to be null");
            }
            else
            {
                Assert.AreEqual(expected.PropertyBigInt, actual.PropertyBigInt, $"Expected {expected.PropertyBigInt}, but got {actual.PropertyBigInt} for {nameof(TestEntityB.PropertyBigInt)}");
            }
        }

        private void AssertAreEqual(TestEntityC expected, TestEntityC actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, $"Expected actual value to be null");
            }
            else
            {
                Assert.AreEqual(expected.PropertyBigInt, actual.PropertyBigInt, $"Expected {expected.PropertyBigInt}, but got {actual.PropertyBigInt} for {nameof(TestEntityC.PropertyBigInt)}");
            }
        }

        private void AssertAreEqual(TestEntityD expected, TestEntityD actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, $"Expected actual value to be null");
            }
            else
            {
                Assert.AreEqual(expected.PropertyBigInt, actual.PropertyBigInt, $"Expected {expected.PropertyBigInt}, but got {actual.PropertyBigInt} for {nameof(TestEntityD.PropertyBigInt)}");
                Assert.AreEqual(expected.PropertyVarChar, actual.PropertyVarChar, $"Expected {expected.PropertyVarChar}, but got {actual.PropertyVarChar} for {nameof(TestEntityD.PropertyVarChar)}");
            }
        }

        private void AssertAreEqual(TestEntityE expected, TestEntityE actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, $"Expected actual value to be null");
            }
            else
            {
                Assert.AreEqual(expected.PropertyBigInt, actual.PropertyBigInt, $"Expected {expected.PropertyBigInt}, but got {actual.PropertyBigInt} for {nameof(TestEntityE.PropertyBigInt)}");
                Assert.AreEqual(expected.PropertyVarChar, actual.PropertyVarChar, $"Expected {expected.PropertyVarChar}, but got {actual.PropertyVarChar} for {nameof(TestEntityE.PropertyVarChar)}");
            }
        }
    }
}
