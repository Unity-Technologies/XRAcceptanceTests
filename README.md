## XR.SDK.GOOGLEVR
1) Install Unity 2019.1.0a11+

2) Clone https://github.cds.internal.unity3d.com/unity/xr.sdk.googlevr

   2a) Clone with LFS enabled
   
   2b) Make sure the cloned repo contains bee.exe (if not, gitignore_global.txt might be excluding .exe files)
   
3) Add environmental variable: ANDROID_NDK_HOME with the location of android-sdk-r16b (location of NDK rev 16b)

4) Run bee.exe

5) Open one of the projects in xr.sdk.googlevr/TestProjects

   5a) The project's manifest should contain an entry for com.unity.xr.googlevr that points to xr.sdk.googlevr/com.unity.xr.googlevr.
   
   5b) If the TestProjects directory has been moved relative to the repo then update the package manifest accordingly.
   
6) Change to an Android build target

7) Open the project's main scene

8) Ensure the project has an XRManager setup correctly in one of two ways:

   8a) XRManager object in the scene should contain the XR Manager script and appropriate loaders (this method will be deprecated).
   
   8b) or Project Settings>XR should contain a link to a prefab that contains the XRManager and appropriate loaders.
   
   8c) In it's current state, it is best to include only Cardboard or Daydream in the XRManager's loader list.
   
9) Build and run if using the Cardboard loader.

10) Add a manifest override at Assets/Plugins/Android/AndroidManifest.xml if using Daydream.  The body of the manifest should be similar to the listing below.  This file was derived from an AndroidManifest.xml obtained from a legacy android build:

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

11) Edit AndroidManifest.xml to include the correct Package Name (line 2)

12) The DAYDREAM, or CARDBOARD activity (or both depending on XRManager settings) must be added in the intent-filters.

~~~xml
      <intent-filter>
        ...
        <category android:name="com.google.intent.category.DAYDREAM" />
        <category android:name="com.google.intent.category.CARDBOARD" />
        ...
    </intent-filter>
~~~

13) Launch from daydream launcher if using Daydream.  Cardboard should launch correctly from android desktop.
