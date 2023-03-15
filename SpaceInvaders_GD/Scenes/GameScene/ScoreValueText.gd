extends RichTextLabel

# Editor State
@export var changeDelay = 0.01

# Private state
var changeTime: float = 0
var scoreDisplayed: int = 0
var scoreCurrent: int = 0


# Called when the node enters the scene tree for the first time.
func _ready():
	CS.connect("ScoreChanged", OnScoreChanged)


# Respond to score change
func OnScoreChanged(newScore: int):
	scoreCurrent = newScore


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	# If the score displayed is behind the actual score - increment by one, but slowly
	if (scoreCurrent > scoreDisplayed):
		changeTime += delta
		if (changeTime > changeDelay):
			scoreDisplayed += 1
			text = str(scoreDisplayed)
			changeTime = 0
