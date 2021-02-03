/* Copyright: Muamer Hrncic */
/* Original author(s): Muamer Hrncic */
/* License: MIT */
/* Purpose: Algorithm to solve the bin packing problem. */

#region Description

/* The three dimensional bin packing problem belongs to the  */
/* class of NP complete problems and can be formulated as follows: */

/* Let be J={1,...,n} a set of small rectangular items with dimensions w_i,h_i,d_i. */
/* All small items must be packed into bins of dimensions W,H,D. */
/* All dimensions are positive integers and the dimensions of the small */
/* items are smaller than or equal to the corresponding bin dimensions.  */

/* The task is to pack all the small items,  */
/* such that: */

/* 1) All small items are packed into a minimum number of bins */
/* 2) No two items overlap. */

/* The algorithm solves the problem for orthogonal packing, which means the items are packed such that*/
/* the edges of the small items are parallel to the correspoding bin edge. */
/* No rotations of items or bins are allowed. */
/* The algorithm is capable to solve the problem for general packings and robot packings. */

/* For a detailed description of the algorithm and original C-Code refer to: */

/* [1] Silvano Martello, David Pisinger, and Daniele Vigo. "The three-dimensional */
/* bin packing problem." Oper. Res., 48(2):256–267, March */
/* 2000. */

/* [2] Silvano Martello, David Pisinger, Daniele Vigo, Edgar Boef, and Jan Korst. */
/* "Algorithm 864: General and robot-packable variants of the threedimensional */
/* bin packing problem." ACM Trans. Math. Softw., 33:7, 01 */
/* 2007. */

#endregion

/* This C# code is a reimplementation of the original C-Code provided */
/* by Daniel Pisinger at http://hjemmesider.diku.dk/~pisinger/codes.html. */
/* This code comes with no warranty. */


using System;
using System.IO;
using BPP3DLib;
using System.Threading;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using EcmaScript.NET;

namespace BPP3DApp
{
    public class Program
    {
        // Main Programm to test the bin packing algorithm.
        // The procedure contains examples how to use the algorithm.
        private static readonly Dictionary<int, int> numofItemType = new Dictionary<int, int>();//每种货物对应的数量;
        private static readonly int Trucktype = 1;//车型类型
        static private readonly int Itemtype = 7;//货物类型的数量
        static void Main(string[] args)
        {

            //Console.ReadKey();
            Console.WriteLine("######################################################################################");
            Console.WriteLine("The algorithm is tested with random values.");
            {
                int n = 1000;  //货物的总数量
                int instanceType = 1;
                int timeLimit = 600;
                int instLimit = 1;
                //numofItemType.Add(0, 1287);
                numofItemType.Add(0, 200);
                numofItemType.Add(1, 300);
                numofItemType.Add(2, 200);
                numofItemType.Add(3, 100);
                numofItemType.Add(4, 200);
                //numofItemType.Add(6, 100);
                //numofItemType.Add(7, 100);
                string outputPath = @"C:\Users\国富\Desktop\code\G-Bin-Packing-Problem-BPP3D-master\Bin-Packing-Problem-BPP3D-master - 输入输出TXT文本\test\";
                try
                {
                    RunRandomTest(n, instanceType, timeLimit, instLimit, outputPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Some error occured.");
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
       
        }

        public static void RunRandomTest(int n, int instanceType, int timeLimit, int instLimit, string outputPath )
        {
            // Create random instance with for the three dimensional bin packing problem. 
            // The bin dimensions are assigned in procedure "CreateRandomTest".
            // For n and the instance Type "instLimit" instances are created.
            // The algorithm is run twice for every instance in order to find a robot packing and a general packing.

            for (int actInstance = 1; actInstance <= instLimit; actInstance++)
            {
                Console.WriteLine("#########################################################");
                Console.WriteLine("Actual instance:");
                Console.WriteLine("n = " + n.ToString());
                Console.WriteLine("Instance type = " + instanceType.ToString());
                Console.WriteLine("Instance number= " + actInstance.ToString());
                CreateRandomTest(n, instanceType, actInstance, timeLimit, outputPath);
                Console.WriteLine("Actual instance finished. See output files for details.");
                Thread.Sleep(1000);
            }
            Console.WriteLine("#########################################################");
            Console.WriteLine("Finished all!");
            Console.ReadKey();
        }

        public static void CreateRandomTest(int n, int instanceType, int actInstance, int timeLimit, string outputPath)
        {
            double[] w = new double[n];
            double[] h = new double[n];
            double[] d = new double[n];
            int[] itemtype = new int[n];
            int W;
            int H;
            int D;
            switch (Trucktype)//四种车型
            {
                case 1:
                    W = 240;H = 290;D = 420;
                    break;
                case 2:
                    W = 240;H = 270;D = 680;
                    break;
                case 3:
                    W = 240;H = 270;D = 960;
                    break;
                case 4:
                    W = 240;H = 250;D = 1350;
                    break;
                //缺省为中杯
                default:
                    W = 240;H = 290;D = 420;
                    break;
            }

            // Create a random instance
            //BPP3DLib.CreateTests.MakeTests(n, w, h, d, W, H, D, equalBinDim, ref instanceType);
            int num = 0;
            foreach (var item in numofItemType)
            {
                double wide; double height; double depth;
                switch (item.Key)//四种车型
                {
                    case 0://软化水
                        wide = 25;height = 22;depth = 37;
                        break;
                    case 1://芒顿 
                        wide = 20.5; height = 22.5; depth = 34;
                        break;
                    case 2: //红茶 
                        wide = 21; height = 21.5; depth = 34;
                        break;
                    case 3: //15入凉白开
                        wide = 18.5; height = 21.5; depth = 30;
                        break;
                    case 4: //24入凉白开
                        wide = 22; height = 21.5; depth = 34.5;
                        break;
                    case 5: //12入凉白开
                        wide = 18.5; height = 21.5; depth = 24;
                        break;
                    case 6: //6×4入凉白开
                        wide = 24; height = 22.5; depth = 35.5;
                        break;
                    default:
                        wide = 240; height = 290; depth = 420;
                        break;
                }
                for (int i=0; i< item.Value; i++)
                {
                    itemtype[i + num] = item.Key;
                    w[i + num] = wide;
                    h[i + num] = height;
                    d[i + num] = depth;
                }
                num += item.Value;
            }

            // Name of the file, which holds the input data of the instance
            string instanceName = "INST" + "_N" + n.ToString() + "_IT" + instanceType.ToString() + "NR" + actInstance.ToString() + ".txt";
            string path = outputPath + instanceName;
            string res;

            // Store the instance data.
            StreamWriter swDat = new StreamWriter(path);
            swDat.WriteLine(n.ToString() + "\t" + W.ToString() + "\t" + H.ToString() + "\t" + D.ToString());
            for (int i = 0; i < n; i++)
            {
                swDat.WriteLine(w[i].ToString() + "\t" + h[i].ToString() + "\t" + d[i].ToString());
            }
            swDat.Close();

            // Start the algorithm
            double[] x;
            double[] y;
            double[] z;
            int[] bno;

            AllInfo aRob;
            int nodeLimit = 0;
            int iterLimit = 0;
            int packingType = 1;

            #region Robot packing

            // Try to find a robot packing.
            x = new double[n];
            y = new double[n];
            z = new double[n];
            bno = new int[n];

            Console.WriteLine("Actual packing: Robot packing.");
            Console.WriteLine("Started at: " + DateTime.Now.ToString());

            // Execute the algorithm
            aRob = BPP3D.BinPack3D(n, W, H, D, w, h, d, x, y, z, bno, nodeLimit, iterLimit, timeLimit, packingType);

            Console.WriteLine("Finished at: " + DateTime.Now.ToString());

            // Name of the file, which holds the result of the instance
            string outputPath1 = @"C:\Users\国富\Desktop\code\G-Bin-Packing-Problem-BPP3D-master\Bin-Packing-Problem-BPP3D-master - 输入输出TXT文本\";

            instanceName = "output" + ".txt";

            // Path of the output file for robot packing.
            res = outputPath1 + instanceName;

            // Store the results for the robot packing.
            WriteResultsToFile(res, aRob, w, h, d, x, y, z, bno, itemtype);
            ShowResultsOnConsole(aRob, w, h, d, x, y, z, bno);
            #endregion

        }
        public static void ShowResultsOnConsole(AllInfo a, double[] w, double[] h, double[] d, double[] x, double[] y, double[] z, int[] bno)
        {
            //Show the results of the test on the console window.

            Console.WriteLine("######################################################################################");
            Console.WriteLine("n" + "\t" + "W" + "\t" + "H" + "\t" + "D");
            Console.WriteLine(a.N.ToString() + "\t" + a.W.ToString() + "\t" + a.H.ToString() + "\t" + a.D.ToString());

            Console.WriteLine("w" + "\t" + "h" + "\t" + "d" + "\t" + "x" + "\t" + "y" + "\t" + "z" + "\t" + "bno");
            for (int i = 0; i < a.N; i++)
            {
                Console.WriteLine(w[i].ToString() + "\t" + h[i].ToString() + "\t" + d[i].ToString() + "\t" +
                                x[i].ToString() + "\t" + y[i].ToString() + "\t" + z[i].ToString() + "\t" +
                                bno[i].ToString());
            }
            Console.WriteLine("######################################################################################");

            Console.WriteLine("Lower bound:" + "\t" + (a.Stopped ? a.Lb : a.Z).ToString());
            Console.WriteLine("Upper bound:" + "\t" + a.Z.ToString());
            Console.WriteLine("######################################################################################");
            Console.WriteLine("Nodes:" + "\t" + a.Nodes.ToString());
            Console.WriteLine("Subnodes:" + "\t" + a.SubNodes.ToString());
            Console.WriteLine("Iterations:" + "\t" + a.Iterat.ToString());
            Console.WriteLine("Subiterations:" + "\t" + a.SubIterat.ToString());
            Console.WriteLine("######################################################################################");
            Console.WriteLine("Calls to FitsTwo:" + "\t" + a.CallsFitsTwo.ToString());
            Console.WriteLine("Calls to FitsThree:" + "\t" + a.CallsFitsThree.ToString());
            Console.WriteLine("Calls to FitsMore:" + "\t" + a.CallsFitsMore.ToString());
            Console.WriteLine("######################################################################################");
            Console.WriteLine("Overall time:" + "\t" + a.Time.ToString());
            Console.WriteLine("Heuristic time:" + "\t" + a.LayHeurTime.ToString());
            if (a.PackType == 1)
            {
                Console.WriteLine("Robot pack time:" + "\t" + a.RobotTime.ToString());
            }
            else
            {
                Console.WriteLine("General pack time:" + "\t" + a.GeneralTime.ToString());
            }
            Console.WriteLine("######################################################################################");
        }
        public static void WriteResultsToFile(string path, AllInfo a, double[] w, double[] h, double[] d, double[] x, double[] y, double[] z, int[] bno, int[] itemtype)
        {
            // Write the results into a text file originated at "path".
            // Ensure to have sufficient rights for writing files into the directory given by path.

            StreamWriter swRes = new StreamWriter(path);
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("n" + "\t" + "W" + "\t" + "H" + "\t" + "D");
            swRes.WriteLine(a.W.ToString() + "\t" + a.H.ToString() + "\t" + a.D.ToString());

            //swRes.WriteLine("w" + "\t" + "h" + "\t" + "d" + "\t" + "x" + "\t" + "y" + "\t" + "z" + "\t" + "bno");
            for (int i = 0; i < a.N; i++)
            {
                swRes.WriteLine(itemtype[i].ToString() + "\t" + w[i].ToString() + "\t" + h[i].ToString() + "\t" + d[i].ToString() + "\t" +
                                x[i].ToString() + "\t" + y[i].ToString() + "\t" + z[i].ToString());
            }
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("Node limit:" + "\t" + a.NodeLimit.ToString());
            //swRes.WriteLine("Iteration limit:" + "\t" + a.IterLimit.ToString());
            //swRes.WriteLine("Time limit:" + "\t" + a.TimeLimit.ToString());

            //swRes.WriteLine("######################################################################################");
            //if (a.Lb == a.Z)
            //{
            //    swRes.WriteLine("Solution: Exact");
            //}
            //else
            //{
            //    swRes.WriteLine("Solution: Heuristic");
            //}
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("Lower bound:" + "\t" + (a.Stopped ? a.Lb : a.Z).ToString());
            //swRes.WriteLine("Upper bound:" + "\t" + a.Z.ToString());
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("Nodes:" + "\t" + a.Nodes.ToString());
            //swRes.WriteLine("Subnodes:" + "\t" + a.SubNodes.ToString());
            //swRes.WriteLine("Iterations:" + "\t" + a.Iterat.ToString());
            //swRes.WriteLine("Subiterations:" + "\t" + a.SubIterat.ToString());
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("Calls to FitsTwo:" + "\t" + a.CallsFitsTwo.ToString());
            //swRes.WriteLine("Calls to FitsThree:" + "\t" + a.CallsFitsThree.ToString());
            //swRes.WriteLine("Calls to FitsMore:" + "\t" + a.CallsFitsMore.ToString());
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("Overall time:" + "\t" + a.Time.ToString());
            //swRes.WriteLine("Heuristic time:" + "\t" + a.LayHeurTime.ToString());
            //if (a.PackType == 1)
            //{
            //    swRes.WriteLine("Robot pack time:" + "\t" + a.RobotTime.ToString());
            //}
            //else
            //{
            //    swRes.WriteLine("General pack time:" + "\t" + a.GeneralTime.ToString());
            //}
            //swRes.WriteLine("######################################################################################");
            //swRes.WriteLine("######################################################################################");
            swRes.Close();
            //ScriptRuntime pyRuntime = Python.CreateRuntime();
            //dynamic obj = pyRuntime.UseFile("F:\\test1.py");
        }

    }
}
