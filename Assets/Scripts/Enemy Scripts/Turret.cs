using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    protected override float maxTimeBetweenAttacks => 6f;

    protected override float minTimeBetweenAttacks => 1f;

    protected override int health { get; set; } = 1;

    protected override int scoreValue => 150;

    public List<Sprite> ceilingSprites;
    public List<Sprite> floorSprites;
    public bool onCeiling = false;

    public override void Move()
    {
        //the turrets don't actually move but tilt, so basicly we only need to change sprite according to the angle
        Vector2 directionToPlayer = (player.transform.position-transform.position).normalized;
        //can't use switch case on ranges in c# apparently, so let the ifs reign.
        //this code is ugly, but honestly it would be way harder to read if I used some sort of for loop and list of ranges.
        if (directionToPlayer.x < -0.8f)
        {
            if (onCeiling)
                spriteRenderer.sprite = ceilingSprites[0];
            else
                spriteRenderer.sprite = floorSprites[0];
            return;
        }
        if (directionToPlayer.x < -0.2f)
        {
            if (onCeiling)
                spriteRenderer.sprite = ceilingSprites[1];
            else
                spriteRenderer.sprite = floorSprites[1];
            return;
        }
        if(directionToPlayer.x < 0)
        {
            if (onCeiling)
                spriteRenderer.sprite = ceilingSprites[2];
            else
                spriteRenderer.sprite = floorSprites[2];
        }
        if (directionToPlayer.x < 0.2f)
        {
            if (onCeiling)
                spriteRenderer.sprite = ceilingSprites[3];
            else
                spriteRenderer.sprite = floorSprites[3];
            return;
        }
        if (directionToPlayer.x < 0.8f)
        {
            if (onCeiling)
                spriteRenderer.sprite = ceilingSprites[4];
            else
                spriteRenderer.sprite = floorSprites[4];
            return;
        }
        if (onCeiling)
            spriteRenderer.sprite = ceilingSprites[5];
        else
            spriteRenderer.sprite = floorSprites[5];
    
    }

    public override void StartAnimation()
    {
        enemyAnimator.enabled = false; // we set sprites manually in move
    }
}
