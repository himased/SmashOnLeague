using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SpearOfMars : MonoBehaviour
{
    [SerializeField] float movementSpeed, rotationSpeed, range, thurstDistance;
    [SerializeField] bool yLock;
    [SerializeField] string[] enemyTagsToCheck;
    [SerializeField] string[] impactTagsToCheck;

    Transform enemy;
    bool hasEnemy, hasImpacted;
    Vector3 spawnPos, impactPos;
    GameObject enemyLocHolder, hitFX, impactFX;

    PhotonView PV;

    public float health, duration;

    //private SkinnedMeshRenderer rend;
    //private MeshCollider col;
    bool isStun = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        hitFX = transform.Find("HitFX").gameObject;
        impactFX = transform.Find("ImpactFX").gameObject;
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!hasImpacted)
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

            var thurstDist = Vector3.Distance(transform.position, impactPos);
            if(thurstDist > thurstDistance && enemyLocHolder)
            {
                enemyLocHolder.transform.SetParent(transform);
            }

            var destroyDist = Vector3.Distance(transform.position, spawnPos);
            if(destroyDist > range)
            {
                Destroy(gameObject);
                if(enemy) enemy.GetComponent<PlayerController>().enabled = true;
            }

            if (enemy)
            {
                Vector3 enemyPos = enemyLocHolder.transform.position;
                if (yLock) enemyPos.y = enemy.position.y;
                enemy.position = enemyPos;
            }
            else
            {
                transform.RotateAround(transform.position, transform.forward, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            duration -= Time.deltaTime;
            if(duration <= 0)
            {
                enemy.GetComponent<PlayerController>().enabled = true;
                Destroy(gameObject);
            }
        }*/
        PV.RPC("SkillEffect", RpcTarget.All);
    }


    private void OnTriggerEnter(Collider other)
    {
        /*if (!hasImpacted)
        {
            if(!hasEnemy && enemyTagsToCheck.Contains(other.gameObject.tag))
            {
                enemyLocHolder = new GameObject("EnemyPositionHolder");
                enemyLocHolder.transform.position = other.transform.position;
                enemy = other.transform;
                impactPos = transform.position;
                hasEnemy = true;
                hitFX.SetActive(true);
                enemy.GetComponent<PlayerController>().enabled = false;
            }
            else if(hasEnemy && impactTagsToCheck.Contains(other.gameObject.tag))
            {
                impactFX.SetActive(true);
                enemyLocHolder.transform.SetParent(null);
                hasImpacted = true;
            }
        }*/
    }

    [PunRPC]
    void SkillEffect()
    {
        if (!hasImpacted)
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

            var thurstDist = Vector3.Distance(transform.position, impactPos);
            if (thurstDist > thurstDistance && enemyLocHolder)
            {
                enemyLocHolder.transform.SetParent(transform);
            }

            var destroyDist = Vector3.Distance(transform.position, spawnPos);
            if (destroyDist > range)
            {
                Destroy(gameObject);
                if (enemy) enemy.GetComponent<PlayerController>().enabled = true;
            }

            if (enemy)
            {
                Vector3 enemyPos = enemyLocHolder.transform.position;
                if (yLock) enemyPos.y = enemy.position.y;
                enemy.position = enemyPos;
            }
            else
            {
                transform.RotateAround(transform.position, transform.forward, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                enemy.GetComponent<PlayerController>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
