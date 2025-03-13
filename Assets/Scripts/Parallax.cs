using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool scrollLeft;
    float singleTextureWidth;
    
    void Start()
    {
        SetupTexture();
    }
    void SetupTexture()
    {
        Sprite sprite= GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth=sprite.texture.width / sprite.pixelsPerUnit;
    }
    public void Scroll(float direction)
    {
        float delta = moveSpeed * Time.deltaTime * direction;
        transform.position += new Vector3(delta, 0f, 0f);
    }
    void CheckReset()
    {
        if((Mathf.Abs(transform.position.x)- singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
    void Update()
    {
        //Scroll(true);
        CheckReset();
    }
}
