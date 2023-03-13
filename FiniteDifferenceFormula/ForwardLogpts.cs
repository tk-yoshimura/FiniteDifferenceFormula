using System.Numerics;

namespace FiniteDifferenceFormula {
    internal class ForwardLogpts {
        static void Main_() {
            const int degree = 16;

            List<BigInteger> vs = new() { 0 };

            for (int i = 0; i < degree; i++) {
                vs.Add(BigInteger.One << i);
            }

            Dictionary<(int degree, int accuracy), Fraction[]> table_all = Coef.Generate(
                max_degree: degree, x0: 0, vs.Select((v) => new Fraction(v)).ToArray()
            );

            var table_maxacc = table_all.GroupBy(item => item.Key.degree).Select(item => item.OrderBy(v => v.Key.accuracy).Last()).ToList();

            using (StreamWriter sw = new($"../../../../results/forward_log2pts_acc{degree}.md")) {
                sw.Write("|derivative|accuracy|");
                for (int i = 0; i <= degree; i++) {
                    sw.Write($"{vs[i]}|");
                }
                sw.Write("\n|----|----|");
                for (int i = 0; i <= degree; i++) {
                    sw.Write("----|");
                }
                sw.Write("\n");

                foreach (((int degree, int accuracy) key, Fraction[] table) in table_maxacc) {
                    sw.Write($"|{key.degree}|{degree}|");

                    foreach (var f in table) {
                        sw.Write($"{f}|");
                    }

                    sw.Write("\n");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
