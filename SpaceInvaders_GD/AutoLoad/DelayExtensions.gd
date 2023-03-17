extends Node2D


## Seconds delay
func Seconds(sec: float):
	await get_tree().create_timer(sec).timeout


## Wait for the next frame
func NextFrame():
	await get_tree().process_frame


## Wait for the next physics frame
func NextPhysicsFrame():
	await get_tree().physics_frame
