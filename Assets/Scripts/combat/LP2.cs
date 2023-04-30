using System;
using UnityEngine;

public class LP2 : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    public void starting()
    {
        
        double[,] payoffs = { 
            
        //This AI chooses COLUMNS in order to MAXIMIZE the payoff it gets against any choice of row
        //analyzing "shoot, block, reload"
        //S  B   R
        { 0, .2, -1}, //S
        { -.2, 0, .2}, //B
        { 1, -.2, 0} //R
        };

        double[] x = LPinator(payoffs);

        for(int i=0; i < x.Length; i++) {
            Debug.Log("Probability of option " + (i+1) + ": " + x[i]);
        }
        
    }
    */

    
    public double[] LPinator (double[,] payoffs) { //maximize c*x subject to Ax <= b

        //mxn matrix (as would appear if typed out)
        int n = payoffs.GetLength(1); //number of items per row vector (number of columns)
        int m = payoffs.GetLength(0); //number of "row vectors"

        double[] x = new double[n]; //output


        //To best use the rows as objects, rather than the columns, we are the player choosing the column vector, and also (as a result) the column of the matrix in the game

        //Variables: z gets added, but the last component of x gets removed. x is a column vector, so there are n variables in total.

        //Constraints: There are m constraints imposed by subtracting row vectors from z, and one additional constraint: sum of components is <= 1

        double[,] mat = new double[m+2, m+n+2];

        int[] basicvar = new int[m+4]; //;-;

        //The objective function is z: an added component of our x vector (and the last component).
        mat[0, n] = 1;


        //iterate over mat rows (constraints)
        for(int i = 0; i < m; i++) {
            //iterate over variables (components of x)
            for(int j = 0; j < n-1; j++) {
                //the coefficient of the jth component of the ith restriction is equal to 
                mat[i+1,j+1] = payoffs[i,n-1] - payoffs[i,j]; //?
            }
            mat[i+1, 0] = payoffs[i,n-1]; //b, ?
            mat[i+1, n] = 1; //z
        }
        //last constraint:
        for(int i = 0; i <= n; i++) {
            mat[m+1,i] = 1; //1 >= x_1+x_2+...+x_n-1
        }

        //filling in the identity.
        for(int i = 1; i <= m+1; i++) {
            mat[i, n+i] = 1;
        }

        revisedSimplex(true, m+n+1, m+1, mat, 1e-6, basicvar); //trying to maximize payout

        // for(int i = 0; i < m+2; i++) {
        //     Debug.Log(mat[i,0] + " " + mat[i,1] + " " + mat[i,2] + " " + mat[i,3] + " " + mat[i,4] + " " + mat[i,5] + " " + mat[i,6] + " " + mat[i,7]);
        // }
        
        if (basicvar[m + 2] > 0)
        {
            //Debug.Log("Infeasible: No solution found.");
        }
        else
        {
            if (basicvar[m + 3] > 0)
            {
                //Debug.Log("Unbounded: No solution found.");
            }
            else
            {
                //Debug.Log("Optimal solution found.");
                double sum = 0;
                for (int i = 1; i <= m+1; i++)
                {
                    //Debug.Log(string.Format("{0,10}\t {1,-10}", basicvar[i], mat[i, 0]));
                    if (1 <= basicvar[i] && basicvar[i] < n) {
                        x[basicvar[i]-1] = mat[i,0];
                        sum += mat[i,0];
                    }
                }
                //Debug.Log(string.Format("Optimal expected value = {0}\n", mat[0, 0]));
                x[n-1] = 1-sum;
            }
        }



        return x;

    }



    //WARNING: [m,n] matrix, will appear n x m when typed out in code
    public static void revisedSimplex(bool maximize, int n, int m, double[,] a, double epsilon, int[] basicvar)
    {
        int i, j, k, m2, p, idx = 0;
        double[] objcoeff = new double[n + 1];
        double[] varsum = new double[n + 1];
        double[] optbasicval = new double[m + 3];
        double[] aux = new double[m + 3];
        double[,] work = new double[m + 3, m + 3];
        double part, sum;
        bool infeasible, unbound, abort, outres, iterate;
        if (maximize)
            for (j = 1; j <= n; j++)
                a[0, j] = -a[0, j];
        infeasible = false;
        unbound = false;
        m2 = m + 2;
        p = m + 2;
        outres = true;
        k = m + 1;
        for (j = 1; j <= n; j++)
        {
            objcoeff[j] = a[0, j];
            sum = 0.0;
            for (i = 1; i <= m; i++)
            sum -= a[i, j];
            varsum[j] = sum;
        }
        sum = 0.0;
        for (i = 1; i <= m; i++)
        {
            basicvar[i] = n + i;
            optbasicval[i] = a[i, 0];
            sum -= a[i, 0];
        }
        optbasicval[k] = 0.0;
        optbasicval[m2] = sum;
        for (i = 1; i <= m2; i++)
        {
            for (j = 1; j <= m2; j++)
            work[i, j] = 0.0;
            work[i, i] = 1.0;
        }
        iterate = true;
        do
        {
            if ((optbasicval[m2] >= -epsilon) && outres)
            {
                outres = false;
                p = m + 1;
            }
            part = 0.0;
            for (j = 1; j <= n; j++)
            {
                sum = work[p,m+1] * objcoeff[j] + work[p,m+2] * varsum[j];
                for (i = 1; i <= m; i++)
                sum += work[p, i] * a[i, j];
                if (part > sum)
                {
                    part = sum;
                    k = j;
                }
            }
            if (part > -epsilon)
            {
                iterate = false;
                if (outres)
                infeasible = true;
                else
                a[0, 0] = -optbasicval[p];
            }
            else
            {
                for (i = 1; i <= p; i++)
                {
                    sum = work[i,m+1] * objcoeff[k] + work[i,m+2] * varsum[k];
                    for (j = 1; j <= m; j++)
                    sum += work[i, j] * a[j, k];
                    aux[i] = sum;
                }
                abort = true;
                for (i = 1; i <= m; i++)
                if (aux[i] >= epsilon)
                {
                    sum = optbasicval[i] / aux[i];
                    if (abort || (sum < part))
                    {
                        part = sum;
                        idx = i;
                    }
                    abort = false;
                }
                if (abort)
                {
                    unbound = true;
                    iterate = false;
                }
                else
                {
                basicvar[idx] = k;
                sum = 1.0 / aux[idx];
                for (j = 1; j <= m; j++)
                    work[idx, j] *= sum;
                i = ((idx == 1) ? 2 : 1);
                do
                {
                    sum = aux[i];
                    optbasicval[i] -= part * sum;
                    for (j = 1; j <= m; j++)
                        work[i, j] -= work[idx, j] * sum;
                    i += ((i == idx - 1) ? 2 : 1);
                } while (i <= p);
                optbasicval[idx] = part;
                }
            }
        } while (iterate);
        // return results
        basicvar[m + 1] = (infeasible ? 1 : 0);
        basicvar[m + 2] = (unbound ? 1 : 0);
        for (i = 1; i <= m; i++)
            a[i, 0] = optbasicval[i];
        if (maximize)
        {
            for (j = 1; j <= n; j++)
            a[0, j] = -a[0, j];
            a[0, 0] = -a[0, 0];
        }
    }
}