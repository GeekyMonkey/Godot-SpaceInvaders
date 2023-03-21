class_name LivesValueText
extends RichTextLabel


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	CS.connect("LivesChanged", OnLivesChanged)


## Respond to lives number change
func OnLivesChanged(newLives: int) -> void:
	text = str(newLives)
