using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace Ancinia.MonoBehaviours.Patcher;

[RequireComponent(typeof(BoxCollider2D))]
class PatchInspectRegion : MonoBehaviour
{
    public string GameTextConvo = "";
    public bool HeroAlwaysLeft = false;
    public bool HeroAlwaysRight = false;

    public void Start()
    {
        try
        {
            PrefabHolder.PopTabletInspectPrefab.SetActive(true);
            GameObject inspect = Instantiate(PrefabHolder.PopTabletInspectPrefab, transform);
            PrefabHolder.PopTabletInspectPrefab.SetActive(false);
            inspect.transform.localPosition = Vector3.zero;
            inspect.transform.localEulerAngles = Vector3.zero;
            inspect.transform.localScale = Vector3.one;

            BoxCollider2D selfBc2d = GetComponent<BoxCollider2D>();
            BoxCollider2D newBc2d = inspect.GetComponentInChildren<BoxCollider2D>();
            newBc2d.offset = selfBc2d.offset;
            newBc2d.size = selfBc2d.size;

            PlayMakerFSM fsm = inspect.LocateMyFSM("inspect_region");
            FsmVariables fsmVar = fsm.FsmVariables;

            fsmVar.GetFsmString("Game Text Convo").Value = GameTextConvo;
            fsmVar.GetFsmBool("Hero Always Left").Value = HeroAlwaysLeft;
            fsmVar.GetFsmBool("Hero Always Right").Value = HeroAlwaysRight;
        }
        catch (Exception e)
        {
            Debug.Log("PatchInspectRegion - " + e);
        }
    }
}