using UnityEngine;

public class Character : MonoBehaviour
{
    private bool isSelected = false;
    public AudioClip selectionMusic; // 角色对应的音乐片段
    private AudioSource audioSource; // 音频源

    public GameObject particlePrefab; // 粒子特效的预制体
    private GameObject activeParticle; // 当前激活的粒子特效

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // 添加音频源组件
        audioSource.clip = selectionMusic; // 设置音频片段
    }

    void OnMouseDown()
    {
        ActivateParticleEffect();
    }

    public void Select()
    {
        isSelected = true;
        audioSource.Play(); // 播放音乐
    }

    public void Deselect()
    {
        isSelected = false;
        audioSource.Stop(); // 停止音乐（可选）
    }

    void Update()
    {
        if (isSelected)
        {
            float move = Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.position += new Vector3(move, 0, 0); // 只移动当前选中的角色
        }
    }

    void ActivateParticleEffect()
    {
        // 如果已经有粒子特效在播放，先销毁它
        if (activeParticle != null)
        {
            Destroy(activeParticle);
        }

        // 实例化新的粒子特效
        activeParticle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(activeParticle, 2f); // 2秒后销毁粒子特效（根据需要修改）
    }
}