using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.Model;

namespace UnitTests
{
    [TestClass]
    public class FilenameAssemblerTests
    {
        [TestMethod]
        public void Disassemble()
        {
            var filename = "1973-08-30 #2 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом";
            FilenameAssembler asm = FilenameAssembler.Disassemble(filename);

            Assert.AreEqual("1973-08-30", asm.Date);
            Assert.AreEqual("#2", asm.Number);
            Assert.AreEqual("Лондон", asm.City);
            Assert.AreEqual("Утренние прогулки", asm.Shastra);
            Assert.AreEqual("Продолжение беседы с Лоуренсом", asm.Title);
        }

        [TestMethod]
        public void Assemble()
        {
            var filename = "1973-08-30 #2 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом";
            FilenameAssembler asm = FilenameAssembler.Disassemble(filename);

            asm.Number = "#7";

            string result = asm.Assemble();
            Assert.AreEqual("1973-08-30 #7 — Лондон — Утренние прогулки — Продолжение беседы с Лоуренсом", result);
        }
    }
}

