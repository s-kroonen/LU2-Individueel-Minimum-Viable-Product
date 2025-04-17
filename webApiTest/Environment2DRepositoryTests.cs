using WebApi.api.Interfaces;
using WebApi.api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPITestsBrabant
{
    [TestClass]
    public sealed class Environment2DRepositoryTests
    {
        private Mock<IEnvironment2DRepository<Environment2D>> _mockRepository;
        private Environment2D _testEnvironment2D;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IEnvironment2DRepository<Environment2D>>();
            _testEnvironment2D = new Environment2D
            {
                Name = "Env 1",
            };
        }

        //[TestMethod]
        //public async Task InsertAsync_ShouldReturnInsertedDoctor()
        //{
        //    // Arrange
        //    _mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<Environment2D>())).ReturnsAsync(_testEnvironment2D);

        //    // Act
        //    var result = await _mockRepository.Object.InsertAsync(_testEnvironment2D);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(_testEnvironment2D.Name, result.Name);
        //}

        [TestMethod]
        public async Task ReadAsync_ById_ShouldReturnDoctor()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ReadAsync(It.IsAny<Guid>())).ReturnsAsync(_testEnvironment2D);

            // Act
            var result = await _mockRepository.Object.ReadAsync(_testEnvironment2D.id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testEnvironment2D.Name, result.Name);
        }

        [TestMethod]
        public async Task ReadAsync_ShouldReturnAllDoctors()
        {
            // Arrange
            var environments = new List<Environment2D> { _testEnvironment2D };
            _mockRepository.Setup(repo => repo.ReadAllAsync()).ReturnsAsync(environments);

            // Act
            var result = await _mockRepository.Object.ReadAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, ((List<Environment2D>)result).Count);
            Assert.AreEqual(_testEnvironment2D.id, ((List<Environment2D>)result)[0].id);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateDoctor()
        {
            // Arrange
            _testEnvironment2D.Name = "Dr. Jane Doe";
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Environment2D>())).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.UpdateAsync(_testEnvironment2D);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Environment2D>(d => d.Name == "Dr. Jane Doe")), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteDoctor()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.DeleteAsync(_testEnvironment2D.id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(It.Is<Guid>(id => id == _testEnvironment2D.id)), Times.Once);
        }
    }
}
