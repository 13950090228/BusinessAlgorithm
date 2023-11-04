using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BusinessAlgorithm.Asset {
    public class GameObjectAsset {
        Dictionary<string, GameObject> all;

        public GameObjectAsset() {
            this.all = new Dictionary<string, GameObject>();
        }

        public async Task LoadAll() {
            AssetLabelReference labelReference = new AssetLabelReference();
            labelReference.labelString = AssetLabelType.GameObj;
            var list = await Addressables.LoadAssetsAsync<GameObject>(labelReference, null).Task;
            for (int i = 0; i < list.Count; i += 1) {
                var go = list[i];
                all.Add(go.name, go);
            }
        }

        public async Task<GameObject> LoadAssetSync(string resourceName) {
            var go = await Addressables.LoadAssetAsync<GameObject>(resourceName).Task;
            return go;
        }

        public void LoadAssetAsync(string resourceName, Action<GameObject> callback) {
            bool has = all.TryGetValue(resourceName, out GameObject go);
            if (has) {
                callback?.Invoke(go);
                return;
            }

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(resourceName);

            handle.Completed += (operation) => OnLoadComplete(operation, resourceName, callback);
        }

        void OnLoadComplete(AsyncOperationHandle<GameObject> handle, string resourceName, Action<GameObject> callback) {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                callback?.Invoke(handle.Result);
            } else {
                // 处理加载失败的情况
                Debug.LogError("Failed to load addressable: " + resourceName);
            }

            Addressables.Release(handle);
        }

        public void ClearAll() {
            all.Clear();
        }
    }
}