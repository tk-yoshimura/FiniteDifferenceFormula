namespace FiniteDifferenceFormula {
    internal class ForwardIntpts {
        static void Main_() {
            const int degree = 16;

            List<int> vs = new() { 0 };

            for (int i = 1; i < degree * 2; i++) {
                vs.Add(i);
            }

            Dictionary<(int degree, int accuracy), Fraction[]> table1 = Coef.Generate(
                max_degree: degree, x0: 0, vs.Select((v) => new Fraction(v)).ToArray()
            );

            using (StreamWriter sw = new($"../../../../results/forward_intway_acc{degree}.md")) {
                sw.Write("|derivative|accuracy|");
                for (int i = 0; i < degree * 2; i++) {
                    sw.Write($"{i}|");
                }
                sw.Write("\n|----|----|");
                for (int i = 0; i < degree * 2; i++) {
                    sw.Write("----|");
                }
                sw.Write("\n");

                foreach (Dictionary<(int degree, int accuracy), Fraction[]> table in new[] { table1 }) {
                    foreach (var key in table.Keys) {
                        if (key.accuracy != degree + ((~key.degree) & 1)) {
                            continue;
                        }

                        sw.Write($"|{key.degree}|{degree}|");

                        Fraction[] fs = table[key];

                        Array.Sort(vs.ToArray(), fs);

                        foreach (var f in fs) {
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
