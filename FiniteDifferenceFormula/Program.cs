using System;
using System.Collections.Generic;

namespace FiniteDifferenceFormula {
    internal class Program {
        static void Main(string[] args) {
            Dictionary<(int degree, int accuracy), Fraction[]> table1 = Coef.Generate(
                max_degree: 4, x0: 0,
                new Fraction[] { 
                    0, 1, -1, 2, -2, 3, -3, 4, -4 
                }
            );

            Dictionary<(int degree, int accuracy), Fraction[]> table2 = Coef.Generate(
                max_degree: 4, x0: 0,
                new Fraction[] { 
                    new Fraction(1, 2), new Fraction(-1, 2), new Fraction(3, 2), new Fraction(-3, 2), 
                    new Fraction(5, 2), new Fraction(-5, 2), new Fraction(7, 2), new Fraction(-7, 2) 
                }
            );

            Dictionary<(int degree, int accuracy), Fraction[]> table3 = Coef.Generate(
                max_degree: 4, x0: 0,
                new Fraction[] { 
                    0, 1, 2, 3, 4, 5, 6, 7, 8
                }
            );

            foreach (Dictionary<(int degree, int accuracy), Fraction[]> table in new[] { table1, table2, table3 }) {
                foreach (var key in table.Keys) {
                    Console.WriteLine(key);

                    foreach (var f in table[key]) {
                        Console.Write($"{f}, ");
                    }

                    Console.Write("\n\n");
                }

                Console.Write("\n");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
