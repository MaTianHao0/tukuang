# Tukuang Usage Examples

## Basic Usage Example

This example shows how to use the Tukuang Paper Frame component in Grasshopper:

1. **Create Reference Point**
   ```
   Point Component → Set coordinates (0, 0, 0)
   ```

2. **Add Paper Frame Component**
   ```
   Tukuang Tab → Frame Panel → Paper Frame
   ```

3. **Configure Inputs**
   - Connect Point to "Reference Point" input
   - Set "Paper Size" to desired value (0=A0, 1=A1, 2=A2, 3=A3, 4=A4, 5=A5)
   - Set "Scale" to 1.0 for actual size
   - Set "Rotation" to 0 for no rotation

4. **View Output**
   - "Frame Curves" output provides the frame geometry
   - "Frame Info" output shows configuration details

## Advanced Usage

### Custom Scaling
To create a 50% scale frame:
- Set Scale input to 0.5

### Rotated Frame
To create a 45-degree rotated frame:
- Set Rotation input to 45

### Multiple Frames
To create multiple frames at different positions:
1. Use a Grid component to generate multiple points
2. Connect the grid points to the Reference Point input
3. The component will generate frames at each point

## Custom DWG Templates

### File Naming Convention
Place your custom frame DWG files in the Resources/DWG folder with these names:
- A0.dwg or A0.3dm
- A1.dwg or A1.3dm
- A2.dwg or A2.3dm
- A3.dwg or A3.3dm
- A4.dwg or A4.3dm
- A5.dwg or A5.3dm

### DWG File Requirements
- Geometry should be positioned with origin at (0,0,0)
- Use actual size dimensions (in millimeters)
- Include all frame elements as curves
- Avoid complex geometry that may not import correctly

## Workflow Example

### Typical Architectural Drawing Setup
1. Import site plan or floor plan
2. Add Paper Frame component
3. Set reference point at desired location
4. Select appropriate paper size (typically A1 or A0 for architecture)
5. Bake the frame curves
6. Continue with annotation and dimensioning

### For Technical Drawings
1. Create your technical geometry
2. Add Paper Frame component
3. Choose A3 or A4 size for detailed drawings
4. Position frame to encompass your geometry
5. Use title block area for drawing information

## Troubleshooting

### Common Issues
1. **Frame not visible**: Check if reference point is within view
2. **Wrong size**: Verify scale factor and paper size selection
3. **Missing DWG**: Component falls back to default geometry if custom DWG not found
4. **Rotation issues**: Ensure rotation angle is in degrees, not radians

### Performance Tips
- Use lower paper sizes (A4, A5) for faster computation
- Avoid excessive scaling (keep scale between 0.1 and 10.0)
- Group frames before baking for better organization