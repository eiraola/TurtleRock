using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFoodType
{
    Turtle,
    StingRay
}
public abstract class Food: MonoBehaviour
{
    [SerializeField]
    protected EFoodType foodType = EFoodType.Turtle;
    protected AnimationCurve _travelCurve;
    protected Rigidbody rb;
    protected bool _isTraveling = false;
    protected float _maxTravelDistance = 0.0f;
    protected float _travelSpeed = 0.0f;
    float traveledDistance = 0.0f;
    float verticalSpeed = 0.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Travel();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            Collect(other.GetComponent<FoodWallet>());
        }
    }
    public virtual void Travel()
    {
        if (!_isTraveling) { return; }
        
        verticalSpeed = _travelCurve.Evaluate(traveledDistance / _maxTravelDistance);
        rb.velocity = transform.forward * _travelSpeed + Vector3.up * 2 * (0.5f - verticalSpeed) * _travelSpeed;
        traveledDistance += _travelSpeed * Time.fixedDeltaTime;
        if (traveledDistance > _maxTravelDistance)
        {
            _isTraveling = false;
            gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_FOOD);
        }
    }
    public virtual void Launch(AnimationCurve travelCurve, float maxTravelDistance, float travelSpeed) {
        gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_LAUNCHED_FOOD);
        _travelCurve = travelCurve;
        _isTraveling = true;
        _travelSpeed = travelSpeed;
        _maxTravelDistance = maxTravelDistance;
    }
    public virtual void Collect(FoodWallet foodWallet) {
        if (!foodWallet) { return; }
        foodWallet.AddFoodToWallet(foodType);
        Destroy(this.gameObject);
    }
    protected abstract void Feed();
}
