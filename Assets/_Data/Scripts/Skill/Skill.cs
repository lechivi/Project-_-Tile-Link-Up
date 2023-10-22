using System.Collections;
using UnityEngine;

public class Skill : SaiMonoBehaviour
{
    [SerializeField] protected float cooldownTime = 10.0f;

    protected bool isSkillReady = true;
    protected float countdownTimer;

    public bool IsSkillReady { get => this.isSkillReady; }

    public virtual void PlaySkill()
    {
        if (this.CheckConditionSkill() && this.isSkillReady)
        {
            this.isSkillReady = false;
            StartCoroutine(this.CountdownCooldown());
            this.Action();
        }
    }

    protected virtual bool CheckConditionSkill()
    {
        return this.isSkillReady;
    }

    protected virtual IEnumerator CountdownCooldown()
    {
        yield return new WaitForSeconds(this.cooldownTime);

        this.isSkillReady = true;
    }

    protected virtual void Action()
    {
        //For override
    }

    public virtual bool CheckCanPlay()
    {
        return this.CheckConditionSkill() && this.isSkillReady;
    }
}
