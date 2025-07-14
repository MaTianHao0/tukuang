using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Rhino.FileIO;
using Rhino.Geometry;

namespace TukuangGH
{
    /// <summary>
    /// Helper class for reading DWG files and extracting geometry
    /// </summary>
    public static class DwgReader
    {
        /// <summary>
        /// Read curves from a DWG file
        /// </summary>
        /// <param name="filePath">Path to the DWG file</param>
        /// <returns>List of curves found in the DWG file</returns>
        public static List<Curve> ReadCurvesFromDwg(string filePath)
        {
            var curves = new List<Curve>();
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"DWG file not found: {filePath}");
            }

            try
            {
                // Use Rhino's file reading capabilities
                var file3dm = File3dm.Read(filePath);
                if (file3dm != null)
                {
                    foreach (var obj in file3dm.Objects)
                    {
                        var geometry = obj.Geometry;
                        
                        // Extract curves
                        if (geometry is Curve curve)
                        {
                            curves.Add(curve.DuplicateCurve());
                        }
                        else if (geometry is PolylineCurve polyline)
                        {
                            curves.Add(polyline.DuplicateCurve());
                        }
                        else if (geometry is LineCurve line)
                        {
                            curves.Add(line.DuplicateCurve());
                        }
                        else if (geometry is ArcCurve arc)
                        {
                            curves.Add(arc.DuplicateCurve());
                        }
                        else if (geometry is NurbsCurve nurbs)
                        {
                            curves.Add(nurbs.DuplicateCurve());
                        }
                        // You could add more geometry types as needed
                    }
                    
                    file3dm.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading DWG file: {ex.Message}");
            }

            return curves;
        }

        /// <summary>
        /// Get the embedded DWG file path for a given paper size
        /// </summary>
        /// <param name="paperSize">Paper size enum</param>
        /// <returns>Path to the DWG file</returns>
        public static string GetDwgFilePath(PaperFrameComponent.PaperSize paperSize)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            string dwgDirectory = Path.Combine(assemblyDirectory, "Resources", "DWG");
            
            string fileName = $"{paperSize}.dwg";
            return Path.Combine(dwgDirectory, fileName);
        }

        /// <summary>
        /// Check if DWG file exists for the given paper size
        /// </summary>
        /// <param name="paperSize">Paper size to check</param>
        /// <returns>True if file exists</returns>
        public static bool DwgFileExists(PaperFrameComponent.PaperSize paperSize)
        {
            string filePath = GetDwgFilePath(paperSize);
            return File.Exists(filePath);
        }
    }
}