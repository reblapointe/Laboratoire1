﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CegepJonquiere.RebLapointe.Laboratoire1;

namespace CegepJonquiere.RebLapointe.Laboratoire1.Tests
{
    [TestClass()]
    public class FabriqueCircuitTests
    {
        [TestMethod()]
        public void FromStringTest()
        {
            string description = "(5,00 [4 (10 2,0)] 8,0)";
            IComposant c = FabriqueCircuit.FromString(description);
            c.MettreSousTension(9);
            Assert.AreEqual(16, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.5625, c.GetCourrant(), 0.001);
            Assert.AreEqual(9, c.GetTension(), 0.1);


            c = FabriqueCircuit.FromString("([100 250] [350//200,25])");
            c.MettreSousTension(9);
            Assert.AreEqual(198.802, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.04529, c.GetCourrant(), 0.001);
            Assert.AreEqual(9, c.GetTension(), 0.1);


            c = FabriqueCircuit.FromString("([([([([6 (2 10)] 8) 6] 4) 8] 4) 8]-6)");
            c.MettreSousTension(9);
            Assert.AreEqual(10, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.9, c.GetCourrant(), 0.01);
            Assert.AreEqual(9, c.GetTension(), 0.1);
        }

        [TestMethod]
        public void FromStringTest1()
        {
            String description = "(NNVNA [NNJNA (NBNNA NNRNA)] NNGNA)";
            IComposant c = FabriqueCircuit.FromString(description);
            c.MettreSousTension(9);
            Assert.AreEqual(16, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.5625, c.GetCourrant(), 0.001);
            Assert.AreEqual(9, c.GetTension(), 0.1);


            c = FabriqueCircuit.FromString("([BNNNA RVNNA] [OVNNA//RNNNA])");
            c.MettreSousTension(9);
            Assert.AreEqual(198.70, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.04529, c.GetCourrant(), 0.001);
            Assert.AreEqual(9, c.GetTension(), 0.1);

            c = FabriqueCircuit.FromString("([([([([NbNo (NRNo BNNo)] NGNo) NbNo] NNJNA) NGNA] NJNA) NNGNo]-NNbNA)");
            c.MettreSousTension(9);
            Assert.AreEqual(10, c.CalculerResistance(), 0.1);
            Assert.AreEqual(0.9, c.GetCourrant(), 0.01);
            Assert.AreEqual(9, c.GetTension(), 0.1);
        }

        [TestMethod()]
        public void FromStringIllegalTest()
        {
            Assert.ThrowsException<ArgumentException>(() => FabriqueCircuit.FromString(")"));
            Assert.ThrowsException<ArgumentException>(() => FabriqueCircuit.FromString("[)])"));
            Assert.ThrowsException<ArgumentException>(() => FabriqueCircuit.FromString("[)])])])"));
        }
    }
}