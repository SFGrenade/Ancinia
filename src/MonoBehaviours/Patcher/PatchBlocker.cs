using UnityEngine;

namespace Ancinia.MonoBehaviours.Patcher;

class PatchBlocker : MonoBehaviour
{
    public enum Type
    {
        WALL,
        FLOOR
    }

    public Type type;
    public string pdBool;

    public void Start()
    {
    }
}