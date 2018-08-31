
#pragma strict

var target : Transform;
var targetPos : Vector2;
private var smoothTime = 0.75;
private var thisTransform : Transform;
private var velocity : Vector2;

function Start()
{
	thisTransform = transform;
	if(target != null)
		targetPos = target.position;
}

function Update() 
{
	if(target != null)
		targetPos = target.position;
		
	thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, 
		targetPos.x, velocity.x, smoothTime);
	thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, 
		targetPos.y + 5f, velocity.y, smoothTime);
}