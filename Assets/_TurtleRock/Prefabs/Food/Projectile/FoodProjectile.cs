using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProjectile : MonoBehaviour
{
    [SerializeField]
    protected EFoodType foodType = EFoodType.Turtle;
    protected AnimationCurve _travelCurve;
    protected Rigidbody _rb;
    protected bool _isTraveling = false;
    protected bool _isMustBeDestroyed = false;
    protected float _maxTravelDistance = 0.0f;
    protected float _travelSpeed = 0.0f;
    protected float _traveledDistance = 0.0f;
    protected float _verticalSpeed = 0.0f;

    public EFoodType FoodType { get => foodType;}

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Travel();
    }
    public virtual void Launch(AnimationCurve travelCurve, float maxTravelDistance, float travelSpeed)
    {
        gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_LAUNCHED_FOOD);
        _travelCurve = travelCurve;
        _travelSpeed = travelSpeed;
        _maxTravelDistance = maxTravelDistance;
        _isTraveling = true;
    }
    public virtual void Travel()
    {
        if (!_isTraveling) { return; }

        _verticalSpeed = _travelCurve.Evaluate(_traveledDistance / _maxTravelDistance);
        _rb.velocity = transform.forward * _travelSpeed + Vector3.up * 2 * (0.5f - _verticalSpeed) * _travelSpeed;
        _traveledDistance += _travelSpeed * Time.fixedDeltaTime;
        if (_traveledDistance > _maxTravelDistance)
        {
            if (_isMustBeDestroyed)
            {
                Destroy(this.gameObject);
            }
            _isTraveling = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Turtle>(out Turtle turtle))
        {
            if (_isTraveling)
            {
                _isMustBeDestroyed = true;
                return;
            }
            Destroy(this.gameObject);
        }
    }
}
