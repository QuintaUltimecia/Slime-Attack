using UnityEngine;

[ExecuteInEditMode]
public class MoveInt : MonoBehaviour
{
    private Transform _transform;

//#if UNITY_EDITOR
//    private void Update()
//    {
//        if (_transform == null)
//        {
//            if (Application.isPlaying == false)
//                _transform = transform;
//        }
//        else
//        {
//            Move();
//        }
//    }
//#endif

    private void Move()
    {
        _transform.position = new Vector3(
            x: Mathf.RoundToInt(transform.position.x),
            y: Mathf.RoundToInt(transform.position.y),
            z: Mathf.RoundToInt(transform.position.z));
    }
}