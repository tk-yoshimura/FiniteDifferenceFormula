namespace FiniteDifferenceFormula {
    internal class CenteredIntpts {
        static void Main(string[] args) {
            const int degree = 16;

            List<int> vs = new() { 0 };

            for (int i = 1; i <= degree; i++) {
                vs.Add(i);
                vs.Add(-i);
            }

            Dictionary<(int degree, int accuracy), Fraction[]> table1 = Coef.Generate(
                max_degree: degree, x0: 0, vs.Select((v) => new Fraction(v)).ToArray()
            );

            using (StreamWriter sw = new($"../../../../results/centered_intway_acc{degree}.md")) {
                sw.Write("|derivative|accuracy|");
                for (int i = 0; i <= degree; i++) {
                    sw.Write($"{i}|");
                }
                sw.Write("\n|----|----|");
                for (int i = 0; i <= degree; i++) {
                    sw.Write("----|");
                }
                sw.Write("\n");

                foreach (Dictionary<(int degree, int accuracy), Fraction[]> table in new[] { table1 }) {
                    foreach (var key in table.Keys) {
                        if (key.accuracy != degree) {
                            continue;
                        }

                        sw.Write($"|{key.degree}|{key.accuracy}|");

                        Fraction[] fs = table[key];

                        Array.Sort(vs.ToArray(), fs);

                        foreach (var f in fs.Skip(fs.Length / 2)) {
                            sw.Write($"{f}|");
                        }

                        sw.Write("\n");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
