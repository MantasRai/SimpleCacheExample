using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using CacheSimpleExample.Controllers;
using CacheSimpleExample.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CacheSimpleExampleTests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void TestThatBySecondFetchDataIsObtainedFromCache()
        {
            //Arange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var dataAccessLayer = fixture.Freeze<Mock<IDAL>>();
            dataAccessLayer.Setup(x => x.GetDateTimeFromSql()).Returns(DateTime.Now.ToString);

            var sut = fixture.Create<Cache>();

            //Act
            var result = sut.GetCachedData();
            var secondResult = sut.GetCachedData();

            //Assert
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(result, secondResult);
            dataAccessLayer.Verify(x => x.GetDateTimeFromSql(), Times.Once);
        }
    }
}
