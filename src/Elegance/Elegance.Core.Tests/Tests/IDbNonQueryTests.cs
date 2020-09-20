using Elegance.Core.Tests.Data.TestEntities;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Tests.Tests
{
    [TestClass]
    public class IDbNonQueryTests : TestBase
    {
        private int seed;

        public IDbNonQueryTests()
            : base()
        {

        }

        /// <summary>
        /// Test Method: Test001_WhenExecutingValidSqlWithOutParameters__OutParametersShouldBeReturned
        /// 
        /// When a valid SQL statement is passed into the internal command with out parameters, out parameters should be returned.
        /// </summary>
        [TestMethod]
        public void Test001_WhenExecutingValidSqlWithOutParameters__OutParametersShouldBeReturned()
        {
            // Arrange
            var testEntity = GenerateTestEntity(insert: false);

            AddCleanupAction(() => { _testEntityARepository.DeleteTestEntities(); });

            // Act
            _testEntityARepository.InsertTestEntity_NonQuery_StoredProcedure(testEntity, out int status, out string message);

            // Assert
            Assert.AreEqual(0, status, $"Expected {nameof(status)} to be '{0}', but got '{status}'");
            Assert.AreEqual("success", message, $"Expected {nameof(message)} to be 'success', but got '{message}'");
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
                PropertyReal = seed / float.MaxValue,
                PropertyFloat = seed / double.MaxValue,
                PropertyDecimal = seed / decimal.MaxValue,
                PropertyVarChar = $"{seed}",
                PropertyDateTime = DateTime.Now.AddDays(seed)
            };

            if (insert)
            {
                _testEntityARepository.InsertTestEntity_Standard(testEntity);
            }

            return testEntity;
        }
    }
}
