using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelWizards.Timeline
{
    [ExecuteAlways]
    [AddComponentMenu("Shared Material/Shared Material Global Helper")]
    public class SharedMaterialGlobalHelper : MonoBehaviour
    {
        [Header("Scene Reference")]
        public Renderer referenceObject;
        private List<Renderer> renderList; 
        public List<Renderer> referenceRenderers;
        private List<Renderer> _gameObjects;
        private MaterialPropertyBlock refPropBlock;
        private MaterialPropertyBlock myPropBlock;

        private void Start()
        {
            //renderList = GetComponentsInChildren<Renderer>().ToList();
            renderList = new List<Renderer>();
            _gameObjects = new List<Renderer>();
            FindObjectsWithRenderer(_gameObjects);

            if (referenceRenderers != null)
            {
                foreach (Renderer _gameObject in _gameObjects)
                {
                    foreach (Renderer refRenderer in referenceRenderers)
                    {
                        if (_gameObject.name == refRenderer.name)
                        {
                            renderList.Add(_gameObject);
                        }
                    }
                    
                }
                
                foreach (Renderer refRenderer in referenceRenderers)
                {
                    if (refRenderer == referenceObject)
                    {
                        renderList.Remove(refRenderer);
                        break;
                    }
                }
            }

            _gameObjects = null;
            
            refPropBlock = new MaterialPropertyBlock();
            myPropBlock = new MaterialPropertyBlock();
        }
        
        public void ResetRenderers()
        {
            renderList = null;
            _gameObjects = null;
            refPropBlock = null;
            myPropBlock = null;

        }

        public void UpdateRenderers()
        {
            renderList = null;
            _gameObjects = null;
            
            renderList = new List<Renderer>();
            _gameObjects = new List<Renderer>();
            FindObjectsWithRenderer(_gameObjects);

            if (referenceRenderers != null)
            {
                foreach (Renderer _gameObject in _gameObjects)
                {
                    foreach (Renderer refRenderer in referenceRenderers)
                    {
                        if (_gameObject.name == refRenderer.name)
                        {
                            renderList.Add(_gameObject);
                        }
                    }
                    
                }
                
                foreach (Renderer refRenderer in referenceRenderers)
                {
                    if (refRenderer == referenceObject)
                    {
                        renderList.Remove(refRenderer);
                        break;
                    }
                }
            }

            _gameObjects = null;
            
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
        
        List<Renderer> FindObjectsWithRenderer(List<Renderer> result)
        {
            List<GameObject> rootObjects = new List<GameObject>();

            Scene scene = gameObject.scene;
            scene.GetRootGameObjects( rootObjects );
 
            // iterate root objects
            for (int i = 0; i < rootObjects.Count; ++i)
            {

                Renderer[] tempRenderers = rootObjects[i].GetComponentsInChildren<Renderer>(true);
                if (tempRenderers != null)
                {
                    result.AddRange(tempRenderers);
                }

            }
 
            return result;
        }
    }
}