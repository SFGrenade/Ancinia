using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SFCore.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Modding.Logger;
using UObject = UnityEngine.Object;

namespace Ancinia;

public class SceneChanger : MonoBehaviour
{
    public AssetBundle AbAcScene { get; private set; } = null;
    public AssetBundle AbAcMat { get; private set; } = null;

    public SceneChanger(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        PrefabHolder.Preloaded(preloadedObjects);

        #region Load AssetBundles
        Assembly asm = Assembly.GetExecutingAssembly();
        if (AbAcScene == null)
        {
            using (Stream s = asm.GetManifestResourceStream("Ancinia.Resources.acscenes"))
            {
                if (s != null)
                {
                    AbAcScene = AssetBundle.LoadFromStream(s);
                }
            }
        }
        if (AbAcMat == null)
        {
            using (Stream s = asm.GetManifestResourceStream("Ancinia.Resources.acassets"))
            {
                if (s != null)
                {
                    AbAcMat = AssetBundle.LoadFromStream(s);
                }
            }
        }
        #endregion
    }

    public void CR_Change_Town(Scene scene)
    {
        GameObject rGo = scene.Find("right1");
        TransitionPoint tp = rGo.GetComponent<TransitionPoint>();
        tp.targetScene = "Ancinia_01";
        tp.entryPoint = "top1";
    }

    private void CreateBreakableWall(string sceneName, string name, Vector3 pos, Vector3 angles, Vector3 scale, Vector2 size, string playerDataBool)
    {
        GameObject breakableWall = Instantiate(PrefabHolder.BreakableWallPrefab);
        breakableWall.SetActive(true);
        breakableWall.name = name;
        breakableWall.transform.position = pos;
        breakableWall.transform.eulerAngles = angles;
        breakableWall.transform.localScale = scale;

        BoxCollider2D wallBc2d = breakableWall.GetComponent<BoxCollider2D>();
        wallBc2d.size = size;
        wallBc2d.offset = Vector2.zero;

        PlayMakerFSM wallFsm = breakableWall.LocateMyFSM("breakable_wall_v2");
        wallFsm.FsmVariables.GetFsmBool("Activated").Value = false;
        wallFsm.FsmVariables.GetFsmBool("Ruin Lift").Value = false;
        wallFsm.FsmVariables.GetFsmString("PlayerData Bool").Value = playerDataBool;
    }

    private void CreateGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string entryGate, Vector2 respawnPoint,
        bool right, bool left, bool onlyOut, GameManager.SceneLoadVisualizations vis)
    {
        GameObject gate = new GameObject(gateName);
        gate.transform.SetPosition2D(pos);
        TransitionPoint tp = gate.AddComponent<TransitionPoint>();
        if (!onlyOut)
        {
            BoxCollider2D bc = gate.AddComponent<BoxCollider2D>();
            bc.size = size;
            bc.isTrigger = true;
            tp.SetTargetScene(toScene);
            tp.entryPoint = entryGate;
        }
        tp.alwaysEnterLeft = left;
        tp.alwaysEnterRight = right;

        GameObject rm = new GameObject("Hazard Respawn Marker");
        rm.transform.parent = gate.transform;
        rm.transform.SetPosition2D(pos.x + respawnPoint.x, pos.y + respawnPoint.y);
        HazardRespawnMarker tmp = rm.AddComponent<HazardRespawnMarker>();
        tmp.respawnFacingRight = right;
        tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
        tp.sceneLoadVisualization = vis;
    }

    private void CreateBench(string benchName, Vector3 pos, string sceneName)
    {
        GameObject bench = Instantiate(PrefabHolder.WhiteBenchPrefab);
        bench.SetActive(true);
        bench.transform.position = pos;
        bench.name = benchName;
        PlayMakerFSM benchFsm = bench.LocateMyFSM("Bench Control");
        benchFsm.FsmVariables.FindFsmString("Scene Name").Value = sceneName;
        benchFsm.FsmVariables.FindFsmString("Spawn Name").Value = benchName;
    }

    private void PrintDebug(GameObject go, string tabindex = "")
    {
        Log(tabindex + "Name: " + go.name);
        foreach (Component comp in go.GetComponents<Component>())
        {
            Log(tabindex + "Component: " + comp.GetType());
        }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            PrintDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
        }
    }

    private void Log(String message)
    {
        Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
    }
    private void Log(System.Object message)
    {
        Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
    }

    private static void SetInactive(GameObject go)
    {
        if (go != null)
        {
            DontDestroyOnLoad(go);
            go.SetActive(false);
        }
    }
    private static void SetInactive(UnityEngine.Object go)
    {
        if (go != null)
        {
            DontDestroyOnLoad(go);
        }
    }
}