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
        float mx = Input.GetAxis("Mouse X"); //마우스 이동 x축
        float my = Input.GetAxis("Mouse Y"); //마우스 이동 y축

        //축과 회전방향은 다르니까 주의
        //deltaTime을 곱하면 회전 속도가 1/60으로 줄어서 일괄적인 속도(rotSpeed) 곱해줌
        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        //rx 회전각 제한
        //범위를 넘어가면 최소, 최댓값 리턴
        rx = Mathf.Clamp(rx, -80, 80);
        rx = 0;

        //transform의 rotation 접근
        //x축의 회전은 마우스의 증가 방향과 반대이기 떄문에 -1 곱해줌
        transform.eulerAngles = new Vector3(-rx, ry, 0);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //2D일 때는 평면이라 up을 사용했고 지금은 3D라 forward
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();

        //카메라가 보는 방향을 앞으로 설정
        //'Camera'는 태그가 MainCamera인 객체를 가져올 수 있음
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

