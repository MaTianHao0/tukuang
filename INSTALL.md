# Installation Guide

## Prerequisites

- Rhino 7 or later
- Grasshopper (comes with Rhino 7+)
- Windows operating system
- .NET Framework 4.8 or later

## Installation Steps

### Option 1: Build from Source

1. **Clone the Repository**
   ```bash
   git clone https://github.com/MaTianHao0/tukuang.git
   cd tukuang
   ```

2. **Open in Visual Studio**
   - Open `TukuangGH.sln` in Visual Studio 2019 or later
   - Ensure Rhino 7 or later is installed

3. **Restore NuGet Packages**
   - Right-click solution → "Restore NuGet Packages"
   - This will download Grasshopper and RhinoCommon dependencies

4. **Build the Project**
   - Build → Build Solution (Ctrl+Shift+B)
   - Check Output window for any errors

5. **Copy to Grasshopper Libraries**
   - Locate the compiled `TukuangGH.dll` in the `bin/Release` folder
   - Copy it to your Grasshopper Libraries folder:
     - Windows: `%AppData%\Grasshopper\Libraries\`
     - Or in Rhino: `Tools → Options → Grasshopper → Folder → Libraries`

### Option 2: Direct Installation (Future)

Once built, the component can be distributed as a GHA file:
1. Download `TukuangGH.gha`
2. Copy to Grasshopper Libraries folder
3. Restart Rhino/Grasshopper

## Verification

1. **Open Grasshopper**
   - In Rhino, type `Grasshopper` and press Enter

2. **Find the Component**
   - Look for the "Tukuang" tab in the component ribbon
   - Find "Paper Frame" component under "Frame" panel

3. **Test the Component**
   - Create a Point component
   - Add the Paper Frame component
   - Connect the point to Reference Point input
   - You should see frame curves in the output

## Custom DWG Templates (Optional)

1. **Create Template Files**
   - Design your frame in AutoCAD or Rhino
   - Export as DWG or save as 3DM

2. **Name Correctly**
   - Name files as: A0.dwg, A1.dwg, A2.dwg, A3.dwg, A4.dwg, A5.dwg
   - Or use 3DM format: A0.3dm, A1.3dm, etc.

3. **Place in Resources Folder**
   - Copy files to: `[Grasshopper Libraries]/TukuangGH/Resources/DWG/`
   - Create the folder structure if it doesn't exist

## Troubleshooting

### Component Not Appearing
- Verify DLL is in correct Libraries folder
- Check Rhino version (7.0+)
- Restart Rhino completely
- Check Windows → Component Galleries for loading errors

### Build Errors
- Ensure Rhino 7+ is installed before building
- Check NuGet package restore completed successfully
- Verify .NET Framework 4.8 is available
- Try cleaning and rebuilding solution

### Runtime Errors
- Check Grasshopper's "Developer" options for detailed error messages
- Ensure input parameters are valid (positive scale, valid paper size index)
- Verify file permissions for DWG template files

### DWG Loading Issues
- Ensure DWG files are valid and readable by Rhino
- Check file naming convention matches exactly
- Try converting DWG to 3DM format for better compatibility
- Files should contain only curve geometry

## Uninstallation

1. Delete `TukuangGH.dll` from Grasshopper Libraries folder
2. Delete any custom DWG template files
3. Restart Rhino/Grasshopper

## Support

For issues and questions:
1. Check existing GitHub issues
2. Create new issue with:
   - Rhino version
   - Operating system
   - Error messages
   - Steps to reproduce

## Next Steps

After installation, see `USAGE.md` for detailed usage examples and workflows.