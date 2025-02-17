using UnityEngine;

namespace _Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "MachinePart", menuName = "Scriptable Objects/MachinePart")]
    public class MachinePart : ScriptableObject
    {
        [SerializeField]private GameObject prefab;

        public void Spawn(Vector3 position)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
