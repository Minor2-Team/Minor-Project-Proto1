using UnityEngine;

namespace _Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "MachinePart", menuName = "Scriptable Objects/MachinePart")]
    public class MachinePart : ScriptableObject
    {
        [SerializeField]private DraggableObject prefab;

        public void Spawn(Vector3 position)
        {
            
            var obj=Instantiate(prefab, position, Quaternion.identity);
            obj.StartDrag();
            
        }
    }
}
