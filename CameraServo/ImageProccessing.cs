using mvIMPACT_NET;
using mvIMPACT_NET.acquire;
using System;
using System.IO;
using System.Globalization;
using CameraServo;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;


namespace IliasPatsiaourasImageProccessing
{
    public class ImageProccessing
    {
        
        public class Robot
        {
            public string name;
            public double sign;           
            public int ID;
            public LED bigLed;
            public LED smallLed;
            public bool Exist;
            public double calculatedSign;
            public double x, y;//robot position (mm)
            public double x_old, y_old; //robot z^-1 position (mm)
            public double u, v; //robot speed (mm/s)
            public double theta;
            public double th_old;
            public double omega;

            //robot constructor
            public Robot(string Name, double Sign, int Port)
            {
                this.name = Name;
                this.ID = Port;
                this.sign = Sign;
                bigLed = new LED();
                smallLed = new LED();
                Exist = false;
            }

            public override string ToString()
            {
                return (name + "   Sign: " + sign.ToString() + "   ID: " + ID.ToString());
            }
        }

        public class LED
        {
            public int xSum, ySum; //variables used to store the sum of x,y for center of weight calc
            public int px_count;
            public int max_width;
            public int height;
            public double x, y;  //image frame Pixel coords
            public double xw, yw; //real world coords (mm)
            public bool assigned;
            public bool isUptodate;

            //instance constructor
            public LED ()
            {
                xSum = 0; 
                ySum = 0;
                px_count = 0;
                max_width = 0;
                height = 0;
                xw = -1000.0;//something not realistic
                yw = -1000.0;
                x = -1000.0;
                y = -1000.0;
                assigned = false; //indicates if this led is led of a robot
                isUptodate = false;
            }

            //copy constructor
            public LED (LED led)
            {
                xSum = led.xSum;
                ySum = led.ySum;
                px_count = led.px_count;
                max_width = led.max_width;
                height = led.height;
                x = led.x;
                y = led.y;
                xw = led.xw;
                yw = led.yw;
                isUptodate = led.isUptodate;
                assigned = led.assigned;
            }

            //Assign Reference of led this led and marked as assigned
            public LED assign()
            {
                this.assigned = true;
                return this;
            }
        }

        // Flow control values
        public bool fullScanRequired;
        public int ledNum;        
        public int rbtNum;
        public List<LED> ledFound;
        public Robot[] rbt;

        // Robot finding Values 
        public double tolerance;

        // Image Proccessing Values
        public byte threshold;
        public byte saw_px;
        public int r_dzone;
        public int l_dzone;
        public int d_dzone;
        public int u_dzone;

        //files
        double[,] xCoord = new double[1600, 1200];
        double[,] yCoord = new double[1600, 1200];

        //constructor
        public ImageProccessing(int ledNumPerRobot,Robot[] robots)//params double[] robotSignslist)
        {
            //Init Flow control values
            fullScanRequired  = true; // Start with full scan
            ledNum    = ledNumPerRobot*robots.Length;//need check !!!!!!!!!!!!!!!!!!!!!!!

            ledFound  = new List<LED>();
            ledFound.Capacity = ledNum + 50;

            rbtNum = robots.Length;

            // default values that is editable
            // Robot finding Values             
            tolerance = 10.0;//mm
            //Init Image Proccessing Values
            threshold = (byte)210;           
            saw_px    = (byte)1;
            r_dzone = 150;//150
            l_dzone = 150;//150
            d_dzone = 5;//5
            u_dzone = 15;//15

            //initialize robots
            rbt = new Robot[rbtNum];
            for (int s = 0; s < rbtNum; s++)
            {
                for (int t = s + 1; t < rbtNum; t++)
                {
                    //check that every sign is different enough
                    if (Math.Abs(robots[s].sign - robots[t].sign) <= tolerance)
                    {
                        MessageBox.Show(String.Format("\t\t\tWARNING:\n"+
                            "{0} and {1} has ambiguous signs,\n"+
                        "differnce must be at least: {2}mm (tolerance value)"
                            , robots[s].name, robots[t].name, tolerance));
                    }
                }
                rbt[s] = new Robot(robots[s].name, robots[s].sign, robots[s].ID);
            }  
             

            //load lookup table files
            loadlookupFile("fileX.txt", "fileY.txt");
        }


        public void loadlookupFile(string filenameX, string filenameY) // run in the constructor of the Class
        {

            int counterX = 0;
            int counterY = 0;
            string line;

            try
            {
                message(String.Format("loading calibration files."));

                System.IO.StreamReader fileX = new System.IO.StreamReader(".\\" + filenameX);
                while ((line = fileX.ReadLine()) != null)
                {
                    string[] values = line.Split('\t');
                    for (int x = 0; x < 1600; x++)
                    {
                        xCoord[x, counterX] = double.Parse(values[x], CultureInfo.InvariantCulture);
                    }
                    counterX++;
                }

                fileX.Close();

                message(".");

                System.IO.StreamReader fileY = new System.IO.StreamReader(".\\" + filenameY);
                while ((line = fileY.ReadLine()) != null)
                {
                    string[] values = line.Split('\t');
                    for (int x = 0; x < 1600; x++)
                    {
                        yCoord[x, counterY] = double.Parse(values[x], CultureInfo.InvariantCulture);
                    }
                    counterY++;
                }

                fileY.Close();

                message(".");

                if (counterY != 1200)
                    message("fileY corrupted");
                if (counterX != 1200)
                    message("fileX corrupted");
            }
            catch (Exception ex)
            {
                message((String.Format("exeption: {0}", ex)));
            }
            //debug 
            //message(String.Format("DEBUG: x[1598, 1198]: {0}" + "y[1598, 1198]: {1}", xCoord[1598, 1198], yCoord[1598, 1198]));
        }


        public void scanForLeds(Image image)//update the ledFound List 
        {
            ledFound.Clear();//reset list for clean data after every run of this
            int padding = 0;
            int icenter = 0;
            int left = 0;
            int right = 0;
            int width = 0;
            double center = 0.0;
            int n = 0, rej = 0;
            
            //lock buffer and grant Access to buffer
            image.prepareReadWriteAccess();

            for (int y = u_dzone + 1; y < image.height -1 - d_dzone; y++)
            {
                for (int x = l_dzone + 1; x < image.width -1 - r_dzone; x++)
                {
                    //led finding
                    if (image.getPixel(x, y) >= threshold)
                    {
                        //reset values for the new led
                        padding = 0;
                        center = 0.0;
                        icenter = 0;
                        ledFound.Insert(n, new LED()); //add a fresh led to the list/ or if the previous rejected re-intialize it

                        //px of led finding
                        while (image.getPixel(x + icenter, y + padding) >= threshold)
                        {
                            //image.drawPoint(new Point(x + icenter, y + padding), saw_px);
                            image.setPixel(x + icenter, y + padding, saw_px);
                            ledFound[n].ySum += y + padding;                              //adding the coords for center of led calc
                            ledFound[n].xSum += x + icenter;

                            //right search
                            right = 0;
                            while (image.getPixel(x + icenter + right + 1, y + padding) >= threshold)
                            {                              
                                //image.drawPoint(new Point(x + icenter + right + 1, y + padding), saw_px); 
                                image.setPixel(x + icenter + right + 1, y + padding, saw_px);
                                ledFound[n].ySum += y + padding;                          //adding the coords for center of led calc
                                ledFound[n].xSum += x + icenter + right + 1;
                                right++;
                                if (x + icenter + right + 1 > image.width -1 - r_dzone)     //compares the next index value to prevent index ovflow
                                    break;
                            }

                            //left search
                            left = 0;
                            while (image.getPixel(x + icenter - left - 1, y + padding) >= threshold)
                            {
                                //image.drawPoint(new Point(x + icenter -left -1, y + padding), saw_px); 
                                image.setPixel(x + icenter - left - 1, y + padding, saw_px);
                                ledFound[n].ySum += y + padding;                          //adding the coords for center of led calc
                                ledFound[n].xSum += x + icenter - left - 1;
                                left++;
                                if (x + icenter - left - 1 < l_dzone)             //compares the next index value to prevent index ovflow
                                    break;
                            }

                            width = left + right + 1;
                            ledFound[n].px_count += width;

                            if (width > ledFound[n].max_width) // statistics
                                ledFound[n].max_width = width;

                            center += (right - left) / 2.0;
                            icenter = (int)center;//?? it must work similar to Math.Truncate()                             

                            padding++;
                            if (y + padding > image.height -1 - d_dzone) break;
                                
                            ledFound[n].height = padding;
                        }//end px of led finding

                        //center of led calc: subtracted the coords sum with count
                        ledFound[n].y = (double)ledFound[n].ySum / (double)ledFound[n].px_count;
                        ledFound[n].x = (double)ledFound[n].xSum / (double)ledFound[n].px_count;
                        image.setPixel((int)ledFound[n].x, (int)ledFound[n].y, threshold - 1);// drawpoint similar method
                        
                        //led check 
                        if ((ledFound[n].px_count > 4) && (ledFound[n].px_count < 1000))
                        {
                            ledFound[n].isUptodate = true;
                            n++; //this will create a new LED in next round or if its the last one it will convert zero-based index to led count
                        }
                        else
                        {
                            ledFound.RemoveAt(n); //ensures that ledFound.length corresponds only in real led and no rejected pixels
                            rej++;//rejected counter  
                        }         
             

                    }//end led finding
                    
                    /*
                    if (ledFound.Count >= ledNum)//this check make full scan a bit faster if it finds all led just end the search
                    { //try it!! when robot is in upper left corner is fast when in down right has to scan all image
                        image.releaseAccess();
                        return;
                    }*/
               
                }//end x loop
            }//end y loop
            
            //if didnt found all leds exit from here

            //unlock buffer and stop Access to buffer
            image.releaseAccess();            
            //ledFound = n;
        }


        public void updateLed(Image image, LED led, int winWidth, int winHeight) //returns number of leds
        {
            int padding = 0;
            int icenter = 0;
            int left = 0;
            int right = 0;
            int width = 0;
            double center = 0.0;
            int n=0, rej = 0;

            int bufferLength = 5;
 
            LED[] ledh = new LED[bufferLength];
            for (int i = 0; i < bufferLength; i++)
            {
                ledh[i] = new LED();
            }

            //lock buffer and grant Access to buffer
            image.prepareReadWriteAccess();
            
            //???????the following limits is not checked if they conforms with the dead zones so
            //??????? near deadzones there is an oscilation between full scan and history based scan
            //??????? due to the break;s in the while of this method
            for (int y = (int)led.y - winHeight / 2; y <= ((int)led.y + winHeight / 2); y++)
            {
                for (int x = (int)led.x - winWidth / 2; x <= ((int)led.x + winWidth / 2); x++)
                {
                    //led finding
                    if (image.getPixel(x, y) >= threshold)
                    {
                        //reset values for the new led
                        padding = 0;
                        center = 0.0;
                        icenter = 0;
                        
                        //px of led finding
                        while (image.getPixel(x + icenter, y + padding) >= threshold)
                        {
                            //image.drawPoint(new Point(x + icenter, y + padding), saw_px);
                            Debug.WriteLine(String.Format("n: {0}", n));
                            image.setPixel(x + icenter, y + padding, saw_px);
                            ledh[n].ySum += y + padding;                              //adding the coords for center of led calc
                            ledh[n].xSum += x + icenter;

                            //right search
                            right = 0;
                            while (image.getPixel(x + icenter + right + 1, y + padding) >= threshold)
                            {
                                //image.drawPoint(new Point(x + icenter + right + 1, y + padding), saw_px); 
                                image.setPixel(x + icenter + right + 1, y + padding, saw_px);
                                ledh[n].ySum += y + padding;                          //adding the coords for center of led calc
                                ledh[n].xSum += x + icenter + right + 1;
                                right++;
                                if (x + icenter + right + 1 > image.width -1 - r_dzone)     //compares the next index value to prevent index ovflow
                                    break;
                            }

                            //left search
                            left = 0;
                            while (image.getPixel(x + icenter - left - 1, y + padding) >= threshold)
                            {
                                //image.drawPoint(new Point(x + icenter -left -1, y + padding), saw_px); 
                                image.setPixel(x + icenter - left - 1, y + padding, saw_px);
                                ledh[n].ySum += y + padding;                          //adding the coords for center of led calc
                                ledh[n].xSum += x + icenter - left - 1;
                                left++;
                                if (x + icenter - left - 1 < l_dzone)             //compares the next index value to prevent index ovflow
                                    break;
                            }

                            width = left + right + 1;
                            ledh[n].px_count += width;

                            if (width > ledh[n].max_width)
                                ledh[n].max_width = width;

                            center += (right - left) / 2.0;
                            icenter = (int)center;//?? it must work similar to Math.Truncate()                             

                            padding++;
                            if (y + padding > image.width -1 - d_dzone) break;

                            ledh[n].height = padding;
                        }//end px of led finding


                        ledh[n].y = (double)ledh[n].ySum / (double)ledh[n].px_count;           //center of led calc: subtracted the coords sum with count
                        ledh[n].x = (double)ledh[n].xSum / (double)ledh[n].px_count;//?????? maybe needs to be double for accurate center or doesnt work                     
                        image.setPixel((int)ledh[n].x, (int)ledh[n].y, threshold - 1);
                        //image.drawPoint(new Point(led[n].x, led[n].y), threshold - 1); 

                        //led check

                        if ((ledh[n].px_count > 4) && (ledh[n].px_count < 3000))
                        {
                            n++;
                            if (n >= bufferLength) return;//preventing from throw exception: ledh out of index
                        }
                        else
                            rej++;//rejected counter                                       


                    }//end led finding

                    //if (n == 1)           //IDEA: check if the led is found to return faster
                        //return ledh[0];

                }//end x loop
            }//end y loop

            image.releaseAccess();          //unlock buffer and stop Access to buffer

            if (n == 1)
            {
                led.x = ledh[0].x;                 //is alway the first the real one
                led.y = ledh[0].y;
                led.xSum = ledh[0].xSum;
                led.ySum = ledh[0].ySum;
                led.max_width = ledh[0].max_width;
                led.px_count = ledh[0].px_count;
                led.height = ledh[0].height;
                led.isUptodate = true;
                //led.assigned
            }
            else
            {
                led.isUptodate = false;
                //if a led dissapear and is not belong to a robot is ignored 
                //led = new LED(); // reset led 
                //ledFound.Remove(led);
                //robot update check if led isUptodate and raise fullScanRequired flag
            }           
        }


        public void calcLedsRealCoords()//reference pass
        {
            foreach (LED l in ledFound)
            {
                if (l.isUptodate) //prevent this method to waist time in out-dated leds
                {
                    //BILINEAR INTERPOLATION

                    //if l.x or l.y is integer Bilinear interpolation will fail (zero devide)
                    if (  ((l.x % 1)==0)  ||  ((l.y % 1)==0)  )
                    {
                        l.xw = xCoord[(int)Math.Round(l.x), (int)Math.Round(l.y)];
                        l.yw = yCoord[(int)Math.Round(l.x), (int)Math.Round(l.y)];
                    }
                    else
                    {
                        // bilinear interpolation
                        int x0 = (int)Math.Floor(l.x);
                        int x1 = (int)Math.Ceiling(l.x);
                        int y0 = (int)Math.Floor(l.y);
                        int y1 = (int)Math.Ceiling(l.y);

                        double a0 = xCoord[x0, y1];
                        double a1 = xCoord[x1, y1];
                        double a2 = xCoord[x0, y0];
                        double a3 = xCoord[x1, y0];

                        double b0 = yCoord[x0, y1];
                        double b1 = yCoord[x1, y1];
                        double b2 = yCoord[x0, y0];
                        double b3 = yCoord[x1, y0];

                        double N = (x1 - x0) * (y1 - y0);
                        double Na = (x1 - l.x) * (l.y - y0) / N;
                        double Nb = (l.x - x0) * (l.y - y0) / N;
                        double Nc = (x1 - l.x) * (y1 - l.y) / N;
                        double Nd = (l.x - x0) * (y1 - l.y) / N;

                        l.xw = a0 * Na + a1 * Nb + a2 * Nc + a3 * Nd;
                        l.yw = b0 * Na + b1 * Nb + b2 * Nc + b3 * Nd;
                    }

                    message(String.Format("real Coords: x:{0} , y:{1}", l.xw, l.yw));
                }
            }
        }


        public void ScanForRobots() //this assignes the led to robots searching all found LEDs
        {
            //numLeds number of leds 
            double[,] dist = new double[ledFound.Count, ledFound.Count];
            
            //creates an lower trigonic array with all available distances
            for (int j = 0; j < ledFound.Count; j++) 
            {
                for (int i = j + 1; i < ledFound.Count; i++)
                {
                    dist[i, j] = Math.Sqrt(Math.Pow(ledFound[i].xw - ledFound[j].xw, 2) + Math.Pow(ledFound[i].yw - ledFound[j].yw, 2));

                    for (int k = 0; k < rbtNum; k++)
                    {
                        //Robot identification
                        if (Math.Abs(dist[i, j] - rbt[k].sign) <= tolerance)
                        {
                            if (ledFound[i].px_count > ledFound[j].px_count)
                            {
                                rbt[k].bigLed = ledFound[i].assign(); //pass ledfound by reference to bigLed and change led property "assigned" to true
                                rbt[k].smallLed = ledFound[j].assign();
                            }
                            else
                            {
                                rbt[k].bigLed = ledFound[j].assign();
                                rbt[k].smallLed = ledFound[i].assign();
                            }
                            rbt[k].Exist = true;
                            rbt[k].calculatedSign = dist[i, j];
                            message(String.Format("Scan found robot: {0}, bigled.assigned:{1}, smallLed.assigned:{2}", k, rbt[k].bigLed.assigned, rbt[k].smallLed.assigned));
                        }
                    }
                }
            }
        }


        public void updateRobot(Robot rob, double refreshRate)
        {
            if (rob.bigLed.isUptodate && rob.smallLed.isUptodate)
            {
                rob.calculatedSign = Math.Sqrt(Math.Pow(rob.smallLed.xw - rob.bigLed.xw, 2) + Math.Pow(rob.smallLed.yw - rob.bigLed.yw, 2));

                if (Math.Abs(rob.calculatedSign - rob.sign) <= tolerance) //checks that is still the correct robot
                {
                    rob.Exist = true;
                    
                    //position
                    rob.x = rob.bigLed.xw;
                    rob.y = rob.bigLed.yw;
                    rob.theta = Math.Atan2(rob.smallLed.yw - rob.bigLed.yw, rob.smallLed.xw - rob.bigLed.xw);
                    message(String.Format("{0} exist and updated with    X:{1}   Y:{2}   Th:{3}   calculated Sign:{4}",rob.name, rob.x, rob.y, rob.theta, rob.calculatedSign));
                    
                    //speed
                    if (rob.x_old == null || rob.y_old == null || rob.th_old == null)
                    {
                        //first time scenario
                        rob.u = 0;
                        rob.v = 0;
                        rob.omega = 0;
                    }
                    else
                    {
                        rob.u = (Math.Round(rob.x - rob.x_old) * refreshRate);
                        rob.v = (Math.Round(rob.y - rob.y_old) * refreshRate);
                        rob.omega = (Math.Round(rob.theta - rob.th_old, 2) * refreshRate);
                    }

                    //prepare next step
                    rob.x_old = rob.x;
                    rob.y_old = rob.y;
                    rob.th_old = rob.theta;
                }
                else
                {
                    rob.Exist = false;
                    //speed consideration????????
                    fullScanRequired = true;
                    message(String.Format("Robot Sign Verification Failed"));
                }

            }
            else
            {
                rob.Exist = false;
                fullScanRequired = true;
                message(String.Format("one or both Robot led is not up-to-date"));
            }

        }


        public void message(string str)
        {
            Debug.WriteLine(str);
        }
    
    }
}
