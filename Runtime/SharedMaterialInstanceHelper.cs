using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace PixelWizards.Timeline
{
    [ExecuteAlways]
    [AddComponentMenu("Shared Material/Shared Material Instance Helper")]
    public class SharedMaterialInstanceHelper : MonoBehaviour
    {
        [Header("Scene Reference")]
        public Renderer referenceObject;
        public bool useRandom = false;
        public bool isAnimated = true;
        public string randomPropertyName = "_GlobalEmissiveIntensity";
        public float minimum = 0;
        public float maximum = 1;
        public bool crossScene = false;
        public Material referenceMaterial;
        public List<Renderer> renderList = new List<Renderer>();

        public List<float> randomList = new List<float>();
        //public List<Renderer> referenceRenderers;
        private List<Renderer> _gameObjects = new List<Renderer>();
        public int seed = 42;
        private MaterialPropertyBlock refPropBlock;
        private MaterialPropertyBlock myPropBlock;

        private int sceneCounter = 1;


        private void Start()
        {
           // 
            if(Application.isPlaying)
                UpdateRenderers();
        }

        private void OnEnable()
        {
            refPropBlock = new MaterialPropertyBlock();
            myPropBlock = new MaterialPropertyBlock();
//#if UNITY_EDITOR
            //renderList = GetComponentsInChildren<Renderer>().ToList();
            //StartCoroutine(StartRenderUpdate());
            //UpdateRenderers();
            
            
            
            if(Application.isEditor)
                EditorApplication.hierarchyChanged += hierarchyChanged;
            else
            {
                //SceneManager.sceneLoaded += OnSceneLoaded;
               // SceneManager.sceneUnloaded += OnSceneUnloaded;
            }
//#endif

        }
        private void hierarchyChanged()
        {

            if (crossScene)
            {
                Scene[] scenes = GetAllOpenScenes();
                int numLoaded = 0;

                foreach (Scene scene in scenes)
                {
                    if (scene.isLoaded)
                        numLoaded++;
                }

                if (sceneCounter != numLoaded)
                {
                    UpdateRenderers();
                    sceneCounter = numLoaded;
                }
            }
        }
        
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("OnSceneLoaded: " + scene.name);
            //StartCoroutine(StartRenderUpdate());
            UpdateRenderers();
        }
        
        void OnSceneUnloaded<Scene>(Scene scene)
        {
           // Debug.Log("OnSceneUnloaded: ");
            //Debug.Log(mode);
            //StartCoroutine(StartRenderUpdate());
            UpdateRenderers();
        }
        
        
        
        public void ResetRenderers()
        {
            renderList.Clear();
            randomList.Clear();
            _gameObjects.Clear();
           // refPropBlock = null;
          //  myPropBlock = null;

        }

        public void UpdateRenderers()
        {
            renderList.Clear();
            randomList.Clear();
            _gameObjects.Clear();
            
            FindObjectsWithRenderer(_gameObjects);

            if (referenceMaterial != null)
            {
                foreach (Renderer _gameObject in _gameObjects)
                {
                    foreach (Material mat in _gameObject.sharedMaterials)
                    {
                        if (mat == referenceMaterial)
                        {
                            renderList.Add(_gameObject);
                        }
                    }
                }
                
                foreach (Renderer refRenderer in renderList)
                {
                    if (refRenderer == referenceObject)
                    {
                        renderList.Remove(refRenderer);
                        break;
                    }
                }
                
                Random.InitState(seed);

                foreach (var renderer in renderList)
                {
                    randomList.Add(Random.Range(minimum, maximum));
                }
            }
            
            _gameObjects.Clear();
            UpdateMaterials();
        }

        private void Update()
        {
            if (isAnimated)
            {
                UpdateMaterials();

            }
        }

        private void UpdateMaterials()
        {
            if (renderList.Count < 1)
                return;

            if ( referenceObject != null)
            {
                referenceObject.GetPropertyBlock(refPropBlock);
                for(int i = 0; i < renderList.Count; i++)
                {
                    if (useRandom)
                    {
                        if(renderList[i].GetComponent<SharedMaterialInstanceOverride>())
                            refPropBlock.SetFloat(randomPropertyName, renderList[i].GetComponent<SharedMaterialInstanceOverride>()._EmissiveIntensity);
                        else
                        {
                            refPropBlock.SetFloat(randomPropertyName, randomList[i]);
                        }
                    }
                    renderList[i].GetPropertyBlock(myPropBlock);
                    renderList[i].SetPropertyBlock(refPropBlock);
                }
            }
        }

        private Scene[] GetAllOpenScenes()
        {
            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];
            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }

        List<Renderer> FindObjectsWithRenderer(List<Renderer> result)
        {
            if (crossScene)
            {
                int countLoaded = SceneManager.sceneCount;
                Scene[] loadedScenes = GetAllOpenScenes();

                foreach (var scene in loadedScenes)
                {
                    if (scene.isLoaded)
                    {
                        List<GameObject> rootObjects = new List<GameObject>();
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
                    }
                }
            }
            else
            {
                List<GameObject> rootObjects = new List<GameObject>();
                gameObject.scene.GetRootGameObjects( rootObjects );
 
                    // iterate root objects
                for (int i = 0; i < rootObjects.Count; ++i)
                {

                    Renderer[] tempRenderers = rootObjects[i].GetComponentsInChildren<Renderer>(true);
                    if (tempRenderers != null)
                    {
                        result.AddRange(tempRenderers);
                    }

                }
                
            }
            
            return result;
        }
    }
}