using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Frequecny_Editor
{
    public partial class frmMain : Form
    {
        List <Bitmap> images = new List<Bitmap>();


        public frmMain()
        {
            InitializeComponent();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            Bitmap input;

            openFile.ShowDialog();
            openFile.Filter = "JPEG|*.jpg";
            foreach (String file in openFile.FileNames)
            {
                input = (Bitmap)Image.FromFile(file);
                images.Add(input);
                lstInputImages.Items.Add(new ListViewItem(file));
            }
        }
        private void modifyAllImages()
        {
            String currentDirectory = System.IO.Directory.GetCurrentDirectory();
            String outputDirectory = System.IO.Path.Combine(currentDirectory, "output");
            String outputName = "output";
            String inputName;
            string pathString;
            int i = 0;
            Bitmap currentImage;
            Bitmap output;

            System.IO.Directory.CreateDirectory(outputDirectory);
            while (images.Count<Bitmap>() > 0)
            {
                outputName = "output_" + i.ToString() + ".jpg";
                inputName = "input_" + i.ToString() + ".jpg";

                pathString = System.IO.Path.Combine(outputDirectory, "output_" + i.ToString());
                System.IO.Directory.CreateDirectory(pathString);

                outputName = System.IO.Path.Combine(pathString, outputName);
                inputName = System.IO.Path.Combine(pathString, inputName);
                
                currentImage = images[0];

                this.Invoke((MethodInvoker)delegate
                {
                    pb1.Maximum = currentImage.Width;
                    pb1.Value = 0;
                });

                if (radHue.Checked == true)
                {
                    output = modifyImage(currentImage, "H");
                }
                else if (radValue.Checked == true)
                {
                    output = modifyImage(currentImage, "V");
                }
                else if (radRed.Checked == true)
                {
                    output = modifyImage(currentImage, "R");
                }
                else
                {
                    output = modifyImage(currentImage, "S");
                }

                output.Save(outputName);
                currentImage.Save(inputName);
                i++;
                images.RemoveAt(0);
                this.Invoke((MethodInvoker)delegate
                {
                    lstOutput.Items.Add(inputName);
                });
            }
        }
        private void btnMod_Click(object sender, EventArgs e)
        {
            Thread modThread = new Thread(modifyAllImages);
            modThread.Start();
        }
        private Bitmap modifyImage(Bitmap inputImage, String typeOfModification)
        {
            // Modifications are either 
            // H - Hue
            // S - Saturation
            // V - Value
            // R - Red
            // G - Green
            // B - Blue
            Bitmap output = new Bitmap(inputImage.Width, inputImage.Height);
            double[,] deviations = new double[inputImage.Width, inputImage.Height];
            Color[,] inputColours = new Color[inputImage.Width, inputImage.Height];
            Color deviatedColour;


            for (int x = 0; x < inputImage.Width; x++)
            {
                for (int y = 0; y < inputImage.Height; y++)
                {
                    inputColours[x, y] = inputImage.GetPixel(x, y);
                }
            }
            for (int x = 0; x < inputImage.Width - 1; x++)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pb1.PerformStep();
                });
                for (int y = 0; y < inputImage.Height - 1; y++)
                {
                    if (typeOfModification == "V")
                    {
                        deviations[x, y] = transformStandardDeviation(calcStandardDeviation(getNeighbourBrightnessArray(inputColours, x, y)));
                        deviatedColour = convertHSBtoRGB(inputColours[x, y].GetHue(), inputColours[x, y].GetSaturation(), deviations[x, y]);
                        output.SetPixel(x, y, deviatedColour);
                    }
                    else if (typeOfModification == "H")
                    {
                        deviations[x, y] = transformStandardDeviation(calcStandardDeviation(getNeighbourHueArray(inputColours, x, y)));
                        deviatedColour = convertHSBtoRGB(deviations[x, y], inputColours[x, y].GetHue(), inputColours[x, y].GetBrightness());
                        output.SetPixel(x, y, deviatedColour);
                    }
                    else if (typeOfModification == "S")
                    {
                        deviations[x, y] = transformStandardDeviation(calcStandardDeviation(getNeighbourSaturationArray(inputColours, x, y)));
                        deviatedColour = convertHSBtoRGB(inputColours[x, y].GetHue(), deviations[x, y], inputColours[x, y].GetBrightness());
                        output.SetPixel(x, y, deviatedColour);
                    }
                    else if (typeOfModification == "R")
                    {
                        deviations[x, y] = transformStandardDeviation(calcStandardDeviation(getNeighbourRedArray(inputColours, x, y)));
                        deviatedColour = Color.FromArgb(transformRGB(deviations[x, y]), inputColours[x, y].G, inputColours[x, y].B) ;
                        output.SetPixel(x, y, deviatedColour);
                    }
                    else if (typeOfModification == "G")
                    {

                    }
                    else
                    {
                        
                    }
                }

            }
            return output; 
        }
        private int transformRGB(double input)
        {
            // Author: Dustin Hu
            // Daet: 2016-03-02
            // Purpose: to map a double to RGB
            return Clamp((int)(input * 255));
        }
        private double transformStandardDeviation(double input)
        {
            // Applies a transformation to the standard deviation, with te output 0<=x <= .
            return Math.Abs(Math.Atan(input * input));
        }
        private double calcStandardDeviation(float[,] inputArray)
        {
            // Calculates the standard deviation

            // Std Deviation = sqrt(1/N * sum(x - mean)^2)
            double sum = 0;
            double mean = 0;
            double value;
            for (int i = 0; i < 9; i++)
            {
                mean = mean + inputArray[i, 0];
            }
            mean = mean / 9;

            for (int i = 0; i < inputArray.GetLength(1); i++)
            {

                sum = sum + (inputArray[i, 0] - mean) * (inputArray[i, 0] - mean);

            }
//            Console.Write(sum);

            value = (1 / 9);
            sum = sum / 9;
            value = Math.Sqrt((double) (sum)) * 100;

            return value ;
        }
        private double calcStandardDeviation(int[,] inputArray)
        {
            // Calculates the standard deviation

            // Std Deviation = sqrt(1/N * sum(x - mean)^2)
            double sum = 0;
            double mean = 0;
            double value;
            for (int i = 0; i < 9; i++)
            {
                mean = mean + inputArray[i, 0];
            }
            mean = mean / 9;

            for (int i = 0; i < inputArray.GetLength(1); i++)
            {

                sum = sum + (inputArray[i, 0] - mean) * (inputArray[i, 0] - mean);

            }
//            Console.Write(sum);

            value = (1 / 9);
            sum = sum / 9;
            value = Math.Sqrt((double) (sum)) * 100;

            return value ;
        }
        private float getBrightness(Color[,] inputArray, int x, int y)
        {
            // Gets the brightness of a pixel at a point
            return inputArray[x, y].GetBrightness();
        }
        private float getSaturation(Color[,] inputArray, int x, int y)
        {
            // Gets the Saturation of a pixel at a point
            return inputArray[x, y].GetSaturation();
        }
        private float getHue(Color[,] inputArray, int x, int y)
        {
            // Gets the brightness of a pixel at a point
            return inputArray[x, y].GetHue();
        }
        private int  getRed(Color[,] inputArray, int x, int y)
        {
            // Gets the brightness of a pixel at a point
            return inputArray[x, y].R;
        }

        private int  getGreen(Color[,] inputArray, int x, int y)
        {
            // Gets the brightness of a pixel at a point
            return inputArray[x, y].G;
        }

        private int[,] getNeighbourRedArray(Color[,] inputArray, int x, int y)
        {
            int[,] colorArray = new int [9, 1];
            // Check each pixel around it, using HSV
            // Put RGB of each pixel into colourArray, numebr it like suchx
            // 0 1 2 
            // 3 4 5
            // 6 7 8 
            for (int i = 0; i < 9; i++)
            {
                colorArray[i, 0] = 0;
            }
            colorArray[4, 0] = inputArray[x, y].R;
            // Check if left edge 
            if (x == 0 )
            {
                // Top left corner
                if (y == 0)
                {
                    colorArray[5, 0] = getRed(inputArray, x + 1, y);
                    colorArray[7, 0] = getRed(inputArray, x, y + 1);
                    colorArray[8, 0] = getRed(inputArray, x + 1, y + 1);
                    
                }
                // Bottom left corner
                else if (y == inputArray.GetLength(1) - 1)
                {
                    colorArray[1, 0] = getRed(inputArray, x, y - 1);
                    colorArray[2, 0] = getRed(inputArray, x + 1, y - 1);
                    colorArray[5, 0] = getRed(inputArray, x + 1, y);
                }
                else
                {
                    colorArray[1, 0] = getRed(inputArray, x, y - 1);
                    colorArray[7, 0] = getRed(inputArray, x, y + 1);
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[2 + (i * 3), 0] = getRed(inputArray, x + 1, y - 1 + i);
                    }
                }
            }
            // Check if right edge
            else if (x == inputArray.GetLength(0) - 1)
            {
                // Top right corner
                if (y == 0)
                {
                    colorArray[3, 0] = getRed(inputArray, x - 1, y);
                    colorArray[6, 0] = getRed(inputArray, x - 1, y + 1);
                    colorArray[7, 0] = getRed(inputArray, x, y + 1);
                }
                // Bottom right corner
                else if (y == inputArray.GetLength(2) - 1)
                {
                    colorArray[0, 0] = getRed(inputArray, x - 1, y - 1);
                    colorArray[1, 0] = getRed(inputArray, x, y - 1);
                    colorArray[3, 0] = getRed(inputArray, x - 1, y);

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[i * 3, 0] = getRed(inputArray, x - 1, y - 1 + i);
                    }
                    colorArray[1, 0] = getRed(inputArray, x, y - 1);
                    colorArray[7, 0] = getRed(inputArray, x, y + 1);
                }
            }
            // Check if top edge
            else if (y == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[6 + i, 0] = getRed(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getRed(inputArray, x - 1, y);
                colorArray[5, 0] = getRed(inputArray, x + 1, y);
            }
            // check if bottom edge
            else if (y == inputArray.GetLength(1) - 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[i, 0] = getRed(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getRed(inputArray, x - 1, y);
                colorArray[5, 0] = getRed(inputArray, x + 1, y);

            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    int currentX = x;
                    int currentY = y;
                    // Check values of x
                    if (i % 3 == 0)
                    {
                        currentX = currentX - 1;
                    }
                    else if (i % 3 == 1)
                    {
                    }
                    else
                    {
                        currentX = currentX + 1;
                    }

                    // check values of y
                    if (i < 3)
                    {
                        currentY = currentY - 1;
                    }
                    else if (2 < y && y < 6)
                    {
                    }
                    else
                    {
                        currentY = currentY + 1;
                    }
                    colorArray[i, 0] = getRed(inputArray, currentX, currentY);
                }
            }
            return colorArray;
        }
        private float[,] getNeighbourBrightnessArray(Color[,] inputArray, int x, int y)
        {
            float[,] colorArray = new float [9, 1];
            // Check each pixel around it, using HSV
            // Put RGB of each pixel into colourArray, numebr it like suchx
            // 0 1 2 
            // 3 4 5
            // 6 7 8 
            for (int i = 0; i < 9; i++)
            {
                colorArray[i, 0] = (float) 0.0;
            }
            colorArray[4, 0] = inputArray[x, y].GetBrightness();
            // Check if left edge 
            if (x == 0 )
            {
                // Top left corner
                if (y == 0)
                {
                    colorArray[5, 0] = getBrightness(inputArray, x + 1, y);
                    colorArray[7, 0] = getBrightness(inputArray, x, y + 1);
                    colorArray[8, 0] = getBrightness(inputArray, x + 1, y + 1);
                    
                }
                // Bottom left corner
                else if (y == inputArray.GetLength(1) - 1)
                {
                    colorArray[1, 0] = getBrightness(inputArray, x, y - 1);
                    colorArray[2, 0] = getBrightness(inputArray, x + 1, y - 1);
                    colorArray[5, 0] = getBrightness(inputArray, x + 1, y);
                }
                else
                {
                    colorArray[1, 0] = getBrightness(inputArray, x, y - 1);
                    colorArray[7, 0] = getBrightness(inputArray, x, y + 1);
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[2 + (i * 3), 0] = getBrightness(inputArray, x + 1, y - 1 + i);
                    }
                }
            }
            // Check if right edge
            else if (x == inputArray.GetLength(0) - 1)
            {
                // Top right corner
                if (y == 0)
                {
                    colorArray[3, 0] = getBrightness(inputArray, x - 1, y);
                    colorArray[6, 0] = getBrightness(inputArray, x - 1, y + 1);
                    colorArray[7, 0] = getBrightness(inputArray, x, y + 1);
                }
                // Bottom right corner
                else if (y == inputArray.GetLength(2) - 1)
                {
                    colorArray[0, 0] = getBrightness(inputArray, x - 1, y - 1);
                    colorArray[1, 0] = getBrightness(inputArray, x, y - 1);
                    colorArray[3, 0] = getBrightness(inputArray, x - 1, y);

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[i * 3, 0] = getBrightness(inputArray, x - 1, y - 1 + i);
                    }
                    colorArray[1, 0] = getBrightness(inputArray, x, y - 1);
                    colorArray[7, 0] = getBrightness(inputArray, x, y + 1);
                }
            }
            // Check if top edge
            else if (y == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[6 + i, 0] = getBrightness(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getBrightness(inputArray, x - 1, y);
                colorArray[5, 0] = getBrightness(inputArray, x + 1, y);
            }
            // check if bottom edge
            else if (y == inputArray.GetLength(1) - 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[i, 0] = getBrightness(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getBrightness(inputArray, x - 1, y);
                colorArray[5, 0] = getBrightness(inputArray, x + 1, y);

            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    int currentX = x;
                    int currentY = y;
                    // Check values of x
                    if (i % 3 == 0)
                    {
                        currentX = currentX - 1;
                    }
                    else if (i % 3 == 1)
                    {
                    }
                    else
                    {
                        currentX = currentX + 1;
                    }

                    // check values of y
                    if (i < 3)
                    {
                        currentY = currentY - 1;
                    }
                    else if (2 < y && y < 6)
                    {
                    }
                    else
                    {
                        currentY = currentY + 1;
                    }
                    colorArray[i, 0] = getBrightness(inputArray, currentX, currentY);
                }
            }
            return colorArray;
        }
        private float[,] getNeighbourSaturationArray(Color[,] inputArray, int x, int y)
        {
            float[,] colorArray = new float [9, 1];
            // Check each pixel around it, using HSV
            // Put RGB of each pixel into colourArray, numebr it like suchx
            // 0 1 2 
            // 3 4 5
            // 6 7 8 
            for (int i = 0; i < 9; i++)
            {
                colorArray[i, 0] = (float) 0.0;
            }
            colorArray[4, 0] = inputArray[x, y].GetSaturation();
            // Check if left edge 
            if (x == 0 )
            {
                // Top left corner
                if (y == 0)
                {
                    colorArray[5, 0] = getSaturation(inputArray, x + 1, y);
                    colorArray[7, 0] = getSaturation(inputArray, x, y + 1);
                    colorArray[8, 0] = getSaturation(inputArray, x + 1, y + 1);
                    
                }
                // Bottom left corner
                else if (y == inputArray.GetLength(1) - 1)
                {
                    colorArray[1, 0] = getSaturation(inputArray, x, y - 1);
                    colorArray[2, 0] = getSaturation(inputArray, x + 1, y - 1);
                    colorArray[5, 0] = getSaturation(inputArray, x + 1, y);
                }
                else
                {
                    colorArray[1, 0] = getSaturation(inputArray, x, y - 1);
                    colorArray[7, 0] = getSaturation(inputArray, x, y + 1);
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[2 + (i * 3), 0] = getSaturation(inputArray, x + 1, y - 1 + i);
                    }
                }
            }
            // Check if right edge
            else if (x == inputArray.GetLength(0) - 1)
            {
                // Top right corner
                if (y == 0)
                {
                    colorArray[3, 0] = getSaturation(inputArray, x - 1, y);
                    colorArray[6, 0] = getSaturation(inputArray, x - 1, y + 1);
                    colorArray[7, 0] = getSaturation(inputArray, x, y + 1);
                }
                // Bottom right corner
                else if (y == inputArray.GetLength(2) - 1)
                {
                    colorArray[0, 0] = getSaturation(inputArray, x - 1, y - 1);
                    colorArray[1, 0] = getSaturation(inputArray, x, y - 1);
                    colorArray[3, 0] = getSaturation(inputArray, x - 1, y);

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[i * 3, 0] = getSaturation(inputArray, x - 1, y - 1 + i);
                    }
                    colorArray[1, 0] = getSaturation(inputArray, x, y - 1);
                    colorArray[7, 0] = getSaturation(inputArray, x, y + 1);
                }
            }
            // Check if top edge
            else if (y == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[6 + i, 0] = getSaturation(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getSaturation(inputArray, x - 1, y);
                colorArray[5, 0] = getSaturation(inputArray, x + 1, y);
            }
            // check if bottom edge
            else if (y == inputArray.GetLength(1) - 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[i, 0] = getSaturation(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getSaturation(inputArray, x - 1, y);
                colorArray[5, 0] = getSaturation(inputArray, x + 1, y);

            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    int currentX = x;
                    int currentY = y;
                    // Check values of x
                    if (i % 3 == 0)
                    {
                        currentX = currentX - 1;
                    }
                    else if (i % 3 == 1)
                    {
                    }
                    else
                    {
                        currentX = currentX + 1;
                    }

                    // check values of y
                    if (i < 3)
                    {
                        currentY = currentY - 1;
                    }
                    else if (2 < y && y < 6)
                    {
                    }
                    else
                    {
                        currentY = currentY + 1;
                    }
                    colorArray[i, 0] = getSaturation(inputArray, currentX, currentY);
                }
            }
            return colorArray;
        }
        private float[,] getNeighbourHueArray(Color[,] inputArray, int x, int y)
        {
            float[,] colorArray = new float [9, 1];
            // Check each pixel around it, using HSV
            // Put RGB of each pixel into colourArray, numebr it like suchx
            // 0 1 2 
            // 3 4 5
            // 6 7 8 
            for (int i = 0; i < 9; i++)
            {
                colorArray[i, 0] = (float) 0.0;
            }
            colorArray[4, 0] = inputArray[x, y].GetHue();
            // Check if left edge 
            if (x == 0 )
            {
                // Top left corner
                if (y == 0)
                {
                    colorArray[5, 0] = getHue(inputArray, x + 1, y);
                    colorArray[7, 0] = getHue(inputArray, x, y + 1);
                    colorArray[8, 0] = getHue(inputArray, x + 1, y + 1);
                    
                }
                // Bottom left corner
                else if (y == inputArray.GetLength(1) - 1)
                {
                    colorArray[1, 0] = getHue(inputArray, x, y - 1);
                    colorArray[2, 0] = getHue(inputArray, x + 1, y - 1);
                    colorArray[5, 0] = getHue(inputArray, x + 1, y);
                }
                else
                {
                    colorArray[1, 0] = getHue(inputArray, x, y - 1);
                    colorArray[7, 0] = getHue(inputArray, x, y + 1);
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[2 + (i * 3), 0] = getHue(inputArray, x + 1, y - 1 + i);
                    }
                }
            }
            // Check if right edge
            else if (x == inputArray.GetLength(0) - 1)
            {
                // Top right corner
                if (y == 0)
                {
                    colorArray[3, 0] = getHue(inputArray, x - 1, y);
                    colorArray[6, 0] = getHue(inputArray, x - 1, y + 1);
                    colorArray[7, 0] = getHue(inputArray, x, y + 1);
                }
                // Bottom right corner
                else if (y == inputArray.GetLength(2) - 1)
                {
                    colorArray[0, 0] = getHue(inputArray, x - 1, y - 1);
                    colorArray[1, 0] = getHue(inputArray, x, y - 1);
                    colorArray[3, 0] = getHue(inputArray, x - 1, y);

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        colorArray[i * 3, 0] = getHue(inputArray, x - 1, y - 1 + i);
                    }
                    colorArray[1, 0] = getHue(inputArray, x, y - 1);
                    colorArray[7, 0] = getHue(inputArray, x, y + 1);
                }
            }
            // Check if top edge
            else if (y == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[6 + i, 0] = getHue(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getHue(inputArray, x - 1, y);
                colorArray[5, 0] = getHue(inputArray, x + 1, y);
            }
            // check if bottom edge
            else if (y == inputArray.GetLength(1) - 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorArray[i, 0] = getHue(inputArray, x - 1 + i, y + 1);
                }
                colorArray[3, 0] = getHue(inputArray, x - 1, y);
                colorArray[5, 0] = getHue(inputArray, x + 1, y);

            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    int currentX = x;
                    int currentY = y;
                    // Check values of x
                    if (i % 3 == 0)
                    {
                        currentX = currentX - 1;
                    }
                    else if (i % 3 == 1)
                    {
                    }
                    else
                    {
                        currentX = currentX + 1;
                    }

                    // check values of y
                    if (i < 3)
                    {
                        currentY = currentY - 1;
                    }
                    else if (2 < y && y < 6)
                    {
                    }
                    else
                    {
                        currentY = currentY + 1;
                    }
                    colorArray[i, 0] = getHue(inputArray, currentX, currentY);
                }
            }
            return colorArray;
        }
        private Color convertHSBtoRGB(double h, double s, double v)
        {
            double H = h;
            double R, G, B;
            while (H < 0)
            {
                H += 360;
            }
            while (H >= 360)
            {
                H = -360;
            }
            if (v <= 0)
            {
                R = G = B = 0;
            }
            else if (s <= 0)
            {
                R = G = B = v;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = v * (1 - s);
                double qv = v * (1 - s * f);
                double tv = v * (1 - s * (1 - f));
                switch (i)
                {
                    case 0:
                        R = v;
                        G = tv;
                        B = pv;
                        break;
                    case 1:
                        R = qv;
                        G = v;
                        B = pv;
                        break;

                    case 2:
                        R = pv;
                        G = v;
                        B = tv;
                        break;

                    case 3:
                        R = pv;
                        G = qv;
                        B = v;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = v;
                        break;

                    case 5:
                        R = v;
                        G = pv;
                        B = qv;
                        break;

                    case 6:
                        R = v;
                        G = tv;
                        B = pv;
                        break;

                    case -1:
                        R = v;
                        G = pv;
                        B = qv;
                        break;

                    default:
                        //LFATAL
                        R = G = B = v;
                        break;

                }


            }
            int outR, outG, outB;
                
            outR = Clamp((int)(R * 255.0));
            outG = Clamp((int)(G * 255.0));
            outB = Clamp((int)(B * 255.0));
            return Color.FromArgb(outR, outG, outB);
        }
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lstInputImages.View = View.List;
            lstOutput.View = View.List;
            folderBrowserDialog1.SelectedPath = System.IO.Directory.GetCurrentDirectory();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }
        
        private Color[,]
        private Color[,] getColorArray(Bitmap image)
        {
            Color[,] output = new Color[image.Width, image.Height]; 

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                   output[x, y] = image.GetPixel(x, y);
                }
            }
            return output;
        }
    }
}
