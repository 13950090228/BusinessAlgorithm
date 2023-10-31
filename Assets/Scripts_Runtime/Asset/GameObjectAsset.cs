using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

        public bool TryGet(string name, out GameObject go) {
            bool has = all.TryGetValue(name, out go);
            return has;
        }

        public void ClearAll() {
            all.Clear();
        }
    }
}