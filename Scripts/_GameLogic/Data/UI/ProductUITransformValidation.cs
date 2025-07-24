using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.UI
{
    public class ProductUITransformValidation : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private Vector3 _scale;

        private void OnEnable()
        {
            gameObject.transform.localPosition = _position;
            gameObject.transform.localRotation = Quaternion.Euler(_rotation);
            gameObject.transform.localScale = _scale;
        }
    }
}