using UnityEngine;

public class ReturnLastSkill : Skill
{
    [SerializeField] private PoolTile poolTile;

    private float boundUp = 11f;
    private float boundDown = -8f;
    private float boundRight = 7.5f;
    private float boundLeft = -7.5f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.poolTile == null )
            this.poolTile = GameObject.Find("PoolTile").GetComponent<PoolTile>();
    }

    protected override bool CheckConditionSkill()
    {
        return (TilesPanel.instance.ListTile.Count > 0 && TilesPanel.instance != null);
    }

    protected override void Action()
    {
        base.Action();
        
        int count = TilesPanel.instance.ListTile.Count;
        Vector3 pos;
        pos.x = Random.Range(this.boundLeft, this.boundRight);
        pos.z = Random.Range(this.boundDown, this.boundUp);
        pos.y = 4f;

        TileCtrl tile = TilesPanel.instance.ListTile[count - 1];

        TilesPanel.instance.ListTile.RemoveAt(count - 1);
        TilesPanel.instance.RemoveKeyTileSO(tile.TileSO, 1);

        tile.TileMovement.Move(pos, 0.5f);
        tile.SetState(true);
        tile.transform.SetParent(this.poolTile.transform);

        TilesPanel.instance.ReorganizePanel();
    }
}
