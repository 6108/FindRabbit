using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    float rx;
    float ry;
    public float rotSpeed = 200;
    bool isJump = false;
    public GameObject rabbitParticle;
    
    void Update()
    {
        RayCast();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
                Jump();
        }
        float mx = Input.GetAxis("Mouse X"); //���콺 �̵� x��
        float my = Input.GetAxis("Mouse Y"); //���콺 �̵� y��

        //��� ȸ�������� �ٸ��ϱ� ����
        //deltaTime�� ���ϸ� ȸ�� �ӵ��� 1/60���� �پ �ϰ����� �ӵ�(rotSpeed) ������
        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        //rx ȸ���� ����
        //������ �Ѿ�� �ּ�, �ִ� ����
        rx = Mathf.Clamp(rx, -80, 80);
        rx = 0;

        //transform�� rotation ����
        //x���� ȸ���� ���콺�� ���� ����� �ݴ��̱� ������ -1 ������
        transform.eulerAngles = new Vector3(-rx, ry, 0);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //2D�� ���� ����̶� up�� ����߰� ������ 3D�� forward
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();

        //ī�޶� ���� ������ ������ ����
        //'Camera'�� �±װ� MainCamera�� ��ü�� ������ �� ����
        dir = Camera.main.transform.TransformDirection(dir);

        //P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }

    void RayCast()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "Rabbit")
                {
                    GameManager.instance.GetRabbit();
                    gameObject.GetComponent<AudioSource>().Play();
                    Instantiate(rabbitParticle, hitInfo.transform.position, Quaternion.identity);
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }

    IEnumerator IeJump()
    {
        isJump = true;
        yield return new WaitForSeconds(1.2f);
        isJump = false;
    }
    void Jump()
    {
        print("Jump");
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 100000f);
        StartCoroutine(IeJump());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rabbit")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 10f);
        }
    }
}

