using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonPressedEvent : UnityEvent { }
    public ButtonPressedEvent OnButtonPressed;

    public Vector3 Axis = new Vector3(0, -1, 0);
    public float MaxDistance;
    public float ReturnSpeed = 10.0f;

    Vector3 m_StartPosition;
    Rigidbody m_Rigidbody;
    Collider m_Collider;

    bool m_Pressed = false;

    [SerializeField] MasterScript Master;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponentInChildren<Collider>();
        m_StartPosition = transform.position;
    }

    void FixedUpdate()
    {
        // �������ޕ����̎w��i���g�̕��������[���h��ԓ��ł̕����֕ϊ�����j
        Vector3 worldAxis = transform.TransformDirection(Axis);
        // MAxDistance�𗘗p���čő�ړ��n�_���w�肷��
        Vector3 end = transform.position + worldAxis * MaxDistance;

        // �X�^�[�g���_�̈ʒu����̌��݂܂ł̈ړ��ʂ𑪒�i�������j
        float m_CurrentDistance = (transform.position - m_StartPosition).magnitude;
        RaycastHit info;

        float move = 0.0f;

        // �߂�����ɕ��̂��Ȃ���Ό��̈ʒu�ɖ߂鏈���B��������t�����i�������܂������j�ֈړ�����B
        if (m_Rigidbody.SweepTest(-worldAxis, out info, ReturnSpeed * Time.deltaTime + 0.005f))
        {//hitting something, if the contact is < mean we are pressed, move downward
            move = (ReturnSpeed * Time.deltaTime) - info.distance;
        }
        else
        {
            move -= ReturnSpeed * Time.deltaTime;
        }

        // �����n�_����ő�ړ������̊ԂɎ��܂�l�Ɉʒu���ϊ������
        float newDistance = Mathf.Clamp(m_CurrentDistance + move, 0, MaxDistance);

        // �V�����ʒu��ݒ肷��
        m_Rigidbody.position = m_StartPosition + worldAxis * newDistance;

        // �{�^����������Ă��Ȃ���� & �V�����ړ�����\��̏ꏊ�܂ł̋����ƍő�ړ��������ߎ��ł���Ȃ�IF������
        // �{�^����������Ă����� & �����ړ�����\��̏ꏊ�܂ł̋����ƍő�ړ��������ߎ��łȂ��Ȃ�ELSEIF������
        if (!m_Pressed && Mathf.Approximately(newDistance, MaxDistance))
        {//was just pressed
            m_Pressed = true;
            OnButtonPressed.Invoke();
        }
        else if (m_Pressed && !Mathf.Approximately(newDistance, MaxDistance))
        {//was just released
            m_Pressed = false;
        }
    }

    public void PushButton1()
    {
        Instantiate(Master.GrabCube);
    }

    public void PushButton2()
    {
        Instantiate(Master.GrabSphere);
    }
}