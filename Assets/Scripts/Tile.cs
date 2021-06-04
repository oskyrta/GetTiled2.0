using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool m_enabled = false;
    Color m_color;

    public void Enable(Color color)
    {
        m_enabled = true;
        m_color = color;

        gameObject.SetActive(true);
        GetComponent<Animator>().Play("Stay");
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void Disable()
    {
        m_enabled = false;
        gameObject.SetActive(false);
    }

    public void DisableWithAnim(float anim_delay)
    {
        m_enabled = false;
        StartCoroutine( WaitAndPlay(anim_delay) );
    }

    IEnumerator WaitAndPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<Animator>().Play("Disappearing");
    }

    public bool Enabled { get { return m_enabled; } }
}
