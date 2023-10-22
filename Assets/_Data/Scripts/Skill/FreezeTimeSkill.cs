using System.Collections;
using UnityEngine;

public class FreezeTimeSkill : Skill
{
    [SerializeField] private float freezeTime = 10f;

    private bool canFreeze = true;

    protected override bool CheckConditionSkill()
    {
        return this.canFreeze;
    }

    protected override void Action()
    {
        base.Action();

        StartCoroutine(this.FreezeTimeRoutine());
    }

    private IEnumerator FreezeTimeRoutine()
    {
        if (GameplayManager.HasInstance)
        {
            this.canFreeze = false;
            GameplayManager.Instance.PlaytimeCountingDown = false;

            yield return new WaitForSeconds(this.freezeTime);

            this.canFreeze = true;
            GameplayManager.Instance.PlaytimeCountingDown = true;
        }
    }
}
