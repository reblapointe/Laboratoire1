﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CegepJonquiere.RebLapointe.Laboratoire1
{
    public class FabriqueCircuit
    {
        public const char OUVERTURE_SERIE = '(';
        public const char FERMETURE_SERIE = ')';
        public const char OUVERTURE_PARALLELE = '[';
        public const char FERMETURE_PARALLELE = ']';
        public const char JETON = '0';

        public const string PATRON_DECIMALE = "\\d+(,\\d+)?" + "|" + FabriqueResistance.PATRON_RESISTANCE;
        

        public static string Help()
        {
            return FabriqueResistance.Help() + "\n" + OUVERTURE_SERIE + "Circuit en série" +
                    FERMETURE_SERIE + " " + OUVERTURE_PARALLELE + "Circuit en parallèle" + FERMETURE_PARALLELE;
        }

        public static Circuit FromString(string description)
        {
            // Lexer
            List<IComposant> jetons = new List<IComposant>();
            description = Lex(description, jetons);

            // Parser
            return Parse(description, jetons);
        }


        public static string Lex(string description, List<IComposant> jetons)
        {
            Match m = Regex.Match(description, PATRON_DECIMALE);
            while (m.Success)
            {
                string next = m.Value;
                jetons.Add(Double.TryParse(next, out double n) ? new Resistance(n) : FabriqueResistance.FromCode(next));
                m = m.NextMatch();
            }
            return Regex.Replace(description, PATRON_DECIMALE, "0");
        }

        private static Circuit Parse(string description, List<IComposant> jetons)
        {
            try
            {
                Stack<Circuit> composants = new Stack<Circuit>();
                Stack<char> delim = new Stack<char>();
                composants.Push(new CircuitSerie());
                for (int i = 0; i < description.Length; i++)
                    switch (description[i])
                    {
                        case OUVERTURE_SERIE:
                            composants.Push(new CircuitSerie());
                            delim.Push(OUVERTURE_SERIE);
                            break;
                        case OUVERTURE_PARALLELE:
                            composants.Push(new CircuitParallele());
                            delim.Push(OUVERTURE_PARALLELE);
                            break;
                        case JETON:
                            composants.Peek().AddSousCircuit(jetons.First<IComposant>());
                            jetons.RemoveAt(0);
                            break;
                        case FERMETURE_PARALLELE:
                            Circuit p = composants.Pop();
                            if (delim.Pop() != OUVERTURE_PARALLELE)
                                throw new ArgumentException("Mauvais parenthésage");
                            composants.Peek().AddSousCircuit(p);
                            break;
                        case FERMETURE_SERIE:
                            Circuit s = composants.Pop();
                            if (delim.Pop() != OUVERTURE_SERIE)
                                throw new ArgumentException("Mauvais parenthésage");
                            composants.Peek().AddSousCircuit(s);
                            break;
                    }
                return composants.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Mauvais parenthésage");
            }
        }
    }

}
