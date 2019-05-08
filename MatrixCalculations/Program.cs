using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using part2Brandt;

//This is your Mat class
namespace part1Brandt
{
    class Program
    {
        public static void Main(string[] args)
        {
        
        //creates a blank matrixA 
        float[,] matrixA  = new float[2, 2];

            //test setItem 
            setItem(matrixA, 0, 0, 2);
            setItem(matrixA, 0, 1, 3);
            setItem(matrixA, 1, 1, 2);
            Console.WriteLine(matrixA[0, 0] + " " + matrixA[0, 1]);
            Console.WriteLine(matrixA[1, 0] + " " + matrixA[1, 1]);

            //test getItem
            Console.WriteLine(getItem(matrixA, 1, 1));

            //create blank matrixB
            float[,] matrixB = new float[2, 2];

            setItem(matrixB, 0, 0, 2);
            setItem(matrixB, 1, 1, 3);
            setItem(matrixB, 0, 1, 4);
            setItem(matrixA, 0, 0, 2);

            Console.WriteLine("This is matrixA, being used for the following tests");
            Console.WriteLine(matrixA[0, 0] + " " + matrixA[0, 1]);
            Console.WriteLine(matrixA[1, 0] + " " + matrixA[1, 1]);

            Console.WriteLine();
            Console.WriteLine("This is matrixB, th other matrix these tests will use");
            Console.WriteLine(matrixB[0, 0] + " " + matrixB[0, 1]);
            Console.WriteLine(matrixB[1, 0] + " " + matrixB[1, 1]);

            Console.WriteLine();

            //test add()
            Console.WriteLine("add matrixA and matrixB");
            matrixA = add(matrixA, matrixB);
            Console.WriteLine();
            Console.WriteLine(matrixA[0, 0] + " " + matrixA[0, 1]);
            Console.WriteLine(matrixA[1, 0] + " " + matrixA[1, 1]);
            Console.WriteLine();

            Console.WriteLine("transpose the new matrixA");
            //test transpose 
            matrixA = transpose(matrixA);
            Console.WriteLine(matrixA[0, 0] + " " + matrixA[0, 1]);
            Console.WriteLine(matrixA[1, 0] + " " + matrixA[1, 1]);

            //create a vector to test functions
            float[,] vector = new float[2, 1];
            setItem(vector, 0, 0, 1);
            setItem(vector, 1, 0, 3);

            Console.WriteLine();
            Console.WriteLine("This is the vector that will be used in these next tests");
            Console.WriteLine(vector[0, 0]);
            Console.WriteLine(vector[1, 0]);

            //test vectorMatrixMulti

            float[,] result = vectorMatrixMulti(vector, matrixA);

            Console.WriteLine();
            Console.WriteLine("This test is vector Matrix multiplication with matrixA and vector");
            Console.WriteLine(result[0, 0]);
            Console.WriteLine(result[1, 0]);

            Console.WriteLine();
            vector = matrixVectorMulti(matrixA, vector);

           
            Console.WriteLine("This test is matrix vector multiplication with matrixA and vector");
            Console.WriteLine(vector[0, 0]);
            Console.WriteLine(vector[1, 0]);

            Console.WriteLine();

            Console.WriteLine("This test is matrix matrix multiplication matrixA * matrixB");
            matrixB = matrixMatrixMulti(matrixA, matrixB);
            Console.WriteLine(matrixB[0, 0]+ " " + matrixB[0, 1]);
            Console.WriteLine(matrixB[1, 0] + " " + matrixB[1, 1]);


            //tests setItem, matrixMatruxMulti, write and load to/from file \
            Console.WriteLine("This test is matrix matrix multiplication with new matrices");
            Console.WriteLine();
            float[,] matA = new float[3, 3];
            float[,] matB = new float[3, 3];
            setItem(matB, 0, 0, 3);
            setItem(matB, 0, 1, 2);
            setItem(matB, 1, 0, 4);
            setItem(matB, 1, 1, 2);
            setItem(matB, 2, 0, 3);
            setItem(matB, 2, 1, 0);
            setItem(matB, 0, 2, 1);
            setItem(matB, 1, 2, 2);
            setItem(matB, 2, 2, 3);
            Console.WriteLine(matB[0, 0] + " " + matB[0, 1] + " " + matB[0, 2]);
            Console.WriteLine(matB[1, 0] + " " + matB[1, 1] + " " + matB[1, 2]);
            Console.WriteLine(matB[2, 0] + " " + matB[2, 1] + " " + matB[2, 2]);
            Console.WriteLine();
            setItem(matA, 0, 0, 1);
            setItem(matA, 0, 1, 2);
            setItem(matA, 0, 2, 3);
            setItem(matA, 1, 0, 2);
            setItem(matA, 1, 1, 1);
            setItem(matA, 1, 2, 3);
            setItem(matA, 2, 0, 3);
            setItem(matA, 2, 1, 2);
            setItem(matA, 2, 2, 3);
            Console.WriteLine(matA[0, 0] + " " + matA[0, 1] + " " + matA[0, 2]);
            Console.WriteLine(matA[1, 0] + " " + matA[1, 1] + " " + matA[1, 2]);
            Console.WriteLine(matA[2,0] + " " + matA[2,1] + " " + matA[2,2]);
           float[,] matrix = matrixMatrixMulti(matA, matB);
            Console.WriteLine();
            Console.WriteLine(matrix[0, 0] + " " + matrix[0, 1] + " " + matrix[0, 2]);
            Console.WriteLine(matrix[1, 0] + " " + matrix[1, 1] + " " + matrix[1, 2]);
            Console.WriteLine(matrix[2, 0] + " " + matrix[2, 1] + " " + matrix[2, 2]);
            Console.WriteLine();
            Console.WriteLine("These following tests the write and read functions to file");

            saveMatrix(matrix, pathTest);
            float[,] newMatrix = loadMatrix(pathTest);
            Console.WriteLine();
            Console.WriteLine(newMatrix[0, 0] + " " + newMatrix[0, 1] + " " + newMatrix[0, 2]);
            Console.WriteLine(newMatrix[1, 0] + " " + newMatrix[1, 1] + " " + newMatrix[1, 2]);
            Console.WriteLine(newMatrix[2, 0] + " " + newMatrix[2, 1] + " " + newMatrix[2, 2]);
            Console.WriteLine();

            Console.WriteLine("points multiplied by projection matrix");
            //load the points matrix 
            matA = loadMatrix(pathTIN);
            //load the projection matrix 
            matB = loadMatrix(pathProjection);
            newMatrix = projectTIN(matA, matB);

            Console.WriteLine(newMatrix[0, 0] + " " + newMatrix[0, 1] + " " + newMatrix[0, 2]);
            Console.WriteLine(newMatrix[1, 0] + " " + newMatrix[1, 1] + " " + newMatrix[1, 2]);
            Console.WriteLine(newMatrix[2, 0] + " " + newMatrix[2, 1] + " " + newMatrix[2, 2]);
            Console.WriteLine(newMatrix[3, 0] + " " + newMatrix[3, 1] + " " + newMatrix[3, 2]);
            Console.WriteLine(newMatrix[4, 0] + " " + newMatrix[4, 1] + " " + newMatrix[4, 2]);

            //create the points to display 
            Point[] matrixPoints = new Point[newMatrix.GetLongLength(rowNum)];
            matrixPoints = CreatePoints(newMatrix, matrixPoints);
            Form1.points = matrixPoints;

            //display the points
            Console.WriteLine();
            Console.WriteLine("The points are set up this was for drawing purposes. ");
            for (int i = 0; i < matrixPoints.Length; i++)
            {
                Console.WriteLine(matrixPoints[i]);
            }

            Console.ReadLine();

           
        }

        //drawing support 
        //public static Rectangle rectangle = new Rectangle(-10, 10, 20, 20);
        //public static PaintEventArgs paint = new PaintEventArgs(graphics, rectangle);
        //public PictureBox pictureBox = new PictureBox();
        private const int rowNum = 0;
        private const int colNum = 1;

    // private const String pathTIN = "c:/Users/Dryan/Documents/matrix/Program_3Brandt/part1Brandt/TIN_D.txt";
    private static String pathTIN = Directory.GetCurrentDirectory() + "\\TIN_D.txt";
        private static String pathProjection = Directory.GetCurrentDirectory() + "\\projectionMatrix.txt";
        private static String pathTest = Directory.GetCurrentDirectory() + "\\file.txt";
        private static String triangleIndices = Directory.GetCurrentDirectory()  + "\\Triangles_D.txt";

        //get function 
        public static float getItem(float[,] matrix, int row, int col)
        {
            return matrix[row, col];
        }

        //equals function
        public static bool equal(float[,] matrixA, float[,] matrixB)
        {
            foreach (int element in matrixA)
            {
                foreach (int elemt in matrixB)
                {
                    if (element == elemt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //set item 
        public static void setItem(float[,] matrix, int row, int col, float value)
            {
                 matrix[row, col] = value;  
            }

        //add, adds matrices
        public static float[,] add(float[,] matrixA, float[,] matrixB)
        {
            for (int i = 0; i < matrixA.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrixA.GetLongLength(colNum); j++)
                {
                    matrixA[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }
            return matrixA;
        }

        //scalar matrix multiplication 
        public static float[,] scalarMultiplication(float[,] matrix, float scalar)
        {
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                {
                    matrix[i,j] = matrix[i,j] * scalar;
                }
            }
            return matrix;
        }

        //transpose the given matrix
        public static float[,] transpose(float[,] matrix)
        {
            float[,] result = new float[matrix.GetLongLength(rowNum), matrix.GetLongLength(colNum)];
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            return result;
        }

        //vector matrix multiplication
        public static float[,] vectorMatrixMulti(float[,] vector, float[,] matrix)
        {
            //check if sizes match to do the math properly
            if (matrix.GetLongLength(colNum) != vector.GetLongLength(rowNum))
            {
                Console.WriteLine("matrix colomns do not match vectors rows");
                return matrix;
            }

            //first multiplies each element in matrix to the correct element in the vector
            float[,] result = new float[vector.GetLongLength(rowNum), vector.GetLongLength(colNum)];
                for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
                {
                    for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                    {
                        matrix[i, j] = matrix[i, j] * vector[j, 0];
                    }
                }

                //adds the elements in the rows of the matrix and stores it in results
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                float sum = 0;
                for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                {
                    sum += matrix[i, j];
                    result[i, 0] = sum;
              }
            }
            return result;
        }

        //matrix vector multiplication
        public static float[,] matrixVectorMulti(float[,] matrix, float[,] vector)
        {
            //check if sizes match to do math properly
            if (matrix.GetLongLength(colNum) != vector.GetLongLength(rowNum))
            {
                Console.WriteLine("matrix colomns do not match vectors rows");
                return matrix;
            }
            
            //first multiplies each element in matrix to the correct element in the vector
            float[,] result = new float[vector.GetLongLength(rowNum), vector.GetLongLength(colNum)];
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                {
                    matrix[i, j] = matrix[i, j] * vector[j, 0];
                }
            }

            //adds the elements in the rows of the matrix and stores it in results
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                float sum = 0;
                for (int j = 0; j < matrix.GetLongLength(colNum); j++)
                {
                    sum += matrix[i, j];
                    result[i, 0] = sum;
                }
            }
            return result;
        }

        //matrix matrix multiplication 
        public static float[,] matrixMatrixMulti(float[,] matrixA, float[,] matrixB)
        {
            //check if sizes match to do math properly
            if (matrixA.GetLongLength(colNum) != matrixB.GetLongLength(rowNum))
            {
                Console.WriteLine("matrix sizes do not match (N x M) * (M x N)");
                return matrixA;
            }

            float[,] result = new float[matrixA.GetLongLength(rowNum), matrixB.GetLongLength(colNum)];
            for (int i = 0; i < matrixA.GetLongLength(colNum); i++)
            {
                for (int j = 0; j < matrixA.GetLongLength(rowNum); j++)
                {
                    for (int x = 0; x < matrixB.GetLongLength(colNum); x++)
                    {
                        result[j, x] += matrixA[j, i] * matrixB[i, x];
                            
                    }
                }
            }
            return result;
        }

        //save a matrix to a file 
        public static void saveMatrix(float[,] matrix, String path)
        {
            StreamWriter file = new StreamWriter(path, false);
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrix.GetLongLength(colNum) - 1; j++)
                {
                    file.Write(matrix[i, j] + " ");
                }
                file.WriteLine(matrix[i, matrix.GetLongLength(colNum) - 1]);
            }
            file.Close();
        }

        //load matrix from a file
        public static float[,] loadMatrix(String path)
        {
            string[] lines = File.ReadAllLines(path);
            float[,] matrix = new float[lines.Length, lines[0].Split(' ').Length];
            
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];
                for (int j = 0; j < matrix.GetLength(colNum); ++j)
                {
                    string[] item = line.Split(' ');
                    matrix[i, j] = float.Parse(item[j]);         
                }
            }    
            return matrix;
        }

        public static float[,] projectTIN(float[,] points, float[,] projection)
        {
            float[,] matrix = Program.matrixMatrixMulti(points, projection);
            return matrix;

        }

        //converts matrix to points
        public static Point[] CreatePoints(float[,] matrix, Point[] matrixPoints)
        {
            //loop through the matrix and save the values as points 
            for (int i = 0; i < matrix.GetLongLength(rowNum); i++)
            {
                for (int j = 0; j < matrix.GetLongLength(colNum) - 1; j++)
                {              
                    Point addPoint = new Point((int)matrix[i, j], (int)matrix[i, j = j + 1]);
                    matrixPoints[i] = addPoint;
                }
            }
            return matrixPoints;    
        }
    }
}
