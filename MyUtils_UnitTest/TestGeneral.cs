﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUtils_UnitTest
{
    [TestClass]
    public class TestGeneral
    {
        [TestMethod]
        public void TestCMD()
        {
            //Arrange
            var fileName = @"C:\NT_Sq\UtilinetSBSConsole\UtilinetSBSConsole.exe";
            var arguments = @"[Operational,1]";
            int resultExpected = -1;
            //Act
            var result = MyUtilis.CMD.Run(fileName, arguments);
            //Assert
            Assert.AreEqual(resultExpected, result);
        }

        [TestMethod]
        public void TestGetPhotoUser()
        {
            //Arrange
            var userName = @"";
            //Act
            var result = MyUtilis.Windows.Login.GetUserPhoto(userName);
            //Assert
            Assert.AreNotEqual(null, result);
        }
    }
}
