package com.unity.gvr;

import android.app.Activity;
import android.view.SurfaceView;
import android.view.View;
import android.util.Log;
import android.view.ViewGroup;

import com.google.vr.ndk.base.AndroidCompat;
import com.unity3d.player.UnityPlayer;
import com.google.vr.ndk.base.GvrLayout;

public class GvrUnity
{
  GvrLayout gvrLayout;
  UnityPlayer player;
  Activity activity;
  View glView;

  public void initGvr(boolean enableAsyncReprojection)
  {
    Log.d("Unity", "initGvr Java!");
    activity = UnityPlayer.currentActivity;

    activity.runOnUiThread(() -> {
      gvrLayout = new GvrLayout(activity);
      boolean asyncReproj = enableAsyncReprojection;
      if (enableAsyncReprojection)
      {
        asyncReproj = gvrLayout.setAsyncReprojectionEnabled(enableAsyncReprojection);
      }

      ViewGroup vg = activity.findViewById(android.R.id.content);

      player = null;
      for (int i = 0; i < vg.getChildCount(); ++i) {
        if (vg.getChildAt(i) instanceof UnityPlayer) {
          player = (UnityPlayer) vg.getChildAt(i);
          break;
        }
      }

      if (player == null) {
        Log.e("Unity", "GVR Failed to find UnityPlayer view!");
        return;
      }

      glView = null;
      for (int i = 0; i < player.getChildCount(); ++i)
      {
        if (player.getChildAt(0) instanceof SurfaceView)
        {
          glView = player.getChildAt(0);
        }
      }

      if (glView == null) {
        Log.e("Unity", "GVR Failed to find GlView!");
      }

      player.addViewToPlayer(gvrLayout, true);
      gvrLayout.setPresentationView(glView);

      AndroidCompat.setVrModeEnabled(activity, true);

      Log.d("Unity", "GVR UI thread done.");

      // We can't call InitializeGL on the gvr context until the above addView / removeView finishes which needs the main thread to pump.
      // So we do a callback from the Ui thread and only give the context to native once this is all done.
      initComplete(gvrLayout.getGvrApi().getNativeGvrContext(), asyncReproj);
    });
  }

  public void pauseGvr()
  {
    activity.runOnUiThread(() -> {
      gvrLayout.onPause();
      glView.setVisibility(View.VISIBLE);
    });
  }

  public void resumeGvr()
  {
    activity.runOnUiThread(() -> {
      gvrLayout.onResume();
    });
  }

  public void destroyGvr()
  {
    activity.runOnUiThread(() -> {
      AndroidCompat.setVrModeEnabled(activity, false);
      gvrLayout.onPause();
      player.removeView(gvrLayout);
      gvrLayout.setPresentationView(new View(activity));
      player.addView(glView);

      gvrLayout.shutdown();
      gvrLayout = null;
    });
  }

  private native void initComplete(long gvrPtr, boolean asyncReprojection);
}
