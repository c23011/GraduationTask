using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerController : MonoBehaviour
{
    //VR�̃R���g���[���[�̐e�I�u�W�F�N�g
    [SerializeField] GameObject PlayerParent;
    
    //CenterEyeAnchor�����_
    [SerializeField] GameObject HeadSet;

    //�R���g���[���[L�R
    [SerializeField] GameObject ControllerLeft;
    [SerializeField] GameObject ControllerRight;

    //RotY���擾�p�x�N�g��
    Quaternion PlayerRot;

    //�R���g���[���[�����ʒu�L���ϐ�
    [SerializeField] float startHandPos;

    //�U�������ϐ�
    [SerializeField] float moveDirection;

    //�U�񐔂̃J�E���g
    [SerializeField] int  minShakeCount;
    [SerializeField] int  nowShakeCount;
    bool  LeftShakeCountSW;
    bool  RightShakeCountSW;

    //�ړ��X�s�[�h
    [SerializeField] float moveSpeed;

    //�ړ����Ă��邩�̃X�C�b�`
    [SerializeField] bool MoveSW;

    //�r��U���ĂȂ����̎��Ԃ��v��
    [SerializeField] float noMoveTimer;
    [SerializeField] float minMoveTimer;
    [SerializeField] bool TimerResetSW;

    //�ړ��p�R���g���[���[���W�擾
    float controllerLeftPosY;
    float controllerRightPosY;

    Vector2 LeftStickVec;

    void Start()
    {
        
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            startHandPos = controllerLeftPosY;
            Debug.Log(startHandPos);
        }

        controllerLeftPosY  = ControllerLeft.transform.position.y;
        controllerRightPosY = ControllerRight.transform.position.y;

        PlayerRot = HeadSet.transform.rotation;

        this.transform.rotation = new Quaternion(PlayerParent.transform.rotation.x,PlayerRot.y, PlayerParent.transform.rotation.z, PlayerRot.w);
        PlayerParent.transform.position = this.transform.position;

        LeftStickVec = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        if (LeftStickVec.y != 0.0f)
        {
            this.transform.position += transform.forward * (LeftStickVec.y * moveSpeed);
            this.transform.position += transform.right   * (LeftStickVec.x * moveSpeed);
        }

        LeftShake();
        RightShake();

        if (nowShakeCount >= minShakeCount)
        {
            MoveSW = true;
        }
        else
        {
            MoveSW = false;
        }

        if (MoveSW == true)
        {
            this.transform.position += transform.forward * moveSpeed;
        }
    }

    void LeftShake()
    {
        if (controllerLeftPosY >= startHandPos + moveDirection || controllerLeftPosY <= startHandPos - moveDirection)
        {
            if (LeftShakeCountSW == true)
            {
                nowShakeCount++;
                Debug.Log(nowShakeCount);
                LeftShakeCountSW = false;
                TimerResetSW = true;
            }
        }

        if (controllerLeftPosY <= startHandPos + moveDirection && controllerLeftPosY >= startHandPos - moveDirection)
        {
            if (LeftShakeCountSW == false)
            {
                LeftShakeCountSW = true;
            }
        }

        if (TimerResetSW == true)
        {
            noMoveTimer = 0;
            TimerResetSW = false;
        }

        if (LeftShakeCountSW == true && TimerResetSW == false)
        {
            noMoveTimer += Time.deltaTime;
        }

        if (LeftShakeCountSW == false && TimerResetSW == false)
        {
            noMoveTimer += Time.deltaTime;
        }

        if (noMoveTimer >= minMoveTimer)
        {
            nowShakeCount = 0;
            noMoveTimer = 0;
        }
    }

    void RightShake()
    {
        if (controllerRightPosY >= startHandPos + moveDirection || controllerRightPosY <= startHandPos - moveDirection)
        {
            if (RightShakeCountSW == true)
            {
                nowShakeCount++;
                RightShakeCountSW = false;
                TimerResetSW = true;
            }
        }

        if (controllerRightPosY <= startHandPos + moveDirection && controllerRightPosY >= startHandPos - moveDirection)
        {
            RightShakeCountSW = true;
        }

        if (TimerResetSW == true)
        {
            noMoveTimer = 0;
            TimerResetSW = false;
        }

        if (RightShakeCountSW == true && TimerResetSW == false)
        {
            noMoveTimer += Time.deltaTime;
        }

        if (RightShakeCountSW == false && TimerResetSW == false)
        {
            noMoveTimer += Time.deltaTime;
        }

        if (noMoveTimer >= minMoveTimer)
        {
            nowShakeCount = 0;
            noMoveTimer = 0;
        }
    }
}