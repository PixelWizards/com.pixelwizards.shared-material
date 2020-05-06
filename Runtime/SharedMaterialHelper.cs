using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelWizards.Timeline
{
    [ExecuteAlways]
    public class SharedMaterialHelper : MonoBehaviour
    {

        [Header("Scene Reference")]
        public Renderer referenceObject;

        private Renderer myRenderer;
        private MaterialPropertyBlock refPropBlock;
        private MaterialPropertyBlock myPropBlock;

        private void OnEnable()
        {
            myRenderer = GetComponent<Renderer>();
            refPropBlock = new MaterialPropertyBlock();
            myPropBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            if (myRenderer == null)
                return;

            if( referenceObject != null)
            {
                referenceObject.GetPropertyBlock(refPropBlock);
                myRenderer.GetPropertyBlock(myPropBlock);
                myRenderer.SetPropertyBlock(refPropBlock);
            }
            
        }
    }
}