using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;

    float delaysecond = 0f;
    public AttackState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Attack);
        _Animals.attackCollider.enabled = false;
        _Animals.nav.isStopped = true;
    }

    public void Exit()
    {
        _Animals.nav.isStopped = false;
        delaysecond = 0f;
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if (delaysecond > 1.0f)
        {
            delaysecond = 0;
            _Animals.attackCollider.enabled = true;
            _Animals.States = AnimalAI.State.Chase;

            Vector3 startPoint = _Animals.transform.position;
            Vector3 forward = _Animals.transform.forward;

            // 부채꼴 영역에 레이캐스트를 쏩니다.
            RaycastHit[] hits = Physics.SphereCastAll(startPoint, _AnimalStats.animalSO.range, forward, 0);

            foreach (RaycastHit hit in hits)
            {
                if (((1 << hit.collider.gameObject.layer) | _Animals.playerLayerMask) == _Animals.playerLayerMask)
                {
                    if (Vector3.Distance(_Animals.transform.position, GameManager.Instance.PlayerObj.transform.position) >= _AnimalStats.animalSO.attackRange)
                    {
                        _Animals.States = AnimalAI.State.Chase;
                    }
                }
            }
        }

    }
}
