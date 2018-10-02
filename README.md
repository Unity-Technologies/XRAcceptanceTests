## UNITY XR ASSESSMENT


This project is used to test Unity’s XR features. Tests in this project are designed to be simple and platform agnostic. The behavior of specific features can be targeted by isolating the systems under test, therefore the scenes have been constructed to limit interaction between different systems and components. Furthermore, display and input systems have been implemented in a generic way so that the project does not rely on the platform specific features of plugins. The project should be used to verify behavior of player configurations, compare features between Unity releases, and test for changes in performance.

Full documentation including comparison images and expected results for all tests are included in the repo.
  

## Test Scenes

 - Materials
 - Baked Lighting
 - Real-time Lighting
 - Effects
 - Terrain
 - Canvas
 - API Checks
 - Performance
 - Input

## Configurations

These tests are designed to be run in the built-in renderer with multi-pass, single-pass, and single-pass instanced stereo rendering modes. Scenes should be tested to make sure they are displayed the same in the left eye, right eye, Editor GameView, and the player’s standalone mirror (if applicable). All combinations of settings supported by the target platform should be tested:

**Stereo Rendering Modes**

 - Multi Pass
 - Single Pass
 - Single Pass Instanced

**Graphics APIs**

 - Direct3D11
 - Direct3D12 (Experimental)
 - OpenGL
 - Vulkan
 - OpenES2
 - OpenES3
 - Metal

**Players**
 - Editor
 - Standalone

  
## Building
Tests can be run manually in the editor or in a built player. Batch building functionality has been included in this project to provide an easy-to-use script and build configuration system that can build a common set of configurations for testing. In addition, this system allows for multiple build configurations to be built at once to reduce overhead of managing multiple test build configurations. Finally, utilization of the batch builds will maintain a consistent set of build settings across test passes to reduce configuration errors.

**Usage**
The build scripts located in the root of this project are available to build the most common configurations of the project for testing across supported platforms. Scripts are available for Windows (.bat) and macOS (.command) in the BuildScripts folder. A Build Windows UI is available in the Editor to run batch builds and allow customization of the build configurations

Pass the location of the Unity executable to run from the command line:

  

**Windows Example:**

    BuildWindows.bat <location of Unity.exe>

  

**MacOS Example:**

    BuildmacOS.command <location of Unity.app>/Content/Unity...

  


The builds will be located in:

  

    <Project Directory>\Builds\<Build Target>\<VR SDK>\<Stereo Rendering Method>-<Graphics API>

  

For example,

    ...\Builds\StandaloneWindows64\Oculus\Instancing-Direct3D11.exe
    
    ...\Builds\StandaloneWindows64\Oculus\MultiPass-Direct3D11.exe


## Scene Navigation

Gaze based controls have been implemented for scene navigation to allow the project to run on a variety of platforms without platform specific input dependencies. Gaze at the gray arrows in any scene to transition to the next or previous scene. The arrows will progressively fill for several seconds as long as gaze focus is maintained. The scene will transition once the arrow is completely filled.