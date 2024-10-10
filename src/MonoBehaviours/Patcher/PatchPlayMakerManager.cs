using UnityEngine;

namespace Ancinia.MonoBehaviours.Patcher;

class PatchPlayMakerManager : MonoBehaviour
{
    public Transform managerTransform;

    public void Awake()
    {
        GameObject tmpPmu2D = Instantiate(PrefabHolder.PopPmU2dPrefab, managerTransform);
        tmpPmu2D.SetActive(true);
        tmpPmu2D.name = "PlayMaker Unity 2D";
    }
}