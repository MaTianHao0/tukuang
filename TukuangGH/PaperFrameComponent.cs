using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace TukuangGH
{
    public class PaperFrameComponent : GH_Component
    {
        /// <summary>
        /// Paper frame sizes enum
        /// </summary>
        public enum PaperSize
        {
            A0, A1, A2, A3, A4, A5
        }

        /// <summary>
        /// Initializes a new instance of the PaperFrameComponent class.
        /// </summary>
        public PaperFrameComponent()
          : base("Paper Frame", "PFrame",
              "Generate paper frames from DWG files based on a reference point",
              "Tukuang", "Frame")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Reference Point", "Pt", "Reference point for frame positioning", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddIntegerParameter("Paper Size", "Size", "Paper size selection (0=A0, 1=A1, 2=A2, 3=A3, 4=A4, 5=A5)", GH_ParamAccess.item, 3);
            pManager.AddNumberParameter("Scale", "Scale", "Scale factor for the frame", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Rotation", "Rot", "Rotation angle in degrees", GH_ParamAccess.item, 0.0);

            // Set up the paper size parameter as a dropdown
            var sizeParam = pManager[1] as Param_Integer;
            if (sizeParam != null)
            {
                sizeParam.AddNamedValue("A0", 0);
                sizeParam.AddNamedValue("A1", 1);
                sizeParam.AddNamedValue("A2", 2);
                sizeParam.AddNamedValue("A3", 3);
                sizeParam.AddNamedValue("A4", 4);
                sizeParam.AddNamedValue("A5", 5);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Frame Curves", "Curves", "Paper frame geometry curves", GH_ParamAccess.list);
            pManager.AddTextParameter("Frame Info", "Info", "Information about the generated frame", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d referencePoint = Point3d.Origin;
            int paperSizeIndex = 3;
            double scale = 1.0;
            double rotation = 0.0;

            if (!DA.GetData(0, ref referencePoint)) return;
            if (!DA.GetData(1, ref paperSizeIndex)) return;
            if (!DA.GetData(2, ref scale)) return;
            if (!DA.GetData(3, ref rotation)) return;

            // Validate inputs
            if (paperSizeIndex < 0 || paperSizeIndex > 5)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Paper size index must be between 0 and 5");
                return;
            }

            if (scale <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Scale must be greater than 0");
                return;
            }

            PaperSize paperSize = (PaperSize)paperSizeIndex;
            
            try
            {
                var frameCurves = GenerateFrameGeometry(referencePoint, paperSize, scale, rotation);
                string frameInfo = $"Paper Size: {paperSize}, Scale: {scale:F2}, Rotation: {rotation:F1}°";

                DA.SetDataList(0, frameCurves);
                DA.SetData(1, frameInfo);
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error generating frame: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate frame geometry based on paper size and reference point
        /// </summary>
        private List<Curve> GenerateFrameGeometry(Point3d referencePoint, PaperSize paperSize, double scale, double rotation)
        {
            List<Curve> curves;

            // Try to read from DWG file first
            if (DwgReader.DwgFileExists(paperSize))
            {
                try
                {
                    curves = DwgReader.ReadCurvesFromDwg(DwgReader.GetDwgFilePath(paperSize));
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Loaded frame from DWG file: {paperSize}.dwg");
                }
                catch (Exception ex)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Failed to read DWG file, using default frame: {ex.Message}");
                    curves = GenerateDefaultFrameGeometry(paperSize, scale);
                }
            }
            else
            {
                // Use default geometry if no DWG file is available
                curves = GenerateDefaultFrameGeometry(paperSize, scale);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Using default frame geometry for {paperSize}");
            }

            // Apply transformations
            ApplyTransformations(curves, scale, rotation, referencePoint);

            return curves;
        }

        /// <summary>
        /// Generate default frame geometry when DWG file is not available
        /// </summary>
        private List<Curve> GenerateDefaultFrameGeometry(PaperSize paperSize, double scale)
        {
            // Get paper dimensions in millimeters
            var dimensions = GetPaperDimensions(paperSize);
            double width = dimensions.Item1;
            double height = dimensions.Item2;

            // Create basic frame rectangle
            var corners = new Point3d[]
            {
                new Point3d(0, 0, 0),
                new Point3d(width, 0, 0),
                new Point3d(width, height, 0),
                new Point3d(0, height, 0),
                new Point3d(0, 0, 0)
            };

            // Create the outer frame
            var outerFrame = new PolylineCurve(corners);
            
            // Create title block (bottom right corner)
            double titleWidth = width * 0.2;
            double titleHeight = height * 0.1;
            var titleCorners = new Point3d[]
            {
                new Point3d(width - titleWidth, 0, 0),
                new Point3d(width, 0, 0),
                new Point3d(width, titleHeight, 0),
                new Point3d(width - titleWidth, titleHeight, 0),
                new Point3d(width - titleWidth, 0, 0)
            };
            var titleBlock = new PolylineCurve(titleCorners);

            // Create inner margin (typically 10mm from edges)
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

            return new List<Curve> { outerFrame, titleBlock, innerFrame };
        }

        /// <summary>
        /// Apply scale, rotation and translation transformations to curves
        /// </summary>
        private void ApplyTransformations(List<Curve> curves, double scale, double rotation, Point3d referencePoint)
        {
            // Apply scale if different from 1.0
            if (Math.Abs(scale - 1.0) > 0.01)
            {
                var scaleTransform = Transform.Scale(Point3d.Origin, scale);
                for (int i = 0; i < curves.Count; i++)
                {
                    curves[i].Transform(scaleTransform);
                }
            }

            // Apply rotation if specified
            if (Math.Abs(rotation) > 0.01)
            {
                var rotationTransform = Transform.Rotation(Math.PI * rotation / 180.0, Vector3d.ZAxis, Point3d.Origin);
                for (int i = 0; i < curves.Count; i++)
                {
                    curves[i].Transform(rotationTransform);
                }
            }

            // Translate to reference point
            if (referencePoint != Point3d.Origin)
            {
                var translationTransform = Transform.Translation(new Vector3d(referencePoint));
                for (int i = 0; i < curves.Count; i++)
                {
                    curves[i].Transform(translationTransform);
                }
            }
        }

        /// <summary>
        /// Get paper dimensions in millimeters (width, height)
        /// </summary>
        private Tuple<double, double> GetPaperDimensions(PaperSize paperSize)
        {
            switch (paperSize)
            {
                case PaperSize.A0: return new Tuple<double, double>(841, 1189);
                case PaperSize.A1: return new Tuple<double, double>(594, 841);
                case PaperSize.A2: return new Tuple<double, double>(420, 594);
                case PaperSize.A3: return new Tuple<double, double>(297, 420);
                case PaperSize.A4: return new Tuple<double, double>(210, 297);
                case PaperSize.A5: return new Tuple<double, double>(148, 210);
                default: return new Tuple<double, double>(297, 420); // Default to A3
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.PaperFrameIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("12345678-1234-5678-9ABC-123456789ABC"); }
        }
    }
}