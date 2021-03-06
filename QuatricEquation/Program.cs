using System;
using System.Collections.Generic;

namespace QuatricEquation
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 0, b, c, d, e, f, g, h;

            //wprowadzanie danych
            Console.WriteLine("Algorytm rozwiązuje równanie czwartego stopnia: ax^4 + bx ^ 3 + cx ^ 2 + dx + e = 0");

            while (a == 0)
            {
                Console.WriteLine("Wpisz a różne od 0");
                a = Double.Parse(Console.ReadLine());
            }

            Console.WriteLine("Wpisz b");
            b = Double.Parse(Console.ReadLine());
            Console.WriteLine("Wpisz c");
            c = Double.Parse(Console.ReadLine());
            Console.WriteLine("Wpisz d");
            d = Double.Parse(Console.ReadLine());
            Console.WriteLine("Wpisz e");
            e = Double.Parse(Console.ReadLine());

            //obliczanie f,g,h
            f = c / a - 3 * b * b / 8 * a * a;
            g = d / a + b * b * b / 8 * a * a * a - b * c / 2 * a * a;
            h = e / a - 3 * b * b * b * b / 256 * a * a * a * a + b * b * c / 16 * a * a * a - b * d / 4 * a * a;

            //obliczanie pomocniczego równania szesciennego
            double hA = 1, hB = f/2, hC = (f*f-4*h)/16, hD = g*g/64;

            double hW = -(hB / 3) * hA;
            double hP = (3 * hA * hW * hW + 2 * hB * hW + hC) / hA;
            double hQ = (hA * hW * hW * hW + hB * hW * hW + hC * hW + hD) / hA;
            double delta = hQ * hQ / 4 + hP * hP * hP / 27;

            string y1, y2;
            if (delta > 0)
            {
                double xR, xIm, u, v;

                u = Math.Cbrt(-hQ / 2 + Math.Sqrt(delta));
                v = Math.Cbrt(-hQ / 2 - Math.Sqrt(delta));
                xR = -1 / 2 * (u + v) + hW;
                xIm = Math.Sqrt(3) / 2 * (u - v);

                y1 = xR.ToString() + " + " + xIm.ToString() + "i";
                y2 = xR.ToString() + " - " + xIm.ToString() + "i";

                Console.WriteLine($" Delta równania pomocniczego > 0 - dwa zespolone pierwiastki sprzężone: {y1}, {y2}");
            }
            else if (delta < 0)
            {
                double x1, x2, x3, fi;
                fi = Math.Acos((3 * hQ) / (2 * hP * Math.Sqrt(-hP / 3)));

                x1 = hW + 2 * Math.Sqrt(-hP / 3.0) * Math.Cos(fi / 3);
                x2 = hW + 2 * Math.Sqrt(-hP / 3.0) * Math.Cos(fi / 3.0 + 2.0 / 3.0 * 3.141592);
                x3 = hW + 2 * Math.Sqrt(-hP / 3.0) * Math.Cos(fi / 3.0 + 4.0 / 3.0 * 3.141592);

                Console.WriteLine($"Delta równania pomocniczego < 0 trzy pierwiastki rzeczywiste: {x1}, {x2}, {x3}");

                if (x1 != 0 && x2 !=0) { y1 = x1.ToString(); y2 = x2.ToString(); }
                else if (x1 != 0 && x3 !=0) { y1 = x1.ToString(); y2 = x3.ToString(); }
                else { y1 = x3.ToString(); y2 = x2.ToString(); }
            }
            else
            {
                double x1, x2;
                x1 = hW - 2 * Math.Cbrt(hQ / 2);
                x2 = hW + Math.Cbrt(hQ / 2);
                Console.WriteLine($"Delta równania pomocniczego = 0, pierwiastki rzeczywiste: {x1}, {x2}");
                y1 = x1.ToString();
                y2 = x2.ToString();
            }

            // obliczanie p, q, r, s
            double y1_double, y2_double;
            if (Double.TryParse(y1, out y1_double) && Double.TryParse(y2, out y2_double)) // y1 i y2 są rzeczywiste
            {
                double p = Math.Sqrt(y1_double), q = Math.Sqrt(y2_double), r = -9 / 8 * p * q, s = b / 4 * a;
                double x1 = p + q + r - s;
                double x2 = p - q - r - s;
                double x3 = -p + q - r - s;
                double x4 = -p - q + r - s;
                Console.WriteLine($"Równanie poiada cztery pierwiastki rzeczywiste: {x1}, {x2}, {x3}, {x4}");
            }
            else // y1 i y2 są zespolone
            {
                string p, q, r;
                double s;

                p = ComplexRoot(y1);
                q = ComplexRoot(y2);

                //obliczanie r
                double r_multiplier = -9 / 8;
                string p_times_q = ComplexNumberModule(y1);
                double r_a, r_b;
                string[] ab = p_times_q.Split(" + ");
                r_a = Convert.ToDouble(ab[0]) * r_multiplier;
                r_b = Convert.ToDouble(ab[1].Trim('i')) * r_multiplier;
                r = r_a.ToString() + " + " + r_b.ToString() + "i";
                s = b / 4 * a;

                //trzeba jeszcze obliczyc pierwiastki - podzielmy to co mam dodac na im i r liczby, a potem zsumowac
                string[] re_im_p = p.Split(" + ");
                string[] re_im_q = p.Split(" + ");
                string[] re_im_r = p.Split(" + ");

                double re_p = Convert.ToDouble(re_im_p[0]);
                double re_q = Convert.ToDouble(re_im_q[0]);
                double re_r = Convert.ToDouble(re_im_r[0]);
                double im_p = Convert.ToDouble(re_im_p[0].Trim('i'));
                double im_q = Convert.ToDouble(re_im_q[0].Trim('i'));
                double im_r = Convert.ToDouble(re_im_r[0].Trim('i'));


                double re_x1 = re_p + re_q + re_r - s;
                double im_x1 = im_p + im_q + im_r;
                double re_x2 = re_p - re_q - re_r - s;
                double im_x2 = im_p - im_q - im_r;
                double re_x3 = -re_p + re_q - re_r - s;
                double im_x3 = -im_p + im_q - im_r;
                double re_x4 = -re_p - re_q + re_r - s;
                double im_x4 = -im_p - im_q + im_r;

                //posumowac je dla poszczególnych pierwiastkow
                string x1 = ReAndImPartsToComplexNumberString(re_x1, im_x1);
                string x2 = ReAndImPartsToComplexNumberString(re_x2, im_x2);
                string x3 = ReAndImPartsToComplexNumberString(re_x3, im_x3);
                string x4 = ReAndImPartsToComplexNumberString(re_x4, im_x4);
                Console.WriteLine($"Równanie poiada cztery pierwiastki zespolone: {x1}, {x2}, {x3}, {x4}");
            }
        }

        static string ComplexRoot(string number)
        {
            string result;

            double a, b;
            string[]ab = number.Split(" + ");
            
            a = Convert.ToDouble(ab[0].Replace(',','.'));
            b = Convert.ToDouble(ab[1].Trim('i').Replace(',', '.'));

            double xIm, xR;
            xR = Math.Sqrt((Math.Sqrt(a * a + b * b) + a) / 2);
            xIm = Math.Sign(b) * Math.Sqrt((Math.Sqrt(a * a + b * b) - a) / 2);

            result = xR.ToString() + " + " + xIm.ToString() + "i";

            return result;
        }
        static string ComplexNumberModule(string number)
        {
            string midresult, result;

            double a, b;
            string[] ab = number.Split(" + ");

            a = Convert.ToDouble(ab[0].Replace(',', '.'));
            b = Convert.ToDouble(ab[1].Trim('i').Replace(',', '.'));

            double xIm = a*a, xR = b*b;
            midresult = xR.ToString() + " + " + xIm.ToString() + "i";
            result = ComplexRoot(midresult);

            return result;
        }
        static string ReAndImPartsToComplexNumberString(double re_part, double im_part)
        {
            string complexNumber = re_part.ToString() + " + " + im_part.ToString() + "i";
            return complexNumber.Replace("+ -", "-");
        }
    }
}
