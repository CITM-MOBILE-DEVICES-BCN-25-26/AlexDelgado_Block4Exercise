1. APPROACH

  - Measure first: read Stats panel + Profiler before changing code.
  - Read Render Thread row to decide CPU vs GPU bound.
  - Fix the biggest issue first, re-measure, repeat.
  - Keep observable behaviour (cubes still know who is near + log, timer still ticks, 50 cubes still spawn).


2. TOOLS

  - Stats panel: FPS, Drawcalls, SetPass.
  - Profiler: analysis of the frame (functions + I/O calls).
  - Frame Debugger: count draw calls per camera (used to confirm duplicate camera).


3. CPU OR GPU BOUND

  In this case is CPU bound. (This example doesn't have material alternation like the one in class)

  - Main Thread: BehaviourUpdate = 79.29 ms -> ~99% of frame in scripts.
  - Inside it: UnoptimizedDistanceCube.Update repeated 50x with an I/O operation, Debug.Log.


4. PROBLEMS AND WHERE

  4.1 Scripts
    - UnoptimizedDistanceCube: FindObjectsByType in every Start. O(N^2) startup in both memory and steps + O(N^2) per frame, Vector3.Distance with sqrt, Debug.LogWarning per pair (2450 calls/frame, string alloc).
    - UnoptimizedDistanceCubeSpawner: CreateCube adds unused BoxCollider.
    - UnoptimizedTimerText: string interpolation every frame + TMP mesh rebuild every frame.

  4.2 Scene
    - Two cameras. That means scene rendered twice.
    - No instancing for the same object type.

  4.3 UI
    - Timer TMP shares Canvas with buttons -> any text change rebuilds whole Canvas batch.


5. OPTIMIZATIONS

  5.1 Scripts
    - OptimizedCubeDistanceManager: one MonoBehaviour holds the cube list + StringBuilder. One Update for all cubes instead of 50. No sqrt (high cost operation for computers); distance is left squared since the absolute value does not matter. All pair lines (same structure as the original "Distance cube X -> Y: sqrDist") are appended into a single shared StringBuilder and flushed with ONE Debug.Log per interval.
    - OptimizedDistanceCube: just register/unregister. No Update.
    - OptimizedDistanceCubeSpawner: prefab required. no collider, no runtime AddComponent.
    - OptimizedTimerText: SetText(StringBuilder) zero alloc. Refresh only when visible digit changes.

  5.2 Scene
    - Duplicate camera removed.
    - Cube_Optimized prefab without BoxCollider.
    - CubeMaterial_Instanced with GPU instancing on.

  5.3 UI
    - Canvas split by update rate. Static (buttons) stay on main Canvas. Timer moves to sub-Canvas (TimerCanvas).

  5.4 Why better
    - Logs: 2450/frame (147k/sec at 60 fps) -> 1 Debug.Log per interval (2/sec). All lines merged into one StringBuilder, one console write per pass. Same info.
    - Manager: 1 Update instead of 50. One config shared.
    - sqrMagnitude(Vector3.Distance) not needed.
    - GPU instancing: 50 draw setups, now 1 per camera.
    - Sub-Canvas: main UI not rebuilt every frame.


6. BEFORE / AFTER

  Stats panel:

                          Baseline      Optimized     Change
  ----------------------------------------------------------
  FPS                     10.9          597.3          55x
  Frame time              92.1 ms       1.7 ms        -98%
  Render thread           88.7 ms       1.2 ms        -99%
  Batches                 383           190           -50%
  SetPass                 33            15            -55%

  Profiler:

                          Baseline           Optimized
  -------------------------------------------------------
  PlayerLoop              130 ms             0.14 ms
  BehaviourUpdate         126.39 ms          0.02 ms
  Scripts                 71.95 ms           0 ms
  Cube Update spikes      50x ~2 ms each     gone
  LogStringToConsole      2450/frame         1 per pass = 2/sec

  Notes:
    - 55x FPS gain mostly from collapsing 2450 per-frame Debug.LogWarning calls into 1 per interval.
    - Draw calls and GPU work halved by removing the duplicate camera.
    - SetPass drop (33 to 15) from removing the second camera + GPU instancing.
