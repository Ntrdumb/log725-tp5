using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _pvs = 3;
    private bool _peutRecevoirDommage = true;

    private void PrendreDuDommage(int dommage)
    {
        if (!_peutRecevoirDommage) return;
        _peutRecevoirDommage = false;
        _pvs -= dommage;
        if (_pvs <= 0)
        {
            _pvs = 0;
            Mourir();
        }
        else
        {
            UiManager.Instance.UpdatePv(_pvs);
            StartCoroutine(AttendreIFrames());
        }
    }

    private void Mourir()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ennemi"))
        {
            PrendreDuDommage(1);
            _peutRecevoirDommage = false;
        }
    }

    private IEnumerator AttendreIFrames()
    {
        yield return new WaitForSeconds(2f);
        _peutRecevoirDommage = true;
    }
}
