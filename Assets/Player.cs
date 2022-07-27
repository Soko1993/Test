using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using UnityEngine.UI;

public enum ControlType { PC, Android }

public class Player : MonoBehaviour
{
    public float speed;
    PhotonView view;
    public Joystick joystick;

    public ControlType controlType;

    bool isPC;
    bool isMoving;
    string debugString;

    void Start()
    {
        isMoving = false;
        view = GetComponent<PhotonView>();
        isPC = (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor);

        var canvas = GetComponentInChildren<Canvas>();
        if (!view.IsMine || isPC) canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (view.IsMine)
        {
            Vector2 moveInput = (isPC) ?
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) :
                new Vector2(joystick.Horizontal, joystick.Vertical);

            Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;
            var vectorSpeed = System.Math.Round(moveInput.magnitude * 10) / 10.0;

            debugString = $"{vectorSpeed}, {isPC}, {Time.deltaTime}";

            transform.position += (Vector3)moveAmount;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUIStyle style = new GUIStyle();
        style.fontSize = 80;
        GUILayout.Label(debugString, style);
        GUILayout.EndArea();
    }
}