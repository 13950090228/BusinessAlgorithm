using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BusinessAlgorithm.Event;

namespace BusinessAlgorithm.Test {
    public class EventTest : MonoBehaviour {

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("注销事件");
                UnRegisterEvent();
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                Debug.Log("注册事件");
                RegisterEvent();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                Debug.Log("派发即时事件");
                EventManager.Emit(EventDefs.SpawnActor, new SpawnActorEvent() {
                    actorSign = "即时派发"
                });
            }

            if (Input.GetKeyDown(KeyCode.RightControl)) {
                Debug.Log("派发延迟事件");
                EventManager.Dispatch(EventDefs.SpawnActor, new SpawnActorEvent() {
                    actorSign = "延迟派发"
                });
            }
        }

        void LateUpdate() {
            EventManager.Tick(Time.deltaTime);
        }

        public void RegisterEvent() {
            EventManager.On(EventDefs.SpawnActor, SpawnActor);
        }

        public void UnRegisterEvent() {
            EventManager.Off(EventDefs.SpawnActor, SpawnActor);
        }

        void SpawnActor(BattleEvent evt) {
            SpawnActorEvent spawnActorEvent = evt as SpawnActorEvent;
            Debug.Log($"角色名:{spawnActorEvent.actorSign}");
        }
    }
}
