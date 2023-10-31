using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace BusinessAlgorithm.Asset {
    public class AssetManager {
        AssetManager instance;
        private AssetManager() { }
        public AssetManager Instance {
            get {
                if (instance == null) {
                    instance = new AssetManager();
                }
                return instance;
            }
        }

        public GameObjectAsset GameObjectAsset { get; set; }

        public void Init() {
            GameObjectAsset = new GameObjectAsset();
        }

        public async Task LoadAll() {
            await GameObjectAsset.LoadAll();
        }

        public void ClearAll() {
            GameObjectAsset.ClearAll();
        }

    }
}

