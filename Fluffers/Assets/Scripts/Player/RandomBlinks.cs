using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlinks : MonoBehaviour
{

    public Sprite NeutralFace;
    public Sprite BlinkFace;
    public SpriteRenderer SR;

    public float blinkRate;
    private float orgRate;

    // Start is called before the first frame update
    void Start()
    {
        orgRate = blinkRate;
    }

    // Update is called once per frame
    void Update()
    {
        blinkRate -= 1 * Time.deltaTime;
        if(blinkRate < 0)
        {
            StartCoroutine(Blink());
            blinkRate = orgRate;
        }
    }

    IEnumerator Blink()
    {
        SR.sprite = BlinkFace;
        yield return new WaitForSeconds(0.05f);
        SR.sprite = NeutralFace;
        yield return new WaitForSeconds(0.05f);
        SR.sprite = BlinkFace;
        yield return new WaitForSeconds(0.05f);
        SR.sprite = NeutralFace;
    }
}
