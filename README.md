# Tukuang - Paper Frame Generator

Grasshopper component for generating standardized paper frames in Rhino based on DWG template files.

## Overview

This Grasshopper component allows users to:
- Input a reference point for frame positioning
- Select from standard paper sizes (A0-A5)
- Generate professional drawing frames with title blocks
- Load custom frame geometry from DWG/3DM files
- Apply scaling and rotation transformations

## Features

### Paper Size Support
- A0 (841 × 1189 mm)
- A1 (594 × 841 mm)
- A2 (420 × 594 mm)
- A3 (297 × 420 mm)
- A4 (210 × 297 mm)
- A5 (148 × 210 mm)

### Input Parameters
1. **Reference Point** - Base point for frame positioning
2. **Paper Size** - Dropdown selection of standard paper sizes
3. **Scale** - Scale factor for the frame (default: 1.0)
4. **Rotation** - Rotation angle in degrees (default: 0°)

### Output Parameters
1. **Frame Curves** - List of curves representing the frame geometry
2. **Frame Info** - Text information about the generated frame

### Frame Components
- Outer border
- Inner margin (10mm from edges)
- Title block (bottom right corner)
- Revision triangle (top right corner)
- Measurement marks (every 100mm)
- Title block subdivisions

## Installation

1. Build the project in Visual Studio
2. Copy the compiled DLL to your Grasshopper Libraries folder
3. Place DWG/3DM template files in the Resources/DWG folder (optional)

## Usage

1. Add the "Paper Frame" component to your Grasshopper canvas
2. Connect a point to the "Reference Point" input
3. Select desired paper size from the dropdown
4. Adjust scale and rotation as needed
5. Connect output curves to your geometry pipeline
6. Bake the curves to create permanent geometry in Rhino

## Custom Frame Templates

To use custom frame templates:
1. Create DWG or 3DM files with your frame geometry
2. Name them according to paper size (e.g., "A3.dwg", "A4.3dm")
3. Place them in the TukuangGH/Resources/DWG folder
4. The component will automatically use custom templates when available

## Building from Source

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.8
- Rhino 7 or later with Grasshopper

### Build Steps
1. Clone the repository
2. Open TukuangGH.sln in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. The output DLL will be in the bin folder

## File Structure

```
TukuangGH/
├── TukuangGH.csproj              # Project file
├── PaperFrameComponent.cs        # Main Grasshopper component
├── DwgReader.cs                  # DWG file reading utilities
├── SampleFrameGenerator.cs       # Sample frame creation
├── TukuangGHInfo.cs             # Assembly information
├── Properties/
│   ├── AssemblyInfo.cs          # Assembly metadata
│   ├── Resources.resx           # Resource definitions
│   └── Resources.Designer.cs    # Generated resource code
└── Resources/
    ├── TukuangIcon.png          # Plugin icon
    ├── PaperFrameIcon.png       # Component icon
    └── DWG/                     # Template files directory
```

## License

MIT License - see LICENSE file for details

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## Support

For issues and questions, please create an issue on the GitHub repository.
