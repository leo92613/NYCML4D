using UnityEngine;
using System.Collections;
using System;

namespace FRL.IO.FourD
{
    public class Trackball
    {
        int size;
        float[,] mat, rot, tmp, err;
        bool isDebug = false;

        public Trackball(int size)
        {
            this.size = size;
            mat = new float[size, size];
            rot = new float[size, size];
            tmp = new float[size, size];
            err = new float[size, size];
            identity();
        }

        public string toString()
        {
            return toString(mat);
        }

        public string toString(float[,] mat)
        {
            string s = "{ ";
            for (int row = 0; row < size; row++)
            {
                s += "{";
                for (int col = 0; col < size; col++)
                    s += round(mat[row, col]) + ",";
                s += "},";
            }
            s += " }";
            return s;
        }

        public void identity()
        {
            identity(mat);
        }

        public void identity(float[,] mat)
        {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    mat[row, col] = row == col ? 1.0f : 0.0f;
        }

        // Compute rotation that brings unit length A to nearby unit length B.

        public void rotate(float[] A, float[] B)
        {
            computeRotation(rot, A, B);
            multiply(rot);
        }

        public void computeRotation(float[,] rot, float[] A, float[] B)
        {

            // Start with matrix I + product ( 2*transpose(B-A) , A )

            identity(rot);
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    rot[row, col] += 2 * (B[row] - A[row]) * A[col];

            // Iterate until matrix is numerically orthonormal:

            for (float totalError = 1.0f; totalError >= 0.00001f;)
            {

                // Initialize each row error to 0:

                for (int i = 0; i < size; i++)
                    for (int k = 0; k < size; k++)
                        err[i, k] = 0.0f;

                // Add to error between each pair of rows:

                for (int i = 0; i < size - 1; i++)
                {
                    for (int j = i + 1; j < size; j++)
                    {
                        float[] row1, row2;
                        row1 = new float[size];
                        row2 = new float[size];
                        for (int k = 0; k < size; k++)
                        {
                            row1[k] = rot[i, k];
                            row2[k] = rot[j, k];
                        }
                        float t = dot(row1, row2);
                        for (int k = 0; k < size; k++)
                        {
                            err[i, k] += rot[j, k] * t / 2.0f;
                            err[j, k] += rot[i, k] * t / 2.0f;
                        }
                    }
                }

                // For each row, subtract errors and normalize:

                totalError = 0.0f;
                for (int i = 0; i < size; i++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        rot[i, k] -= err[i, k];
                        totalError += err[i, k] * err[i, k];
                    }
                    float[] row = new float[size];
                    for (int k = 0; k < size; k++)
                    {
                        row[k] = rot[i, k];
                    }
                    normalize(rot, i, row);
                }
            }
        }

        public void multiply(float[,] src)
        {
            multiply(src, mat, tmp);
            copy(tmp, mat);
        }

        public void multiply(float[,] a, float[,] b, float[,] dst)
        {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                {
                    dst[row, col] = 0.0f;
                    for (int k = 0; k < size; k++)
                        dst[row, col] += a[row, k] * b[k, col];
                }
        }

        public void transform(float[] src, float[] dst)
        {
            transform(mat, src, dst);
        }

        public void transform(float[,] mat, float[] src, float[] dst)
        {
            for (int row = 0; row < size; row++)
            {
                dst[row] = 0.0f;
                for (int col = 0; col < size; col++)
                    dst[row] += mat[row, col] * src[col];
            }
        }

        public void copy(float[,] src, float[,] dst)
        {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    dst[row, col] = src[row, col];
        }

        public float dot(float[] a, float[] b)
        {
            float t = 0.0f;
            for (int k = 0; k < size; k++)
                t += a[k] * b[k];
            return t;
        }

        public void normalize(float[,] a, int i, float[] b)
        {
            float s = (float)Math.Sqrt(dot(b, b));
            for (int k = 0; k < size; k++)
                a[i, k] /= s;
        }

        public string round(float t)
        {
            return "" + ((int)(t * 1000) / 1000.0f);
        }

        public void transpose(float[,] src, float[,] dst)
        {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    dst[col, row] = src[row, col];
        }
    }
}
