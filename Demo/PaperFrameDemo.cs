using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace TukuangGH.Demo
{
    /// <summary>
    /// Demo script showing how the Paper Frame component would work
    /// This can be run independently to test the geometry generation logic
    /// </summary>
    public class PaperFrameDemo
    {
        public static void Main()
        {
            Console.WriteLine("Tukuang Paper Frame Component Demo");
            Console.WriteLine("==================================");

            // Test different paper sizes
            var testCases = new[]
            {
                new { Size = PaperFrameComponent.PaperSize.A4, Point = new Point3d(0, 0, 0), Scale = 1.0, Rotation = 0.0 },
                new { Size = PaperFrameComponent.PaperSize.A3, Point = new Point3d(300, 0, 0), Scale = 0.5, Rotation = 0.0 },
                new { Size = PaperFrameComponent.PaperSize.A2, Point = new Point3d(0, 300, 0), Scale = 1.0, Rotation = 45.0 },
            };

            foreach (var test in testCases)
            {
                Console.WriteLine($"\nTesting {test.Size} frame:");
                Console.WriteLine($"  Reference Point: {test.Point}");
                Console.WriteLine($"  Scale: {test.Scale}");
                Console.WriteLine($"  Rotation: {test.Rotation}°");

                try
                {
                    var curves = SimulateFrameGeneration(test.Point, test.Size, test.Scale, test.Rotation);
                    Console.WriteLine($"  Generated {curves.Count} curves successfully");
                    
                    // Display curve information
                    for (int i = 0; i < curves.Count; i++)
                    {
                        var curve = curves[i];
                        var bbox = curve.GetBoundingBox(false);
                        Console.WriteLine($"    Curve {i + 1}: Length={curve.GetLength():F1}mm, BBox=({bbox.Min.X:F1},{bbox.Min.Y:F1}) to ({bbox.Max.X:F1},{bbox.Max.Y:F1})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ERROR: {ex.Message}");
                }
            }

            Console.WriteLine("\nDemo completed. Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Simulate the frame generation process
        /// </summary>
        private static List<Curve> SimulateFrameGeneration(Point3d referencePoint, PaperFrameComponent.PaperSize paperSize, double scale, double rotation)
        {
            // This simulates what the actual component would do
            var dimensions = GetPaperDimensions(paperSize);
            double width = dimensions.Item1;
            double height = dimensions.Item2;

            var curves = new List<Curve>();

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
            curves.Add(outerFrame);

            // Create title block
            double titleWidth = Math.Min(width * 0.25, 200);
            double titleHeight = Math.Min(height * 0.15, 50);
            var titleCorners = new Point3d[]
            {
                new Point3d(width - titleWidth, 0, 0),
                new Point3d(width, 0, 0),
                new Point3d(width, titleHeight, 0),
                new Point3d(width - titleWidth, titleHeight, 0),
                new Point3d(width - titleWidth, 0, 0)
            };
            var titleBlock = new PolylineCurve(titleCorners);
            curves.Add(titleBlock);

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
            curves.Add(innerFrame);

            // Apply transformations
            ApplyTransformations(curves, scale, rotation, referencePoint);

            return curves;
        }

        private static void ApplyTransformations(List<Curve> curves, double scale, double rotation, Point3d referencePoint)
        {
            // Apply scale
            if (Math.Abs(scale - 1.0) > 0.01)
            {
                var scaleTransform = Transform.Scale(Point3d.Origin, scale);
                foreach (var curve in curves)
                {
                    curve.Transform(scaleTransform);
                }
            }

            // Apply rotation
            if (Math.Abs(rotation) > 0.01)
            {
                var rotationTransform = Transform.Rotation(Math.PI * rotation / 180.0, Vector3d.ZAxis, Point3d.Origin);
                foreach (var curve in curves)
                {
                    curve.Transform(rotationTransform);
                }
            }

            // Apply translation
            if (referencePoint != Point3d.Origin)
            {
                var translationTransform = Transform.Translation(new Vector3d(referencePoint));
                foreach (var curve in curves)
                {
                    curve.Transform(translationTransform);
                }
            }
        }

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
                default: return new Tuple<double, double>(297, 420);
            }
        }
    }
}