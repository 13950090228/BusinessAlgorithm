using System.Collections.Generic;

namespace BusinessAlgorithm.Event {
    public delegate void EventHandler(BattleEvent evt);

    public static class EventManager {

        static Dictionary<string, List<EventHandler>> eventListenersDic = new Dictionary<string, List<EventHandler>>();
        public static Dictionary<string, List<EventHandler>> EventListenersDic { get { return eventListenersDic; } }

        // 延时触发 需在外部 tick
        static List<EventRequest> eventRequests = new List<EventRequest>();
        public static List<EventRequest> EventRequests { get { return eventRequests; } }

        /// <summary>
        /// 事件派发（不立即执行）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="evt"></param>
        public static void Dispatch(string name, BattleEvent evt) {

            EventRequest request = new EventRequest {
                name = name,
                evt = evt
            };

            eventRequests.Add(request);
        }

        /// <summary>
        /// 事件派发（立即执行）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="evt"></param>
        public static void Emit(string name, BattleEvent evt) {
            List<EventHandler> emitterToHandler;
            if (evt != null) {
                evt.eventName = name;
            }
            if (eventListenersDic.TryGetValue(name, out emitterToHandler)) {
                for (int j = 0; j < emitterToHandler.Count; j++) {
                    if (emitterToHandler[j] != null) {
                        emitterToHandler[j](evt);
                    }
                }
            }
        }

        /// <summary>
        /// 事件监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public static void On(string eventName, EventHandler callback) {
            if (!eventListenersDic.ContainsKey(eventName)) {
                eventListenersDic.Add(eventName, new List<EventHandler>());
            }

            if (!eventListenersDic[eventName].Contains(callback)) {
                eventListenersDic[eventName].Add(callback);
            }
        }

        /// <summary>
        /// 事件注销
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handler"></param>
        public static void Off(string eventName, EventHandler handler) {
            if (handler == null) {
                return;
            }

            if (eventListenersDic.ContainsKey(eventName)) {
                EventHandler targetHandler = eventListenersDic[eventName].Find(handle => handle == handler);
                if (targetHandler != null) {
                    eventListenersDic[eventName].Remove(targetHandler);
                }
            }
        }

        /// <summary>
        /// 事件 tick
        /// </summary>
        /// <param name="timeSpan"></param>
        public static void Tick(float timeSpan) {
            for (int i = 0; i < eventRequests.Count; i++) {
                EventRequest request = eventRequests[i];
                List<EventHandler> emitterToHandler;

                if (eventListenersDic.TryGetValue(request.name, out emitterToHandler)) {
                    for (int j = 0; j < emitterToHandler.Count; j++) {
                        if (emitterToHandler[j] != null) {
                            emitterToHandler[j](request.evt);
                        }
                    }
                }
            }
            eventRequests.Clear();
        }

        public static void RemoveAllListeners() {
            eventListenersDic.Clear();
            eventRequests.Clear();
        }
    }
}
