using MultiPrecision;

namespace ToHexcode {
    internal class Program {
        static void Main() {
            using StreamReader sr = new($"../../../../results/forward_intway_acc16.md");
            using StreamWriter sw = new($"../../../../results/forward_intway_acc16_haxcode.txt");

            sr.ReadLine();
            sr.ReadLine();

            int derivative = 1;

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] line_split = line.Split('|', StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();

                sw.WriteLine($"{{ {derivative}, new ReadOnlyCollection<ddouble>(new ddouble[]{{");

                for (int i = 0; i < line_split.Length; i++) {
                    string num = line_split[i];
                    string[] ss = num.Split('/');

                    MultiPrecision<Pow2.N8> m;

                    if (ss.Length == 1) {
                        m = ss[0];
                    }
                    else {
                        m = MultiPrecision<Pow2.N8>.Div(ss[0], ss[1]);
                    }

                    sw.WriteLine($"    {ToFP128(m)},");
                }

                sw.WriteLine("}) },");
                derivative++;
            }

            sw.Close();

            static string ToFP128<N>(MultiPrecision<N> x) where N : struct, IConstant {
                Sign sign = x.Sign;
                long exponent = x.Exponent;
                uint[] mantissa = x.Mantissa.Reverse().ToArray();

                string code = $"({(sign == Sign.Plus ? "+1" : "-1")}, {exponent}, 0x{mantissa[0]:X8}{mantissa[1]:X8}uL, 0x{mantissa[2]:X8}{mantissa[3]:X8}uL)";

                return code;
            }
        }
    }
}