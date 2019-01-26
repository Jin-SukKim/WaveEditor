using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    struct Complex
    {
        public double real;
        public double imaginary;
    }
    class Fourier
    {
        int N;
        //Number of samples

        //Sound Data
        float[] lData;
        float[] rData;
        float[] S;

        public Fourier(float[] l, float[] r, int samps)
        {
            lData = l;
            rData = r;
            N = samps;
        }

        //stores real and imaginary numbers of a frequency bin

        public void FrequencyGraph(float[] L, float[] R, int s)
        {
            lData = L;
            rData = R;
            N = s;
        }
        
        private Complex[] FourierTransform(double[] S)
        {
            //Imaginary values related to frequency bin's amplitude
            Complex[] A = new Complex[N];
            Complex temp;
            for (int f = 0; f < N; f++)
            {
                temp.real = 0;
                temp.imaginary = 0;
                for (int t = 0; t < N; t++)
                {
                    temp.real += S[t] * Math.Cos(2 * Math.PI * t * f / N);
                    temp.imaginary -= S[t] * Math.Sin(2 * Math.PI * t * f / N);
                }
                A[f] = temp;
            }
            inverseFourier(A);
            return A;
        }

        private double getAmplitude(Complex A)
        {
            double a, b, c;
            a = A.real * A.real;
            b = A.imaginary * A.imaginary;
            c = Math.Sqrt(a + b);
            return c;
        }

        public double[] amplitudes()
        {
            Complex[] A;
            double[] tempAmplitudes = new double[N];
            double[] tempSamples = new double[N];
            for (int i = 0; i < lData.Length - N; i += N)
            {
                for (int j = 0; j < N; j++)
                {
                    tempSamples[j] = lData[i + j];
                 }
                A = FourierTransform(tempSamples);
                for (int k = 0; k < A.Length; k++)
                {
                    tempAmplitudes[k] = getAmplitude(A[k]);
                }
            }
            return tempAmplitudes;
        }
        
        private void inverseFourier(Complex[] A)
        {
            //Sample Value
            S = new float[N];
            double[] Stemp = new double[N];
            double temp;
            for (int t = 0; t < N; t++)
            {
                temp = 0;
                for (int f = 0; f < N; f++)
                {
                    temp += A[f].real * Math.Cos(2 * Math.PI * t * f / N)
                        - A[f].imaginary * Math.Sin(2 * Math.PI * t * f / N);
                }
                Stemp[t] = temp / N;
            }
            for(int i = 0; i<N;i++)
            {
                S[i] = (float)Stemp[i];
            }
        }

        public float[] getInverse()
        {
            return S;
        }
    }
}
