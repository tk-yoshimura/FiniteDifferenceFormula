using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Reference:
// Bengt Fornberg, Generation of Finite Difference Formulas of Arbitrarily Spaced Grids
// Mathematics of Computaion, vol.51, no.184, (1988)

namespace FiniteDifferenceFormula {
    internal static class Coef {
        public static Dictionary<(int degree, int accuracy), Fraction[]> Generate(int max_degree, Fraction x0, IReadOnlyList<Fraction> points) {
            int m = max_degree, n = points.Count - 1;
        
            Fraction c1 = 1;
            Dictionary<(int accuracy, int point_index), Fraction[]> delta = new();
            delta.Add((0, 0), new Fraction[n + 1]);
            delta[(0, 0)][0] = 1;
        
            for (int i = 1; i <= n; i++) {
                Fraction c2 = 1;
        
                for (int j = 0; j < i; j++) {
                    delta.Add((i, j), new Fraction[n + 1]);
        
                    Fraction c3 = points[i] - points[j];
                    c2 *= c3;
        
                    if (i <= m) {
                        delta[(i - 1, j)][i] = 0;
                    }
        
                    delta[(i, j)][0] = ((points[i] - x0) * delta[(i - 1, j)][0]) / c3;
                    for (int k = 1; k <= Math.Min(i, m); k++) {
                        delta[(i, j)][k] = ((points[i] - x0) * delta[(i - 1, j)][k] - k * delta[(i - 1, j)][k - 1]) / c3;
                    }
                }
        
                delta.Add((i, i), new Fraction[n + 1]);
        
                delta[(i, i)][0] = c1 / c2 * ((x0 - points[i - 1]) * delta[(i - 1, i - 1)][0]);
                for (int k = 1; k <= Math.Min(i, m); k++) {
                    delta[(i, i)][k] = c1 / c2 * (k * delta[(i - 1, i - 1)][k - 1] + (x0 - points[i - 1]) * delta[(i - 1, i - 1)][k]);
                }
        
                c1 = c2;
            }

            Dictionary<(int degree, int accuracy), Fraction[]> table = new();

            for (int j = 1; j <= max_degree; j++) {
                for (int i = j; i <= max_degree * 2; i++) { 
                    int degree = j;
                    int accuracy = i - j + (((degree & 1) == 0) ? 2 : 1);

                    Fraction[] fs = new Fraction[points.Count];
                    Fraction s = 0;
                    bool is_set = false;

                    for (int k = 0; k < points.Count; k++) {
                        fs[k] = (delta.ContainsKey((i, k))) ? delta[(i, k)][j] : null;
                        if (fs[k] is null) {
                            fs[k] = 0;
                        }
                        else if (fs[k].Numer != 0) {
                            s += fs[k];
                            is_set = true;
                        }
                    }

                    if (is_set && s.Numer == 0) {
                        table.Add((degree, accuracy), fs);
                    }
                }
            }
        
            return table;
        }
    }
}
