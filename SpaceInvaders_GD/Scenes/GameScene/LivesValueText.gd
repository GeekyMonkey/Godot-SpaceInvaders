extends RichTextLabel


# Called when the node enters the scene tree for the first time.
func _ready():
	CS.connect("LivesChanged", OnLivesChanged)


## Respond to lives number change
func OnLivesChanged(newLives: int):
	text = str(newLives)
