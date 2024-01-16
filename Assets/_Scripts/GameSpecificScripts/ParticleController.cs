using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particleCircle;
    public ParticleSystem particleSparkle;
    public ParticleSystem failSmoke;
    //public ParticleSystem winConfetti;

    private IEnumerator PlayParticlesOnLevelEnd()
    {
        var particle1 = Instantiate(particleCircle, transform.position, particleCircle.transform.rotation);
        var particle2 = Instantiate(particleSparkle, transform.position + Vector3.up * .5f, particleSparkle.transform.rotation);
        yield return new WaitForSeconds(.5f);
        transform.DOScale(0, 0.5f).Play();
        Destroy(particle1.gameObject, 3f);
        Destroy(particle2.gameObject, 3f);
    }

    public IEnumerator FailParticle()
    {
        var particle = Instantiate(failSmoke, new Vector3(5f, 1f, 3f), failSmoke.transform.rotation);
        Destroy(particle.gameObject, 3f);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.LevelFail();
    }

    //public IEnumerator WinParticle()
    //{
    //    var particle = Instantiate(winConfetti, transform.position, particleCircle.transform.rotation);
    //    Destroy(particle.gameObject, 3f);
    //    yield return new WaitForSeconds(.5f);
    //}
}
