using System;
using System.Collections.Generic;
using Rhino.FileIO;
using Rhino.Geometry;

namespace TukuangGH
{
    /// <summary>
    /// Helper class to create sample frame files for testing
    /// </summary>
    public static class SampleFrameGenerator
    {
        /// <summary>
        /// Create sample frame files for all paper sizes
        /// </summary>
        public static void CreateSampleFrames(string outputDirectory)
        {
            var paperSizes = new[] { 
                PaperFrameComponent.PaperSize.A0, 
                PaperFrameComponent.PaperSize.A1, 
                PaperFrameComponent.PaperSize.A2, 
                PaperFrameComponent.PaperSize.A3, 
                PaperFrameComponent.PaperSize.A4, 
                PaperFrameComponent.PaperSize.A5 
            };

            foreach (var size in paperSizes)
            {
                CreateSampleFrame(size, outputDirectory);
            }
        }

        /// <summary>
        /// Create a sample frame file for a specific paper size
        /// </summary>
        private static void CreateSampleFrame(PaperFrameComponent.PaperSize paperSize, string outputDirectory)
        {
            var dimensions = GetPaperDimensions(paperSize);
            double width = dimensions.Item1;
            double height = dimensions.Item2;

            var file3dm = new File3dm();
            
            // Create outer frame
            var outerCorners = new Point3d[]
            {
                new Point3d(0, 0, 0),
                new Point3d(width, 0, 0),
                new Point3d(width, height, 0),
                new Point3d(0, height, 0),
                new Point3d(0, 0, 0)
            };
            var outerFrame = new PolylineCurve(outerCorners);
            file3dm.Objects.AddCurve(outerFrame);

            // Create title block (bottom right)
            double titleWidth = Math.Min(width * 0.25, 200); // Max 200mm
            double titleHeight = Math.Min(height * 0.15, 50); // Max 50mm
            var titleCorners = new Point3d[]
            {
                new Point3d(width - titleWidth, 0, 0),
                new Point3d(width, 0, 0),
                new Point3d(width, titleHeight, 0),
                new Point3d(width - titleWidth, titleHeight, 0),
                new Point3d(width - titleWidth, 0, 0)
            };
            var titleBlock = new PolylineCurve(titleCorners);
            file3dm.Objects.AddCurve(titleBlock);

            // Create inner margin
            double margin = 10;
            var innerCorners = new Point3d[]
            {
                new Point3d(margin, margin, 0),
                new Point3d(width - margin, margin, 0),
                new Point3d(width - margin, height - margin, 0),
                new Point3d(margin, height - margin, 0),
                new Point3d(margin, margin, 0)
            };
            var innerFrame = new PolylineCurve(innerCorners);
            file3dm.Objects.AddCurve(innerFrame);

            // Add some additional frame details for professional look
            AddFrameDetails(file3dm, width, height, titleWidth, titleHeight);

            // Save the file
            string fileName = $"{paperSize}.3dm";
            string filePath = System.IO.Path.Combine(outputDirectory, fileName);
            file3dm.Write(filePath, 7); // Rhino 7 format
            file3dm.Dispose();
        }

        /// <summary>
        /// Add additional frame details like revision triangles, measurement marks, etc.
        /// </summary>
        private static void AddFrameDetails(File3dm file3dm, double width, double height, double titleWidth, double titleHeight)
        {
            // Add revision triangle in top right
            var revTriangle = new Point3d[]
            {
                new Point3d(width - 20, height, 0),
                new Point3d(width, height, 0),
                new Point3d(width, height - 20, 0),
                new Point3d(width - 20, height, 0)
            };
            var revTriangleCurve = new PolylineCurve(revTriangle);
            file3dm.Objects.AddCurve(revTriangleCurve);

            // Add measurement marks along edges (every 100mm)
            for (double x = 100; x < width; x += 100)
            {
                // Bottom edge marks
                var bottomMark = new LineCurve(new Point3d(x, 0, 0), new Point3d(x, 5, 0));
                file3dm.Objects.AddCurve(bottomMark);
                
                // Top edge marks
                var topMark = new LineCurve(new Point3d(x, height, 0), new Point3d(x, height - 5, 0));
                file3dm.Objects.AddCurve(topMark);
            }

            for (double y = 100; y < height; y += 100)
            {
                // Left edge marks
                var leftMark = new LineCurve(new Point3d(0, y, 0), new Point3d(5, y, 0));
                file3dm.Objects.AddCurve(leftMark);
                
                // Right edge marks
                var rightMark = new LineCurve(new Point3d(width, y, 0), new Point3d(width - 5, y, 0));
                file3dm.Objects.AddCurve(rightMark);
            }

            // Add title block subdivisions
            double titleX = width - titleWidth;
            double titleY = titleHeight;
            
            // Horizontal lines in title block
            for (int i = 1; i < 4; i++)
            {
                double y = (titleY / 4) * i;
                var hLine = new LineCurve(new Point3d(titleX, y, 0), new Point3d(width, y, 0));
                file3dm.Objects.AddCurve(hLine);
            }

            // Vertical line in title block
            double titleMidX = titleX + titleWidth / 2;
            var vLine = new LineCurve(new Point3d(titleMidX, 0, 0), new Point3d(titleMidX, titleY, 0));
            file3dm.Objects.AddCurve(vLine);
        }

        /// <summary>
        /// Get paper dimensions in millimeters (width, height)
        /// </summary>
        private static Tuple<double, double> GetPaperDimensions(PaperFrameComponent.PaperSize paperSize)
        {
            switch (paperSize)
            {
                case PaperFrameComponent.PaperSize.A0: return new Tuple<double, double>(841, 1189);
                case PaperFrameComponent.PaperSize.A1: return new Tuple<double, double>(594, 841);
                case PaperFrameComponent.PaperSize.A2: return new Tuple<double, double>(420, 594);
                case PaperFrameComponent.PaperSize.A3: return new Tuple<double, double>(297, 420);
                case PaperFrameComponent.PaperSize.A4: return new Tuple<double, double>(210, 297);
                case PaperFrameComponent.PaperSize.A5: return new Tuple<double, double>(148, 210);
                default: return new Tuple<double, double>(297, 420); // Default to A3
            }
        }
    }
}