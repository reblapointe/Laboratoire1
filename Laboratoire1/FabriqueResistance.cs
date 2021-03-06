﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CegepJonquiere.RebLapointe.Laboratoire1
{
    public class FabriqueResistance
    {
        public const string PATRON_RESISTANCE = "[NBROJVbMGL]{2,3}[NBROJVbMoA][NBROJVbMoA]";
        
        public static Resistance FromCode(string code)
        {
            if (!Regex.Match(code, PATRON_RESISTANCE).Success)
                throw new ArgumentException("Pas un code couleur valide de résistance");

            double valeur = 0;
            int ind = 0;
            int mult = code.Length == 5 ? 100 : 10;

            valeur += CodeCouleur.ValueOf(Char.ToString(code[ind++])).PremiereBande * mult;
            valeur += CodeCouleur.ValueOf(Char.ToString(code[ind++])).DeuxiemeBande * (mult/10);
            valeur += code.Length == 5 ? CodeCouleur.ValueOf(Char.ToString(code[ind++])).TroisiemeBande : 0;
            valeur *= CodeCouleur.ValueOf(Char.ToString(code[ind++])).Multiplicateur;

            return new Resistance(valeur, CodeCouleur.ValueOf(Char.ToString(code[ind++])).Tolerance);
        }

        public static string Help() {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < CodeCouleur.Values().Count; i++)
                s.Append(CodeCouleur.Values()[i].Description).Append(i != CodeCouleur.Values().Count - 1 ? ", " : ".");
            return s.ToString();
        }
        
       
    }
}
