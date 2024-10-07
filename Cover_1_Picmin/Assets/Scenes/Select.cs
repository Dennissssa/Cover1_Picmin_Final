using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSelection : MonoBehaviour
{
    public Camera mainCamera; // 主摄像头
    public List<Character> characters; // 角色列表
    public CinemachineVirtualCamera[] characterCameras; // 角色对应的虚拟相机

    private Character selectedCharacter; // 当前选择的角色

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            SelectCharacter();
        }
        else if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            DeselectCharacter();
        }
    }

    void SelectCharacter()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Character clickedCharacter = hit.collider.GetComponent<Character>();
            if (clickedCharacter != null)
            {
                if (selectedCharacter != null)
                {
                    selectedCharacter.Deselect(); // 取消当前选择的角色
                }

                selectedCharacter = clickedCharacter; // 选择新角色
                selectedCharacter.Select(); // 选择角色

                // 保存选择的角色索引
                int index = characters.IndexOf(clickedCharacter);
                PlayerPrefs.SetInt("SelectedCharacterIndex", index);
                PlayerPrefs.Save(); // 保存 PlayerPrefs

                // 切换相机
                for (int i = 0; i < characterCameras.Length; i++)
                {
                    characterCameras[i].Priority = (clickedCharacter == characters[i]) ? 10 : 0;
                }
            }
        }
    }

    void DeselectCharacter()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.Deselect();
            selectedCharacter = null; // 清空选择
            foreach (var cam in characterCameras)
            {
                cam.Priority = 0; // 禁用所有相机
            }
        }
    }
}