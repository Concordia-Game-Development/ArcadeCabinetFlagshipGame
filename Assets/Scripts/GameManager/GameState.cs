// Possible states for the GameLoop singleton.
public enum GameState
{
	Null, // No state.
	Entering, // During the room's start transition.
	Wave, // The combat phase.
	LevelComplete, // After the combat phase if a level has been completed. A stair or something (?) appears, leading to the next level (or to the win screen ig).
	PanSpawn, // After the combat phase during a level. Summons the pan.
	ItemPickup, // The Pan is opened, items are waiting to be picked.
	RoomComplete, // Items have been picked up, the exits appear.
	Exiting, // During the room's end transition.
}