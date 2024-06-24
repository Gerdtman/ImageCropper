using System;
using System.Drawing;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        bool isPublished = File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ImageCropper.exe"));


        string inputDirectory;
        string outputDirectory;

        if (isPublished)
        {
            Console.WriteLine("Live version");
            // Running as published
            inputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input");
            outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
        }
        else
        {
            Console.WriteLine("Dev version");
            // Running in development
            inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "input");
            outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
        }

        double resizePercentageTwoX = 0.6;
        double resizePercentageOneX = 0.3;

        Directory.CreateDirectory(inputDirectory);
        Directory.CreateDirectory(outputDirectory);

        string[] imageFiles = Directory.GetFiles(inputDirectory, "*.*", SearchOption.TopDirectoryOnly);
        if (imageFiles.Length == 0)
        {
            Console.WriteLine("No images present in the input directory. Please add images into the input folder and run the app again");
            Console.ReadLine();
            return;
        }
        foreach (string imageFile in imageFiles)
        {
            try
            {
                using (Image image = Image.FromFile(imageFile))
                {
                    //Saves original image size as 3x
                    string outputFileName = Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(imageFile)}@3x.png");
                    image.Save(outputFileName);

                    int newWidth = (int)(image.Width * resizePercentageTwoX);
                    int newHeight = (int)(image.Height * resizePercentageTwoX);

                    using (Bitmap bitmap = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
                        }

                        outputFileName = Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(imageFile)}@2x.png");
                        bitmap.Save(outputFileName);
                    }

                    newWidth = (int)(image.Width * resizePercentageOneX);
                    newHeight = (int)(image.Height * resizePercentageOneX);

                    using (Bitmap bitmap = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
                        }

                        outputFileName = Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(imageFile)}.png");
                        bitmap.Save(outputFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {imageFile}: {ex.Message}");
            }
        }

        Console.WriteLine("Processing complete.");
        Console.ReadKey();
    }
}