using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Litkey.Utility;
using UnityEngine.Events;
using DG.Tweening;
using System.Threading.Tasks;

public class UnitAttack : MonoBehaviour
{

    [Header("Unit Settings")]
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] public float _attackRange;
    [SerializeField] private float _attackPerTime;
    [SerializeField] private float _attackDelay; // delay time after vfx is over to attack
    [SerializeField] private Vector3 attackPosOffset;
    [SerializeField] private bool cantMoveWhenAttack; // stops when enemy is attacking 


    [Header("Projectile Settings")]
    public bool shootStraight;
    [SerializeField] private float projectileSpeed = 150f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject onAttackVFX;
    [SerializeField] private float onAttackVFXTime; // time vfx lasts
    [SerializeField] private GameObject onHitVFX;

    [Header("Other Settings")]
    public string enemyTag;
    [SerializeField] private GameObject body;

    [SerializeField] private GameObject _target;
    public bool stopAttacking; // stops the attack
    private bool stopAttackTimer;

    private float currentAttackSpeedTimer = 0f;
    private GameObject onAttackVFXCopy;
    private float startScale = 0f;
    private float endScale = 1f;

    Vector3 dirToEnemy;

    private Monster unitMovement;


    private Vector3 lookDir => body.transform.localScale.x < 0 ? Vector3.left : Vector3.right;

    public UnityAction<GameObject> OnAttack;

    // Update is called once per frame
    void Update()
    {
        if (stopAttackTimer) return;

        currentAttackSpeedTimer -= Time.deltaTime;
        if (stopAttacking) return;
        if (currentAttackSpeedTimer <= 0 && _target != null)
        {
            // attack here
            currentAttackSpeedTimer = _attackPerTime;
            DoAttack();
        }
    }

    private void DoAttack()
    {
        stopAttackTimer = true;
        //var dirToEnemy = (_target.transform.position - (transform.position + lookDir)).normalized;
        if (cantMoveWhenAttack)
        {
            if (TryGetComponent<Monster>(out unitMovement))
            {
                Debug.Log("CanMove false");
                unitMovement.canMove = false;
            }
        }
        OnAttack?.Invoke(gameObject);
        // Creates the copy of attack VFX
        if (onAttackVFXCopy == null && onAttackVFX != null)
            onAttackVFXCopy = Instantiate(onAttackVFX, transform.position + attackPosOffset, onAttackVFX.transform.rotation, transform);

        // if attack vfx exists, make projectile after vfx is ran
        if (onAttackVFXCopy)
        {
            onAttackVFXCopy.gameObject.SetActive(true);
            startScale = 0f;
            DOTween.To(() => startScale, x => startScale = x, endScale, onAttackVFXTime)
                    .OnUpdate(() =>
                    {
                        onAttackVFXCopy.transform.localScale = Vector3.one * startScale;
                    })
                    .OnComplete(() => {
                        onAttackVFXCopy.gameObject.SetActive(false);
                        dirToEnemy = (_target.transform.position - (transform.position + attackPosOffset)).normalized;
                        if (shootStraight)
                            dirToEnemy = lookDir;
                        //CreateProjectile(dirToEnemy);
                    });
        }
        else
        {
            dirToEnemy = (_target.transform.position - (transform.position + attackPosOffset)).normalized;
            if (shootStraight)
                dirToEnemy = lookDir;
            //CreateProjectile(dirToEnemy);
        }

    }

    // Creates projectile and shoots to the direction given
    //private async void CreateProjectile(Vector3 dirToEnemy)
    //{
    //    if (_target == null) return;
    //    await Task.Delay((int)(1000f * _attackDelay));

    //    var proj = Instantiate(projectile, transform.position + attackPosOffset, Quaternion.identity);

    //    proj.transform.rotation = UtilClass.GetRotationFromDirection(dirToEnemy);
    //    proj.GetComponent<Projectile>().Setup(dirToEnemy, projectileSpeed, enemyTag, _attackDamage);

    //    if (onHitVFX) proj.GetComponent<Projectile>().Setup(dirToEnemy, projectileSpeed, enemyTag, _attackDamage, onHitVFX);

    //    stopAttackTimer = false;

    //    // enables the movement again
    //    if (cantMoveWhenAttack)
    //    {
    //        if (unitMovement != null)
    //        {
    //            unitMovement.canMove = true;
    //            Debug.Log("Canmove true");
    //        }
    //    }
    //}

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}