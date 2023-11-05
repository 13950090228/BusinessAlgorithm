namespace BusinessAlgorithm.Event {
    public class BattleEvent {
        public string eventName;
    }

    public class SpawnActorEvent : BattleEvent {
        public string actorSign;
    }
}