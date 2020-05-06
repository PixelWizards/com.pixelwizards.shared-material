using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelWizards.Timeline
{
    [ExecuteAlways]
    [AddComponentMenu("Shared Material/Shared Material Group Helper")]
    public class SharedMaterialGroupHelper : MonoBehaviour
    {
        [Header("Scene Reference")]
        public Renderer referenceObject;

        private List<Renderer> renderList = new List<Renderer>();
        private MaterialPropertyBlock refPropBlock;
        private MaterialPropertyBlock myPropBlock;

        private void OnEnable()
        {
            renderList = GetComponentsInChildren<Renderer>().ToList();
            refPropBlock = new MaterialPropertyBlock();
            myPropBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            if (renderList.Count < 1)
                return;

            if( referenceObject != null)
            {
                referenceObject.GetPropertyBlock(refPropBlock);
                foreach (var entry in renderList)
                {
                    entry.GetPropertyBlock(myPropBlock);
                    entry.SetPropertyBlock(refPropBlock);
                }
            }
        }
    }
}