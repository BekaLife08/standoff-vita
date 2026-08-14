using Axlebolt.Standoff.Common;
using UnityEngine;

public class MagazineController : MonoBehaviour
{
	public enum State
	{
		NotStated,
		Attached,
		Thrown
	}

	private State state;

	public TransformTRS handOffset;

	public float lifeTime;

	private float timeFixPoint;

	private Vector3 velocity;

	private Vector3 prevPos;

	public Vector3 throwDir;

	public float throwDirCoeff;

	public Vector3 throwTorque;

	public float throwTorqueCoeff;

	public void AttachHand(Transform hand)
	{
		state = State.Attached;
		base.transform.SetParent(hand);
		base.transform.localPosition = handOffset.pos;
		base.transform.localEulerAngles = handOffset.rot;
		base.transform.localScale = handOffset.scale;
		base.gameObject.SetActive(value: true);
		prevPos = base.transform.position;
		state = State.Attached;
	}

	public void Throw()
	{
		timeFixPoint = Time.time;
		base.transform.SetParent(null);
		Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
		rigidbody.velocity = velocity / 3f;
		rigidbody.AddTorque(new Vector3(1000f, 0f, 0f), ForceMode.Force);
		GetComponent<Collider>().enabled = true;
		state = State.Thrown;
	}

	private void Attached()
	{
		velocity = Vector3.Lerp(velocity, (base.transform.position - prevPos) / Time.deltaTime, 2f * Time.deltaTime);
	}

	private void NotStated()
	{
	}

	private void Thrown()
	{
		if (Time.time - timeFixPoint > lifeTime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		switch (state)
		{
		case State.Attached:
			Attached();
			break;
		case State.NotStated:
			NotStated();
			break;
		case State.Thrown:
			Thrown();
			break;
		}
	}
}
