using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour // doesnt have to be monobehaviour for now might change later
{
    Player player => GetComponent<Player>(); // doesnt need player for now might change to serialized field later
    
    #region Locomotion Methods

    public void Move(float _direction, float _speed){

        //figure out the delta time thing later might need to move it to fixedUpdate instead.
        player.rb.velocity = new Vector2(_direction * _speed /* * _deltaTime */, player.rb.velocity.y);
    }

    public void AllowMovement(bool allowFlipping = true){
        player.forceController.Move(InputReader.instance.moveDirection, player.moveSpeed);
        if(allowFlipping) { player.forceController.HandleSpriteDirection(); }
    }

    public void Jump(Vector2 direction, float jumpForce, bool cancelXVelocity = true){
        
        if(cancelXVelocity) {player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);}
        NullifyYVelocity();
        player.rb.AddForce(direction * jumpForce /* * _deltaTime */, ForceMode2D.Impulse);
    }

    public void VariableJump(Vector2 direction, float jumpForce, bool cancelXVelocity = true){
        
        if(cancelXVelocity) {player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);}
        
        player.rb.AddForce(direction * jumpForce /* * _deltaTime */, ForceMode2D.Force);
    }

    public void InitialJump() => Jump(Vector2.up, player.initialJumpForce, false);

    public void NullifyYVelocity() => player.rb.velocity = new Vector2(player.rb.velocity.x, 0);

    public void FallControl(float fallingGravity, float ascendingGravity){ // meant to be used in the update method
        

        if(player.rb.velocity.y < 0){
            player.rb.gravityScale = fallingGravity;
        }
        else{
            player.rb.gravityScale = ascendingGravity; 
        }
    }

    public void WallSlide(float wallSlidingSpeed){
        player.rb.velocity = new Vector2(player.rb.velocity.x, Mathf.Clamp(player.rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    public IEnumerator NullifyXVelocityAfterSeconds( float seconds){

        yield return new WaitForSeconds(seconds);
        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        
    }
    public IEnumerator ForceSwitchStateAfterSeconds( float seconds){

        yield return new WaitForSeconds(seconds);
        player.stateMachine.SwitchState(player.moveState);
    }

    public IEnumerator Dash(Vector2 direction, float dashSpeed, float dashLength, float dashWaitTime, float deltaTime){

        float dashStartTime = Time.time;
        float originalGravity = player.rb.gravityScale;

        player.hasDashed = true;
        player.isDashing = true;

        //player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
        player.rb.gravityScale = 0f;

        Vector2 finalDir = SnapToEightDirections(direction);//SnapToEightDirections(direction);

        Vector2 finalVelocity = new Vector2((finalDir.normalized.x * dashSpeed)* 1.5f, (finalDir.normalized.y * dashSpeed) / 1.2f); // might want a different x to y ratio


        while(Time.time < dashStartTime + dashLength){

            player.rb.velocity =  finalVelocity;//* finalDir.normalized * dashSpeed  *deltaTime */
            yield return null;
        }

        player.rb.gravityScale = originalGravity;
        player.isDashing = false;
        
        yield return new WaitForSeconds(dashWaitTime);

        player.hasDashed = false;
        
        

    }

    private Vector2 GetDashDirection(Vector2 inputVector){
        if(inputVector == Vector2.zero){return new Vector2((player.facingRight ? 1 : -1), 0);}
        return inputVector;
    }

    public Vector2 SnapToEightDirections(Vector2 inputVector)
    {   
        inputVector = GetDashDirection(inputVector);
        // Normalize the input vector to get the direction
        Vector2 normalizedInput = inputVector.normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(normalizedInput.y, normalizedInput.x) * Mathf.Rad2Deg;

        // Snap the angle to the closest 45-degree interval
        float snappedAngle = Mathf.Round(angle / 45.0f) * 45.0f;

        // Convert the snapped angle back to radians
        snappedAngle *= Mathf.Deg2Rad;

        // Create the snapped vector using trigonometry
        Vector2 snappedVector = new Vector2(Mathf.Cos(snappedAngle), Mathf.Sin(snappedAngle));

        return snappedVector;
    }

    #endregion

    #region Check Methods

    public bool CheckIsGrounded() => CheckIsGrounded(player.groundRaycastOffset, player.transform.position,player.groundRaycastLength, player.whatIsGround );

    public bool CheckIsGrounded(Vector3 _groundRaycastOffset,Vector3 _position, float _groundRaycastLength, LayerMask _groundLayer){
        
        //checks in two directs for the case when the player is on a cliff.
        return Physics2D.Raycast(_position + _groundRaycastOffset,Vector2.down, _groundRaycastLength, _groundLayer) 
        || Physics2D.Raycast(_position - _groundRaycastOffset,Vector2.down, _groundRaycastLength, _groundLayer);
        
    }

    public bool CheckCanCornerCorrect(Vector3 edgeRaycastOffset, Vector3 innerRaycastOffset, float topRaycastLength, Vector3 position, LayerMask cornerCorrectLayers){

        // Check Corner Correction 
        return Physics2D.Raycast(position + edgeRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayers) &&
                            !Physics2D.Raycast(position + innerRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayers) ||
                            Physics2D.Raycast(position - edgeRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayers) &&
                            !Physics2D.Raycast(position - innerRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayers);
    }
    
    public bool CheckOnAnyWall(float wallRaycastLength, Vector3 position, LayerMask wallLayer){

        //checks for walls
        return Physics2D.Raycast(position, Vector2.right, wallRaycastLength, wallLayer) || Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, wallLayer);
        
        
    }

    public bool CheckOnRightWall(float wallRaycastLength, Vector3 position, LayerMask wallLayer){
        
        //checks for the Right side wall
        // left wall is onAnyWall and !onRightWall
        return Physics2D.Raycast(position, Vector2.right, wallRaycastLength, wallLayer);

        
    }

    #endregion

    public void CornerCorrect(float yVelocity, Vector3 edgeRaycastOffset, Vector3 innerRaycastOffset, float topRaycastLength, Vector3 position, LayerMask groundLayer )
    {
        // Push Player to the right
        RaycastHit2D hit = Physics2D.Raycast(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.left, topRaycastLength, groundLayer);

        if(hit.collider != null){
            float newPosition = Vector3.Distance(new Vector3(hit.point.x, position.y, 0f) + Vector3.up * topRaycastLength, transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x + newPosition, transform.position.y, transform.position.z);
            player.rb.velocity = new Vector2(player.rb.velocity.x, yVelocity);
        }

        // Push Player Left
        hit = Physics2D.Raycast(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.right, topRaycastLength, groundLayer);

        if(hit.collider != null){
            float newPosition = Vector3.Distance(new Vector3(hit.point.x, position.y, 0f) + Vector3.up * topRaycastLength, transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
            transform.position = new Vector3(transform.position.x - newPosition, transform.position.y, transform.position.z);
            player.rb.velocity = new Vector2(player.rb.velocity.x, yVelocity);
        }
    }

    #region Flipping
    public void Flip(Vector3 scale) => player.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);

    public void Flip() => Flip(player.transform.localScale);

    public void HandleSpriteDirection(){
        if(InputReader.instance.moveDirection > 0  && !player.facingRight ){
            Flip();
            player.SetFacingRight(!player.facingRight);
        }
        else if(InputReader.instance.moveDirection < 0 && player.facingRight ){
            Flip();
            player.SetFacingRight(!player.facingRight);

        }
        
    }

    #endregion

    private void OnDrawGizmos() {
        // Ground Check
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + player.groundRaycastOffset,transform.position + player.groundRaycastOffset + Vector3.down * player.groundRaycastLength);
        Gizmos.DrawLine(transform.position - player.groundRaycastOffset,transform.position - player.groundRaycastOffset + Vector3.down * player.groundRaycastLength);
        
        // Corner Check
        Gizmos.DrawLine(transform.position + player.edgeRaycastOffset, transform.position + player.edgeRaycastOffset + Vector3.up * player.topRaycastLength);
        Gizmos.DrawLine(transform.position - player.edgeRaycastOffset, transform.position - player.edgeRaycastOffset + Vector3.up * player.topRaycastLength);
        Gizmos.DrawLine(transform.position + player.innerRaycastOffset, transform.position + player.innerRaycastOffset + Vector3.up * player.topRaycastLength);
        Gizmos.DrawLine(transform.position - player.innerRaycastOffset, transform.position - player.innerRaycastOffset + Vector3.up * player.topRaycastLength);
        
        // Corner Distance Check
        Gizmos.DrawLine(transform.position - player.innerRaycastOffset + Vector3.up * player.topRaycastLength,
                        transform.position - player.innerRaycastOffset + Vector3.up * player.topRaycastLength + Vector3.left * player.topRaycastLength);
        Gizmos.DrawLine(transform.position + player.innerRaycastOffset + Vector3.up * player.topRaycastLength,
                        transform.position + player.innerRaycastOffset + Vector3.up * player.topRaycastLength + Vector3.right * player.topRaycastLength);


        // Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * player.wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * player.wallRaycastLength); 
        
    }

    
}
