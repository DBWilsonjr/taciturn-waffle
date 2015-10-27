using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CPT185_Test_2_Revamp
{
    public partial class Form1 : Form
    {
        Boolean valid = true; //This is a 'global' variable, though if we were working with multipe forms it couldn't
        //actually be called from this form. This is just me being nit-picky since we're only using this form.

        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            double[] fishRet = new double[8]; //Local variable where returns from our method will be stored
            string statMsg = ""; //Local variable that will hold what we what to output in pretty text

            /*Our method call, which will compute data from our file and do all the math we need done.
             *Methods are very fun, but it can take a bit to understand them. The default setting for a method is 'in', so if
             *we called the method as Method(fishRet[0], fishRet[1], ...), it would be sending those values to the method.
             *We don't actully need to send anything to the method, we only need information from it.
             *Our method is defined with 'out' variables, so when we call the method we need to specify where the output is
             *going and make sure we have as many 'out's in the call as we do in the defined method. The 'out' portion of
             *the method call goes to local variable(s) and doesn't have to match the names declared in the method.
             *Since this uses an array, you have to mentally assign what each element is; for this case, element-0
             *is the total of our numbers, e-1 is the total number of records in our file, e's 2 - 4 are the biggest
             *fish, and e's 5 - 7 are the smallest. If we were using single variables, we could just call them things like
             *total, count, big1, big2, etc.; that would be easier to read through, but it uses more variables and arrays
             *are part of the current topic (and it never hurts to practice those anyway).
             */
            SuperComputerator(out fishRet[0], out fishRet[1], out fishRet[2], out fishRet[3], out fishRet[4],
                out fishRet[5], out fishRet[6], out fishRet[7]);

            if (valid == true) //Reference to a global boolean variable that is set false if the method fails
            {
                statMsg = "The weight of all caught fish: " + fishRet[0].ToString() + "lbs.\n";
                statMsg += "The total fish caught: " + fishRet[1] + "lbs.\n";
                statMsg += "The average weight of fish: " + Math.Round(fishRet[0] / fishRet[1],2).ToString() + "\n";
                lblOutput.Text = statMsg;
                txtLrg1.Text = fishRet[2].ToString();
                txtLrg2.Text = fishRet[3].ToString();
                txtLrg3.Text = fishRet[4].ToString();
                txtSml1.Text = fishRet[5].ToString();
                txtSml2.Text = fishRet[6].ToString();
                txtSml3.Text = fishRet[7].ToString();
                grbContain.Visible = true;
                
            }
            else
                MessageBox.Show("Make sure your input file is located with the program and named flyingfish.txt", "File Read Error"); 
            //The first quoted text is what is show in the box, the second quoted text is shown in the titlebar of the
            //message box. Useful to help categorize messageboxes if you want to use more than one type in an else/catch.
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SuperComputerator(out double total, out double cnt, out double large, out double large2, out double large3, out double small, out double small2, out double small3)
        //Since this is returning to an array, all declared outs must be the same type (doubles in this case)
        //Another note about 'out's from a method; they obviously go out in the order they're declared. In this case, total
        //will go out first no matter where it is a dealt with in the actual method. This is important to keep in mind since
        //we're working with an array and have to keep track of these things ourselves. Like any other time, declare in a way
        //that makes it easiest to work with.
        {
            //These have to be set so the method doesn't break. If the try/catch is removed, the code below works with
            //the code below, but be sure to uncomment line 68; the method needs a starting point for the variables and
            //that isn't set when you declare them ("a ref or out parameter can't have a default value").
            total = cnt = large = large2 = large3 = small = small2 = small3 = 0;
            try
            {
                double[] fishies = new double[25];
                int idx = 0;
                //total = 0;

                StreamReader infile1;
                infile1 = File.OpenText("flyingfish.txt");

                //After this loop, we will have the final amounts for total and cnt, Elements 0 and 1 in our array above
                while (idx < fishies.Length && !infile1.EndOfStream)
                {
                    fishies[idx] = double.Parse(infile1.ReadLine());
                    total += fishies[idx];
                    //if (large < fishies[idx])
                        //large = fishies[idx]
                    //if (small == 0)
                        //small = fishies[idx]
                    //else if (small > fishies[idx])
                        //small = fishies[idx]
                    idx++;
                }
                infile1.Close();
                cnt = idx;

                //Gives large and small a starting value, the first from our file. We need to do this mostly for the small
                //variables because they're currently set to 0, which is smaller than any fish in the file
                large = large2 = large3 = small = small2 = small3 = fishies[0];

                /*These loops will get us the numbers for large and small fish, or elements 2 - 4 and 5 - 7 in our
                 *fishRet array above. If we were only getting the biggest and smallest fish, this could be worked into the loop
                 *above, but even so it's easier to comprehend this way.
                 *We're doing a top 3, just to do something different. This is much harder (impossible?) to do in a single
                 *loop (and honestly the whole thing is better suited to its own method, but this is what happens when I 
                 *change my mind mid-coding.
                */

                //The first loop will get the largest and smallest fish.
                for (int i = 1; i < fishies.Length; i++) //i is set to 1 because we don't need to read the zero element again
                {                           
                    if (large < fishies[i])
                        large = fishies[i];
                    if (small > fishies[i])
                        small = fishies[i]; 
                }

                //This time we do the same, but for the 2nd biggest (e-3) and smallest (e-6) This means we need an added
                //check to avoid taking the same number as our largest and smallest fish from above
                for (int i = 1; i < fishies.Length; i++) 
                {                                        
                    if (large2 < fishies[i] && fishies[i] < large) 
                        large2 = fishies[i];
                    if (small2 > fishies[i] && fishies[i] > small) 
                        small2 = fishies[i]; 
                }

                //Same as previous, plus one more condition. I have little doubt there's a better way to do all of this
                //Since this is all based on classwork, I'm not going to look into that. Keeping to known material for now.
                for (int i = 1; i < fishies.Length; i++) 
                {
                    if (large3 < fishies[i] && fishies[i] < large && fishies[i] < large2)
                        large3 = fishies[i];
                    if (small3 > fishies[i] && fishies[i] > small && fishies[i] > small2)
                        small3 = fishies[i];
                }
            }
            catch
            {
                valid = false; //If something breaks, this will stop everything after the method call from happening
            }
        }
    }
}
