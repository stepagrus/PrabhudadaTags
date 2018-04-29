using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.Model;

namespace UnitTests
{
    [TestClass]
    public class FilenameAssemblerTests
    {
        [TestMethod]
        public void DisassembleValidFileName()
        {
            var filename = "1973-08-30 #2 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом";
            FilenameAssembler asm = new FilenameAssembler(filename);

            Assert.AreEqual("1973-08-30", asm.Date);
            Assert.AreEqual("#2", asm.Number);
            Assert.AreEqual("Лондон", asm.City);
            Assert.AreEqual("Утренние прогулки", asm.Shastra);
            Assert.AreEqual("Продолжение беседы с Лоуренсом", asm.Title);
        }

        [TestMethod]
        public void DisassembleInvalidFileName()
        {
            var filename = "Govinda — Prabhupada singing";
            FilenameAssembler asm = new FilenameAssembler(filename);

            Assert.AreEqual(filename, asm.FileName);

            Assert.AreEqual(String.Empty, asm.Date);
            Assert.AreEqual(String.Empty, asm.Number);
            Assert.AreEqual(String.Empty, asm.City);
            Assert.AreEqual(String.Empty, asm.Shastra);
            Assert.AreEqual(String.Empty, asm.Title);

        }

        [TestMethod]
        public void Assemble()
        {
            var filename = "1973-08-30 #2 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом";
            FilenameAssembler asm = new FilenameAssembler(filename);

            asm.Number = "#7";

            string result = asm.FileName;
            Assert.AreEqual("1973-08-30 #7 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом", result);
        }
    }
}

