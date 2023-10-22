using UnityEngine;

public class BounceUpSkill : Skill
{
    [SerializeField] private PoolTile poolTile;
    [SerializeField] private Vector2 flipForceRange = new Vector2(200, 500);

    private float flipTorque = 50;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.poolTile == null )
            this.poolTile = GameObject.Find("PoolTile").GetComponent<PoolTile>();
    }

    protected override bool CheckConditionSkill()
    {
        return (this.poolTile != null);
    }

    protected override void Action()
    {
        base.Action();

        foreach (TileCtrl tile in this.poolTile.PoolingTile)
        {
            if (tile == null) continue;

            float randomForce = Random.Range(this.flipForceRange.x - 50, this.flipForceRange.y + 50);
            tile.Rb.AddForce(Vector3.up * randomForce);

            int randomDir = Random.Range(0, 2) == 0 ? -1 : 1;
            tile.Rb.AddTorque(tile.transform.up * randomDir * this.flipTorque);
        }
    }
}
