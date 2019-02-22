## XR.SDK.GOOGLEVR
1) Install Unity 2019.1.0a11+

2) Clone https://github.cds.internal.unity3d.com/unity/xr.sdk.googlevr

   a) Clone with LFS enabled
   
   b) Make sure the cloned repo contains bee.exe (if not, gitignore_global.txt might be excluding .exe files)

3) Copy/move/or clone the xr.sdk.googlevr repo to the xr.xracceptancetests directory.
   
4) Add environmental variable: ANDROID_NDK_HOME with the location of android-sdk-r16b (location of NDK rev 16b)

5) Run xr.sdk.googlevr/bee.exe

6) Locate the project's package manifest in ../Packages/manifest.json.

   a) Edit the manifest by adding a "registry" line (see example below)

   b) Edit the manifest to point "com.unity.xr.googlevr" at the cloned repo location (see example below)

~~~json
{
  "registry": "https://staging-packages.unity.com",
  "dependencies": {
   ...
    "com.unity.xr.googlevr": "file:../xr.sdk.googlevr/com.unity.xr.googlevr",
   ...
  }
}

~~~

7) Open the xr.xracceptancetests project.
   
8) Change to an Android build target

9) Add an XRManager:
   
   a) Create a new object and add an XRManager component.
   
   b) Add/create the desired loaders in the XRManager object's inspector.
   
   c) Create a prefab of this XRManager object.  Delete any instances of this prefab.
   
   d) Navigate to Project Settings>XR and attach the XRManager object to the XR Manager field.

   e) In it's current state, it is best to include only Cardboard or Daydream in the XRManager's loader list (not both).
   
11) Remove any unused sdks and disable XR in Player Settings>XR
   
12) Build and run if using the Cardboard loader.

13) For Daydream, add a manifest override at Assets/Plugins/Android/AndroidManifest.xml.  The body of the manifest should be similar to the listing below.  This file was derived from an AndroidManifest.xml obtained from a legacy android build:

~~~xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" package="com.example.daydreamdisplayprovider" xmlns:tools="http://schemas.android.com/tools" android:installLocation="preferExternal">
  <supports-screens android:smallScreens="true" android:normalScreens="true" android:largeScreens="true" android:xlargeScreens="true" android:anyDensity="true" />
  <application android:icon="@mipmap/app_icon" android:label="@string/app_name" android:theme="@style/VrActivityTheme">
    <activity 
    android:name="com.unity3d.player.UnityPlayerActivity" 
    android:label="@string/app_name" 
    android:screenOrientation="landscape" 
    android:launchMode="singleTask" 
    android:configChanges="mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale|layoutDirection|density" 
    android:hardwareAccelerated="false" 
    android:enableVrMode="@string/gvr_vr_mode_component" 
    android:resizeableActivity="false">
      <intent-filter>
        <action android:name="android.intent.action.MAIN" />
        <category android:name="android.intent.category.LAUNCHER" />
        <category android:name="com.google.intent.category.DAYDREAM" />
      </intent-filter>
      <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
    </activity>
    <meta-data android:name="unityplayer.SkipPermissionsDialog" android:value="true" />
    <meta-data android:name="unity.splash-mode" android:value="0" />
    <meta-data android:name="unity.splash-enable" android:value="False" />
  </application>
  <uses-sdk android:minSdkVersion="24" android:targetSdkVersion="24"/>
  <uses-feature android:glEsVersion="0x00020000" android:required="true"/>
  <uses-feature android:name="android.hardware.vr.headtracking" android:required="false" android:version="1" />
  <uses-feature android:name="android.software.vr.mode" android:required="true" />
  <uses-feature android:name="android.hardware.vr.high_performance" android:required="true" />
  <uses-permission android:name="android.permission.INTERNET" />
  <uses-feature android:name="android.hardware.touchscreen" android:required="false" />
  <uses-feature android:name="android.hardware.touchscreen.multitouch" android:required="false" />
  <uses-feature android:name="android.hardware.touchscreen.multitouch.distinct" android:required="false" />
</manifest>
~~~

14) Edit AndroidManifest.xml to include the correct Package Name (line 2)

15) The DAYDREAM, or CARDBOARD activity (or both depending on XRManager settings) must be added in the intent-filters.

~~~xml
      <intent-filter>
        ...
        <category android:name="com.google.intent.category.DAYDREAM" />
        <category android:name="com.google.intent.category.CARDBOARD" />
        ...
    </intent-filter>
~~~

16) Launch from daydream launcher if using Daydream.  Cardboard should launch correctly from android desktop.

## XR.SDK.OCULUS
1) Install Unity 2019.1.0a11+

2) Clone https://github.cds.internal.unity3d.com/unity/xr.sdk.oculus

   2a) Clone with LFS enabled
   
   2b) Make sure the cloned repo contains bee.exe (if not, gitignore_global.txt might be excluding .exe files)

3) Copy/move/or clone the xr.sdk.oculus repo to the xr.xracceptancetests directory.

4) Run xr.sdk.oculus/bee.exe

5) Locate the project's package manifest in ../Packages/manifest.json.

   5a) Edit the manifest by adding a "registry" line (see example below)

   5b) Edit the manifest to point "com.unity.xr.oculus" at the cloned repo location (see example below)

~~~json
{
  "registry": "https://staging-packages.unity.com",
  "dependencies": {
   ...
    "com.unity.xr.oculus": "file:../xr.sdk.oculus/com.unity.xr.oculus",
   ...
  }
}

~~~

6) Open the xr.xracceptancetests project.
   
7) Change to the desired build target (windows or android)

8) Add an XRManager:
   
   9a) Create a new object and add an XRManager component.
   
   9b) Add a loader to the XRManager object in the object's inspector.
   
   9c) Create a prefab of this XRManager object.  Delete any instances of this prefab.
   
   9d) Navigate to Project Settings>XR and attach the XRManager object to the XR Manager field.
   
9) Set the stereo rendering mode in Project Settings>XR>Oculus

10) Select the appropriate build target in Project Settings>XR>Oculus (Windows or Android from the two buttons at the top).

11) Remove any unused sdks and disable XR in Player Settings>XR
   
12) Build and run if using Windows standalone.

13) Add an oculus signature file to Plugins/Android/assets if targeting GearVR before building and running.
