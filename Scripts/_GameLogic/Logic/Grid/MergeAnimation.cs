using _Game.Scripts._GameLogic.Logic.Grid.Native;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class MergeAnimation : MonoBehaviour, IGridObjectClickableAction
    {
        [SerializeField] private GameObject _mergeParticle;
        
        public void OnObjectGenerated(GridObject gridObject)
        {
        }

        public void OnObjectClick(GridObject gridObject)
        {
        }

        public void OnObjectMerge(GridObject gridObject)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(0.2f) // Adds a 0.2 second delay
                .AppendCallback(() => 
                {
                    // Get the pooled particle object
                    GameObject new_mergeParticle = PoolManager.Instance.GetPooledObject(_mergeParticle.name);
            
                    // Position it at the grid object's position
                    new_mergeParticle.transform.position = gridObject.transform.position;

                    // Return the particle object to the pool after its duration ends
                    DOVirtual.DelayedCall(new_mergeParticle.GetComponent<ParticleSystem>().main.duration, 
                        () => PoolManager.Instance.ReturnToPool(new_mergeParticle));
                });
        }

    }
}